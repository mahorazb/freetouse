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
using TexasHoldem;
using static TexasHoldem.Card;
using Trigger = Redage.SDK.Trigger;

namespace client.Core
{
    class TexasPoker : Script
    {
        private static nLog Log = new nLog("TexasPoker");


        [ServerEvent(Event.ResourceStart)]
        public static void OnResourceStart()
        {
            int i = 0;

            foreach(Vector3 pos in Tables)
            {
                PlayerJoined.Add(new List<Player>() );
                PlayerPlaying.Add(new List<PockerPlayer>() );
                PlayerAccepted.Add(0);
                PlayerTable.Add(new List<PockerPlayer>() { null, null, null, null, null });
                TableCards.Add(new List<Card>() );
                TableDeck.Add(new List<Card>() );
                BetSpot.Add(0);
                LastBet.Add(0);
                TableTimer.Add(0);
                TableDealer.Add(0);
                TableStatus.Add(0);

                var colshape = NAPI.ColShape.CreateCylinderColShape(pos, 2, 3);
                colshape.SetData("TABLE", i);
  
                colshape.OnEntityEnterColShape += (shape, player) =>
                {
                    player.SetData("INTERACTIONCHECK", 666);
                    player.SetData("TABLE", shape.GetData<int>("TABLE"));
                    //Trigger.ClientEvent(player, "client_press_key_to", "open", JsonConvert.SerializeObject(new List<object>() { "E", "сесть за стол" }));
                };
                colshape.OnEntityExitColShape += (shape, player) =>
                {
                    Trigger.ClientEvent(player, "client_press_key_to", "close");
                    player.SetData("INTERACTIONCHECK", 0);
                    player.ResetData("TABLE");
                };

                NAPI.TextLabel.CreateTextLabel($"Стол #{i+1} [E]\n\nСтавка ~y~{TableBets[i].Item1}~w~\nВход от ~y~{TableBets[i].Item2}", pos + new Vector3(0, 0, 0.3), 30, 2, 0, new Color(255, 255, 255), true);

                Thread thd = new Thread(new ParameterizedThreadStart(StartDealing));
                thd.Start(i);

           
                i++;
            }
        }

        public static List<Vector3> Tables = new List<Vector3>()
        {
            new Vector3(1133.7095, 261.65905, -51.5409),
        };

        public static List<Tuple<int, int>> TableBets = new List<Tuple<int, int>>()
        {
            new Tuple<int, int>(50, 2000),
        };

        public static Dictionary<int, string> WinTypes = new Dictionary<int, string>()
        {
            {10, "ROYAL-FLUSH" },
            {9, "STRAIGHT-FLUSH" },
            {8, "CARE" },
            {7, "FULL HOUSE" },
            {6, "FLUSH" },
            {5, "STRAIGHT" },
            {4, "SET" },
            {3, "TWO PAIR" },
            {2, "PAIR" },
            {1, "HIGH CARD" },
        };


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


        public static List<List<Player>> PlayerJoined = new List<List<Player>>();
        public static List<PockerPlayer> QueueRemoveBots = new List<PockerPlayer>();
        public static List<List<PockerPlayer>> PlayerPlaying = new List<List<PockerPlayer>>() ;
        public static List<int> PlayerAccepted = new List<int>() { };

        public static List<List<PockerPlayer>> PlayerTable = new List<List<PockerPlayer>>() ;
        public static List<List<Card>> TableCards = new List<List<Card>>();
        public static List<List<Card>> TableDeck = new List<List<Card>>() ;
        public static List<int> TableStatus = new List<int>();

        public static Random rand = new Random();

        public static List<int> LastBet = new List<int>();
        public static List<int> BetSpot = new List<int>();
        public static List<int> TableTimer = new List<int>();
        public static List<int> TableDealer = new List<int>();

        public static int CheckComb(PockerPlayer player, int table)
        {
            try
            {
                List<Card> playerCards = player.Cards;

                if (HoldemComb.IsRoyalFlush(TableCards[table], playerCards).Item1)
                {
                    player.WinnerCards = HoldemComb.IsRoyalFlush(TableCards[table], playerCards).Item2;
                    return 10;
                }
                else if (HoldemComb.IsStraight(TableCards[table], playerCards).Item1 && HoldemComb.IsFlush(TableCards[table], playerCards).Item1)
                {
                    player.WinnerCards = HoldemComb.IsStraight(TableCards[table], playerCards).Item2;
                    return 9;
                }
                else if (HoldemComb.IsCare(TableCards[table], playerCards).Item1)
                {
                    player.WinnerCards = HoldemComb.IsCare(TableCards[table], playerCards).Item2;
                    return 8;
                }
                else if (HoldemComb.IsFullHouse(TableCards[table], playerCards).Item1)
                {
                    player.WinnerCards = HoldemComb.IsFullHouse(TableCards[table], playerCards).Item2;
                    return 7;
                }
                else if (HoldemComb.IsFlush(TableCards[table], playerCards).Item1)
                {
                    player.WinnerCards = HoldemComb.IsFlush(TableCards[table], playerCards).Item2;
                    return 6;
                }
                else if (HoldemComb.IsStraight(TableCards[table], playerCards).Item1)
                {
                    player.WinnerCards = HoldemComb.IsStraight(TableCards[table], playerCards).Item2;
                    return 5;
                }
                else if (HoldemComb.IsSet(TableCards[table], playerCards).Item1)
                {
                    player.WinnerCards = HoldemComb.IsSet(TableCards[table], playerCards).Item2;
                    return 4;
                }
                else if (HoldemComb.IsTwoPair(TableCards[table], playerCards).Item1)
                {
                    player.WinnerCards = HoldemComb.IsTwoPair(TableCards[table], playerCards).Item2;
                    return 3;
                }
                else if (HoldemComb.IsPair(TableCards[table], playerCards).Item1)
                {
                    player.WinnerCards = HoldemComb.IsPair(TableCards[table], playerCards).Item2;
                    return 2;
                }
                else
                {
                    player.WinnerCards = playerCards;
                    return 1;
                }
            }
            catch(Exception ex)
            {
                Log.Write(ex.ToString());
                return 1;
            }
        }

