﻿using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using NeptuneEvo.GUI;
using NeptuneEvo.MoneySystem;
using Redage.SDK;
using System.Threading;


namespace NeptuneEvo.Core
{
    class RodManager : Script
    {

        // Как сделать чтобы вещь выпадала с большим шансом?
        // Добавляем намного больше строчек одного и тоже предмета
        // напрмер предмет Щука, добавим 10 раз[цифры менять надо], значит
        // в 10 раз будет чаще выпадать

        [ServerEvent(Event.PlayerDeath)]
        public void OnPlayerDeath(Player player, Player killer, uint reason)
        {
            BasicSync.DetachObject(player);
        }

        public static Dictionary<int, ItemType> FishItems1 = new Dictionary<int, ItemType>
        {
            {1, ItemType.Lococ },
            {2, ItemType.Okyn },
            {3, ItemType.Okyn },
            {4, ItemType.Okyn },
            {5, ItemType.Okyn },
            {6, ItemType.Ocetr },
            {7, ItemType.Ygol },
            {8, ItemType.Chyka },
            {9, ItemType.Chyka },
            {10, ItemType.Chyka },
        };

        public static Dictionary<int, ItemType> FishItems2 = new Dictionary<int, ItemType>
        {
            {1, ItemType.Koroska },
            {2, ItemType.Koroska },
            {4, ItemType.Lococ },
            {5, ItemType.Okyn },
            {6, ItemType.Okyn },
            {7, ItemType.Okyn },
            {8, ItemType.Ocetr },
            {9, ItemType.Skat },
            {10, ItemType.Skat },
            {12, ItemType.Ygol },
            {13, ItemType.Ygol },
            {15, ItemType.Chyka },
            {16, ItemType.Chyka },
            {17, ItemType.Chyka },
        };

        public static Dictionary<int, ItemType> FishItems3 = new Dictionary<int, ItemType>
        {
            {1, ItemType.Koroska },
            {2, ItemType.Kyndja },
            {3, ItemType.Lococ },
            {4, ItemType.Okyn },
            {5, ItemType.Okyn },
            {6, ItemType.Ocetr },
            {7, ItemType.Skat },
            {8, ItemType.Tunec },
            {9, ItemType.Ygol },
            {10, ItemType.Amyr },
            {11, ItemType.Chyka },
            {12, ItemType.Chyka },
        };

        public static Dictionary<int, ItemType> TypeRod = new Dictionary<int, ItemType>
        {
            {1, ItemType.Rod },
            {2, ItemType.RodUpgrade },
            {3, ItemType.RodMK2 },
        };

        public static ItemType GetSellingItemType(string name)
        {
            var type = ItemType.Naz;
            switch (name)
            {
                case "Корюшка":
                    type = ItemType.Koroska;
                    break;
                case "Кунджа":
                    type = ItemType.Kyndja;
                    break;
                case "Лосось":
                    type = ItemType.Lococ;
                    break;
                case "Окунь":
                    type = ItemType.Okyn;
                    break;
                case "Осётр":
                    type = ItemType.Ocetr;
                    break;
                case "Скат":
                    type = ItemType.Skat;
                    break;
                case "Тунец":
                    type = ItemType.Tunec;
                    break;
                case "Угорь":
                    type = ItemType.Ygol;
                    break;
                case "Чёрный амур":
                    type = ItemType.Amyr;
                    break;
                case "Щука":
                    type = ItemType.Chyka;
                    break;
            }
            return type;
        }

        public static string GetNameByItemType(ItemType tupe)
        {
            string type = "nope";
            switch (tupe)
            {
                case ItemType.Koroska:
                    type = "Корюшка";
                    break;
                case ItemType.Kyndja:
                    type = "Кунджа";
                    break;
                case ItemType.Lococ:
                    type = "Лосось";
                    break;
                case ItemType.Okyn:
                    type = "Окунь";
                    break;
                case ItemType.Ocetr:
                    type = "Осётр";
                    break;
                case ItemType.Skat:
                    type = "Скат";
                    break;
                case ItemType.Tunec:
                    type = "Тунец";
                    break;
                case ItemType.Ygol:
                    type = "Угорь";
                    break;
                case ItemType.Amyr:
                    type = "Чёрный амур";
                    break;
                case ItemType.Chyka:
                    type = "Щука";
                    break;
            }

            return type;
        }

