﻿using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Golemo.Core.nAccount;
using Golemo.Core.Character;
using System.Linq;
using GolemoSDK;

namespace Golemo.Core
{
    class CarRoom : Script
    {
        private static nLog Log = new nLog("CARROOM");

        public static Vector3 CamPosition = new Vector3(-42.3758, -1101.672, 26.42235); // Позиция камеры
        public static Vector3 CamRotation = new Vector3(0, 0, 1.701622); // Rotation камеры
        public static Vector3 CarSpawnPos = new Vector3(-42.79771, -1095.676, 26.0117); // Место для спавна машины в автосалоне
        public static Vector3 CarSpawnRot = new Vector3(0, 0, -136.246); // Rotation для спавна машины в автосалоне

        public static void onPlayerDissonnectedHandler(Player player, DisconnectionType type, string reason)
        {
            try
            {
                NAPI.Task.Run(() =>
                {
                    if (!player.HasData("CARROOMTEST")) return;

                    Vehicle veh = player.GetData<Vehicle>("CARROOMTEST");
                    veh.Delete();
                });

                player.ResetData("CARROOMTEST");

            }
            catch (Exception e) { Log.Write("PlayerDisconnected: " + e.Message, nLog.Type.Error); }
        }

        public static void enterCarroom(Player player, string name)
        {
            if (NAPI.Player.IsPlayerInAnyVehicle(player)) return;
            uint dim = Dimensions.RequestPrivateDimension(player);
            NAPI.Entity.SetEntityDimension(player, dim);
            Main.Players[player].ExteriorPos = player.Position;
            NAPI.Entity.SetEntityPosition(player, new Vector3(CamPosition.X, CamPosition.Y - 2, CamPosition.Z));
            Trigger.ClientEvent(player, "freeze", true);

            player.SetData("INTERACTIONCHECK", 0);
            Trigger.ClientEvent(player, "carRoom");

            OpenCarromMenu(player, BusinessManager.BizList[player.GetData<int>("CARROOMID")].Type);
        }

		[ServerEvent(Event.PlayerExitVehicle)]
        public void Event_OnPlayerExitVehicle(Player player, Vehicle vehicle)
        {
            if (!player.HasData("CARROOMID")) return;
            if (player.HasData("CARROOMTEST"))
            {
                player.ResetData("CARROOMTEST");
                return;
            }
        }

        [ServerEvent(Event.PlayerExitVehicleAttempt)]
        public void Event_OnPlayerExitVehicleAttempt(Player player, Vehicle vehicle)
        {
            try
            {
                if (!player.HasData("CARROOMTEST")) return;

                var veh = player.GetData<Vehicle>("CARROOMTEST");
                veh.Delete();

                uint dim = Dimensions.RequestPrivateDimension(player);
                NAPI.Entity.SetEntityDimension(player, dim);
                NAPI.Entity.SetEntityPosition(player, new Vector3(CamPosition.X, CamPosition.Y - 2, CamPosition.Z));

                Trigger.ClientEvent(player, "carRoom");
                OpenCarromMenu(player, BusinessManager.BizList[player.GetData<int>("CARROOMID")].Type);
            }
            catch (Exception e)
            {
                Log.Write("PlayerExitVehicle: " + e.Message, nLog.Type.Error);
            }
        }

        [RemoteEvent("carroomTestDrive")]
        public static void RemoteEvent_carroomTestDrive(Player player, string vName, int color1, int color2, int color3)
        {
            try
            {
                if (!player.HasData("CARROOMID")) return;

                Trigger.ClientEvent(player, "destroyCamera");

                var mydim = Dimensions.RequestPrivateDimension(player);
                NAPI.Entity.SetEntityDimension(player, mydim);
                VehicleHash vh = (VehicleHash)NAPI.Util.GetHashKey(vName);
                var veh = NAPI.Vehicle.CreateVehicle(vh, new Vector3(-58.264317, -1110.5774, 26.218988), new Vector3(-0.27027863, 0.0050534788, 70.07986), 0, 0);
                NAPI.Vehicle.SetVehicleCustomSecondaryColor(veh, color1, color2, color3);
                NAPI.Vehicle.SetVehicleCustomPrimaryColor(veh, color1, color2, color3);
                veh.Dimension = mydim;
                veh.NumberPlate = "TESTDRIVE";
                veh.SetData("BY", player.Name);
                VehicleStreaming.SetEngineState(veh, true);
                player.SetIntoVehicle(veh, 0);
                player.SetData("CARROOMTEST", veh);
                player.SetData("LAST_HP", player.Health);
            }
            catch (Exception e)
            {
                Log.Write("TestDrive: " + e.Message, nLog.Type.Error);
            }
        }

        #region Menu
        private static Dictionary<string, Color> carColors = new Dictionary<string, Color>
        {
            { "Черный", new Color(0, 0, 0) },
            { "Белый", new Color(225, 225, 225) },
            { "Красный", new Color(230, 0, 0) },
            { "Оранжевый", new Color(255, 115, 0) },
            { "Желтый", new Color(240, 240, 0) },
            { "Зеленый", new Color(0, 230, 0) },
            { "Голубой", new Color(0, 205, 255) },
            { "Синий", new Color(0, 0, 230) },
            { "Фиолетовый", new Color(190, 60, 165) },
        };