        public static PockerPlayer GetWinner(int table)
        {
            try
            {
                Dictionary<PockerPlayer, int> combs = new Dictionary<PockerPlayer, int>();

                for (int i = 0; i < PlayerTable[table].Count; i++)
                {
                    if (PlayerTable[table][i] == null) continue;

                    if (!PlayerPlaying[table].Contains(PlayerTable[table][i])) continue;

                    int comb = CheckComb(PlayerTable[table][i], table);

                    combs.Add(PlayerTable[table][i], comb);

                    PlayerTable[table][i].LastAction = WinTypes[comb];
                }

                List<KeyValuePair<PockerPlayer, int>> myList = combs.ToList();

                myList.Sort(
                    delegate (KeyValuePair<PockerPlayer, int> pair1,
                    KeyValuePair<PockerPlayer, int> pair2)
                    {
                        return pair2.Value.CompareTo(pair1.Value);
                    }
                );

                List<KeyValuePair<PockerPlayer, int>> ls = myList.Where(x => x.Value == myList[0].Value).ToList();

                if (ls.Count == 1)
                {
                    return ls[0].Key;
                }
                else
                {
                    KeyValuePair<PockerPlayer, int> hCard = ls[0];

                    Card card = ls[0].Key.Cards.OrderByDescending(x => x.cardValue).ToList()[0];
                    Card card2 = ls[0].Key.Cards.OrderByDescending(x => x.cardValue).ToList()[1];

                    for (int i = 1; i < ls.Count; i++)
                    {
                        List<Card> tempCards = ls[i].Key.Cards.OrderByDescending(x => x.cardValue).ToList();

                        if (tempCards[0].cardValue > card.cardValue)
                        {
                            hCard = ls[i];
                            card = tempCards[0];
                        }
                        else if (tempCards[0].cardValue == card.cardValue && tempCards[1].cardValue > card2.cardValue)
                        {
                            hCard = ls[i];
                            card = tempCards[0];
                        }
                    }

                    return hCard.Key;
                }
            }
            catch(Exception ex)
            {
                Log.Write(ex.ToString());
                return null;
            }
        }

        public static void SetPlayers(int table)
        {
            List<object> data = new List<object>();

            for(int i = 0; i < PlayerTable[table].Count; i++)
            {
                if (PlayerTable[table][i] == null)
                {
                    Dictionary<string, object> dict = new Dictionary<string, object>();

                    dict.Add("active", false);
                    dict.Add("now", false);
                    dict.Add("chips", 0);
                    dict.Add("name", "assa");
                    dict.Add("lastAction", "assa");
                    dict.Add("winner", false);
                    //dict.Add("fold", PlayerTable[i].Fold);

                    data.Add(dict);
                }
                else
                {
                    Dictionary<string, object> dict = new Dictionary<string, object>();

                    dict.Add("active", PlayerTable[table][i].IsActive);
                    dict.Add("now", PlayerTable[table][i].IsActiveNow);
                    dict.Add("chips", PlayerTable[table][i].Bet);
                    dict.Add("name", PlayerTable[table][i].Name);
                    dict.Add("lastAction", PlayerTable[table][i].LastAction);
                    dict.Add("winner", PlayerTable[table][i].Winner);
                    //dict.Add("fold", PlayerTable[i].Fold);

                    data.Add(dict);
                }
            }

            if(TableTimer[table] == 0)
                Trigger.ClientEventToPlayers(PlayerJoined[table].ToArray(), "pocker_TimerSet", false, 0);
            else if (TableTimer[table] == 1)
                Trigger.ClientEventToPlayers(PlayerJoined[table].ToArray(), "pocker_TimerSet", false, 0);
            else if (TableTimer[table] > 1)
                Trigger.ClientEventToPlayers(PlayerJoined[table].ToArray(), "pocker_TimerSet", true, TableTimer[table] - 1);

            
            Trigger.ClientEventToPlayers(PlayerJoined[table].ToArray(), "pocker_setPlayers", JsonConvert.SerializeObject(data), BetSpot[table]);
        }

        public static void AddPlayerCards(PockerPlayer player)
        {
            Trigger.ClientEventToPlayers(PlayerJoined[player.Table].ToArray(), "addPlayerCard", player.Seat);

            if (!player.IsBot)
            {
                if (Main.Players.ContainsKey(player.Handle))
                {
                    string card1 = $"{HoldemComb.CardsTypes[player.Cards[0].cardType]}_{HoldemComb.CardsValues[player.Cards[0].cardValue]}";
                    string card2 = $"{HoldemComb.CardsTypes[player.Cards[1].cardType]}_{HoldemComb.CardsValues[player.Cards[1].cardValue]}";

                    NAPI.Task.Run(() =>
                    {
                        Trigger.ClientEvent(player.Handle, "openPlayerCard", player.Seat, card1, card2);
                    }, 255);
                }
            }
        }

