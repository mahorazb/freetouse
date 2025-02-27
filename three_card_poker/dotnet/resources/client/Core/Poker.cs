using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using GTANetworkAPI;
using NeptuneEvo;
using NeptuneEvo.Core;
using Newtonsoft.Json;
using Redage.SDK;
using Trigger = NeptuneEvo.Trigger;

namespace client.Core
{
    class Poker : Script
    {
        private static nLog Log = new nLog("Poker");

        public static List<string> Cards = new List<string>()
        {
            "vw_prop_cas_card_club_ace", 
            "vw_prop_cas_card_club_02", 
            "vw_prop_cas_card_club_03", 
            "vw_prop_cas_card_club_04", 
            "vw_prop_cas_card_club_05",
            "vw_prop_cas_card_club_06", 
            "vw_prop_cas_card_club_07", 
            "vw_prop_cas_card_club_08", 
            "vw_prop_cas_card_club_09", 
            "vw_prop_cas_card_club_10",
            "vw_prop_cas_card_club_jack", 
            "vw_prop_cas_card_club_queen", 
            "vw_prop_cas_card_club_king", 
            "vw_prop_cas_card_dia_ace", 
            "vw_prop_cas_card_dia_02",
            "vw_prop_cas_card_dia_03",
            "vw_prop_cas_card_dia_04",
            "vw_prop_cas_card_dia_05",
            "vw_prop_cas_card_dia_06",
            "vw_prop_cas_card_dia_07",
            "vw_prop_cas_card_dia_08",
            "vw_prop_cas_card_dia_09",
            "vw_prop_cas_card_dia_10",
            "vw_prop_cas_card_dia_jack",
            "vw_prop_cas_card_dia_queen",
            "vw_prop_cas_card_dia_king",
            "vw_prop_cas_card_hrt_ace",
            "vw_prop_cas_card_hrt_02",
            "vw_prop_cas_card_hrt_03",
            "vw_prop_cas_card_hrt_04",
            "vw_prop_cas_card_hrt_05",
            "vw_prop_cas_card_hrt_06",
            "vw_prop_cas_card_hrt_07",
            "vw_prop_cas_card_hrt_08",
            "vw_prop_cas_card_hrt_09",
            "vw_prop_cas_card_hrt_10",
            "vw_prop_cas_card_hrt_jack",
            "vw_prop_cas_card_hrt_queen",
            "vw_prop_cas_card_hrt_king",
            "vw_prop_cas_card_spd_ace",
            "vw_prop_cas_card_spd_02",
            "vw_prop_cas_card_spd_03",
            "vw_prop_cas_card_spd_04",
            "vw_prop_cas_card_spd_05",
            "vw_prop_cas_card_spd_06",
            "vw_prop_cas_card_spd_07",
            "vw_prop_cas_card_spd_08",
            "vw_prop_cas_card_spd_09",
            "vw_prop_cas_card_spd_10",
            "vw_prop_cas_card_spd_jack",
            "vw_prop_cas_card_spd_queen",
            "vw_prop_cas_card_spd_king",
        };

        public static List<string> MainCards = new List<string>()
        {
            "vw_prop_cas_card_club_ace",
            "vw_prop_cas_card_club_queen", 
            "vw_prop_cas_card_club_king", 
            "vw_prop_cas_card_dia_ace",
            "vw_prop_cas_card_dia_queen",
            "vw_prop_cas_card_dia_king",
            "vw_prop_cas_card_hrt_ace",
            "vw_prop_cas_card_hrt_queen",
            "vw_prop_cas_card_hrt_king",
            "vw_prop_cas_card_spd_ace",
            "vw_prop_cas_card_spd_queen",
            "vw_prop_cas_card_spd_king",
        };

        public static int GetCardType(string name)
        {

            string cType = name.Split('_')[1];

            switch (cType)
            {
                case "02":
                    return 2;
                case "03":
                    return 3;
                case "04":
                    return 4;
                case "05":
                    return 5;
                case "06":
                    return 6;
                case "07":
                    return 7;
                case "08":
                    return 8;
                case "09":
                    return 9;
                case "10":
                    return 10;
                case "jack":
                    return 11;
                case "queen":
                    return 12;
                case "king":
                    return 13;
                case "ace":
                    return 14;
                default:
                    return 1;
            }
        }

        public static int GetCardSuit(string name)
        {
            string cSuit = name.Split('_')[0];

            switch (cSuit)
            {
                case "club":
                    return 0;
                case "dia":
                    return 1;
                case "hrt":
                    return 2;
                case "spd":
                    return 3;
                default:
                    return -1;
            }
        }