        public static void OpenBizSellShopMenu(Player player)
        {
            Business biz = BusinessManager.BizList[player.GetData<int>("BIZ_ID")];
            List<List<string>> items = new List<List<string>>();

            foreach (var p in biz.Products)
            {
                List<string> item = new List<string>();
                item.Add(p.Name);
                item.Add($"{p.Price * Main.pluscost}$");
                items.Add(item);
            }
            string json = JsonConvert.SerializeObject(items);
            Trigger.ClientEvent(player, "fishshop", json);
        }

        private static nLog Log = new nLog("RodManager");

        private static int lastRodID = -1;

        [ServerEvent(Event.ResourceStart)]

        public void onResourceStart()
        {
            try
            {
                var result = MySQL.QueryRead($"SELECT * FROM rodings");
                if (result == null || result.Rows.Count == 0)
                {
                    Log.Write("DB rod return null result.", nLog.Type.Warn);
                    return;
                }
                foreach (DataRow Row in result.Rows)
                {
                    Vector3 pos = JsonConvert.DeserializeObject<Vector3>(Row["pos"].ToString());

                    Roding data = new Roding(Convert.ToInt32(Row["id"]), pos, Convert.ToInt32(Row["radius"]));
                    int id = Convert.ToInt32(Row["id"]);
                    lastRodID = id;
                }
            }
            catch (Exception e)
            {
                Log.Write("EXCEPTION AT \"RODINGS\":\n" + e.ToString(), nLog.Type.Error);
            }
        }


        public static void createRodAreaCommand(Player player, float radius)
        {
            if (!Group.CanUseCmd(player, "createbusiness")) return;

            var pos = player.Position;
            pos.Z -= 1.12F;

            ++lastRodID;
            Roding biz = new Roding(lastRodID, pos, radius);

            MySQL.Query($"INSERT INTO rodings (id, pos, radius) " + $"VALUES ({lastRodID}, '{JsonConvert.SerializeObject(pos)}', {radius})");

        }

        public enum AnimationFlags
        {
            Loop = 1 << 0,
            StopOnLastFrame = 1 << 1,
            OnlyAnimateUpperBody = 1 << 4,
            AllowPlayerControl = 1 << 5,
            Cancellable = 1 << 7
        }

        public static void setallow(Player player)
        {
            player.SetData("FISHING", true);
            Main.OnAntiAnim(player);
            player.PlayAnimation("amb@world_human_stand_fishing@base", "base", 31);
            BasicSync.AttachObjectToPlayer(player, NAPI.Util.GetHashKey("prop_fishing_rod_01"), 60309, new Vector3(0.03, 0, 0.02), new Vector3(0, 0, 50));
            NAPI.Task.Run(() => {
                try
                {
                    if (player != null && Main.Players.ContainsKey(player))
                    {
                        RodManager.allowfish(player);
                    }
                }
                catch { }
            }, 18000);
        }

        public static void allowfish(Player player)
        {
            player.PlayAnimation("amb@world_human_stand_fishing@idle_a", "idle_c", 31);
            Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"Что-то клюнуло", 1000);

            player.TriggerEvent("fishingBaitTaken");

        }

        public static void crashpros(Player player)
        {
            player.StopAnimation();
            Main.OffAntiAnim(player);
            BasicSync.DetachObject(player);
            player.SetData("FISHING", false);
        }

