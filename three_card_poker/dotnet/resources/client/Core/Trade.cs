using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using Newtonsoft.Json;
using Redage.SDK;
using NeptuneEvo.GUI;
using NeptuneEvo.Core;

namespace NeptuneEvo
{
    class Trade : Script
    {
        private static nLog Log = new nLog("Trade");

        [RemoteEvent("tradeCheck")]
        public static void TradeCheck(Player player, bool check, string money)
        {
            //Log.Write($"tradeCheck: {player.Name} {money}");
            try
            {
                if (player.HasData("TRADE_LOCK"))
                    return;

                int playerMoney = money.Length == 0 ? 0 : Convert.ToInt32(money);

                player.SetData("TRADE_MONEY", playerMoney);

                if (player.GetData<int>("TRADE_MONEY") > Main.Players[player].Money)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"У Вас нет столько денежных средств", 3000);
                    Trigger.ClientEvent(player, "board", "tradeError"); // Снимает галочку
                    return;
                }

                if (player.HasData("TRADE_WITH"))
                {
                    UpdateTradeMoney(player.GetData<Player>("TRADE_WITH"), playerMoney);
                    if (player.GetData<bool>("TRADE_READY"))
                    {
                        Player target = player.GetData<Player>("TRADE_WITH");

                        if (target.GetData<bool>("TRADE_READY") == false)
                        {
                            player.SetData("TRADE_READY", false);
                        }
                    }
                    else
                    {
                        Player target = player.GetData<Player>("TRADE_WITH");

                        player.SetData("TRADE_READY", true);

                        if (target.GetData<bool>("TRADE_READY") == true)
                        {
                            Trigger.ClientEvent(player, "board", "tradeReady", true);
                            Trigger.ClientEvent(target, "board", "tradeReady", true);
                        }
                    }
                }
                return;
            }
            catch (Exception e)
            {
                Log.Write($"tradeCheck: " + e.Message, logType: nLog.Type.Error);
            }
        }

        [RemoteEvent("tradeAccept")]
        public static void TradeAccept(Player player)
        {
            //Log.Write($"TradeAccept: {player.Name}");
            try
            {
                if (player.HasData("TRADE_LOCK"))
                    return;

                if (player.GetData<int>("TRADE_MONEY") > Main.Players[player].Money)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"У Вас нет столько денежных средств", 3000);
                    return;
                }

                Player tr = player.GetData<Player>("TRADE_WITH");
                if (tr.Position.DistanceTo(player.Position) > 5.0)
                {
                    Notify.Send(tr, NotifyType.Error, NotifyPosition.BottomCenter, "Вы слишком далеко от игрока", 3000);
                    TradeCancel(player);
                    return;
                }

                if (player.HasData("TRADE_READY"))
                {
                    if (player.GetData<bool>("TRADE_READY"))
                    {
                        Player target = player.GetData<Player>("TRADE_WITH");

                        if (target.GetData<bool>("TRADE_READY") == true)
                        {
                            player.SetData("TRADE_DO", true);

                            if (target.GetData<bool>("TRADE_DO") == true)
                            {
                                TradeStart(player, target);
                            }
                            else
                            {
                                Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, "Ожидание ответа от игрока...", 3000);
                                return;
                            }
                        }
                        else
                        {
                            Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, "Игрок не готов к обмену!", 3000);
                            return;
                        }
                    }
                    else
                    {
                        Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, " Ошибка готовности!", 3000);
                        return;
                    }
                }
                return;
            }
            catch (Exception e)
            {
                Log.Write($"tradeAccept: " + e.Message, logType: nLog.Type.Error);
            }
        }

        [RemoteEvent("tradeCancel")]
        public static void TradeCancel(Player player)
        {
            try
            {
                if (player.HasData("TRADE_LOCK"))
                    return;

                if (player.HasData("TRADE_WITH"))
                {

                    Player target = player.GetData<Player>("TRADE_WITH");
                    Dashboard.Close(target);
                    Dashboard.Close(player);

                    Notify.Send(target, NotifyType.Error, NotifyPosition.BottomCenter, "Игрок отменил обмен", 3000);

                    List<nItem> playerItems = player.GetData<List<nItem>>("TRADE_ITEMS");
                    List<nItem> targetItems = target.GetData<List<nItem>>("TRADE_ITEMS");

                    for (int i = 0; i < 5; i++)
                    {
                        if (playerItems[i].Type != ItemType.Debug)
                            nInventory.Add(player, new nItem(playerItems[i].Type, playerItems[i].Count, playerItems[i].Data, false));

                        if (targetItems[i].Type != ItemType.Debug)
                            nInventory.Add(target, new nItem(targetItems[i].Type, targetItems[i].Count, targetItems[i].Data, false));
                    }

                    player.ResetData("TRADE_WITH");
                    target.ResetData("TRADE_WITH");

                    player.ResetData("TRADE_ITEMS");
                    target.ResetData("TRADE_ITEMS");

                    player.ResetData("TRADE_READY");
                    target.ResetData("TRADE_READY");

                    player.ResetData("TRADE_GO");
                    target.ResetData("TRADE_GO");

                    player.ResetData("TRADE_MONEY");
                    target.ResetData("TRADE_MONEY");

                    player.ResetData("TRADE_LOCK");
                    target.ResetData("TRADE_LOCK");

                    Dashboard.sendItems(player);
                    Dashboard.sendItems(target);

                }
                return;
            }
            catch (Exception e)
            {
                Log.Write($"tradeCancel: " + e.Message, logType: nLog.Type.Error);
            }
        }

        public static void TradeStart(Player player, Player target)
        {
            try
            {
                player.SetData("TRADE_LOCK", true);
                target.SetData("TRADE_LOCK", true);

                List<nItem> playerItems = player.GetData<List<nItem>>("TRADE_ITEMS");
                List<nItem> targetItems = target.GetData<List<nItem>>("TRADE_ITEMS");

                int playerMoney = player.GetData<int>("TRADE_MONEY");
                int targetMoney = target.GetData<int>("TRADE_MONEY");

                for (int i = 0; i < 5; i++)
                {
                    if (playerItems[i].Type != ItemType.Debug)
                        nInventory.Add(target, new nItem(playerItems[i].Type, playerItems[i].Count, playerItems[i].Data, false));

                    if (targetItems[i].Type != ItemType.Debug)
                        nInventory.Add(player, new nItem(targetItems[i].Type, targetItems[i].Count, targetItems[i].Data, false));
                }

                if (targetMoney != 0)
                {
                    MoneySystem.Wallet.Change(target, -targetMoney);
                    MoneySystem.Wallet.Change(player, +targetMoney);
                }

                if (playerMoney != 0)
                {
                    MoneySystem.Wallet.Change(player, -playerMoney);
                    MoneySystem.Wallet.Change(target, +playerMoney);
                }

                player.ResetData("TRADE_ITEMS");
                target.ResetData("TRADE_ITEMS");

                player.ResetData("TRADE_WITH");
                target.ResetData("TRADE_WITH");

                player.ResetData("TRADE_READY");
                target.ResetData("TRADE_READY");

                player.ResetData("TRADE_DO");
                target.ResetData("TRADE_DO");

                player.ResetData("TRADE_MONEY");
                target.ResetData("TRADE_MONEY");

                player.ResetData("TRADE_LOCK");
                target.ResetData("TRADE_LOCK");

                Dashboard.Close(player);
                Dashboard.Close(target);

                Dashboard.sendItems(player);
                Dashboard.sendItems(target);

                Notify.Send(target, NotifyType.Info, NotifyPosition.BottomCenter, $"Вы обменялись вещами с игроком {player.Name}", 3000);
                Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"Вы обменялись вещами с игроком {target.Name}", 3000);

                return;
            }
            catch (Exception e)
            {
                Log.Write($"Trade: " + e.Message, nLog.Type.Error);
            }
        }

        public static void OpenTrade(Player Player, List<nItem> items, string title, int type = 1)
        {
            try
            {
                if (type == 0) return;
                List<object> data = new List<object>();
                data.Add(type);
                data.Add(title);

                List<object> PlayerItems = new List<object>();
                foreach (nItem item in items)
                {
                    List<object> idata = new List<object>
                    {
                        item.ID,
                        item.Count,
                        $"{MathF.Round(item.Weight, 2)}".Replace(',', '.'),
                        (item.IsActive) ? 1 : 0,
                        Dashboard.GetItemSubData(item),
                        //(nInventory.WeaponsItems.Contains(item.Type) || item.Type == ItemType.StunGun) ? "Serial: " + item.Data : (item.Type == ItemType.CarKey) ? $"{item.Data}" : ""
                    };
                    PlayerItems.Add(idata);
                }
                data.Add(PlayerItems);

                Log.Write($"{data[2]}");

                List<object> TargeItems = new List<object>();
                foreach (nItem item in items)
                {
                    List<object> idata = new List<object>
                    {
                        item.ID,
                        item.Count,
                        $"{MathF.Round(item.Weight, 2)}".Replace(',', '.'),
                        (item.IsActive) ? 1 : 0,
                        Dashboard.GetItemSubData(item),
                        //(nInventory.WeaponsItems.Contains(item.Type) || item.Type == ItemType.StunGun) ? "Serial: " + item.Data : (item.Type == ItemType.CarKey) ? $"{item.Data}" : ""
                    };
                    TargeItems.Add(idata);
                }

                data.Add(TargeItems);
                data.Add(Main.Players[Player].Money);

                string json = JsonConvert.SerializeObject(data);

                //Log.Write($"{json}");

                Player.SetData("OPENOUT_TYPE", type);
                float totalWeight = MathF.Round(nInventory.GetTotalWeight(Player), 2);

                Trigger.ClientEvent(Player, "board", "setTotalWeight", totalWeight.ToString().Replace(',', '.'));
                //Trigger.ClientEvent(Player, "board", "setTotalWeightOut", outWeight.ToString().Replace(',', '.'));
                Trigger.ClientEvent(Player, "board", "tradeSet", json);
                Trigger.ClientEvent(Player, "board", "outside", true, type);
                Trigger.ClientEvent(Player, "board", "open");
            }
            catch (Exception e)
            {
                Log.Write("EXCEPTION AT \"DASHBOARD_OPENOUT\":\n" + e.ToString(), nLog.Type.Error);
            }
        }

        public static void TradeFromUpdate(Player Player, nItem item, int index)
        {
            List<object> idata = new List<object>
                    {
                        item.ID,
                        item.Count,
                         $"{MathF.Round(item.Weight, 2)}".Replace(',', '.'),
                        item.IsActive ? 1 : 0,
                        Dashboard.GetItemSubData(item),
                        //(nInventory.WeaponsItems.Contains(item.Type) || item.Type == ItemType.StunGun) ? "Serial: " + item.Data : (item.Type == ItemType.CarKey) ? $"{(string)item.Data.ToString().Split('_')[0]}" : ""
                    };

            string json = JsonConvert.SerializeObject(idata);
            float totalWeight = MathF.Round(nInventory.GetTotalWeight(Player), 2);

            //Log.Write($"{json}");

            Trigger.ClientEvent(Player, "board", "setTotalWeight", totalWeight.ToString().Replace(',', '.'));
            Trigger.ClientEvent(Player, "board", "updateItemFromPlayer", json, index);
        }

        public static void UpdateTradeMoney(Player Player, int money)
        {
            Trigger.ClientEvent(Player, "board", "updateMoneyFrom", money);
        }

        public static void TradeToUpdate(Player Player, nItem item, int index)
        {
            List<object> idata = new List<object>
                    {
                        item.ID,
                        item.Count,
                         $"{MathF.Round(item.Weight, 2)}".Replace(',', '.'),
                        item.IsActive ? 1 : 0,
                        Dashboard.GetItemSubData(item),
                        //(nInventory.WeaponsItems.Contains(item.Type) || item.Type == ItemType.StunGun) ? "Serial: " + item.Data : (item.Type == ItemType.CarKey) ? $"{(string)item.Data.ToString().Split('_')[0]}" : ""
                    };

            string json = JsonConvert.SerializeObject(idata);
            float totalWeight = MathF.Round(nInventory.GetTotalWeight(Player), 2);

            //Log.Write($"{json}");

            Trigger.ClientEvent(Player, "board", "setTotalWeight", totalWeight.ToString().Replace(',', '.'));
            Trigger.ClientEvent(Player, "board", "updateItemToPlayer", json, index);
        }
    }
}