        public static List<Vector3> TablePos = new List<Vector3>()
        {
            new Vector3(1146.329, 261.2543, -52.8409),
            new Vector3(1143.338, 264.2453, -52.8409),
        };

        public static List<List<Vector3>> TableSeatsPos = new List<List<Vector3>>()
        {
            new List<Vector3>(){
                
              
         
                new Vector3(1145.63, 260.7765, -51.7979),
                new Vector3(1146.325, 260.7546, -51.812),
                new Vector3(1146.865, 261.2238, -51.8003),
                new Vector3(1146.849, 261.9344, -51.8167),
            },
            new List<Vector3>(){
                new Vector3(1142.798f, 263.5501f, -51.7869f),
                new Vector3(1142.82f, 264.2595f, -51.8004f),
                new Vector3(1143.339f, 264.7519f, -51.8289f),
                new Vector3(1144.052f, 264.7396f, -51.7913f),
            }
};

        public static List<List<int>> PokerTablePlayCards = new List<List<int>>()
        {
            new List<int>(){ },
            new List<int>(){ },
            new List<int>(){ },
            new List<int>(){ },
        };

        public static List<int> PokerBetTime = new List<int>()
        {
            0, 0, 0, 0
        };

        public static List<List<Player>> PokerPlayers = new List<List<Player>>() {
            new List<Player>(){ },
            new List<Player>(){ },
            new List<Player>(){ },
            new List<Player>(){ },
        };

        public static List<List<Player>> PokerSeatsPlayers = new List<List<Player>>() {
            new List<Player>(){ },
            new List<Player>(){ },
            new List<Player>(){ },
            new List<Player>(){ },
        };

        public static List<int> PokerTableSeatsNow = new List<int>()
        {
            0,0,0,0
        };

        public static List<int> PokerTableMaxBet = new List<int>()
        {
            100,100,100,100
        };

        public static List<int> PokerTableMinBet = new List<int>()
        {
            10,10,10,10
        };


        public static List<Player> PokerTablePlayerNow = new List<Player>()
        {
            null,null,null,null
        };

        public static List<List<List<string>>> PokerTables = new List<List<List<string>>>()
        {
            new List<List<string>>() {
                new List<string>{"", "", "" },
                new List<string>{"", "", "" },
                new List<string>{"", "", "" },
                new List<string>{"", "", "" },
                new List<string>{"", "", "" },
            },
            new List<List<string>>() {
                new List<string>{"", "", "" },
                new List<string>{"", "", "" },
                new List<string>{"", "", "" },
                new List<string>{"", "", "" },
                new List<string>{"", "", "" },
            },
            new List<List<string>>() {
                new List<string>{"", "", "" },
                new List<string>{"", "", "" },
                new List<string>{"", "", "" },
                new List<string>{"", "", "" },
                new List<string>{"", "", "" },
            },
            new List<List<string>>() {
                new List<string>{"", "", "" },
                new List<string>{"", "", "" },
                new List<string>{"", "", "" },
                new List<string>{"", "", "" },
                new List<string>{"", "", "" },
            },
        };


        public static List<List<Player>> PokerTablePlayers = new List<List<Player>>()
        {
            new List<Player>(){null, null, null, null},
            new List<Player>(){null, null, null, null},
            new List<Player>(){null, null, null, null},
            new List<Player>(){null, null, null, null},
        };

        public static List<int> PokerTableDealerWinType = new List<int>()
        {
            0,
            0,
            0,
            0
        };

        [ServerEvent(Event.ResourceStart)]
        public static void onResourceStart()
        {
            NAPI.Task.Run(() =>
            {
             
                    Thread thd = new Thread(new ParameterizedThreadStart(StartDealing));
                    thd.Start(0);
                
            }, 5000);


            for (int i = 0; i < TableSeatsPos.Count; i++)
            {
                for (int j = 0; j < TableSeatsPos[i].Count; j++)
                {
                    var colshape = NAPI.ColShape.CreateCylinderColShape(TableSeatsPos[i][j], 1, 2);
                    colshape.SetData("TABLE", i);
                    colshape.SetData("SEAT", j);
                    colshape.OnEntityEnterColShape += (shape, player) =>
                    {
                        player.SetData("INTERACTIONCHECK", 665);
                        player.SetData("TABLE", shape.GetData<int>("TABLE"));
                        player.SetData("SEAT", shape.GetData<int>("SEAT"));
                       // if(!player.HasData("P_SEAT")) Trigger.ClientEvent(player, "client_press_key_to", "open", JsonConvert.SerializeObject(new List<object>() { "E", "сесть за стул" }));
                    };
                    colshape.OnEntityExitColShape += (shape, player) =>
                    {
                        Trigger.ClientEvent(player, "client_press_key_to", "close");
                        player.SetData("INTERACTIONCHECK", 0);
                        player.ResetData("TABLE");
                        player.ResetData("SEAT");
                    };
                }
            }
        }