        [RemoteEvent("giveRandomFish")]
        public static void giveRandomFish(Player player)
        {
            var tryAdd = nInventory.TryAdd(player, new nItem(ItemType.Ocetr));
            if (tryAdd == -1 || tryAdd > 0)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Недостаточно места в инвентаре", 3000);
                RodManager.crashpros(player);
                return;
            }
            if (tryAdd == -2)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Ваш инвентарь не может вместить больше {nInventory.maxPlayerWeight} кг", 3000);
                RodManager.crashpros(player);
                return;
            }
            if (player.GetData<int>("FISHLEVEL") == 1)
            {
                var rnd = new Random();
                int fishco = rnd.Next(1, RodManager.FishItems1.Count);
                nInventory.Add(player, new nItem(RodManager.FishItems1[fishco], 1));
                Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"Вы поймали рыбу {GetNameByItemType(RodManager.FishItems1[fishco])}", 3000);
            }
            if (player.GetData<int>("FISHLEVEL") == 2)
            {
                var rnd = new Random();
                int fishco = rnd.Next(1, RodManager.FishItems2.Count);
                nInventory.Add(player, new nItem(RodManager.FishItems2[fishco], 1));
                Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"Вы поймали рыбу {GetNameByItemType(RodManager.FishItems2[fishco])}", 3000);
            }
            if (player.GetData<int>("FISHLEVEL") == 3)
            {
                var rnd = new Random();
                int fishco = rnd.Next(1, RodManager.FishItems3.Count);
                nInventory.Add(player, new nItem(RodManager.FishItems3[fishco], 1));
                Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"Вы поймали рыбу {GetNameByItemType(RodManager.FishItems3[fishco])}", 3000);
            }
            RodManager.crashpros(player);
        }

        [RemoteEvent("stopFishDrop")]
        public static void stopFishDrop(Player player)
        {
            Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"Рыба сошла с крючка!", 3000);
            RodManager.crashpros(player);
        }

        public static void useInventory(Player player, int level)
        {
            nInventory.Add(player, new nItem(TypeRod[level], 1));
            if (player.IsInVehicle)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Вы не должны находится в машине!", 3000);
                GUI.Dashboard.Close(player);
                return;
            }
            if (player.GetData<bool>("FISHING") == true)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Вы уже рыбачите!", 3000);
                return;
            }
            var aItem = nInventory.Find(Main.Players[player].UUID, ItemType.Naz);
            if (aItem == null)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "У Вас нет наживки", 3000);
                return;
            }
            if (player.GetData<bool>("ALLOWFISHING") == false || player.GetData<bool>("ALLOWFISHING") == false)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "В данном месте нельзя рыбачить", 3000);
                return;
            }
            var rndf = new Random();
            nInventory.Remove(player, ItemType.Naz, 1);
            player.SetData("FISHLEVEL", level);
            RodManager.setallow(player);
            Commands.RPChat("me", player, $"Начал(а) рыбачить");
        }

        public class Roding
        {
            public int ID { get; set; }
            public float Radius { get; set; }
            public Vector3 AreaPoint { get; set; }

            [JsonIgnore]
            private Blip blip = null;
            [JsonIgnore]
            private ColShape shape = null;
            //[JsonIgnore]
            //private TextLabel label = null;
            //[JsonIgnore]
            //private Marker marker = null;

            public Roding(int id, Vector3 areapoint, float radius)
            {
                // var
                ID = id;
                AreaPoint = areapoint;
                Radius = radius;


                // Create blip
                blip = NAPI.Blip.CreateBlip(68, AreaPoint, 1, 67, "Место для рыбалки", 255, 0, true);

                //Create shape
                shape = NAPI.ColShape.CreateCylinderColShape(AreaPoint, Radius, 3, 0);


                //Shape rules
                shape.OnEntityEnterColShape += (s, entity) =>
                {
                    try
                    {
                        entity.SetData("ALLOWFISHING", true);
                        //Debug
                        //Log.Write("Player enter in ColShape.", nLog.Type.Info);
                    }
                    catch (Exception e) { Console.WriteLine("shape.OnEntityEnterColshape: " + e.Message); }
                };
                shape.OnEntityExitColShape += (s, entity) =>
                {
                    try
                    {
                        //Set Data
                        entity.SetData("ALLOWFISHING", false);
                        //Debug
                        //Log.Write("Player exit in ColShape.", nLog.Type.Info);
                        //Stop Animation
                        //NAPI.Player.StopPlayerAnimation(entity);
                        //Remove object from left hand
                        //BasicSync.DetachObject(entity);
                    }
                    catch (Exception e) { Console.WriteLine("shape.OnEntityEnterColshape: " + e.Message); }
                };

                //Debug place
                //label = NAPI.TextLabel.CreateTextLabel("Место ловли рыбы", new Vector3(AreaPoint.X, AreaPoint.Y, AreaPoint.Z + 1.5), 20F, 0.5F, 0, new Color(255, 255, 255), true, 0);

                //Create marker
                //marker = NAPI.Marker.CreateMarker(1, AreaPoint - new Vector3(0, 0, 1f - 0.3f), new Vector3(), new Vector3(), Radius, new Color(255, 255, 255, 220), false, 0);

            }

        }



    }
}
