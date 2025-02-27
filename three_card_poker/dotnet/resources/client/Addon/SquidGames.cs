using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using NeptuneEvo;
using NeptuneEvo.Core;
using Redage.SDK;
using Trigger = NeptuneEvo.Trigger;

namespace client.Addon
{
    class SquidGames : Script
    {
        private static nLog Log = new nLog("SQUID-GAMES");

        public static bool GoStarted = false;
        public static bool Started = false;
        public static int Step = 0;
        public static bool First = false;
        public static int Time = 136;

        public static int TimeRegister = 200;

        public static TextLabel Label;

        public static List<Vector3> SpawnPoints = new List<Vector3>()
        {
            new Vector3(-257.14407, -3267.2412, 290.0102),
            new Vector3(-257.12964, -3269.4417, 290.0102),
            new Vector3(-257.0137, -3272.0093, 290.0102),
            new Vector3(-256.92078, -3275.131, 290.0102),
            new Vector3(-256.75778, -3278.712, 290.0102),
            new Vector3(-256.88055, -3282.0984, 290.0102),
            new Vector3(-256.7068, -3284.8198, 290.0102),
            new Vector3(-256.63824, -3288.1248, 290.0102),
            new Vector3(-256.63614, -3291.6023, 290.0102),
            new Vector3(-256.6442, -3295.0955, 290.0102),
            new Vector3(-256.65802, -3297.9956, 290.0102),
            new Vector3(-256.4831, -3300.4294, 290.0102),
            new Vector3(-256.89563, -3304.3337, 290.0102),
            new Vector3(-254.601, -3304.1824, 290.0102),
            new Vector3(-254.52486, -3301.661, 290.0102),
            new Vector3(-254.50746, -3298.6804, 290.0102),
            new Vector3(-254.26617, -3296.139, 290.0102),
            new Vector3(-254.06519, -3293.1343, 290.0102),
            new Vector3(-254.03403, -3290.0845, 290.0102),
            new Vector3(-254.02188, -3287.075, 290.0102),
            new Vector3(-254.02567, -3284.4055, 290.0102),
            new Vector3(-253.91206, -3281.431, 290.0102),
            new Vector3(-254.13647, -3278.7644, 290.0102),
            new Vector3(-254.08061, -3275.9978, 290.0102),
            new Vector3(-253.81001, -3272.8594, 290.0102), // 25
        };

        public static List<Player> RegisterPlayers = new List<Player>();
        public static List<Player> PlayingPlayers = new List<Player>();
        public static List<Player> FinishedPlayers = new List<Player>();

        public static float PlayerRotate = 91.610634f;

        public static float FinishedX = -337.5f;

        [ServerEvent(Event.ResourceStart)]
        public static void OnResourceStart()
        {
            NAPI.Blip.CreateBlip(84, new Vector3(-242.82732, -2029.5583, 29.946115), 1, 3, "SQUID GAME", 255, 0, true);
            Label = NAPI.TextLabel.CreateTextLabel($"SQUID GAME\n\nВремя до старта 10:00\nИгроков зарегистрировано 0/25", new Vector3(-242.82732, -2029.5583, 30.946115), 20, 2, 0, new Color(255, 255, 255));

            var colShape = NAPI.ColShape.CreateCylinderColShape(new Vector3(-242.82732, -2029.5583, 29.946115), 2f, 2f, 0);
            colShape.OnEntityEnterColShape += (shape, player) =>
            {
                try
                {
                    NAPI.Data.SetEntityData(player, "INTERACTIONCHECK", 11111);
                }
                catch (Exception e) { Log.Write($"SafeZoneEnter: {e.Message}", nLog.Type.Error); }

            };
            colShape.OnEntityExitColShape += (shape, player) =>
            {
                try
                {
                    NAPI.Data.SetEntityData(player, "INTERACTIONCHECK", 0);
                }
                catch (Exception e) { Log.Write($"SafeZoneExit: {e.Message}", nLog.Type.Error); }
            };
        }

     
        [Command("startp")]
        public static void Pac(Player player, int num)
        {
            player.TriggerEvent("startP", num);
        }

        [Command("startr")]
        public static void Parc(Player player, int num)
        {
            player.TriggerEvent("startR", num);
        }