        public static void OpenPlayerCards(PockerPlayer player)
        {
            string card1 = $"{HoldemComb.CardsTypes[player.Cards[0].cardType]}_{HoldemComb.CardsValues[player.Cards[0].cardValue]}";
            string card2 = $"{HoldemComb.CardsTypes[player.Cards[1].cardType]}_{HoldemComb.CardsValues[player.Cards[1].cardValue]}";

            Trigger.ClientEventToPlayers(PlayerJoined[player.Table].ToArray(), "openPlayerCard", player.Seat, card1, card2);
        }

        public static void AddTableCards(int table)
        {
            List<string> cards = new List<string>();

            cards.Add($"{HoldemComb.CardsTypes[TableCards[table][0].cardType]}_{HoldemComb.CardsValues[TableCards[table][0].cardValue]}");
            cards.Add($"{HoldemComb.CardsTypes[TableCards[table][1].cardType]}_{HoldemComb.CardsValues[TableCards[table][1].cardValue]}");
            cards.Add($"{HoldemComb.CardsTypes[TableCards[table][2].cardType]}_{HoldemComb.CardsValues[TableCards[table][2].cardValue]}");

            Trigger.ClientEventToPlayers(PlayerJoined[table].ToArray(), "addTableCards", JsonConvert.SerializeObject(cards));
        }

        public static void AddTableCard(int table, int num)
        {
            string card = $"{HoldemComb.CardsTypes[TableCards[table][num].cardType]}_{HoldemComb.CardsValues[TableCards[table][num].cardValue]}";

            Trigger.ClientEventToPlayers(PlayerJoined[table].ToArray(), "addTableCard", num, card);
        }

        public static void ClearCards(int table)
        {
            Trigger.ClientEventToPlayers(PlayerJoined[table].ToArray(), "clearCards");
        }
        public static void CleanPlayerCards(PockerPlayer player)
        {
            Trigger.ClientEventToPlayers(PlayerJoined[player.Table].ToArray(), "cleanPlayerCards", player.Seat);
        }

        public static void ShowWinnerCards(PockerPlayer player)
        {
            List<int> playerCards = new List<int>();
            List<int> tableCards = new List<int>();

            List<Card> winnerCards = player.WinnerCards;

            foreach(Card card in winnerCards)
            {
                if (player.Cards.Contains(card))
                {
                    playerCards.Add(player.Cards.IndexOf(card));
                }
                else
                {
                    tableCards.Add(TableCards[player.Table].IndexOf(card));
                }
            }

            Trigger.ClientEventToPlayers(PlayerJoined[player.Table].ToArray(), "showWinnerCards", player.Seat, JsonConvert.SerializeObject(playerCards), JsonConvert.SerializeObject(tableCards));
        }