        public static void StartDealing(object table)
        {
            try
            {
                int tb = Convert.ToInt32(table);

                while (true)
                {
                    if (PokerBetTime[tb] != 0)
                    {
                        if (PokerSeatsPlayers[tb].Count != 0)
                        {
                            for (int i = 0; i < PokerSeatsPlayers[tb].Count; i++)
                            {
                                //Trigger.ClientEvent(BlackJackSeatsPlayers[tb][i], "blackjack_show_bet", 0, 0);
                                PokerSeatsPlayers[tb][i].SendChatMessage($"Ожидание ставок [ {PokerBetTime[tb]} ]");
                                //Trigger.ClientEvent(BlackJackSeatsPlayers[tb][i], "casinoKeys", "setTime", 0, 0);
                            }

                            if (PokerSeatsPlayers[tb].Count == PokerPlayers[tb].Count)
                            {
                                PokerBetTime[tb] = 1;
                            }
                        }
                        PokerBetTime[tb] -= 1;

                        Thread.Sleep(1000);

                        if (PokerBetTime[tb] != 0)
                            continue;
                    }

                    if (PokerPlayers[tb].Count == 0 && PokerBetTime[tb] == 0)
                    {
                        PokerBetTime[tb] = 20;
                        Thread.Sleep(1000);
                        continue;
                    }

                    for (int i = 0; i < PokerSeatsPlayers[tb].Count; i++)
                    {
                        Trigger.ClientEvent(PokerSeatsPlayers[tb][i], "casinoPoker", "toggleStart", true);

                        PokerTables[tb][PokerSeatsPlayers[tb][i].GetData<int>("P_SEAT")] = new List<string>() { "", "", "" };
                    }

                    PokerTables[tb][4] = new List<string>() { "", "", "" };
                    PokerTablePlayCards[tb].Clear();

                    ShuffleDeck(tb);

                    Thread.Sleep(3000);

                    for (int j = 0; j < PokerPlayers[tb].Count; j++)
                    {
                        if (PokerPlayers[tb][j] == null) continue;

                        GetCards(PokerPlayers[tb][j], tb);
                        CheckCombination(tb, PokerPlayers[tb][j].GetData<int>("P_SEAT"));
                        Thread.Sleep(2000);
                        //Trigger.ClientEvent(BlackJackPlayers[tb][i], "casinoKeys", "setChips", BlackJackPlayers[tb][i].GetData<int>("BJ_SUM").ToString(), BlackJackDealSum[tb].ToString());
                    }

                    GetCardsDealer(tb);

                    Thread.Sleep(3000);

                    CheckCombination(tb, 4);

                    for (int i = 0; i < PokerPlayers[tb].Count; i++)
                    {
                        if (PokerPlayers[tb][i] == null) continue;

                        PokerTablePlayerNow[tb] = PokerPlayers[tb][i];
                        PokerTableSeatsNow[tb] = PokerPlayers[tb][i].GetData<int>("P_SEAT");
                        while (true)
                        {
                            try
                            {
                                if (PokerPlayers[tb][i].GetData<int>("P_ANSWER") == 0)
                                {
                                    Trigger.ClientEvent(PokerSeatsPlayers[tb][i], "pokerAnswer", true);
                                    PokerPlayers[tb][i].SetData("P_ANSWER", 1);
                                }
                                else if (PokerPlayers[tb][i].GetData<int>("P_ANSWER") == 1)
                                {
                                    if (PokerPlayers[tb][i].GetData<int>("P_TIMER") == 30000)
                                    {
                                        PokerPlayers[tb][i].SetData("P_ANSWER", 3);
                                        PokerPlayers[tb][i].SetData("P_TIMER", 0);
                                        Trigger.ClientEvent(PokerPlayers[tb][i], "pokerAnswer", false);
                                        Trigger.ClientEvent(PokerPlayers[tb][i], "casinoPoker", "toggleStart", false);
                                        Trigger.ClientEvent(PokerPlayers[tb][i], "casinoPoker", "setTime", 0, 0);
                                        CollectCard(tb, PokerPlayers[tb][i].GetData<int>("P_SEAT"));
                                        Notify.Send(PokerPlayers[tb][i], NotifyType.Info, NotifyPosition.BottomCenter, "Вы отказались продолжать игру.", 3000);
                                        PokerPlayers[tb].Remove(PokerPlayers[tb][i]);
                                        break;
                                    }
                                    else
                                    {
                                        Thread.Sleep(1000);
                                        PokerPlayers[tb][i].SetData("P_TIMER", PokerPlayers[tb][i].GetData<int>("P_TIMER") + 1000);
                                        //BlackJackPlayers[tb][i].SendChatMessage($"{15 - BlackJackPlayers[tb][i].GetData<int>("BJ_TIMER") / 1000}");
                                        Trigger.ClientEvent(PokerPlayers[tb][i], "casinoPoker", "setTime", 0, 30 - PokerPlayers[tb][i].GetData<int>("P_TIMER") / 1000);
                                        //Trigger.ClientEvent(BlackJackSeatsPlayers[tb][i], "blackjack_update_time", $"{15 - BlackJackPlayers[tb][i].GetData<int>("BJ_TIMER") / 1000}");
                                    }
                                }
                                else
                                {
                                    if (PokerPlayers[tb][i].GetData<int>("P_ANSWER") == 2)
                                    {
                                        //Trigger.ClientEvent(BlackJackPlayers[tb][i], "blackjack_hide_game");
                                        //GetCard(BlackJackPlayers[tb][i]);
                                        nInventory.Remove(PokerPlayers[tb][i], ItemType.CasinoChips, PokerPlayers[tb][i].GetData<int>("P_BET"));
                                        PokerPlayers[tb][i].SetData("P_ANSWER", 0);
                                        PokerPlayers[tb][i].SetData("P_TIMER", 0);
                                        Trigger.ClientEvent(PokerPlayers[tb][i], "pokerAnswer", false);
                                        Trigger.ClientEvent(PokerPlayers[tb][i], "casinoPoker", "toggleStart", false);
                                        Notify.Send(PokerPlayers[tb][i], NotifyType.Info, NotifyPosition.BottomCenter, "Вы продолжили игру.", 3000);
                                        Trigger.ClientEvent(PokerPlayers[tb][i], "casinoPoker", "setTime", 0, 0);
                                        break;
                                        //Trigger.ClientEvent(BlackJackPlayers[tb][i], "blackjack_update_result", BlackJackDealSum[tb], BlackJackPlayers[tb][i].GetData<int>("SUM"));
                                    }
                                    else
                                    {

                                        PokerPlayers[tb][i].SetData("P_ANSWER", 0);
                                        PokerPlayers[tb][i].SetData("P_TIMER", 0);
                                        CollectCard(tb, PokerPlayers[tb][i].GetData<int>("P_SEAT"));
                                        Trigger.ClientEvent(PokerPlayers[tb][i], "pokerAnswer", false);
                                        Trigger.ClientEvent(PokerPlayers[tb][i], "casinoPoker", "toggleStart", false);
                                        Notify.Send(PokerPlayers[tb][i], NotifyType.Info, NotifyPosition.BottomCenter, "Вы отказались продолжать игру.", 3000);
                                        Trigger.ClientEvent(PokerPlayers[tb][i], "casinoPoker", "setTime", 0, 0);
                                        PokerPlayers[tb].Remove(PokerPlayers[tb][i]);
                                       
                                        Thread.Sleep(1500);

                                        break;
                                    }

                    
                                }
                            }
                            catch (Exception e)
                            {
                          
                                CollectCard(tb, PokerTableSeatsNow[tb]);
                                Thread.Sleep(2000);
                                Log.Write(e.Message);
                                break;
                            }
                        }

                        PokerTablePlayerNow[tb] = null;
                        PokerTableSeatsNow[tb] = 0;
                    }

                    Thread.Sleep(2000);

                    for (int j = 0; j < PokerPlayers[tb].Count; j++)
                    {
                        Trigger.ClientEvent(PokerPlayers[tb][j], "poker_camera", tb);
                    
                        //Trigger.ClientEvent(BlackJackPlayers[tb][i], "casinoKeys", "setChips", BlackJackPlayers[tb][i].GetData<int>("BJ_SUM").ToString(), BlackJackDealSum[tb].ToString());
                    }

                    Thread.Sleep(1000);

                    RevealDiler(tb);

                    Thread.Sleep(6000);

                    for (int j = 0; j < PokerPlayers[tb].Count; j++)
                    {
                        RevealPlayer(tb, PokerPlayers[tb][j].GetData<int>("P_SEAT"));
                        
                        Thread.Sleep(2000);
                        CheckWin(PokerPlayers[tb][j], tb);
                        //Trigger.ClientEvent(BlackJackPlayers[tb][i], "casinoKeys", "setChips", BlackJackPlayers[tb][i].GetData<int>("BJ_SUM").ToString(), BlackJackDealSum[tb].ToString());
                    }

                    Thread.Sleep(2000);

                    for (int j = 0; j < PokerPlayers[tb].Count; j++)
                    {
                       
                        CollectCard(tb, PokerPlayers[tb][j].GetData<int>("P_SEAT"));
                        PokerPlayers[tb].Remove(PokerPlayers[tb][j]);
                        Thread.Sleep(2000);
                    }

                    CollectCard(tb, -1);

                    Thread.Sleep(3000);
                    

                }
            }
            catch (Exception e)
            {
                Log.Write("Dealing: " + e.Message);
            }
        }