        public static void OpenCarromMenu(Player player, int biztype)
        {
            var bizid = player.GetData<int>("CARROOMID");
            Business biz = BusinessManager.BizList[player.GetData<int>("CARROOMID")];
            var prices = new List<int>();

            if (biz.Type == 17)
            {
                player.SetSharedData("CARROOM-DONATE", true);
                biztype = 5;
            }
            else
            {
                player.SetSharedData("CARROOM-DONATE", false);
                biztype -= 2;
            }

            foreach (var p in biz.Products)
            {
                prices.Add(p.Price);
            }

            Trigger.ClientEvent(player, "openAuto", JsonConvert.SerializeObject(BusinessManager.CarsNames[biztype]), JsonConvert.SerializeObject(prices));
        }

        private static string BuyVehicle(Player player, Business biz, string vName, string color)
        {
            var prod = biz.Products.FirstOrDefault(p => p.Name == vName);
            string vNumber = "none";
            if (Main.Players[player].Fines != 0)
            {
                Notify.Error(player, "Вы не можете купить машину, у вас есть неоплаченные штрафы.", 2500);
                return vNumber;
            }
            //Если нет лицензии на вождение автомобилем
            if (Main.Players[player].Licenses[1] == false && biz.Type != 5)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "У вас нет лицензии на управление транспорта категории [B]", 2500);
                return vNumber;
            }
            //Если тип бизнеса мотосалон и нет лицензии на вождение мотоциклов
            if (Main.Players[player].Licenses[0] == false && biz.Type == 5)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "У вас нет лицензии на управление транспорта категории [A]", 2500);
                return vNumber;
            }
            if (biz.Type != 17)
            {
                // Check products available
                if (Main.Players[player].Money < prod.Price)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Недостаточно средств", 3000);
                    return vNumber;
                }
                if (!BusinessManager.takeProd(biz.ID, 1, vName, prod.Price))
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Транспортного средства больше нет на складе", 3000);
                    return vNumber;
                }

                MoneySystem.Wallet.Change(player, -prod.Price);

                GameLog.Money($"player({Main.Players[player].UUID})", $"biz({biz.ID})", prod.Price, $"buyCar({vName})");
            }
            else if (biz.Type == 17)
            {
                Account acc = Main.Accounts[player];

                if (acc.RedBucks < prod.Price)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Недостаточно Redbucks!", 3000);
                    return vNumber;
                }
                acc.RedBucks -= prod.Price;
                GameLog.Money(acc.Login, "server", prod.Price, "donateAutoroom");
            }

            vNumber = VehicleManager.Create(player.Name, vName, carColors[color], carColors[color], new Color(0, 0, 0));

            Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"Вы купили {vName} с идентификатором {vNumber} ", 3000);
            Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"Автомобиль доставлен в ваш гараж!", 5000);

            return vNumber;
        }


        [RemoteEvent("carroomBuy")]
        public static void RemoteEvent_carroomBuy(Player player, string vName, string color)
        {
            try
            {
                Business biz = BusinessManager.BizList[player.GetData<int>("CARROOMID")];
                NAPI.Entity.SetEntityPosition(player, new Vector3(biz.EnterPoint.X, biz.EnterPoint.Y, biz.EnterPoint.Z + 1.5));
                Trigger.ClientEvent(player, "freeze", false);

                Main.Players[player].ExteriorPos = new Vector3();
                Trigger.ClientEvent(player, "destroyCamera");
                NAPI.Entity.SetEntityDimension(player, 0);
                Dimensions.DismissPrivateDimension(player);

                var house = Houses.HouseManager.GetHouse(player, true);
                if (house == null || house.GarageID == 0)
                {
                    // Player without garage
                    string vNumber = BuyVehicle(player, biz, vName, color);
                    if (vNumber != "none")
                    {
                        // VehicleManager.Spawn(vNumber, biz.UnloadPoint, 90, player);
                    }
                }
                else
                {
                    var garage = Houses.GarageManager.Garages[house.GarageID];
                    // Проверка свободного места в гараже
                    if (VehicleManager.getAllPlayerVehicles(player.Name).Count >= Houses.GarageManager.GarageTypes[garage.Type].MaxCars)
                    {
                        Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Ваши гаражи полны", 3000);
                        return;
                    }
                    string vNumber = BuyVehicle(player, biz, vName, color);
                    if (vNumber != "none")
                    {
                        garage.SpawnCar(vNumber);
                    }
                }
            }
            catch (Exception e) { Log.Write("CarroomBuy: " + e.Message, nLog.Type.Error); }
        }

        [RemoteEvent("carroomCancel")]
        public static void RemoteEvent_carroomCancel(Player player)
        {
            try
            {
                if (!player.HasData("CARROOMID")) return;
                var enterPoint = BusinessManager.BizList[player.GetData<int>("CARROOMID")].EnterPoint;
                NAPI.Entity.SetEntityPosition(player, new Vector3(enterPoint.X, enterPoint.Y, enterPoint.Z + 1.5));
                Main.Players[player].ExteriorPos = new Vector3();
                Trigger.ClientEvent(player, "freeze", false);
                NAPI.Entity.SetEntityDimension(player, 0);
                Dimensions.DismissPrivateDimension(player);
                player.ResetData("CARROOMID");
                player.ResetData("LAST_HP");

                if (!player.HasData("CARROOMTEST")) Trigger.ClientEvent(player, "destroyCamera");
            }
            catch (Exception e) { Log.Write("carroomCancel: " + e.Message, nLog.Type.Error); }
        }
        #endregion
    }
}