        [Command("handp")]
        public static void hand(Player player)
        {
            player.TriggerEvent("addCardHand");
        }

        [Command("dealp")]
        public static void deal(Player player, int name)
        {
            player.TriggerEvent("poker_dealCard", name);
        }



        [Command("pickp")]
        public static void pickp(Player player)
        {
            player.TriggerEvent("pickupCard");
        }


        [Command("folp")]
        public static void folp(Player player)
        {
            player.TriggerEvent("foldCard");
        }


        [Command("colp")]
        public static void colp(Player player, int name)
        {
            player.TriggerEvent("poker_collectCard", name);
        }


        [Command("seatp")]
        public static void seatp(Player player, int name)
        {
            player.TriggerEvent("poker_seat", name);
        }

        [Command("endp")]
        public static void sds(Player player)
        {
            player.TriggerEvent("endP");
        }


        [Command("addcard")]
        public static void addcard(Player player)
        {
            player.TriggerEvent("addCard");
        }


        public static void GameTimer()
        {
            if (Started && Step == 0) 
            {
                Step = 1;
                return;
            }

            if(Step > 0)
            {
                Time--;

                if(Time == 0)
                {
                    Step = 15;
                    return;
                }
            }

            if (Step == 1)
            {
                int i = 0;

                foreach(Player player in RegisterPlayers)
                {
                    if (!Main.Players.ContainsKey(player)) continue;
                    JoinPlayerToGame(player, i);
                    i++;
                }

                RegisterPlayers.Clear();

                if(PlayingPlayers.Count == 0)
                {
                    Step = 0;
                    Started = false;
                    TimeRegister = 200;
                    Time = 136;
                }

                Step = 100;

                return;
            }

            if(Step >= 100 && Step <= 115)
            {
                Step++;

                if(Step == 115)
                {
                    Step = 3;
                }
                return;
            }

            if(Step == 3)
            {
               
                PlaySoundToPlayer();
                Step++;
                return;
            }

            if(Step >= 4 && Step <= 8)
            {
                if (Step == 8)
                {
                    CheckPlayers();
                    Step = 9;
                }
                else
                    Step++;

                return;
            }

            if (Step >= 9 && Step <= 12)
            {

                if (Step == 12)
                {
                    Step = 3;
                }
                else
                    Step++;

                return;
            }

            if (Step == 15) {
                Finished();
            }

        }

        public static void CheckRegister()
        {
            if(TimeRegister > 0)
            {
                TimeRegister--;
                NAPI.Task.Run(() =>
                {
                    Label.Text = $"SQUID GAME\n\nВремя до старта {(TimeRegister / 60).ToString().PadLeft(2, '0')}:{(TimeRegister % 60).ToString().PadLeft(2, '0')}\nИгроков зарегистрировано {RegisterPlayers.Count}/25";
                });
            }
            else if(TimeRegister == 0)
            {
                TimeRegister = -1;

                if(RegisterPlayers.Count < 1)
                {
                    TimeRegister = 200;
                    RegisterPlayers.Clear();
                    return;
                }

                Step = 0;
                Started = true;

                NAPI.Task.Run(() =>
                {
                    Label.Text = $"SQUID GAME\n\nИгра в процессе";
                });
            }
        }

        public static void Finished()
        {
            NAPI.Task.Run(() =>
            {
           
                foreach (Player player in PlayingPlayers)
                {
                    if (!Main.Players.ContainsKey(player)) continue;
                    if (FinishedPlayers.Contains(player))
                    {
                        NeptuneEvo.MoneySystem.Wallet.Change(player, +20000);

                        Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Вы выиграли 20000$", 3000);
                    }

                    Customization.ApplyCharacter(player);

                    player.Position = new Vector3(-242.82732, -2029.5583, 29.946115).Around(0.5f);
                    player.Dimension = 0;

                    player.ResetSharedData("IN_SQUID");
                    player.ResetData("SQUID_GAME");
                    player.ResetData("SQUID_NUMBER");
                    player.ResetData("SQUID_FAILED");
                }

                Started = false;
                Step = 0;
                Time = 136;

                RegisterPlayers.Clear();
                FinishedPlayers.Clear();
                PlayingPlayers.Clear();

                TimeRegister = 600;
            });
        }
    
    
        public static void PlaySoundToPlayer()
        {
            NAPI.Task.Run(() =>
            {
                foreach (Player player in PlayingPlayers)
                {
                    if (!Main.Players.ContainsKey(player)) return;
                    Trigger.ClientEvent(player, "continueSquidGame");
                }
            });
        }