        [RemoteEvent("pokerHit")]
        public static void CMD_Hit(Player player)
        {
            if (!player.HasData("P_TABLE"))
                return;

            int tb = player.GetData<int>("P_TABLE");
            int seat = player.GetData<int>("P_SEAT");

            if (player.GetData<int>("P_ANSWER") == 1)
            {
                Trigger.ClientEventInRange(player.Position, 150, "poker_decline", player.Handle);
                player.SetData("P_ANSWER", 3);
            }
        }

        [RemoteEvent("pokerStand")]
        public static void CMD_Stand(Player player)
        {
            if (!player.HasData("P_TABLE"))
                return;

            int tb = player.GetData<int>("P_TABLE");
            int seat = player.GetData<int>("P_SEAT");

            if (player.GetData<int>("P_ANSWER") == 1)
            {
                Trigger.ClientEventInRange(player.Position, 150, "poker_bet", player.Handle);
                player.SetData("P_ANSWER", 2);
            }
        }

        public static void RevealDiler(int table)
        {
            List<string> cards = PokerTables[table][4];

            Trigger.ClientEventInRange(TablePos[table], 250f, "revealSelf", table, JsonConvert.SerializeObject(cards));
        }

        public static void RevealPlayer(int table, int seat)
        {

            Trigger.ClientEventInRange(TablePos[table], 250f, "revealPlayer", table, seat);
        }

