using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using NeptuneEvo.Core;
using Redage.SDK;
using NeptuneEvo.MoneySystem;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace NeptuneEvo.GUI
{
    class Dashboard : Script
    {
        public static void Event_OnPlayerDisconnected(Player player, DisconnectionType type, string reason)
        {
            try
            {
                if (!isopen.ContainsKey(player)) return;
                isopen.Remove(player);
            }
            catch (Exception e) { Log.Write("PlayerDisconnected: " + e.Message, nLog.Type.Error); }
        }

        private static nLog Log = new nLog("Dashboard");
        public static Dictionary<Player, bool> isopen = new Dictionary<Player, bool>();
        private static Dictionary<int, string> Status = new Dictionary<int, string>
        {// Group id, Status
            {0, "Игрок" },
            {16, "Администратор" }
        };
        private static Dictionary<int, string> Gender = new Dictionary<int, string>
        {// Group id, Status
            {0, "Женский" },
            {1, "Мужской" }
        };

        [RemoteEvent("removeClothes")]
        public static void RemoveClothes(Player player, int ind, int outIndex)
        {
            if (!Main.Players.ContainsKey(player))
                return;

            int UUID = Main.Players[player].UUID;


            if ((player.GetData<bool>("ON_DUTY") && Fractions.Manager.FractionTypes[Main.Players[player].FractionID] == 2 && Main.Players[player].FractionID != 9) || player.GetData<bool>("ON_WORK"))
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Вы не можете использовать это сейчас", 3000);
                GUI.Dashboard.Close(player);
                GUI.Dashboard.sendItems(player);
                return;
            }

            if (nInventory.Items[UUID][outIndex].Type != ItemType.Debug)
            {
                GUI.Dashboard.ClothesUpdate(player, nInventory.ActiveClothes[UUID][ind], ind);
                return;
            }

            if (ind == 14 && outIndex != -1)
            {
                nItem it = new nItem(nInventory.ActiveClothes[UUID][ind].Type, 1, nInventory.ActiveClothes[UUID][ind].Data, true);
                nInventory.Add(player, new nItem(nInventory.ActiveClothes[UUID][ind].Type, 1, nInventory.ActiveClothes[UUID][ind].Data, true), toIndex: outIndex);
                nInventory.ActiveClothes[UUID][ind] = new nItem(ItemType.Debug, 1);

                GUI.Dashboard.Update(player, it, outIndex);
                GUI.Dashboard.ClothesUpdate(player, nInventory.ActiveClothes[UUID][ind], ind);

                GUI.Dashboard.sendItems(player);
                return;
            }

            if (ind < 0 && ind > 13 && outIndex != -1)
            {
                GUI.Dashboard.ClothesUpdate(player, nInventory.ActiveClothes[UUID][ind], ind);
                return;
            }

            if (!nInventory.ActiveClothes.ContainsKey(UUID))
            {
                GUI.Dashboard.ClothesUpdate(player, nInventory.ActiveClothes[UUID][ind], ind);
                return;
            }

            if (nInventory.ActiveClothes[UUID][ind].Type != ItemType.Debug && nInventory.ActiveClothes[UUID][ind].IsActive)
            {
                if (nInventory.TryAdd(player, nInventory.ActiveClothes[UUID][ind]) == 0)
                {
                    if (nInventory.Items[UUID][outIndex].Type != ItemType.Debug)
                    {
                        //GUI.Dashboard.Update(player, it, outIndex);
                        GUI.Dashboard.ClothesUpdate(player, nInventory.ActiveClothes[UUID][ind], ind);
                        return;
                    }

                    nItem it = new nItem(nInventory.ActiveClothes[UUID][ind].Type, 1, nInventory.ActiveClothes[UUID][ind].Data, true);
                    nInventory.Add(player, new nItem(nInventory.ActiveClothes[UUID][ind].Type, 1, nInventory.ActiveClothes[UUID][ind].Data, true), toIndex: outIndex);
                    nInventory.ActiveClothes[UUID][ind] = new nItem(ItemType.Debug, 1);

                    GUI.Dashboard.Update(player, it, outIndex);
                    GUI.Dashboard.ClothesUpdate(player, nInventory.ActiveClothes[UUID][ind], ind);

                    Items.onUse(player, it, outIndex);
                }
                else
                {
                    Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, "Недостаточно места в инвентаре", 3000);
                    GUI.Dashboard.ClothesUpdate(player, nInventory.ActiveClothes[UUID][ind], ind);
                }
            }
        }

        [RemoteEvent("removeWeapon")]
        public static void RemoveWeapon(Player player, int ind, int outIndex)
        {

            if (!Main.Players.ContainsKey(player))
                return;

            int UUID = Main.Players[player].UUID;

            if ((ind < 0 || ind > 2) || outIndex == -1)
            {
                GUI.Dashboard.WeaponsUpdate(player, nInventory.ActiveWeapons[UUID][ind], ind);
                return;
            }

            if (!nInventory.ActiveWeapons.ContainsKey(UUID))
            {
                GUI.Dashboard.WeaponsUpdate(player, nInventory.ActiveWeapons[UUID][ind], ind);
                return;
            }

            if (nInventory.ActiveWeapons[UUID][ind].Type != ItemType.Debug)
            {
                if (nInventory.TryAdd(player, nInventory.ActiveWeapons[UUID][ind]) == 0)
                {
                    if (nInventory.Items[UUID][outIndex].Type != ItemType.Debug)
                    {
                        GUI.Dashboard.WeaponsUpdate(player, nInventory.ActiveWeapons[UUID][ind], ind);
                        return;
                    }

                    if (nInventory.ActiveWeapons[UUID][ind].IsActive)
                    {
                        Items.onWeaponUse(player, nInventory.ActiveWeapons[UUID][ind], ind);
                    }

                    nItem it = new nItem(nInventory.ActiveWeapons[UUID][ind].Type, 1, nInventory.ActiveWeapons[UUID][ind].Data, false);
                    nInventory.Add(player, new nItem(nInventory.ActiveWeapons[UUID][ind].Type, 1, nInventory.ActiveWeapons[UUID][ind].Data, false), toIndex: outIndex);
                    nInventory.ActiveWeapons[UUID][ind] = new nItem(ItemType.Debug, 1);

                    GUI.Dashboard.WeaponsUpdate(player, nInventory.ActiveWeapons[UUID][ind], ind);
                    GUI.Dashboard.Update(player, it, outIndex);
                    sendItems(player);
                    return;
                }
                else
                {
                    Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, "Недостаточно места в инвентаре", 3000);
                    GUI.Dashboard.WeaponsUpdate(player, nInventory.ActiveWeapons[UUID][ind], ind);

                    return;
                }
            }
            else
            {
                GUI.Dashboard.WeaponsUpdate(player, nInventory.ActiveWeapons[UUID][ind], ind);
                return;
            }
        }


        [RemoteEvent("Inventory")]
        public void ClientEvent_Inventory(Player player, params object[] arguments)
        {
            try
            {
                if (player == null || !Main.Players.ContainsKey(player)) return;
                if (arguments.Length < 3) return;
                int type = Convert.ToInt32(arguments[0]);
                int index = Convert.ToInt32(arguments[1]);
                string data = Convert.ToString(arguments[2]);
                Log.Debug($"Type: {type} | Index: {index} | Data: {data}");
                Core.Character.Character acc = Main.Players[player];
                List<nItem> items;
                nItem item;
                switch (type)
                {
                    case 0:
                        {// self inventory
                            items = nInventory.Items[acc.UUID];
                            item = items[index];
                            if (data == "drop")
                            {//remove one item from player inventory
                                if (item.Type == ItemType.GasCan)
                                {
                                    GUI.Dashboard.Update(player, item, index);
                                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Возможность выкладывать канистры временно отключена", 3000);
                                    return;
                                }
                                else if (item.Type == ItemType.BagWithDrill)
                                {
                                    GUI.Dashboard.Update(player, item, index);
                                    SafeMain.dropDrillBag(player);
                                    return;
                                }
                                else if (item.Type == ItemType.BagWithMoney)
                                {
                                    GUI.Dashboard.Update(player, item, index);
                                    SafeMain.dropMoneyBag(player);
                                    return;
                                }
                                else if (nInventory.ClothesItems.Contains(item.Type))
                                {
                                    if (item.IsActive)
                                    {
                                        GUI.Dashboard.Update(player, item, index);
                                        Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Вы должны сначала снять эту одежду", 3000);
                                        sendItems(player);
                                        return;
                                    }
                                    items[index] = new nItem(ItemType.Debug, 1);
                                    Items.onDrop(player, new nItem(item.Type, 1, item.Data), null);
                                    sendItems(player);
                                    return;
                                }
                                else if (nInventory.WeaponsItems.Contains(item.Type) || nInventory.MeleeWeaponsItems.Contains(item.Type) || item.Type == ItemType.StunGun)
                                {
                                    if (item.IsActive)
                                    {
                                        GUI.Dashboard.Update(player, item, index);
                                        Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Вы должны убрать оружие из рук", 3000);
                                        return;
                                    }
                                    items[index] = new nItem(ItemType.Debug, 1);
                                    Items.onDrop(player, new nItem(item.Type, 1, item.Data), null);
                                    sendItems(player);
                                    return;
                                }
                                else if (item.Type == ItemType.CarKey)
                                {
                                    items[index] = new nItem(ItemType.Debug, 1);
                                    Items.onDrop(player, new nItem(item.Type, 1, item.Data), null);
                                    sendItems(player);
                                    return;
                                }
                                if (player.IsInVehicle)
                                {
                                    GUI.Dashboard.Update(player, item, index);
                                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Нельзя выбрасывать вещи, находясь в машине", 3000);
                                    return;
                                }

                                nInventory.Remove(player, index, item.Count);
                                Items.onDrop(player, new nItem(item.Type, item.Count, item.Data), null);
                            }
                            else if (data == "use")
                            {
                                try
                                {
                                    Log.Debug($"ItemID: {item.ID} | ItemType: {item.Type} | ItemData: {item.Data} | ItemName: {nInventory.ItemsNames[(int)item.Type]}");
                                    if (player.HasData("CHANGE_WITH"))
                                    {
                                        Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Чтобы использовать вещи, нужно закрыть обмен вещами", 3000);
                                        return;
                                    }
                                    Items.onUse(player, item, index);
                                    return;
                                }
                                catch (Exception e)
                                {
                                    Log.Write(e.ToString(), nLog.Type.Error);
                                }
                            }
                            else if (data == "use_clothes")
                            {
                                try
                                {
                                    int toIndex = Math.Abs(item.ID) -1; // (Convert.ToInt32(arguments[3]) * -1) - 1;
                                    //Log.Write($"{toIndex} {(int)item.Type}");
                                    Log.Debug($"ItemID: {item.ID} | ItemType: {item.Type} | ItemData: {item.Data} | ItemName: {nInventory.ItemsNames[(int)item.Type]}");
                                    if (player.HasData("TRADE_WITH"))
                                    {
                                        Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Чтобы использовать вещи, нужно закрыть обмен вещами", 3000);
                                        GUI.Dashboard.Update(player, item, index);
                                        return;
                                    }
                                    if (!nInventory.ClothesItems.Contains(item.Type))
                                    {
                                        Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Незя", 3000);
                                        GUI.Dashboard.Update(player, item, index);
                                        return;
                                    }

                                    if (nInventory.ActiveClothes[Main.Players[player].UUID][Math.Abs(item.ID) - 1].Type != ItemType.Debug)
                                    {
                                        Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Сначала снимите то, что одето на вас", 3000);
                                        GUI.Dashboard.Update(player, item, index);
                                        return;
                                    }

                                    if ((int)item.Type != toIndex)
                                    {
                                        Items.onUse(player, item, index);
                                        //GUI.Dashboard.Update(player, item, index);
                                        sendItems(player);
                                        return;
                                    }
                                    Items.onUse(player, item, index);
                                    return;
                                }
                                catch (Exception e)
                                {
                                    Log.Write(e.ToString(), nLog.Type.Error);
                                }
                            }
                            else if (data == "use_weapon")
                            {
                                try
                                {
                                    int toIndex = Convert.ToInt32(arguments[3]);
                                    //Log.Write($"{toIndex} {(int)item.Type}");
                                    Log.Debug($"ItemID: {item.ID} | ItemType: {item.Type} | ItemData: {item.Data} | ItemName: {nInventory.ItemsNames[(int)item.Type]}");
                                    if (player.HasData("TRADE_WITH"))
                                    {
                                        Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Чтобы использовать вещи, нужно закрыть обмен вещами", 3000);
                                        return;
                                    }
                                    if (!nInventory.WeaponsItems.Contains(item.Type) && !nInventory.MeleeWeaponsItems.Contains(item.Type))
                                    {
                                        Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Это не оружие", 3000);
                                        GUI.Dashboard.Update(player, item, index);
                                        GUI.Dashboard.sendItems(player);
                                        return;
                                    }
                                    if (nInventory.ActiveWeapons[Main.Players[player].UUID][toIndex].Type != ItemType.Debug)
                                    {
                                        Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Тут уже есть оружие", 3000);
                                        GUI.Dashboard.Update(player, item, index);
                                        GUI.Dashboard.sendItems(player);
                                        return;
                                    }
                                    Items.onUse(player, item, index, toIndex);
                                    return;
                                }
                                catch (Exception e)
                                {
                                    Log.Write(e.ToString(), nLog.Type.Error);
                                }
                            }
                            else if (data == "transfer")
                            {
                                if (!player.HasData("OPENOUT_TYPE")) return;
                                if (nInventory.ClothesItems.Contains(item.Type) && item.IsActive == true)
                                {
                                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Вы должны сначала снять эту одежду", 3000);
                                    return;
                                }
                                else if ((nInventory.WeaponsItems.Contains(item.Type) || nInventory.MeleeWeaponsItems.Contains(item.Type)) && item.IsActive == true)
                                {
                                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Вы должны убрать оружие из рук", 3000);
                                    return;
                                }
                                switch (player.GetData<int>("OPENOUT_TYPE"))
                                {
                                    case 1:
                                        return;
                                    case 2:
                                        {
                                            int toIndex = Convert.ToInt32(arguments[3]);

                                            Vehicle veh = player.GetData<Vehicle>("SELECTEDVEH");
                                            if (veh is null) return;
                                            if (veh.Dimension != player.Dimension)
                                            {
                                                Commands.SendToAdmins(3, $"!{{#d35400}}[CAR-INVENTORY-EXPLOIT] {player.Name} ({player.Value}) dimension");
                                                return;
                                            }
                                            if (veh.Position.DistanceTo(player.Position) > 10f)
                                            {
                                                Commands.SendToAdmins(3, $"!{{#d35400}}[CAR-INVENTORY-EXPLOIT] {player.Name} ({player.Value}) distance");
                                                return;
                                            }

                                            int tryAdd = VehicleInventory.TryAdd(veh, new nItem(item.Type, item.Count));
                                            if (tryAdd == -1 || tryAdd > 0)
                                            {
                                                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "В машине недостаточно места", 3000);
                                                return;
                                            }
                                            if (tryAdd == -2)
                                            {
                                                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "В машине недостаточно места (вес)", 3000);
                                                return;
                                            }

                                            if (item.Type == ItemType.BagWithDrill)
                                            {
                                                player.SetClothes(5, 0, 0);
                                                player.ResetData("HEIST_DRILL");
                                            }
                                            else if (item.Type == ItemType.BagWithMoney)
                                            {
                                                player.SetClothes(5, 0, 0);
                                                player.ResetData("HAND_MONEY");
                                            }

                                            /*if (item.Count > 1)
                                            {
                                                Close(player);
                                                player.SetData("ITEMTYPE", item.Type);
                                                player.SetData("ITEMINDEX", index);
                                                Trigger.ClientEvent(player, "openInput", "Переложить предмет", "Введите количество", 3, "item_transfer_toveh");
                                                return;
                                            }*/
                                            if (item.Type == ItemType.Material)
                                            {
                                                int maxMats = (Fractions.Stocks.maxMats.ContainsKey(veh.DisplayName)) ? Fractions.Stocks.maxMats[veh.DisplayName] : 600;
                                                if (VehicleInventory.GetCountOfType(veh, ItemType.Material) + 1 > maxMats)
                                                {
                                                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Невозможно загрузить такое кол-во матов", 3000);
                                                    return;
                                                }
                                            }
                                            if (veh.GetData<List<nItem>>("ITEMS")[toIndex].Type != ItemType.Debug)
                                            {
                                                nInventory.SwapFromVehicle(player, veh, toIndex, index);
                                                return;
                                            }

                                            VehicleInventory.Add(veh, new nItem(item.Type, item.Count, item.Data), toIndex);
                                            nInventory.Remove(player, item);
                                            GameLog.Items($"player({Main.Players[player].UUID})", $"vehicle({veh.NumberPlate})", Convert.ToInt32(item.Type), 1, $"{item.Data}");
                                            return;
                                        }
                                    case 3:
                                        {
                                            if (item.Type == ItemType.BagWithDrill || item.Type == ItemType.BagWithMoney || item.Type == ItemType.CarKey || item.Type == ItemType.KeyRing || nInventory.ClothesItems.Contains(item.Type) || nInventory.WeaponsItems.Contains(item.Type))
                                            {
                                                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Эта вещь не предназначена для этого шкафа", 3000);
                                                return;
                                            }
                                            if (Main.Players[player].InsideHouseID == -1) return;
                                            int houseID = Main.Players[player].InsideHouseID;
                                            int furnID = player.GetData<int>("OpennedSafe");
                                            if (item.Count > 1)
                                            {
                                                Close(player);
                                                player.SetData("ITEMTYPE", item.Type);
                                                player.SetData("ITEMINDEX", index);
                                                Trigger.ClientEvent(player, "openInput", "Переложить предмет", "Введите количество", 3, "item_transfer_tosafe");
                                                return;
                                            }

                                            int tryAdd = Houses.FurnitureManager.TryAdd(houseID, furnID, item);
                                            if (tryAdd == -1 || tryAdd > 0)
                                            {
                                                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Недостаточно места в инвентаре", 3000);
                                                return;
                                            }
                                            GameLog.Items($"player({Main.Players[player].UUID})", $"itemSafe({furnID} | house: {houseID})", Convert.ToInt32(item.Type), 1, $"{item.Data}");
                                            nInventory.Remove(player, item.Type, 1);
                                            sendItems(player);
                                            Houses.FurnitureManager.Add(houseID, furnID, new nItem(item.Type));
                                            return;
                                        }
                                    case 4:
                                        {
                                            if (!nInventory.ClothesItems.Contains(item.Type))
                                            {
                                                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Шкаф для одежды может хранить только одежду", 3000);
                                                return;
                                            }
                                            if (Main.Players[player].InsideHouseID == -1) return;
                                            int houseID = Main.Players[player].InsideHouseID;
                                            int furnID = player.GetData<int>("OpennedSafe");

                                            int tryAdd = Houses.FurnitureManager.TryAdd(houseID, furnID, item);
                                            if (tryAdd == -1 || tryAdd > 0)
                                            {
                                                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Недостаточно места в инвентаре", 3000);
                                                return;
                                            }
                                            GameLog.Items($"player({Main.Players[player].UUID})", $"clothSafe({furnID} | house: {houseID})", Convert.ToInt32(item.Type), 1, $"{item.Data}");
                                            nInventory.Remove(player, item);
                                            sendItems(player);
                                            Houses.FurnitureManager.Add(houseID, furnID, new nItem(item.Type, 1, item.Data));
                                            return;
                                        }
                                    case 5:
                                        {
                                            int toIndex = Convert.ToInt32(arguments[3]);

                                            Log.Write($"{toIndex}");

                                            if (toIndex < 0 || toIndex > 4)
                                                return;

                                            if (!player.HasData("TRADE_WITH"))
                                                return;

                                            if (player.GetData<bool>("TRADE_READY") == true)
                                            {
                                                GUI.Dashboard.Update(player, nInventory.Items[Main.Players[player].UUID][index], index);
                                                return;
                                            }

                                            List<nItem> tradeItems = player.GetData<List<nItem>>("TRADE_ITEMS");

                                            int tryAdd = nInventory.TryAdd(player.GetData<Player>("TRADE_WITH"), new nItem(item.Type, item.Count, item.Data, false));
                                            if (tryAdd == -1 || tryAdd > 0)
                                            {
                                                Trade.TradeToUpdate(player, tradeItems[toIndex], toIndex);
                                                GUI.Dashboard.Update(player, nInventory.Items[Main.Players[player].UUID][index], index);
                                                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Недостаточно места в инвентаре игрока", 3000);
                                                return;
                                            }
                                            else if (tryAdd == -2)
                                            {
                                                Trade.TradeToUpdate(player, tradeItems[toIndex], toIndex);
                                                GUI.Dashboard.Update(player, nInventory.Items[Main.Players[player].UUID][index], index);
                                                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Недостаточно места в инвентаре игрока (вес)", 3000);
                                                return;
                                            }

                                            if (tradeItems[toIndex].Type == ItemType.Debug)
                                            {
                                                tradeItems[toIndex] = new nItem(item.Type, item.Count, item.Data, false);
                                                nInventory.Remove(player, index, item.Count);
                                            }
                                            else
                                            {
                                                nItem tempItem = (nItem)tradeItems[toIndex].Clone();
                                                tradeItems[toIndex] = (nItem)nInventory.Items[Main.Players[player].UUID][index].Clone();
                                                nInventory.Items[Main.Players[player].UUID][index] = tempItem;
                                                Update(player, nInventory.Items[Main.Players[player].UUID][index], index);
                                            }

                                            player.SetData("TRADE_ITEMS", tradeItems);

                                            Trade.TradeToUpdate(player, tradeItems[toIndex], toIndex);
                                            Trade.TradeFromUpdate(player.GetData<Player>("TRADE_WITH"), tradeItems[toIndex], toIndex);

                                            return;
                                        }
                                    case 6:
                                        {
                                            int toIndex = Convert.ToInt32(arguments[3]);
                                            if (!nInventory.WeaponsItems.Contains(item.Type) && !nInventory.AmmoItems.Contains(item.Type) && item.Type != ItemType.StunGun && item.Type != ItemType.Nightstick)
                                            {
                                                sendItems(player);
                                                return;
                                            }
                                            int onFraction = player.GetData<int>("ONFRACSTOCK");

                                            if (onFraction == 0) return;

                                            if (Fractions.Stocks.TryAdd(onFraction, new nItem(item.Type, item.Count)) != 0)
                                            {
                                                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "На складе недостаточно места", 3000);
                                                return;
                                            }

                                            /*if (item.Count > 1)
                                            {
                                                Close(player, true);
                                                player.SetData("ITEMTYPE", item.Type);
                                                player.SetData("ITEMINDEX", index);
                                                Trigger.ClientEvent(player, "openInput", "Передать предмет", "Введите количество", 3, "item_transfer_tofracstock");
                                                return;
                                            }*/
                                            if (Fractions.Stocks.fracStocks[onFraction].Weapons[toIndex].Type != ItemType.Debug)
                                            {
                                                sendItems(player);
                                                return;
                                            }

                                            string serial = (nInventory.WeaponsItems.Contains(item.Type)) ? $"({(string)item.Data})" : "";
                                            GameLog.Stock(Main.Players[player].FractionID, Main.Players[player].UUID, $"{nInventory.ItemsNames[(int)item.Type]}{serial}", 1, false);
                                            Fractions.Stocks.Add(onFraction, item, toIndex);
                                            nInventory.Remove(player, index);
                                            GameLog.Items($"player({Main.Players[player].UUID})", $"fracstock({onFraction})", Convert.ToInt32(item.Type), 1, $"{item.Data}");
                                            sendItems(player);
                                            return;
                                        }
                                    case 7:
                                        {
                                            nItem keyring = nInventory.Items[Main.Players[player].UUID][player.GetData<int>("KEYRING")];
                                            string keysData = Convert.ToString(keyring.Data);
                                            List<string> keys = (keysData.Length == 0) ? new List<string>() : new List<string>(keysData.Split('/'));
                                            if (keys.Count > 0 && string.IsNullOrEmpty(keys[keys.Count - 1]))
                                                keys.RemoveAt(keys.Count - 1);

                                            if (keys.Count >= 5)
                                            {
                                                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Максимум 5 ключей", 3000);
                                                return;
                                            }

                                            if (item.Type != ItemType.CarKey)
                                            {
                                                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Применимо только для ключей", 3000);
                                                return;
                                            }

                                            keys.Add(item.Data);
                                            keysData = "";
                                            foreach (string key in keys)
                                                keysData += $"{key}/";
                                            keyring.Data = keysData; // ¯\_(ツ)_/¯
                                            nInventory.Items[Main.Players[player].UUID][player.GetData<int>("KEYRING")] = keyring;

                                            nInventory.Remove(player, item);

                                            List<nItem> keyringItems = new List<nItem>();
                                            foreach (string key in keys)
                                                keyringItems.Add(new nItem(ItemType.CarKey, 1, key));

                                            player.SetData("KEYRING", nInventory.Items[Main.Players[player].UUID].IndexOf(keyring));
                                            OpenOut(player, keyringItems, "Связка ключей", 7);
                                            return;
                                        }
                                    case 8: // Оружейный сейф
                                        {
                                            if (!nInventory.WeaponsItems.Contains(item.Type) && !nInventory.MeleeWeaponsItems.Contains(item.Type))
                                            {
                                                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Оружейный сейф может хранить только оружие", 3000);
                                                return;
                                            }
                                            if (Main.Players[player].InsideHouseID == -1) return;
                                            int houseID = Main.Players[player].InsideHouseID;
                                            int furnID = player.GetData<int>("OpennedSafe");

                                            int tryAdd = Houses.FurnitureManager.TryAdd(houseID, furnID, item);
                                            if (tryAdd == -1 || tryAdd > 0)
                                            {
                                                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Недостаточно места в инвентаре", 3000);
                                                return;
                                            }
                                            GameLog.Items($"player({Main.Players[player].UUID})", $"weapSafe({furnID} | house: {houseID})", Convert.ToInt32(item.Type), 1, $"{item.Data}");
                                            nInventory.Remove(player, item);
                                            sendItems(player);
                                            Houses.FurnitureManager.Add(houseID, furnID, new nItem(item.Type, 1, item.Data));
                                            return;
                                        }
                                }
                                Items.onTransfer(player, item, null);
                                Close(player);
                                return;
                            }
                            else if (data == "swap")
                            {
                                try
                                {
                                    
                                    int toIndex = Convert.ToInt32(arguments[3]);

                                    if (index != -1 && toIndex != -1)
                                    {
                                        nInventory.Swap(player, index, toIndex);
                                        GameLog.Items($"player({Main.Players[player].UUID})", "swap", index, toIndex, "");
                                        sendItems(player);
                                    }

                                }
                                catch (Exception e)
                                {
                                    Log.Debug(e.Message);
                                }
                            }
                            else if (data == "split")
                            {
                                try
                                {
                                    int amount = Convert.ToInt32(arguments[3]);

                                    if (amount <= 0)
                                    {
                                        Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Значение должно быть больше 0", 3000);
                                        GUI.Dashboard.Update(player, item, index);
                                        return;
                                    }

                                    if (item.Count <= amount)
                                    {
                                        Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "У вас нет столько предметов", 3000);
                                        GUI.Dashboard.Update(player, item, index);
                                        return;
                                    }

                                    if (nInventory.WeaponsItems.Contains(item.Type) || nInventory.MeleeWeaponsItems.Contains(item.Type) || item.Type == ItemType.StunGun)
                                    {
                                        Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Вы должны убрать оружие из рук", 3000);
                                        GUI.Dashboard.Update(player, item, index);
                                        return;
                                    }

                                    if (item.IsActive == true)
                                    {
                                        Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Предмет используется", 3000);
                                        GUI.Dashboard.Update(player, item, index);
                                        return;
                                    }

                                    nInventory.Split(player, index, amount);
                                    GameLog.Items($"player({Main.Players[player].UUID})", "split", Convert.ToInt32(item.Type), amount, $"{item.Data}");
                                }
                                catch (Exception e)
                                {
                                    Log.Write("Split: " + e.ToString(), nLog.Type.Error);
                                }
                            }
                            else if (data == "stack")
                            {

                                try
                                {
                                    int toStack = Convert.ToInt32(arguments[3]);
                                    //Log.Write($"{index} {toStack}");
                                    var toItem = items[toStack];

                                    if (toStack < 0)
                                    {
                                        Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Значение должно быть больше 0", 3000);
                                        GUI.Dashboard.Update(player, item, index);
                                        return;
                                    }

                                    if (toItem.Type != item.Type)
                                    {
                                        Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Два разных объекта", 3000);
                                        GUI.Dashboard.Update(player, item, index);
                                        sendItems(player);
                                        return;
                                    }

                                    if (!nInventory.ItemsStacks.ContainsKey(item.Type))
                                    {
                                        Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Эти объекты не стакаются", 3000);
                                        GUI.Dashboard.Update(player, item, index);
                                        sendItems(player);
                                        return;
                                    }

                                    nInventory.Stack(player, index, toStack);
                                    GameLog.Items($"player({Main.Players[player].UUID})", "stack", Convert.ToInt32(item.Type), toStack, $"{item.Data}");
                                }
                                catch (Exception e)
                                {
                                    Log.Write("Split: " + e.ToString(), nLog.Type.Error);
                                }

                            }
                            break;
                        }
                    case 1:
                        { // droped items
                          //TODO
                            break;
                        }
                    case 2:
                        { // in car items
                            Vehicle veh = player.GetData<Vehicle>("SELECTEDVEH");

                            if (veh is null) return;
                            if (veh.Dimension != player.Dimension)
                            {
                                Commands.SendToAdmins(3, $"!{{#d35400}}[CAR-INVENTORY-EXPLOIT] {player.Name} ({player.Value}) dimension");
                                return;
                            }
                            if (veh.Position.DistanceTo(player.Position) > 10f)
                            {
                                Commands.SendToAdmins(3, $"!{{#d35400}}[CAR-INVENTORY-EXPLOIT] {player.Name} ({player.Value}) distance");
                                return;
                            }

                            items = veh.GetData<List<nItem>>("ITEMS");
                            item = items[index];

                            if (data == "split")
                            {

                                int amount = Convert.ToInt32(arguments[3]);

                                if (amount <= 0)
                                {
                                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Значение должно быть больше 0", 3000);
                                    GUI.Dashboard.OpenOut(player, veh.GetData<List<nItem>>("ITEMS"), "Багажник", 2);
                                    return;
                                }

                                if (item.Count <= amount)
                                {
                                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Нет столько предметов", 3000);
                                    GUI.Dashboard.OpenOut(player, veh.GetData<List<nItem>>("ITEMS"), "Багажник", 2);
                                    return;
                                }

                                /*if (nInventory.WeaponsItems.Contains(item.Type) || nInventory.MeleeWeaponsItems.Contains(item.Type) || item.Type == ItemType.StunGun)
                                {
                                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Вы должны убрать оружие из рук", 3000);
                                    return;
                                }*/

                                /*if (item.IsActive == true)
                                {
                                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Предмет используется", 3000);
                                    return;
                                }*/

                                VehicleInventory.Split(veh, index, amount);
                                GUI.Dashboard.OpenOut(player, veh.GetData<List<nItem>>("ITEMS"), "Багажник", 2);
                                //GameLog.Items($"player({Main.Players[player].UUID})", "split", Convert.ToInt32(item.Type), amount, $"{item.Data}");
                            }
                            else if (data == "stack")
                            {
                                try
                                {
                                    int toStack = Convert.ToInt32(arguments[3]);
                                    var toItem = items[toStack];

                                    if (toStack < 0)
                                    {
                                        Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Значение должно быть больше 0", 3000);
                                        GUI.Dashboard.OpenOut(player, veh.GetData<List<nItem>>("ITEMS"), "Багажник", 2);

                                        return;
                                    }

                                    if (toItem.Type != item.Type)
                                    {
                                        Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Два разных объекта", 3000);

                                        GUI.Dashboard.OpenOut(player, veh.GetData<List<nItem>>("ITEMS"), "Багажник", 2);

                                        return;
                                    }

                                    if (!nInventory.ItemsStacks.ContainsKey(item.Type))
                                    {
                                        Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Эти объекты не стакаются", 3000);
                                        GUI.Dashboard.OpenOut(player, veh.GetData<List<nItem>>("ITEMS"), "Багажник", 2);

                                        return;
                                    }

                                    VehicleInventory.Stack(veh, index, toStack);
                                    GUI.Dashboard.OpenOut(player, veh.GetData<List<nItem>>("ITEMS"), "Багажник", 2);
                                    GameLog.Items($"player({Main.Players[player].UUID})", "stack", Convert.ToInt32(item.Type), toStack, $"{item.Data}");
                                }
                                catch (Exception e)
                                {
                                    Log.Write("Split: " + e.ToString(), nLog.Type.Error);
                                }
                            }
                            else if (data == "swap")
                            {
                                try
                                {
                                    int toIndex = Convert.ToInt32(arguments[3]);

                                    if (index != -1 && toIndex != -1)
                                    {
                                        VehicleInventory.Swap(veh, index, toIndex);
                                        GameLog.Items($"vehicle({Main.Players[player].UUID})", "swap", index, toIndex, "");
                                    }

                                }
                                catch (Exception e)
                                {
                                    Log.Debug(e.Message);
                                }
                            }
                            else if (data == "take")
                            {
                                //Log.Write("");
                                int toIndex = Convert.ToInt32(arguments[3]);
                                List<nItem> vehItems = veh.GetData<List<nItem>>("ITEMS");

                                int tryAdd = nInventory.TryAdd(player, new nItem(item.Type, item.Count));//add 
                                if (tryAdd == -1 || tryAdd > 0)
                                {
                                    GUI.Dashboard.OpenOut(player, veh.GetData<List<nItem>>("ITEMS"), "Багажник", 2);
                                    GUI.Dashboard.Update(player, nInventory.Items[Main.Players[player].UUID][toIndex], toIndex);
                                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Недостаточно места в инвентаре", 3000);
                                    return;
                                }
                                else if (tryAdd == -2)
                                {
                                    GUI.Dashboard.OpenOut(player, veh.GetData<List<nItem>>("ITEMS"), "Багажник", 2);
                                    GUI.Dashboard.Update(player, nInventory.Items[Main.Players[player].UUID][toIndex], toIndex);
                                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Недостаточно места в инвентаре (вес)", 3000);
                                    return;
                                }

                                if (item.Type == ItemType.BodyArmor && nInventory.Find(Main.Players[player].UUID, ItemType.BodyArmor) != null)
                                {
                                    GUI.Dashboard.OpenOut(player, veh.GetData<List<nItem>>("ITEMS"), "Багажник", 2);
                                    GUI.Dashboard.Update(player, nInventory.Items[Main.Players[player].UUID][toIndex], toIndex);
                                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Недостаточно места в инвентаре", 3000);
                                    return;
                                }

                                if (item.Type == ItemType.BagWithDrill)
                                {
                                    if (player.HasData("HEIST_DRILL") || player.HasData("HAND_MONEY"))
                                    {
                                        GUI.Dashboard.OpenOut(player, veh.GetData<List<nItem>>("ITEMS"), "Багажник", 2);
                                        GUI.Dashboard.Update(player, nInventory.Items[Main.Players[player].UUID][toIndex], toIndex);
                                        Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"У Вас уже есть дрель или деньги в руках", 3000);
                                        return;
                                    }

                                    player.SetClothes(5, 41, 0);
                                    player.SetData("HEIST_DRILL", true);
                                }
                                else if (item.Type == ItemType.BagWithMoney)
                                {
                                    if (player.HasData("HEIST_DRILL") || NAPI.Data.HasEntityData(player, "HAND_MONEY"))
                                    {
                                        GUI.Dashboard.OpenOut(player, veh.GetData<List<nItem>>("ITEMS"), "Багажник", 2);
                                        GUI.Dashboard.Update(player, nInventory.Items[Main.Players[player].UUID][toIndex], toIndex);
                                        Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"У Вас уже есть сумка", 3000);
                                        return;
                                    }

                                    player.SetClothes(5, 45, 0);
                                    player.SetData("HAND_MONEY", true);
                                }

                                /*if (item.Count > 1)
                                {
                                    Close(player);
                                    player.SetData("ITEMTYPE", item.Type);
                                    player.SetData("ITEMINDEX", index);
                                    Trigger.PlayerEvent(player, "openInput", "Взять предмет", "Введите количество", 3, "item_transfer_fromveh");
                                    return;
                                }*/

                                if (nInventory.Items[Main.Players[player].UUID][toIndex].Type != ItemType.Debug)
                                {
                                    nInventory.SwapFromVehicle(player, veh, index, toIndex);

                                    return;
                                }

                                //player.SendChatMessage($"{index} - {toIndex}");
                                nInventory.Add(player, item, toIndex: toIndex);
                                VehicleInventory.RemoveAt(veh, index, item.Count);
                                sendItems(player);
                                GameLog.Items($"vehicle({veh.NumberPlate})", $"player({Main.Players[player].UUID})", Convert.ToInt32(item.Type), 1, $"{item.Data}");
                            }
                            break;
                        }
                    case 3: // Взять
                        {
                            if (Main.Players[player].InsideHouseID == -1) return;
                            int houseID = Main.Players[player].InsideHouseID;
                            int furnID = player.GetData<int>("OpennedSafe");
                            Houses.HouseFurniture furniture = Houses.FurnitureManager.HouseFurnitures[houseID][furnID];
                            items = Houses.FurnitureManager.FurnituresItems[houseID][furnID];
                            item = items[index];

                            int tryAdd = nInventory.TryAdd(player, new nItem(item.Type));
                            if (tryAdd == -1 || tryAdd > 0)
                            {                   
                                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Недостаточно места в инвентаре", 3000);
                                return;
                            }
                            if (tryAdd == -2)
                            {
                                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Ваш инвентарь не может вместить больше {nInventory.maxPlayerWeight} кг", 3000);
                                return;
                            }

                            if (item.Count > 1)
                            {
                                Close(player);
                                player.SetData("ITEMTYPE", item.Type);
                                player.SetData("ITEMINDEX", index);
                                Trigger.ClientEvent(player, "openInput", "Взять предмет", "Введите количество", 3, "item_transfer_fromsafe");
                                return;
                            }
                            GameLog.Items($"itemSafe({furnID} | house: {houseID})", $"player({Main.Players[player].UUID})", Convert.ToInt32(item.Type), 1, $"{item.Data}");
                            items[index] = new nItem(ItemType.Debug);
                            Houses.FurnitureManager.FurnituresItems[houseID][furnID] = items;
                            nInventory.Add(player, new nItem(item.Type, 1, item.Data));
                            sendItems(player);
                            foreach (Player p in NAPI.Pools.GetAllPlayers())
                            {
                                if (p == null || !Main.Players.ContainsKey(p)) continue;
                                if ((p.HasData("OPENOUT_TYPE") && p.GetData<int>("OPENOUT_TYPE") == 3) && (Main.Players[p].InsideHouseID != -1 && Main.Players[p].InsideHouseID == houseID) && (p.HasData("OpennedSafe") && p.GetData<int>("OpennedSafe") == furnID))
                                    GUI.Dashboard.OpenOut(p, items, furniture.Name, 3);
                            }
                            break;
                        }
                    case 4:
                        {
                            if (Main.Players[player].InsideHouseID == -1) return;
                            int houseID = Main.Players[player].InsideHouseID;
                            int furnID = player.GetData<int>("OpennedSafe");
                            Houses.HouseFurniture furniture = Houses.FurnitureManager.HouseFurnitures[houseID][furnID];
                            items = Houses.FurnitureManager.FurnituresItems[houseID][furnID];
                            item = items[index];

                            int tryAdd = nInventory.TryAdd(player, new nItem(item.Type));
                            if (tryAdd == -1 || tryAdd > 0)
                            {
                                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Недостаточно места в инвентаре", 3000);
                                return;
                            }
                            if (tryAdd == -2)
                            {
                                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Ваш инвентарь не может вместить больше {nInventory.maxPlayerWeight} кг", 3000);
                                return;
                            }

                            if (item.Type == ItemType.BodyArmor && nInventory.Find(Main.Players[player].UUID, ItemType.BodyArmor) != null)
                            {
                                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Недостаточно места в инвентаре", 3000);
                                return;
                            }
                            GameLog.Items($"clothSafe({furnID} | house: {houseID})", $"player({Main.Players[player].UUID})", Convert.ToInt32(item.Type), 1, $"{item.Data}");
                            nInventory.Add(player, item);
                            sendItems(player);

                            items[index] = new nItem(ItemType.Debug);
                            Houses.FurnitureManager.FurnituresItems[houseID][furnID] = items;
                            foreach (Player p in NAPI.Pools.GetAllPlayers())
                            {
                                if (p == null || !Main.Players.ContainsKey(p)) continue;
                                if ((p.HasData("OPENOUT_TYPE") && p.GetData<int>("OPENOUT_TYPE") == 4) && (Main.Players[p].InsideHouseID != -1 && Main.Players[p].InsideHouseID == houseID) && (p.HasData("OpennedSafe") && p.GetData<int>("OpennedSafe") == furnID))
                                    GUI.Dashboard.OpenOut(p, items, furniture.Name, 4);
                            }
                            break;
                        }
                    case 5:
                        {
                            if (!player.HasData("TRADE_WITH"))
                                return;

                            if (data == "use" || data == "drop")
                                return;

                            List<nItem> tradeItems = player.GetData<List<nItem>>("TRADE_ITEMS");
                            int toIndex = Convert.ToInt32(arguments[3]);

                            if (player.GetData<bool>("TRADE_READY") == true)
                            {
                                Trade.TradeToUpdate(player, tradeItems[index], index);
                                GUI.Dashboard.Update(player, nInventory.Items[Main.Players[player].UUID][toIndex], toIndex);
                                return;
                            }

                            item = tradeItems[index];

                            int tryAdd = nInventory.TryAdd(player, new nItem(item.Type, item.Count, item.Data, false));
                            if (tryAdd == -1 || tryAdd > 0)
                            {
                                Trade.TradeToUpdate(player, tradeItems[index], index);
                                GUI.Dashboard.Update(player, nInventory.Items[Main.Players[player].UUID][toIndex], toIndex);
                                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Недостаточно места в инвентаре", 3000);
                                return;
                            }
                            else if (tryAdd == -2)
                            {
                                Trade.TradeToUpdate(player, tradeItems[index], index);
                                GUI.Dashboard.Update(player, nInventory.Items[Main.Players[player].UUID][toIndex], toIndex);
                                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Недостаточно места в инвентаре (вес)", 3000);
                                return;
                            }

                            if (nInventory.Items[Main.Players[player].UUID][toIndex].Type == ItemType.Debug)
                            {
                                nInventory.Add(player, new nItem(item.Type, item.Count, item.Data, false), toIndex: toIndex);
                                tradeItems[index] = new nItem(ItemType.Debug, 1);
                            }
                            else
                            {
                                nItem tempItem = (nItem)nInventory.Items[Main.Players[player].UUID][toIndex].Clone();
                                nInventory.Items[Main.Players[player].UUID][toIndex] = (nItem)tradeItems[index].Clone();
                                tradeItems[index] = tempItem;
                                Update(player, nInventory.Items[Main.Players[player].UUID][toIndex], toIndex);
                            }
                            player.SetData("TRADE_ITEMS", tradeItems);

                            Trade.TradeToUpdate(player, tradeItems[index], index);
                            Trade.TradeFromUpdate(player.GetData<Player>("TRADE_WITH"), tradeItems[index], index);
                            break;
                        }
                    case 6:
                        {
                            int toIndex = Convert.ToInt32(arguments[3]);
                            if (!player.HasData("ONFRACSTOCK") || player.GetData<int>("ONFRACSTOCK") == 0) return;
                            int onFrac = player.GetData<int>("ONFRACSTOCK");
                            items = Fractions.Stocks.fracStocks[onFrac].Weapons;
                            item = items[index];
                            if (data != "take")
                            {
                                OpenOut(player, items, "Склад оружия", 6);
                                return;
                            }

                            int tryAdd = nInventory.TryAdd(player, new nItem(item.Type, 1));
                            if (tryAdd == -1 || tryAdd > 0)
                            {
                                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Недостаточно места в инвентаре", 3000);
                                return;
                            }
                            if (tryAdd == -2)
                            {
                                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Ваш инвентарь не может вместить больше {nInventory.maxPlayerWeight} кг", 3000);
                                return;
                            }

                            if (nInventory.Items[Main.Players[player].UUID][toIndex].Type != ItemType.Debug)
                            {
                                OpenOut(player, items, "Склад оружия", 6);
                                return;
                            }

                            if (item.Count > 1)
                            {
                                Close(player);
                                player.SetData("ITEMTYPE", item.Type);
                                player.SetData("ITEMINDEX", index);
                                Trigger.ClientEvent(player, "openInput", "Взять предмет", "Введите количество", 3, "item_transfer_fromfracstock");
                                return;
                            }

                            nInventory.Add(player, item, toIndex: toIndex);
                            Fractions.Stocks.Remove(onFrac, item);
                            string serial = (nInventory.WeaponsItems.Contains(item.Type)) ? $"({(string)item.Data})" : "";
                            GameLog.Stock(Main.Players[player].FractionID, Main.Players[player].UUID, $"{nInventory.ItemsNames[(int)item.Type]}{serial}", 1, true);
                            GameLog.Items($"fracstock({onFrac})", $"player({Main.Players[player].UUID})", Convert.ToInt32(item.Type), 1, $"{item.Data}");
                            break;
                        }
                    case 7:
                        { // keyring items
                            nItem keyring = nInventory.Items[Main.Players[player].UUID][player.GetData<int>("KEYRING")];
                            string keysData = Convert.ToString(keyring.Data);
                            List<string> keys = (keysData.Length == 0) ? new List<string>() : new List<string>(keysData.Split('/'));
                            if (keys.Count > 0 && keys[keys.Count - 1] == "")
                                keys.RemoveAt(keys.Count - 1);

                            item = new nItem(ItemType.CarKey, 1, keys[index]);
                            int tryAdd = nInventory.TryAdd(player, item);
                            if (tryAdd == -1 || tryAdd > 0)
                            {
                                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "У Вас недостаточно места", 3000);
                                return;
                            }
                            if (tryAdd == -2)
                            {
                                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Ваш инвентарь не может вместить больше {nInventory.maxPlayerWeight} кг", 3000);
                                return;
                            }

                            keys.RemoveAt(index);
                            nInventory.Add(player, new nItem(item.Type, 1, item.Data));

                            keysData = "";
                            foreach (string key in keys)
                                keysData += $"{key}/";
                            keyring.Data = keysData; // ¯\_(ツ)_/¯
                            nInventory.Items[Main.Players[player].UUID][player.GetData<int>("KEYRING")] = keyring;

                            List<nItem> keyringItems = new List<nItem>();
                            foreach (string key in keys)
                                keyringItems.Add(new nItem(ItemType.CarKey, 1, key));
                            OpenOut(player, keyringItems, "Связка ключей", 7);
                            break;
                        }
                    case 8: // Взять
                        {
                            if (Main.Players[player].InsideHouseID == -1) return;
                            int houseID = Main.Players[player].InsideHouseID;
                            int furnID = player.GetData<int>("OpennedSafe");
                            Houses.HouseFurniture furniture = Houses.FurnitureManager.HouseFurnitures[houseID][furnID];
                            items = Houses.FurnitureManager.FurnituresItems[houseID][furnID];
                            item = items[index];

                            int tryAdd = nInventory.TryAdd(player, new nItem(item.Type));
                            if (tryAdd == -1 || tryAdd > 0)
                            {
                                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Недостаточно места в инвентаре", 3000);
                                return;
                            }
                            if (tryAdd == -2)
                            {
                                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Ваш инвентарь не может вместить больше {nInventory.maxPlayerWeight} кг", 3000);
                                return;
                            }

                            if (item.Type == ItemType.BodyArmor && nInventory.Find(Main.Players[player].UUID, ItemType.BodyArmor) != null)
                            {
                                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Недостаточно места в инвентаре", 3000);
                                return;
                            }
                            GameLog.Items($"weapSafe({furnID} | house: {houseID})", $"player({Main.Players[player].UUID})", Convert.ToInt32(item.Type), 1, $"{item.Data}");
                            nInventory.Add(player, item);
                            sendItems(player);

                            items[index] = new nItem(ItemType.Debug);
                            Houses.FurnitureManager.FurnituresItems[houseID][furnID] = items;
                            foreach (Player p in NAPI.Pools.GetAllPlayers())
                            {
                                if (p == null || !Main.Players.ContainsKey(p)) continue;
                                if ((p.HasData("OPENOUT_TYPE") && p.GetData<int>("OPENOUT_TYPE") == 8) && (Main.Players[p].InsideHouseID != -1 && Main.Players[p].InsideHouseID == houseID) && (p.HasData("OpennedSafe") && p.GetData<int>("OpennedSafe") == furnID))
                                    GUI.Dashboard.OpenOut(p, items, furniture.Name, 8);
                            }
                            break;
                        }
                    case 20:
                        {
                            if (Main.Players[player].AdminLVL >= 6 && Main.Players[player].InsideHouseID == -1)
                            {
                                if (!player.HasData("CHANGE_WITH"))
                                {
                                    Close(player);
                                    return;
                                }
                                Player target = player.GetData<Player>("CHANGE_WITH");
                                if (!Main.Players.ContainsKey(target))
                                {
                                    Close(player);
                                    return;
                                }
                                items = nInventory.Items[Main.Players[target].UUID];
                                item = items[index];
                                if (nInventory.ClothesItems.Contains(item.Type) && item.IsActive == true)
                                {
                                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Игрок должен снять эту одежду", 3000);
                                    return;
                                }
                                else if ((nInventory.WeaponsItems.Contains(item.Type) || nInventory.MeleeWeaponsItems.Contains(item.Type)) && item.IsActive == true)
                                {
                                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Игрок должен убрать это оружие из рук", 3000);
                                    return;
                                }
                                int tryAdd1 = nInventory.TryAdd(player, new nItem(item.Type, 1));
                                if (tryAdd1 == -1 || tryAdd1 > 0)
                                {
                                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "У Вас недостаточно места", 3000);
                                    return;
                                }
                                if (tryAdd1 == -2)
                                {
                                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Ваш инвентарь не может вместить больше {nInventory.maxPlayerWeight} кг", 3000);
                                    return;
                                }

                                if (item.Type == ItemType.BodyArmor && nInventory.Find(Main.Players[player].UUID, ItemType.BodyArmor) != null)
                                {
                                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Недостаточно места в инвентаре", 3000);
                                    return;
                                }
                                if (item.Count > 1)
                                {
                                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Такие вещи нельзя забрать", 3000);
                                    return;
                                }
                                nInventory.Add(player, item);
                                nInventory.Remove(target, item);
                                Close(player);
                                Commands.CMD_showPlayerStats(player, target.Value); // reopen target inventory
                                GameLog.Admin(player.Name, $"takeItem({item.Type} | {item.Data})", target.Name);
                                return;
                            }
                            break;
                        }
                }
            }
            catch (Exception e) { Log.Write("Inventory: " + e.Message, nLog.Type.Error); }
        }

        [RemoteEvent("openInventory")]
        public void ClientEvent_openInventory(Player player, params object[] arguments)
        {
            try
            {
                if (isopen[player])
                {
                    Close(player);

                }
                else
                    Open(player);
            }
            catch (Exception e) { Log.Write("openInventory: " + e.Message, nLog.Type.Error); }
        }
        [RemoteEvent("closeInventory")]
        public void ClientEvent_closeInventory(Player player, params object[] arguments)
        {
            try
            {
                if (player.HasData("OPENOUT_TYPE") && player.GetData<int>("OPENOUT_TYPE") == 20) sendItems(player);

                if (player.HasData("SELECTEDVEH"))
                {
                    Vehicle vehicle = player.GetData<Vehicle>("SELECTEDVEH");
                    vehicle.SetData("BAGINUSE", false);
                }

                player.ResetData("OPENOUT_TYPE");

                if (player.HasData("CHANGE_WITH") && Main.Players.ContainsKey(player.GetData<Player>("CHANGE_WITH")))
                {
                    Close(player.GetData<Player>("CHANGE_WITH"));
                    NAPI.Data.ResetEntityData(player.GetData<Player>("CHANGE_WITH"), "CHANGE_WITH");
                    player.ResetData("CHANGE_WITH");
                    if (Main.Players[player].AdminLVL != 0) sendStats(player);
                }
            }
            catch (Exception e) { Log.Write($"CloseInventory: " + e.Message, nLog.Type.Error); }
        }

        public static void Close(Player player, bool resetOpenOut = false)
        {
            string data = (resetOpenOut) ? "closeBoard" : "close_Board";
            Trigger.ClientEvent(player, "board", data);
            player.ResetData("OPENOUT_TYPE");
        }
        public static void sendStats(Player player)
        {
            try
            {
                if (!Main.Players.ContainsKey(player)) return;
                Core.Character.Character acc = Main.Players[player];

                string status =
                    (acc.AdminLVL >= 1) ? "Администратор" :
                    (Main.Accounts[player].VipLvl > 0) ? $"{Group.GroupNames[Main.Accounts[player].VipLvl]} до {Main.Accounts[player].VipDate.ToString("dd.MM.yyyy")}" :
                    $"{Group.GroupNames[Main.Accounts[player].VipLvl]}";

                long bank = (acc.Bank != 0) ? Bank.Accounts[acc.Bank].Balance : 0;

                string lic = "";
                for (int i = 0; i < acc.Licenses.Count; i++)
                    if (acc.Licenses[i]) lic += $"{Main.LicWords[i]} / ";
                if (lic == "") lic = "Отсутствуют";

                string work = (acc.WorkID > 0) ? Jobs.WorkManager.JobStats[acc.WorkID - 1] : "Безработный";
                string fraction = (acc.FractionID > 0) ? Fractions.Manager.FractionNames[acc.FractionID] : "Нет";


                string number = (acc.Sim == -1) ? "Нет сим-карты" : Main.Players[player].Sim.ToString();


                List<object> data = new List<object>
                {
                    acc.LVL, //0
                    $"{acc.EXP}/{acc.EXP + 3 + acc.LVL * 3}", //1
                    number, //2
                    status, //3
                    acc.Warns,//4
                    lic,//5
                    acc.CreateDate.ToString("dd.MM.yyyy"),//6
                    work,//7
                    fraction,//8
                    acc.FractionLVL,//9
                    acc.FirstName,//10
                    acc.LastName,//11
                    acc.UUID,//12
                    acc.Bank,//13
                    acc.GenPromo,//14
                };

                string json = JsonConvert.SerializeObject(data);
                Log.Debug("data is: " + json.ToString());
                Trigger.ClientEvent(player, "board", 2, json);

                data.Clear();

            }
            catch (Exception e)
            {
                Log.Write("EXCEPTION AT \"DASHBOARD_SENDSTATS\":\n" + e.ToString(), nLog.Type.Error);
            }
        }
        public static Task SendStatsAsync(Player player)
        {
            try
            {
                if (!Main.Players.ContainsKey(player))
                    return Task.CompletedTask;
                Core.Character.Character acc = Main.Players[player];

                string status =
                    acc.AdminLVL >= 1 ? "Администратор" :
                    Main.Accounts[player].VipLvl > 0 ? $"{Group.GroupNames[Main.Accounts[player].VipLvl]} до {Main.Accounts[player].VipDate.ToString("dd.MM.yyyy")}" :
                    $"{Group.GroupNames[Main.Accounts[player].VipLvl]}";

                long bank = acc.Bank != 0 ? Bank.Accounts[acc.Bank].Balance : 0;

                string lic = "";
                for (int i = 0; i < acc.Licenses.Count; i++)
                    if (acc.Licenses[i]) lic += $"{Main.LicWords[i]} / ";
                if (lic == "") lic = "Отсутствуют";

                string work = acc.WorkID > 0 ? Jobs.WorkManager.JobStats[acc.WorkID - 1] : "Отсутствует";
                string fraction = acc.FractionID > 0 ? Fractions.Manager.FractionNames[acc.FractionID] : "Отсутствует";

                string number = acc.Sim == -1 ? "Нет сим-карты" : Main.Players[player].Sim.ToString();



                List<object> data = new List<object>
                {
                    acc.LVL, //0
                    $"{acc.EXP}/{acc.EXP + 3 + acc.LVL * 3}", //1
                    number, //2
                    status, //3
                    acc.Warns,//4
                    lic,//5
                    acc.CreateDate.ToString("dd.MM.yyyy"),//6
                    work,//7
                    fraction,//8
                    acc.FractionLVL,//9
                    acc.FirstName,//10
                    acc.LastName,//11
                    acc.UUID,//12
                    acc.Bank,//13
                };

                string json = JsonConvert.SerializeObject(data);
                Log.Debug("data is: " + json.ToString());
                Trigger.ClientEvent(player, "board", 2, json);

                data.Clear();
            }
            catch (Exception e)
            {
                Log.Write("EXCEPTION AT \"DASHBOARD_SENDSTATS\":\n" + e.ToString(), nLog.Type.Error);
            }

            return Task.CompletedTask;
        }

        public static string GetSerializeInvetoryItems(Player player)
        {
            int UUID = Main.Players[player].UUID;
            List<nItem> items = new List<nItem>(nInventory.Items[UUID]);

            List<object> data = new List<object>();
            foreach (nItem item in items)
            {
                List<object> idata = new List<object>
                    {
                        item.ID,
                        item.Count,
                        $"{item.Weight}".Replace(',', '.'),
                        (item.IsActive) ? 1 : 0,
                        GetItemSubData(item)
                    };
                data.Add(idata);
            }

            return JsonConvert.SerializeObject(data);
        }

        public static string GetSerializeClothesItems(Player player)
        {
            int UUID = Main.Players[player].UUID;
            List<nItem> items = new List<nItem>(nInventory.ActiveClothes[UUID]);

            List<object> data = new List<object>();
            foreach (nItem item in items)
            {
                List<object> idata = new List<object>
                    {
                        item.ID,
                        item.Count,
                        $"{item.Weight}".Replace(',', '.'),
                        (item.IsActive) ? 1 : 0,
                        GetItemSubData(item)
                    };
                data.Add(idata);
            }

            return JsonConvert.SerializeObject(data);
        }

        public static string GetSerializeWeaponsItems(Player player)
        {
            int UUID = Main.Players[player].UUID;
            List<nItem> items = new List<nItem>(nInventory.ActiveWeapons[UUID]);

            List<object> data = new List<object>();
            foreach (nItem item in items)
            {
                List<object> idata = new List<object>
                    {
                        item.ID,
                        item.Count,
                        $"{item.Weight}".Replace(',', '.'),
                        (item.IsActive) ? 1 : 0,
                        GetItemSubData(item)
                    };
                data.Add(idata);
            }

            return JsonConvert.SerializeObject(data);
        }

        public static string GetItemSubData(nItem item)
        {
            try
            {
                if (nInventory.WeaponsItems.Contains(item.Type) || item.Type == ItemType.StunGun)
                    return "Serial: " + item.Data.ToString().Split('_')[0];
                if (item.Type == ItemType.CarKey)
                    return item.Data.ToString().Split('_')[0];
                return "";
            }
            catch (Exception)
            {
                Log.Write("GetItemSubData:\n" + JsonConvert.SerializeObject(item), nLog.Type.Warn);
                return "Error";
            }
        }

        public static void sendItems(Player player)
        {
            try
            {
                if (!Main.Players.ContainsKey(player)) return;
                int UUID = Main.Players[player].UUID;

                if (!nInventory.Items.ContainsKey(UUID)) return;

                string json = GetSerializeInvetoryItems(player);
                string data = GetSerializeClothesItems(player);
                string weapons = GetSerializeWeaponsItems(player);

                float totalWeight = MathF.Round(nInventory.GetTotalWeight(player), 2);

                Trigger.ClientEvent(player, "board", "setTotalWeight", totalWeight.ToString().Replace(',', '.'));

                Log.Debug(json);
                Trigger.ClientEvent(player, "board", "itemSet", json, data);
                Trigger.ClientEvent(player, "board", "weaponsSet", weapons);
            }
            catch (Exception e)
            {
                Log.Write("EXCEPTION AT \"DASHBOARD_SENDITEMS\":\n" + e.ToString(), nLog.Type.Error);
            }
        }

        [RemoteEvent("REMOTE::LOAD_PROPERTIES_INFO_TO_BOARD")]
        public static void SendPropertiesPlayer(Player player)
        {
            try
            {
                if (!Main.Players.ContainsKey(player)) return;

                int UUID = Main.Players[player].UUID;
                List<object> data = new List<object>();

                Houses.House house = Houses.HouseManager.GetHouse(player, true);
                Business business = BusinessManager.GetBusinessToPlayer(player);

                string vehicleDatas = "[";
                VehicleManager.getAllPlayerVehicles(player.Name)?.ForEach(number =>
                {
                    if (VehicleManager.Vehicles.ContainsKey(number))
                        vehicleDatas += VehicleManager.Vehicles[number]?.GetVehicleDataToJson(number) + ',';
                });
                if(vehicleDatas.Length > 1) vehicleDatas = vehicleDatas.Remove(vehicleDatas.Length - 1, 1) + ']';
                else vehicleDatas = "[]";

                string houseData = house?.GetHouseInfoToJson();
                string businessData = business?.GetBusinessToJson();

                Trigger.ClientEvent(player, "BOARD::LOAD_ASSETS_INFO", houseData, businessData, vehicleDatas);
            }
            catch (Exception e)
            {
                Log.Write(e.Message, nLog.Type.Error);
            }
        }

        public static async Task SendItemsAsync(Player player)
        {
            try
            {
                if (!Main.Players.ContainsKey(player)) return;
                int UUID = Main.Players[player].UUID;

                if (!nInventory.Items.ContainsKey(UUID)) return;
                List<nItem> items = new List<nItem>(nInventory.Items[UUID]);

                List<object> data = new List<object>();
                foreach (nItem item in items)
                {
                    List<object> idata = new List<object>
                    {
                        item.ID,
                        item.Count,
                        (item.IsActive) ? 1 : 0,
                        (nInventory.WeaponsItems.Contains(item.Type) || item.Type == ItemType.StunGun) ? "Serial: " + item.Data : (item.Type == ItemType.CarKey) ? $"{(string)item.Data.Split('_')[0]}" : "",
                        item.Weight.ToString().Replace(',', '.'),
                    };
                    data.Add(idata);
                }

                string json = JsonConvert.SerializeObject(data);
                await Log.DebugAsync(json);
                NAPI.Task.Run(() => Trigger.ClientEvent(player, "board", "itemSet", json));

                items.Clear();
                data.Clear();
            }
            catch (Exception e)
            {
                Log.Write("EXCEPTION AT \"DASHBOARD_SENDITEMS\":\n" + e.ToString(), nLog.Type.Error);
            }
        }
        public static void Open(Player Player)
        {
            Trigger.ClientEvent(Player, "board", "open");
        }
        public static void OpenOut(Player Player, List<nItem> items, string title, int type = 1)
        {
            try
            {
                if (type == 0) return;
                List<object> data = new List<object>();
                data.Add(type);
                data.Add(title);
                List<object> Items = new List<object>();
                foreach (nItem item in items)
                {
                    List<object> idata = new List<object>
                    {
                        item.ID,
                        item.Count,
                        item.Weight.ToString().Replace(',', '.'),
                        (item.IsActive) ? 1 : 0,
                        (nInventory.WeaponsItems.Contains(item.Type) || item.Type == ItemType.StunGun) ? "Serial: " + item.Data : (item.Type == ItemType.CarKey) ? $"{item.Data}" : "",
                    };
                    Items.Add(idata);
                }
                data.Add(Items);

                string json = JsonConvert.SerializeObject(data);
                Log.Debug(json);
                Player.SetData("OPENOUT_TYPE", type);
                Trigger.ClientEvent(Player, "board", "outSet", json);
                Trigger.ClientEvent(Player, "board", "outside", true);
                Trigger.ClientEvent(Player, "board", "open");
            }
            catch (Exception e)
            {
                Log.Write("EXCEPTION AT \"DASHBOARD_OPENOUT\":\n" + e.ToString(), nLog.Type.Error);
            }
        }

        public static void Update(Player Player, nItem item, int index)
        {
            item = nInventory.Items[Main.Players[Player].UUID][index];
            List<object> idata = new List<object>
                    {
                        item.ID,
                        item.Count,
                        item.Weight.ToString().Replace(',', '.'),
                        (item.IsActive) ? 1 : 0,
                        (nInventory.WeaponsItems.Contains(item.Type) || item.Type == ItemType.StunGun) ? "Serial: " + item.Data : (item.Type == ItemType.CarKey) ? $"{(string)item.Data.Split('_')[0]}" : "",
                    };
            string json = JsonConvert.SerializeObject(idata);
            Trigger.ClientEvent(Player, "board", "itemUpdate", json, index);
        }
        public static Task UpdateAsync(Player Player, nItem item, int index)
        {
            try
            {
                List<object> idata = new List<object>
                    {
                        item.ID,
                        item.Count,
                        item.Weight.ToString().Replace(',', '.'),
                        item.IsActive ? 1 : 0,
                        nInventory.WeaponsItems.Contains(item.Type) || item.Type == ItemType.StunGun ? "Serial: " + item.Data : item.Type == ItemType.CarKey ? $"{(string)item.Data.Split('_')[0]}" : "",
                    };
                string json = JsonConvert.SerializeObject(idata);
                NAPI.Task.Run(() => Trigger.ClientEvent(Player, "board", "itemUpdate", json, index));
            }
            catch (Exception e) { Log.Write("UpdateAsync: " + e.Message); }

            return Task.CompletedTask;
        }

        public static void ClothesUpdate(Player Player, nItem item, int index)
        {
            List<object> idata = new List<object>
                    {
                        item.ID,
                        item.Count,
                         $"{MathF.Round(item.Weight, 2)}".Replace(',', '.'),
                        item.IsActive ? 1 : 0,
                        GetItemSubData(item),
                        //(nInventory.WeaponsItems.Contains(item.Type) || item.Type == ItemType.StunGun) ? "Serial: " + item.Data : (item.Type == ItemType.CarKey) ? $"{(string)item.Data.ToString().Split('_')[0]}" : ""
                    };

            string json = JsonConvert.SerializeObject(idata);
            float totalWeight = MathF.Round(nInventory.GetTotalWeight(Player), 2);

            //Log.Write($"{json} {index}");

            Trigger.ClientEvent(Player, "board", "setTotalWeight", totalWeight.ToString().Replace(',', '.'));
            Trigger.ClientEvent(Player, "board", "clothesItemUpdate", json, index);
        }

        public static void WeaponsUpdate(Player Player, nItem item, int index)
        {
            List<object> idata = new List<object>
                    {
                        item.ID,
                        item.Count,
                         $"{MathF.Round(item.Weight, 2)}".Replace(',', '.'),
                        item.IsActive ? 1 : 0,
                        GetItemSubData(item),
                        //(nInventory.WeaponsItems.Contains(item.Type) || item.Type == ItemType.StunGun) ? "Serial: " + item.Data : (item.Type == ItemType.CarKey) ? $"{(string)item.Data.ToString().Split('_')[0]}" : ""
                    };

            string json = JsonConvert.SerializeObject(idata);

            //Log.Write($"{json} {index}");

            Trigger.ClientEvent(Player, "board", "weaponsItemUpdate", json, index);
        }
    }
}