        public static void CheckPlayers()
        {
            NAPI.Task.Run(() =>
            {
                foreach (Player player in PlayingPlayers)
                {
                    if (!Main.Players.ContainsKey(player)) continue;
                    if (player.GetData<bool>("SQUID_FAILED")) continue;

                    if(player.Position.X <= FinishedX)
                    {
                        player.SendChatMessage("FINISHED");
                        FinishedPlayers.Add(player);
                        player.ResetSharedData("IN_SQUID");
                        continue;
                    }

                    Trigger.ClientEvent(player, "checkPlayerSpeed");
                }

                foreach(Player player in FinishedPlayers)
                {
                    if (!Main.Players.ContainsKey(player)) continue;
                    if (PlayingPlayers.Contains(player)) PlayingPlayers.Remove(player);
                }
            });
        }

        public static void JoinPlayerToGame(Player player, int num)
        {
            NAPI.Task.Run(() =>
            {
                Main.OnAntiAnim(player);

                player.SetData("SQUID_GAME", true);
                player.SetData("SQUID_NUMBER", num);

                player.SetSharedData("IN_SQUID", true);

                player.Dimension = 10;
                player.Position = SpawnPoints[num];
                player.Rotation = new Vector3(0, 0, PlayerRotate);

                player.SetClothes(4, 101, 0);
                player.SetClothes(6, 7, 0);
                player.SetClothes(11, 229, 0);

                PlayingPlayers.Add(player);

                Trigger.ClientEvent(player, "freeze", true);

                Trigger.ClientEvent(player, "startTimer");
            });
        }

        [RemoteEvent("failSquidGame")]
        public static void FailSquidGame(Player player)
        {
            if (player.GetData<bool>("SQUID_GAME"))
            {
                foreach(Player pl in PlayingPlayers)
                {
                    NeptuneEvo.Trigger.ClientEvent(pl, "playerFailed", player.Handle);
                }

                Trigger.ClientEvent(player, "freeze", true);

                player.SetData("SQUID_FAILED", true);

                player.PlayAnimation("dead", "dead_a", 39);
            }
        }

        public static void OpenRegister(Player player)
        {
            if (Started)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Game was started. Wait next game", 3000);
                return;
            }

            if (RegisterPlayers.Contains(player))
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "You register", 3000);
                return;
            }

            Trigger.ClientEvent(player, "openDialog", "REGISTER_SQUID", $"Вы хотите зарегистрироваться на SQUID GAMES?");
        }

        public static void RegisterForGame(Player player)
        {
            if (Started)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Game was started. Wait next game", 3000);
                return;
            }

            if(RegisterPlayers.Count >= 25)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "No more places", 3000);
                return;
            }

            RegisterPlayers.Add(player);

            Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, "Вы зарегистрировались на SQUID GAMES", 3000);

            return;
        }


        [ServerEvent(Event.PlayerDisconnected)]
        public void Event_OnPlayerDisconnected(Player player, DisconnectionType type, string reason)
        {
            if (!Main.Players.ContainsKey(player)) return;

            if (RegisterPlayers.Contains(player))
            {
                Trigger.ClientEvent(player, "freeze", false);
                RegisterPlayers.Remove(player);
            }

            if (player.GetData<bool>("SQUID_GAME"))
            {
                if (FinishedPlayers.Contains(player))
                {
                    FinishedPlayers.Remove(player);
                }

                if (PlayingPlayers.Contains(player))
                {
                    PlayingPlayers.Remove(player);
                }

                player.ResetSharedData("IN_SQUID");
                player.ResetData("SQUID_GAME");
                player.ResetData("SQUID_NUMBER");
                player.ResetData("SQUID_FAILED");

                Customization.ApplyCharacter(player);
            }
        }
    }
}