        public static void CheckWin(Player player, int table)
        {
            int dWin = PokerTableDealerWinType[table];

            int pWin = player.GetData<int>("P_WIN_TYPE");

            int pSeat = player.GetData<int>("P_SEAT");


            if (pWin > dWin)
            {
                int win = player.GetData<int>("P_BET") * 4;

                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, $"Вы выиграли {win} фишек", 3000);

                nInventory.Add(player, new nItem(ItemType.CasinoChips, win));
            }
            else if(pWin < dWin)
            {
                int win = player.GetData<int>("P_BET") * 2;

                Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"Вы проиграли {win} фишек", 3000);

            }
            else 
            {
                int pSum = PokerTables[table][pSeat].Max((t) => GetCardType(t));
                int dSum = PokerTables[table][4].Max((t) => GetCardType(t));


                if (pSum > dSum)
                {

                    int win = player.GetData<int>("P_BET") * 4;

                    Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, $"Вы выиграли {win} фишек", 3000);

                    nInventory.Add(player, new nItem(ItemType.CasinoChips, win));
                }
                else
                {
                    int win = player.GetData<int>("P_BET") * 2;

                    Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"Вы проиграли {win} фишек", 3000);
                }
            }

            player.SetData("P_STATUS", 0);
            player.SetData("P_ANSWER", 0);
            player.SetData("P_TIMER", 0);

            player.SetData("P_BET", 0);

        }

        public static void CheckCombination(int table, int seat)
        {
            int winType = 0;

            if (seat == 4)
            {
                if (PokerTables[table][4].Count((t) => GetCardType(t) >= 12) == 0)
                {
                    winType = -1;
                    PokerTableDealerWinType[table] = -1;
                    return;
                }
            }

            if (seat != 4)
                PokerTablePlayers[table][seat].ResetData("P_WIN_TYPE");
            else
                PokerTableDealerWinType[table] = 0;

            string card1 = PokerTables[table][seat][0];
            string card2 = PokerTables[table][seat][1];
            string card3 = PokerTables[table][seat][2];

            int sum = 0;

            int c1 = GetCardType(card1);
            int c2 = GetCardType(card2);
            int c3 = GetCardType(card3);

            bool bVar = false;

            if (c1 != c2
                && c1 != c3
                && c2 != c3)
            {

                sum = c1 + c2 + c3;

                if (sum == 19)
                {
                    if ((((c1 == 14 || c1 == 2) || c1 == 3)
                        && ((c2 == 14 || c2 == 2) || c2 == 3))
                        && ((c3 == 14 || c3 == 2) || c3 == 3))
                    {
                           
                        bVar = true;
                    }
                }
                else if (sum == 9)
                {
                    if ((((c1 == 2 || c1 == 3) || c1 == 4)
                        && ((c2 == 2 || c2 == 3) || c2 == 4))
                        && ((c3 == 2 || c3 == 3) || c3 == 4))
                    {
                            
                        bVar = true;
                    }
                }
                else if (sum == 12)
                {
                    if ((((c1 == 3 || c1 == 4) || c1 == 5)
                        && ((c2 == 3 || c2 == 4) || c2 == 5))
                        && ((c3 == 3 || c3 == 4) || c3 == 5))
                    {
                            
                        bVar = true;
                    }
                }
                else if (sum == 15)
                {
                    if ((((c1 == 4 || c1 == 5) || c1 == 6)
                        && ((c2 == 4 || c2 == 5) || c2 == 6))
                        && ((c3 == 4 || c3 == 5) || c3 == 6))
                    {
                            
                        bVar = true;
                    }
                }
                else if (sum == 18)
                {
                    if ((((c1 == 5 || c1 == 6) || c1 == 7)
                        && ((c2 == 5 || c2 == 6) || c2 == 7))
                        && ((c3 == 5 || c3 == 6) || c3 == 7))
                    {
                           
                        bVar = true;
                    }
                }
                else if (sum == 21)
                {
                    if ((((c1 == 6 || c1 == 7) || c1 == 8)
                        && ((c2 == 6 || c2 == 7) || c2 == 8))
                        && ((c3 == 6 || c3 == 7) || c3 == 8))
                    {
                           
                        bVar = true;
                    }
                }
                else if (sum == 24)
                {
                    if ((((c1 == 7 || c1 == 8) || c1 == 9)
                        && ((c2 == 7 || c2 == 8) || c2 == 9))
                        && ((c3 == 7 || c3 == 8) || c3 == 9))
                    {
                           
                        bVar = true;
                    }
                }
                else if (sum == 27)
                {
                    if ((((c1 == 8 || c1 == 9) || c1 == 10)
                        && ((c2 == 8 || c2 == 9) || c2 == 10))
                        && ((c3 == 8 || c3 == 9) || c3 == 10))
                    {
                          
                        bVar = true;

                    }
                }
                else if (sum == 30)
                {
                    if ((((c1 == 9 || c1 == 10) || c1 == 11)
                        && ((c2 == 9 || c2 == 10) || c2 == 11))
                        && ((c3 == 9 || c3 == 10) || c3 == 11))
                    {
                           
                        bVar = true;

                    }
                }
                else if (sum == 33)
                {
                    if ((((c1 == 10 || c1 == 11) || c1 == 12)
                        && ((c2 == 10 || c2 == 11) || c2 == 12))
                        && ((c3 == 10 || c3 == 11) || c3 == 12))
                    {
                          
                        bVar = true;

                    }
                }
                else if (sum == 36)
                {
                    if ((((c1 == 11 || c1 == 12) || c1 == 13)
                        && ((c2 == 11 || c2 == 12) || c2 == 13))
                        && ((c3 == 11 || c3 == 12) || c3 == 13))
                    {
                          
                        bVar = true;

                    }
                }
                else if (sum == 39)
                {
                    if ((((c1 == 12 || c1 == 13) || c1 == 14)
                        && ((c2 == 12 || c2 == 13) || c2 == 14))
                        && ((c3 == 12 || c3 == 13) || c3 == 14))
                    {
                         
                        bVar = true;

                    }
                }

                if (GetCardSuit(card1) == GetCardSuit(card2) && GetCardSuit(card1) == GetCardSuit(card3))
                {
                    if (bVar)
                    {
                        if (winType <= 5) winType = 5;
                    }
                    else
                    {
                        if (winType <= 2) winType = 2;
                    }
                }
                else
                {
                    if (!bVar)
                    {
                        if (winType <= 0) winType = 0;
                    }
                    else
                    {
                        if (winType <= 3) winType = 3;
                    }
                }    
            }
            else if(c1 == c2 && c1 == c3)
            {
                if (winType <= 4) winType = 4;
            }
            else if(c1 == c2 || c2 == c3 || c1 == c3)
            {
                if (winType <= 1) winType = 1;
            }
            else
            {
                if (winType <= 0) winType = 0;
            }
             
            if(seat == 4)
            {
                PokerTableDealerWinType[table] = winType;
            }
            else
            {
                PokerTablePlayers[table][seat].SetData<int>("P_WIN_TYPE", winType);
            }
        }

        public static void Pokers(Player player, int table, int seat)
        {
       
            player.SetData("P_TABLE", table);
            player.SetData("P_SEAT", seat);
    
            player.SetData("P_STATUS", 0);
            player.SetData("P_ANSWER", 0);
            player.SetData("P_TIMER", 0);

            player.SetData("P_INBET", PokerTableMinBet[table]);

            PokerTablePlayers[table][seat] = player;

            PokerSeatsPlayers[table].Add(player);
            PokerTables[table][seat] = new List<string> { "", "", ""};
        }

        public static void Seat(Player player, int table, int seat)
        {
            if (player.HasData("P_TABLE"))
            {
                Exit(player);
                return;
            }

            if (PokerTablePlayers[table][seat] != null)
            {
                return;
            }

            Trigger.ClientEvent(player, "client_press_key_to", "close");
           

            Pokers(player, table, seat);
            Trigger.ClientEventInRange(player.Position, 20, "seat_to_poker_table", table, seat, player.Handle);
            Trigger.ClientEvent(player, "casinoPoker", "show");
            Trigger.ClientEvent(player, "casinoPoker", "setBet", PokerTableMinBet[player.GetData<int>("P_TABLE")], PokerTableMinBet[player.GetData<int>("P_TABLE")]);
            //Trigger.PlayerEvent(player, "seat_to_blackjack_table", 1, 0);
        }

        public static void Exit(Player player)
        {
            if (!player.HasData("P_TABLE"))
                return;
            if (player.GetData<int>("P_STATUS") != 0)
                return;
            if (PokerPlayers[player.GetData<int>("P_TABLE")].Contains(player))
                PokerPlayers[player.GetData<int>("P_TABLE")].Remove(player);

            PokerSeatsPlayers[player.GetData<int>("P_TABLE")].Remove(player);

            Trigger.ClientEvent(player, "casinoPoker", "hide");
            Trigger.ClientEventInRange(player.Position, 50, "exit_poker_table", player.Handle);
            ExitPlayer(player);
            //Trigger.PlayerEvent(player, "seat_to_blackjack_table", 1, 0);
        }

        public static void ExitPlayer(Player player)
        {
            int seat = player.GetData<int>("P_SEAT");
            int table = player.GetData<int>("P_TABLE");
            PokerTables[table][seat] = new List<string> {"", "", "" };
            PokerTablePlayers[table][seat] = null;

            player.ResetData("P_TABLE");
            player.ResetData("P_SEAT");
            player.ResetData("P_SUM");
            player.ResetData("P_CARDS");
            player.ResetData("P_STATUS");
            player.ResetData("P_ANSWER");
            player.ResetData("P_TIMER");
            player.ResetData("P_BET");

            Trigger.ClientEvent(player, "poker_hide");
        }

        [RemoteEvent("pokerBetDown")]
        public static void BetDown(Player player)
        {
            if (!player.HasData("P_TABLE"))
                return;

            if (PokerBetTime[player.GetData<int>("P_TABLE")] <= 0)
            {
                return;
            }

            int chips = player.GetData<int>("P_INBET");

            if (PokerTableMinBet[player.GetData<int>("P_TABLE")] > chips - PokerTableMinBet[player.GetData<int>("P_TABLE")])
            {
                return;
            }

            player.SetData("P_INBET", chips - PokerTableMinBet[player.GetData<int>("P_TABLE")]);
            Trigger.ClientEvent(player, "casinoPoker", "setBet", chips - PokerTableMinBet[player.GetData<int>("P_TABLE")], PokerTableMinBet[player.GetData<int>("P_TABLE")]);
        }

        [RemoteEvent("pokerBetUp")]
        public static void BetUp(Player player)
        {
            if (!player.HasData("P_TABLE"))
                return;

            if (PokerBetTime[player.GetData<int>("P_TABLE")] <= 0)
            {
                return;
            }

            int chips = player.GetData<int>("P_INBET");

            if (PokerTableMaxBet[player.GetData<int>("P_TABLE")] < chips + PokerTableMinBet[player.GetData<int>("P_TABLE")])
            {
                return;
            }

            player.SetData("P_INBET", chips + PokerTableMinBet[player.GetData<int>("P_TABLE")]);
            Trigger.ClientEvent(player, "casinoPoker", "setBet", chips + PokerTableMinBet[player.GetData<int>("P_TABLE")], PokerTableMinBet[player.GetData<int>("P_TABLE")]);

        }

        [RemoteEvent("pokerSetBet")]
        public static void Bet(Player player)
        {
            if (!player.HasData("P_TABLE"))
                return;

            if (PokerBetTime[player.GetData<int>("P_TABLE")] <= 0)
            {
               // CMD_Stand(player);
                return;
            }

            int chips = player.GetData<int>("P_INBET");

            if (DiamondCasino.GetAllChips(player) < chips * 2)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "У Вас недостаточно фишек. Должно быть в два раза больше ставки.", 3000);
                return;
            }


            if (PokerPlayers[player.GetData<int>("P_TABLE")].Contains(player))
            {
                //player.SetData("BJ_BET", player.GetData<int>("BJ_BET") + chips);
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Вы уже сделали ставку", 3000);
                return;
            }
            else
            {
                PokerPlayers[player.GetData<int>("P_TABLE")].Add(player);
                player.SetData("P_BET", chips);
            }

            nInventory.Remove(player, ItemType.CasinoChips, chips);

            Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"Вы сделали ставку в размере {chips}", 3000);
            Trigger.ClientEventInRange(player.Position, 150, "poker_bet", player.Handle);
            //Trigger.ClientEventInRange(player.Position, 50, "bet_poker", player.GetData<int>("P_TABLE"), player.GetData<int>("P_SEAT"), player.Handle);
            //Trigger.PlayerEvent(player, "seat_to_blackjack_table", 1, 0);
        }

        public static void ShuffleDeck(int table)
        { 

            Trigger.ClientEventInRange(TablePos[table], 20, "poker_shuffleDeck", table);
        }

        public static void CollectCard(int table, int seat)
        {

            Trigger.ClientEventInRange(TablePos[table], 20, "poker_collectCard", table, seat);

            
        }


        public static void GetCards(Player player, int table)
        {
            int seat = player.GetData<int>("P_SEAT");
           

            player.SetData("P_STATUS", 1);

            int randomCard = 0;

            Random rand = new Random();

            for (int i = 0; i < 3; i++)
            {
                randomCard = rand.Next(0, Cards.Count - 1);

                while (PokerTablePlayCards[table].Contains(randomCard))
                    randomCard = rand.Next(0, Cards.Count - 1);

                PokerTablePlayCards[table].Add(randomCard);

                PokerTables[table][seat][i] = Cards[randomCard].Substring(17);

                Log.Write(PokerTables[table][seat][i]);
            }

            Trigger.ClientEventInRange(TablePos[table], 20, "poker_dealCards", table, seat, JsonConvert.SerializeObject(new List<string>(0) { PokerTables[table][seat][0], PokerTables[table][seat][1], PokerTables[table][seat][2] }));
        }

        public static void GetCardsDealer(int table)
        {
            int randomCard = 0;

            Random rand = new Random();

            int mainCard = rand.Next(0, 3);

            for (int i = 0; i < 3; i++)
            {
                if (i == mainCard)
                {
                    if (rand.Next(0, 2) != 0)
                    {
                        randomCard = rand.Next(0, MainCards.Count - 1);

                        while (PokerTablePlayCards[table].Contains(randomCard))
                            randomCard = rand.Next(0, MainCards.Count - 1);

                        PokerTables[table][4][i] = MainCards[randomCard].Substring(17);
                    }
                    else
                    {
                        randomCard = rand.Next(0, Cards.Count - 1);

                        while (PokerTablePlayCards[table].Contains(randomCard))
                            randomCard = rand.Next(0, Cards.Count - 1);

                        PokerTables[table][4][i] = Cards[randomCard].Substring(17);
                    }
                }
                else
                {

                    randomCard = rand.Next(0, Cards.Count - 1);

                    while (PokerTablePlayCards[table].Contains(randomCard))
                        randomCard = rand.Next(0, Cards.Count - 1);

                    PokerTables[table][4][i] = Cards[randomCard].Substring(17);
                }

                PokerTablePlayCards[table].Add(randomCard);

                Log.Write($"d {PokerTables[table][4][i]}");
            }

            Trigger.ClientEventInRange(TablePos[table], 20, "poker_dealCards", table, -1, "[]");
        }

        [Command("revp")]
        public static void revp(Player player)
        {
            player.TriggerEvent("revealSelf");
        }
    }
}