        public static void StartDealing(object d)
        {
            try
            {
                int table = Convert.ToInt32(d);

                while (true)
                {
                   
                    TableDeck[table].Clear();
                    TableCards[table].Clear();
                    PlayerPlaying[table].Clear();

                    TableStatus[table] = -1;

                    if (QueueRemoveBots.Count != 0)
                    {
                        for(int i = 0; i < QueueRemoveBots.Count; i++)
                        {
                            if(QueueRemoveBots[i].Table == table)
                            {
                                PlayerTable[table][QueueRemoveBots[i].Seat] = null;
                                QueueRemoveBots.RemoveAt(i);
                            }
                        }
                    }

                    if (TableTimer[table] == 0)
                    {

                        int count = 0;
                        for (int i = 0; i < PlayerTable[table].Count; i++)
                        {

                            if (PlayerTable[table][i] != null)
                            {
                                if (PlayerTable[table][i].IsBot)
                                {
                                    count++;
                                }
                                else
                                {
                                    if (DiamondCasino.GetAllChips(PlayerTable[table][i].Handle) < TableBets[table].Item2)
                                    {
                                        ExitPockerTable(PlayerTable[table][i].Handle);
                                        Notify.Send(PlayerTable[table][i].Handle, NotifyType.Info, NotifyPosition.Center, "У вас нехватает фишек для игры", 3000);
                                        continue;
                                    }
                                    else count++;
                                }
                            }
                          
                        }

                        if (count >= 2)
                        {
                            TableTimer[table] = 15;
                            SetPlayers(table);
                            continue;
                        }
                        else
                        {
                            SetPlayers(table);
                            Thread.Sleep(1000);
                            continue;
                        }
                    }
                    else if (TableTimer[table] > 1)
                    {
                        int count = 0;
                        for (int i = 0; i < PlayerTable[table].Count; i++)
                        {
                            if (PlayerTable[table][i] != null) count++;
                        }

                        if (count < 2)
                        {
                            TableTimer[table] = 0;
                            Thread.Sleep(1000);
                            continue;
                        }

                        for (int i = 0; i < PlayerTable[table].Count; i++)
                        {
                            if (PlayerTable[table][i] != null)
                            {
                                if (!PlayerTable[table][i].IsBot)
                                {
                                    ///PlayerTable[table][i].Handle.SendChatMessage($"Обратный отсчет [{TableTimer[table]}]");
                                }
                            }
                        }

                        TableTimer[table]--;
                        SetPlayers(table);
                        Thread.Sleep(1000);
                        continue;
                    }
                    else if (TableTimer[table] == 1)
                    {
                        int count = 0;
                        for (int i = 0; i < PlayerTable[table].Count; i++)
                        {
                            if (PlayerTable[table][i] != null)
                            {
                                PlayerPlaying[table].Add(PlayerTable[table][i]);
                                count++;
                            }

                        }

                        if (count < 2)
                        {
                            TableTimer[table] = 0;
                            Thread.Sleep(1000);
                            continue;
                        }
                    }

                    TableTimer[table] = 0;

                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 1; j < 14; j++)
                        {
                            TableDeck[table].Add(new Card((eCardType)i, (eCardValue)j));
                        }
                    }

                    Shuffle(TableDeck[table]);

                    BetSpot[table] = 0;


                    for (int i = 0; i < PlayerPlaying[table].Count; i++)
                    {
                        PockerPlayer pplayer = PlayerPlaying[table][i];
                        pplayer.IsActive = true;
                        if (!pplayer.IsBot) Trigger.ClientEvent(PlayerPlaying[table][i].Handle, "pocker_setMySeat", PlayerPlaying[table][i].Seat);
                    }

                    SetPlayers(table);

                    Thread.Sleep(1000);

                    int n = TableDealer[table] + 1;
                    int k = 0;
                    int bet = TableBets[table].Item1;

                    TableDealer[table] = n;

                    if (n > PlayerPlaying[table].Count - 1) n = 0;
              


                    while (k < 2)
                    {
                      
                        if (k == 0)
                        {
                            PlayerPlaying[table][n].Bet = bet / 2;
                            PlayerPlaying[table][n].RoundBet = bet / 2;
                            PlayerPlaying[table][n].LastAction = "small blind";
                            if (!PlayerPlaying[table][n].IsBot)
                                nInventory.Remove(PlayerTable[table][n].Handle, ItemType.CasinoChips, bet / 2);
                        }
                        else if (k == 1)
                        {
                            PlayerPlaying[table][n].Bet = bet;
                            PlayerPlaying[table][n].RoundBet = bet;
                            PlayerPlaying[table][n].LastAction = "big blind";

                            if (!PlayerPlaying[table][n].IsBot)
                                nInventory.Remove(PlayerTable[table][n].Handle, ItemType.CasinoChips, bet);
                        }

                        if (n >= PlayerPlaying[table].Count - 1) n = 0;
                        else n++;

                        k++;
                    }

                    if (n >= PlayerPlaying[table].Count - 1) n = 0;
                    else n++;

                    BetSpot[table] += bet + (bet / 2);
                    LastBet[table] = bet;

                    SetPlayers(table);

                    Thread.Sleep(1000);

                    Shuffle(TableDeck[table]);

                    for (int i = 0; i < PlayerTable[table].Count; i++)
                    {

                        if (PlayerTable[table][i] == null) continue;

                        PlayerTable[table][i].Cards = new List<Card>() { TableDeck[table][TableDeck[table].Count - 1], TableDeck[table][TableDeck[table].Count - 2] };
                        TableDeck[table].RemoveAt(TableDeck[table].Count - 1);
                        TableDeck[table].RemoveAt(TableDeck[table].Count - 1);

                        //Log.Write($"{CardsTypes[PlayerTable[i].Cards[0].cardType]} {CardsValues[PlayerTable[i].Cards[0].cardValue]}");
                        // Log.Write($"{CardsTypes[PlayerTable[i].Cards[1].cardType]} {CardsValues[PlayerTable[i].Cards[1].cardValue]}");

                        AddPlayerCards(PlayerTable[table][i]);

                        // Thread.Sleep(500);
                    }

                    TableStatus[table] = 0;

                    for (int j = 0; j < 4; j++)
                    {
                        if (j == 0) LastBet[table] = 50;
                        else LastBet[table] = 0;

                        PlayerAccepted[table] = 0;

                        while (PlayerAccepted[table] != PlayerPlaying[table].Count && PlayerPlaying[table].Count != 1)
                        {
                            if (PlayerTable[table][n] == null)
                            {
                                if (n >= PlayerTable[table].Count - 1) n = 0;
                                else n++;
                                continue;
                            }

                            if (PlayerPlaying[table].Contains(PlayerTable[table][n]))
                            {
                                PlayerTable[table][n].IsActiveNow = true;
                                PlayerTable[table][n].LastAction = "...";

                                SetPlayers(table);

                                if (PlayerTable[table][n].IsBot)
                                {
                                    Thread.Sleep(500);

                                    int val = 0;

                                    if (LastBet[table] == 0)
                                        val = rand.Next(1, 6);
                                    else
                                        val = rand.Next(0, 6);

                                    if (val == 0)
                                    {
                                        PlayerTable[table][n].Fold = true;

                                        PlayerPlaying[table].Remove(PlayerTable[table][n]);

                                        PlayerTable[table][n].LastAction = "fold";

                                        // Log.Write($"{PlayerTable[n].Name} fold | {PlayerTable[n].Bet}");

                                        CleanPlayerCards(PlayerTable[table][n]);
                                    }
                                    else if (val >= 1 && val <= 4)
                                    {
                                        if (PlayerTable[table][n].Bet < LastBet[table])
                                        {
                                            int spot = LastBet[table] - PlayerTable[table][n].Bet;

                                            BetSpot[table] += spot;
                                            PlayerTable[table][n].Bet += spot;
                                            PlayerTable[table][n].RoundBet += spot;
                                            PlayerAccepted[table]++;

                                            // Log.Write($"{PlayerTable[n].Name} call {spot} | {PlayerTable[n].Bet}");

                                            PlayerTable[table][n].LastAction = $"call {spot}";

                                            Trigger.ClientEventToPlayers(PlayerJoined[table].ToArray(), "pockerBetSound");
                                        }
                                        else
                                        {
                                            PlayerAccepted[table]++;

                                            //Log.Write($"{PlayerTable[n].Name} check | {PlayerTable[n].Bet}");

                                            PlayerTable[table][n].LastAction = $"check";

                                        }
                                    }
                                    else if (val >= 5)
                                    {
                                        if (LastBet[table] == 0)
                                        {
                                           
                                            int spot = rand.Next(TableBets[table].Item1, TableBets[table].Item1 + 250);

                                            if (PlayerTable[table][n].Bet + spot > TableBets[table].Item2)
                                            {
                                                PlayerAccepted[table]++;

                                                //Log.Write($"{PlayerTable[n].Name} check | {PlayerTable[n].Bet}");

                                                PlayerTable[table][n].LastAction = $"check";
                                            }
                                            else
                                            {

                                                PlayerTable[table][n].Bet += spot;

                                                LastBet[table] += PlayerTable[table][n].Bet;
                                                BetSpot[table] += spot;
                                                PlayerTable[table][n].RoundBet += spot;

                                                PlayerAccepted[table] = 1;

                                                //Log.Write($"{PlayerTable[n].Name} bet {spot} | {PlayerTable[n].Bet}");

                                                PlayerTable[table][n].LastAction = $" bet {spot}";

                                                Trigger.ClientEventToPlayers(PlayerJoined[table].ToArray(), "pockerBetSound");
                                            }
                                        }
                                        else
                                        {
                                            int spot = 0;

                                            if (PlayerTable[table][n].Bet < LastBet[table])
                                            {
                                                spot = LastBet[table] - PlayerTable[table][n].Bet;

                                                BetSpot[table] += spot;
                                                PlayerTable[table][n].Bet += spot;
                                                PlayerTable[table][n].RoundBet += spot;

                                            }

                                            spot = rand.Next(TableBets[table].Item1, TableBets[table].Item1 + 250);

                                            if (PlayerTable[table][n].Bet + spot > TableBets[table].Item2)
                                            {
                                                spot = LastBet[table] - PlayerTable[table][n].Bet;

                                                BetSpot[table] += spot;
                                                PlayerTable[table][n].Bet += spot;
                                                PlayerTable[table][n].RoundBet += spot;
                                                PlayerAccepted[table]++;

                                                // Log.Write($"{PlayerTable[n].Name} call {spot} | {PlayerTable[n].Bet}");

                                                PlayerTable[table][n].LastAction = $"call {spot}";

                                                Trigger.ClientEventToPlayers(PlayerJoined[table].ToArray(), "pockerBetSound");
                                            }
                                            else
                                            {
                                                PlayerTable[table][n].Bet += spot;

                                                LastBet[table] = PlayerTable[table][n].Bet;

                                                PlayerTable[table][n].RoundBet += spot;
                                                BetSpot[table] += spot;
                                                PlayerAccepted[table] = 1;

                                                // Log.Write($"{PlayerTable[n].Name} rise up {spot} | {PlayerTable[n].Bet}");

                                                PlayerTable[table][n].LastAction = $"rise up {spot} ";

                                                Trigger.ClientEventToPlayers(PlayerJoined[table].ToArray(), "pockerBetSound");
                                            }

                                        }

                                        
                                    }
                                }
                                else
                                {
                                    Player player = PlayerTable[table][n].Handle;

                                    while (true)
                                    {

                                        if (!Main.Players.ContainsKey(player))
                                        {
                                            PlayerTable[table][n].Fold = true;

                                            PlayerTable[table][n].IsActiveNow = false;

                                            PlayerPlaying[table].Remove(PlayerTable[table][n]);

                                            PlayerTable[table][n] = null;

                                            n--;
                                            break;
                                        }

                                        if (PlayerTable[table][n].Answer == -1)
                                        {
                                            int spot = LastBet[table] - PlayerTable[table][n].Bet;

                                            string button2 = $"CALL {spot}";
                                            string button3 = "RISE UP";

                                            if (PlayerTable[table][n].Bet >= LastBet[table])
                                                button2 = "CHECK";
                                            if (LastBet[table] == 0)
                                                button3 = "BET";

                                            Trigger.ClientEvent(player, "pocker_setButtons", button2, button3);
                                            Trigger.ClientEvent(player, "pocker_showPanel");

                                            player.SetData("PT_SEAT", n);
                                            PlayerTable[table][n].Answer = 0;
                                            PlayerTable[table][n].Timer = 0;
                                        }
                                        else if (PlayerTable[table][n].Answer == 0)
                                        {
                                            if (PlayerTable[table][n].Timer >= 30)
                                            {
                                                PlayerTable[table][n].Fold = true;

                                                PlayerPlaying[table].Remove(PlayerTable[table][n]);

                                                PlayerTable[table][n].LastAction = "fold";

                                                CleanPlayerCards(PlayerTable[table][n]);

                                                PlayerTable[table][n].Timer = 0;
                                                PlayerTable[table][n].Answer = -1;

                                                Trigger.ClientEvent(player, "pocker_hidePanel");
                                                break;
                                            }
                                            else
                                            {
                                                PlayerTable[table][n].Timer += 1;
                                                Thread.Sleep(1000);

                                            }
                                        }
                                        else
                                        {
                                            if (PlayerTable[table][n].Answer == 1)
                                            {
                                                PlayerTable[table][n].Fold = true;

                                                PlayerPlaying[table].Remove(PlayerTable[table][n]);

                                                PlayerTable[table][n].LastAction = "fold";

                                                CleanPlayerCards(PlayerTable[table][n]);
                                            }
                                            else if (PlayerTable[table][n].Answer == 2)
                                            {
                                                if (PlayerTable[table][n].Bet < LastBet[table])
                                                {
                                                    int spot = LastBet[table] - PlayerTable[table][n].Bet;

                                                    nInventory.Remove(PlayerTable[table][n].Handle, ItemType.CasinoChips, spot);

                                                    BetSpot[table] += spot;
                                                    PlayerTable[table][n].Bet += spot;
                                                    PlayerTable[table][n].RoundBet += spot;
                                                    PlayerAccepted[table]++;

                                                    // Log.Write($"{PlayerTable[n].Name} call {spot} | {PlayerTable[n].Bet}");

                                                    PlayerTable[table][n].LastAction = $"call {spot}";

                                                    Trigger.ClientEventToPlayers(PlayerJoined[table].ToArray(), "pockerBetSound");
                                                }
                                                else
                                                {
                                                    PlayerAccepted[table]++;

                                                    //Log.Write($"{PlayerTable[n].Name} check | {PlayerTable[n].Bet}");

                                                    PlayerTable[table][n].LastAction = $"check";

                                                }
                                            }
                                            else if (PlayerTable[table][n].Answer == 3)
                                            {
                                                if (LastBet[table] == 0)
                                                {
                                                    int spot = PlayerTable[table][n].Count;

                                                    nInventory.Remove(PlayerTable[table][n].Handle, ItemType.CasinoChips, spot);

                                                    PlayerTable[table][n].Bet += spot;

                                                    LastBet[table] += PlayerTable[table][n].Bet;
                                                    BetSpot[table] += spot;
                                                    PlayerTable[table][n].RoundBet += spot;

                                                    PlayerAccepted[table] = 1;

                                                    //Log.Write($"{PlayerTable[n].Name} bet {spot} | {PlayerTable[n].Bet}");

                                                    PlayerTable[table][n].LastAction = $" bet {spot}";
                                                }
                                                else
                                                {
                                                    int spot = 0;

                                                    if (PlayerTable[table][n].Bet < LastBet[table])
                                                    {
                                                        spot = LastBet[table] - PlayerTable[table][n].Bet;

                                                        BetSpot[table] += spot;
                                                        PlayerTable[table][n].Bet += spot;
                                                        PlayerTable[table][n].RoundBet += spot;

                                                        nInventory.Remove(PlayerTable[table][n].Handle, ItemType.CasinoChips, spot);

                                                    }

                                                    spot = PlayerTable[table][n].Count;

                                                    nInventory.Remove(PlayerTable[table][n].Handle, ItemType.CasinoChips, spot);

                                                    PlayerTable[table][n].Bet += spot;

                                                    LastBet[table] = PlayerTable[table][n].Bet;

                                                    PlayerTable[table][n].RoundBet += spot;
                                                    BetSpot[table] += spot;
                                                    PlayerAccepted[table] = 1;

                                                    // Log.Write($"{PlayerTable[n].Name} rise up {spot} | {PlayerTable[n].Bet}");

                                                    PlayerTable[table][n].LastAction = $"rise up {spot} ";

                                                }

                                                Trigger.ClientEventToPlayers(PlayerJoined[table].ToArray(), "pockerBetSound");
                                            }

                                            PlayerTable[table][n].Timer = 0;
                                            PlayerTable[table][n].Answer = -1;
                                            PlayerTable[table][n].Count = 0;

                                            PlayerTable[table][n].Handle.TriggerEvent("poker_SetChips", DiamondCasino.GetAllChips(PlayerTable[table][n].Handle));

                                            Trigger.ClientEvent(player, "pocker_hidePanel");
                                            break;

                                        }
                                    }
                                }
                                SetPlayers(table);
                                Thread.Sleep(500);

                                if (PlayerTable[table].Count - 1 >= n) PlayerTable[table][n].IsActiveNow = false;

                            }

                            if (n >= PlayerTable[table].Count - 1) n = 0;
                            else n++;
                        }


                        for (int i = 0; i < PlayerTable[table].Count; i++)
                        {
                            if (PlayerTable[table][i] == null) continue;

                            if (PlayerPlaying[table].Contains(PlayerTable[table][i])) PlayerTable[table][i].LastAction = "";
                        }

                        SetPlayers(table);

                        Thread.Sleep(1000);

                        n = 1;

                        if (j == 3)
                        {
                            for (int i = 0; i < PlayerTable[table].Count; i++)
                            {
                                if (PlayerTable[table][i] == null) continue;

                                if (PlayerPlaying[table].Contains(PlayerTable[table][i]))
                                {
                                    OpenPlayerCards(PlayerTable[table][i]);
                                }

                                Thread.Sleep(150);
                            }

                            try
                            {
                                for (int i = 0; i < PlayerTable[table].Count; i++)
                                {
                                    if (PlayerTable[table][i] == null) continue;

                                    PlayerTable[table][i].Bet = 0;
                                    if (!PlayerTable[table][i].IsBot)
                                    {
                                        Trigger.ClientEvent(PlayerTable[table][i].Handle, "pocker_setMySeat", -1);


                                    }
                                }

                                PockerPlayer winner = GetWinner(table);

                                winner.Winner = true;

                                if (!winner.IsBot)
                                {
                                    nInventory.Add(winner.Handle, new nItem(ItemType.CasinoChips, winner.Bet));
                                    winner.Handle.TriggerEvent("poker_SetChips", DiamondCasino.GetAllChips(winner.Handle));
                                }

                                winner.Bet = BetSpot[table];

                                BetSpot[table] = 0;

                                SetPlayers(table);

                                ShowWinnerCards(winner);
                            }
                            catch (Exception ex)
                            {
                                Log.Write(ex.ToString());
                            }

                            Thread.Sleep(5000);


                            for (int i = 0; i < PlayerTable[table].Count; i++)
                            {
                                if (PlayerTable[table][i] == null) continue;
                                PlayerTable[table][i].RoundBet = 0;
                                PlayerTable[table][i].Winner = false;
                                PlayerTable[table][i].Bet = 0;
                                PlayerTable[table][i].LastAction = "";
                            }

                            SetPlayers(table);
                            //Log.Write("Finish");
                        }
                        else
                        {
                            if (j == 0)
                            {
                                TableStatus[table] = 1;
                                TableCards[table] = new List<Card>();
                                TableCards[table].Add(TableDeck[table][TableDeck[table].Count - 1]);
                                TableDeck[table].RemoveAt(TableDeck[table].Count - 1);
                                TableCards[table].Add(TableDeck[table][TableDeck[table].Count - 1]);
                                TableDeck[table].RemoveAt(TableDeck[table].Count - 1);
                                TableCards[table].Add(TableDeck[table][TableDeck[table].Count - 1]);
                                TableDeck[table].RemoveAt(TableDeck[table].Count - 1);

                                AddTableCards(table);
                            }
                            else
                            {
                                TableStatus[table]++;

                                TableCards[table].Add(TableDeck[table][TableDeck[table].Count - 1]);
                                TableDeck[table].RemoveAt(TableDeck[table].Count - 1);

                                AddTableCard(table, j + 2);
                            }

                            // OutputTableCards();
                            // Log.Write($"Table spot: {BetSpot}");


                            SetPlayers(table);
                            Thread.Sleep(2000);

                            for (int i = 0; i < PlayerTable[table].Count; i++)
                            {
                                if (PlayerTable[table][i] == null) continue;
                                PlayerTable[table][i].RoundBet = 0;
                            }
                        }

                    }

                    ClearCards(table);
                    Thread.Sleep(3000);
                }
            }
            catch(Exception ex)
            {
                Log.Write(ex.ToString());
            }
        }

        [RemoteEvent("exit_poker_table")]
        public static void ExitPockerTable(Player player)
        {
            if (!player.HasData("PT_SEAT")) return;

            int seat = player.GetData<int>("PT_SEAT");
            int table = player.GetData<int>("PT_TABLE");

            if (PlayerTable[table][seat] == null) return;

            if (PlayerTable[table][seat].Handle != player) return;

            PlayerTable[table][seat] = null;
            PlayerJoined[table].Remove(player);

            player.ResetData("PT_SEAT");
            player.ResetData("PT_TABLE");

            SetPlayers(table);

            Trigger.ClientEvent(player, "exitPokerTable");
        }

        [RemoteEvent("pockerAction")]
        public static void PockerAction(Player player, string action, int count)
        {
            if (!player.HasData("PT_SEAT")) return;

            int seat = player.GetData<int>("PT_SEAT");
            int table = player.GetData<int>("PT_TABLE");

            Log.Write($"{action} {count}");

            if (PlayerTable[table].Count - 1 < seat) return;

            if (PlayerTable[table][seat].Handle != player) return;

            if (PlayerTable[table][seat].Answer != 0 || !PlayerTable[table][seat].IsActiveNow) return;

            switch (action)
            {
                case "fold":
                    PlayerTable[table][seat].Answer = 1;
                    break;
                case "call":
                    PlayerTable[table][seat].Answer = 2;
                    break;
                case "bet":
                    PlayerTable[table][seat].Answer = 3;
                    PlayerTable[table][seat].Count = count;
                    break;
            }

            return;
        }

       
        public static void Shuffle(List<Card> list)
        {
            Random rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                Card value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
        public static void SeatToTable(Player player)
        {
            if (!player.HasData("TABLE")) return;

            int table = player.GetData<int>("TABLE");

            if(TableBets[table].Item2 > DiamondCasino.GetAllChips(player))
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Для этого стола нужно минимум {TableBets[table].Item2} фишек", 3000);
                return;
            }

            int empty = -1;

            for(int i = 0; i < PlayerTable[table].Count; i++)
            {
                if (PlayerTable[table][i] == null)
                {
                    empty = i;
                    break;
                }
            }

            if(empty == -1)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"За столом нет свободных мест", 3000);
                return;
            }

            PlayerTable[table][empty] = new PockerPlayer(player);
            PlayerJoined[table].Add(player);
            PlayerTable[table][empty].IsActive = true;
            PlayerTable[table][empty].Table = table;
            PlayerTable[table][empty].Seat = empty;

            player.SetData("PT_TABLE", table);
            player.SetData("PT_SEAT", empty);

            player.TriggerEvent("openPokerTable", table);
            player.TriggerEvent("poker_SetChips", DiamondCasino.GetAllChips(player));
            player.TriggerEvent("poker_SetTableBets", TableBets[table].Item1, TableBets[table].Item2);

            Trigger.ClientEvent(player, "client_press_key_to", "open", JsonConvert.SerializeObject(new List<object>() { "E", "выйти из-за стола" }));

            SetPlayers(table);

            if(TableStatus[table] == 0)
            {
                for(int i = 0; i < PlayerPlaying[table].Count; i++)
                {
                    Trigger.ClientEvent(player, "addPlayerCard", PlayerPlaying[table][i].Seat);
                }
            }

            if (TableStatus[table] == 1)
            {
                for (int i = 0; i < PlayerPlaying[table].Count; i++)
                {
                    Trigger.ClientEvent(player, "addPlayerCard", PlayerPlaying[table][i].Seat);
                }

                List<string> cards = new List<string>();

                cards.Add($"{HoldemComb.CardsTypes[TableCards[table][0].cardType]}_{HoldemComb.CardsValues[TableCards[table][0].cardValue]}");
                cards.Add($"{HoldemComb.CardsTypes[TableCards[table][1].cardType]}_{HoldemComb.CardsValues[TableCards[table][1].cardValue]}");
                cards.Add($"{HoldemComb.CardsTypes[TableCards[table][2].cardType]}_{HoldemComb.CardsValues[TableCards[table][2].cardValue]}");

                Trigger.ClientEvent(player, "addTableCards", JsonConvert.SerializeObject(cards));
            }

            else if (TableStatus[table] == 2)
            {
                for (int i = 0; i < PlayerPlaying[table].Count; i++)
                {
                    Trigger.ClientEvent(player, "addPlayerCard", PlayerPlaying[table][i].Seat);
                }

                List<string> cards = new List<string>();

                cards.Add($"{HoldemComb.CardsTypes[TableCards[table][0].cardType]}_{HoldemComb.CardsValues[TableCards[table][0].cardValue]}");
                cards.Add($"{HoldemComb.CardsTypes[TableCards[table][1].cardType]}_{HoldemComb.CardsValues[TableCards[table][1].cardValue]}");
                cards.Add($"{HoldemComb.CardsTypes[TableCards[table][2].cardType]}_{HoldemComb.CardsValues[TableCards[table][2].cardValue]}");

                Trigger.ClientEvent(player, "addTableCards", JsonConvert.SerializeObject(cards));

                string card1 = $"{HoldemComb.CardsTypes[TableCards[table][3].cardType]}_{HoldemComb.CardsValues[TableCards[table][3].cardValue]}";

                Trigger.ClientEvent(player, "addTableCard", 3, card1);

            }
            else if(TableStatus[table] == 3)
            {
                for (int i = 0; i < PlayerPlaying[table].Count; i++)
                {
                    Trigger.ClientEvent(player, "addPlayerCard", PlayerPlaying[table][i].Seat);
                }

                List<string> cards = new List<string>();

                cards.Add($"{HoldemComb.CardsTypes[TableCards[table][0].cardType]}_{HoldemComb.CardsValues[TableCards[table][0].cardValue]}");
                cards.Add($"{HoldemComb.CardsTypes[TableCards[table][1].cardType]}_{HoldemComb.CardsValues[TableCards[table][1].cardValue]}");
                cards.Add($"{HoldemComb.CardsTypes[TableCards[table][2].cardType]}_{HoldemComb.CardsValues[TableCards[table][2].cardValue]}");

                Trigger.ClientEvent(player, "addTableCards", JsonConvert.SerializeObject(cards));

                string card1 = $"{HoldemComb.CardsTypes[TableCards[table][3].cardType]}_{HoldemComb.CardsValues[TableCards[table][3].cardValue]}";

                Trigger.ClientEvent(player, "addTableCard", 3, card1);

                string card2 = $"{HoldemComb.CardsTypes[TableCards[table][4].cardType]}_{HoldemComb.CardsValues[TableCards[table][4].cardValue]}";

                Trigger.ClientEvent(player, "addTableCard", 4, card2);
            }

        }

        [Command("addbot")]
        public static void AddBot(Player player, int table, int seat)
        {
            if (Main.Players[player].AdminLVL < 8) return;
            
            if(PlayerTable[table][seat] != null)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Место уже занято", 3000);
                return;
            }

            PlayerTable[table][seat] = new PockerPlayer("Bot");
            PlayerTable[table][seat].IsActive = true;
            PlayerTable[table][seat].Table = table;
            PlayerTable[table][seat].Seat = seat;
        }

        [Command("delbot")]
        public static void RemoveBot(Player player, int table, int seat)
        {
            if (Main.Players[player].AdminLVL < 8) return;

            if (PlayerTable[table][seat] == null)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Место пусто", 3000);
                return;
            }

            if (!PlayerTable[table][seat].IsBot)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Не бот", 3000);
                return;
            }

            if (QueueRemoveBots.Contains(PlayerTable[table][seat]))
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Бот уже в очереди на удаление", 3000);
                return;
            }

            QueueRemoveBots.Add(PlayerTable[table][seat]);

            Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Бот добавлен в очередь на удаление", 3000);
        }



    }

    class PockerPlayer
    {
        public bool IsBot;
        public string Name;
        public int Bet = 0;
        public int Chips = 1000;
        public bool Fold = false;
        public bool IsDealer = false;
        public bool IsActiveNow = false;
        public bool IsActive = false;

        public int Answer = -1;
        public int Count = 0;
        public int Timer = 0;

        public int RoundBet = 0;
        public int Combination = 0;
        public int Seat = -1;
        public int Table = -1;

        public bool Winner = false;

        public string LastAction = "";

        public List<Card> Cards = new List<Card>();
        public List<Card> WinnerCards = new List<Card>();

        public Player Handle;
        
        public PockerPlayer(string name)
        {
            Name = name;
            IsBot = true;
        }

        public PockerPlayer(Player player)
        {
            IsBot = false;
            Handle = player;
            Name = player.Name;
        }
    }
}
