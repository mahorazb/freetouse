using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Golemo.GUI;
using GolemoSDK;
using GTANetworkAPI;

namespace Golemo.Core
{
    class Billiards : Script
    {
        private static nLog Log = new nLog("Billiards");
        private static Random rng = new Random();

        private static int Price = 1000;

        public static List<int> Shuffle(List<int> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                int value = list[k];
                list[k] = list[n];
                list[n] = value;
            }

            return list;
        }

        public static List<Vector3> TablePos = new List<Vector3>()
        {
            new Vector3(200.33763, -938.28668, 29.68678),
        };

        public static Dictionary<int, PoolTable> Tables = new Dictionary<int, PoolTable>();

        [ServerEvent(Event.ResourceStart)]
        public static void OnResourceStart()
        {
            for (int i = 0; i < TablePos.Count; i++) {
                Tables.Add(i, new PoolTable(i, TablePos[i]));
            }
        }

        public static void TryRentPoolTable(Player player)
        {
            int table = player.GetData<int>("PTABLE");

            if (player.HasData("POOL") || player.HasData("PLAY_POOL"))
            {
                if (player.GetData<int>("POOL") == table || player.GetData<int>("PLAY_POOL") == table)
                {
                    if (Tables[table].HitterNow == player)
                    {
                        StopHitting(player);
                    }
                    else if (Tables[table].HitterNow == null)
                    {
                        StartHitting(player);
                    }
                    
                    return;
                }
            }

            if(player.HasData("POOL") || player.HasData("PLAY_POOL"))
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Вы уже играете за столом", 3000);
                return;
            }

            if (Tables[table].IsRent)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Этот стол уже арендован другим игроком", 3000);
                return;
            }

            player.SetData("POOL_RENT", table);

            Trigger.ClientEvent(player, "openDialog", "POOL_RENT", $"Арендовать стол для бильярда за ${Price} на один час?");
        }

        public static void AcceptRent(Player player, int table)
        {

            if (Tables[table].IsRent)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Этот стол уже арнедован другим игроком", 3000);
                return;
            }

            if(!MoneySystem.Wallet.Change(player, -Price))
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "У вас нехватает наличных денег", 3000);
                return;
            }

            Tables[table].IsRent = true;
            Tables[table].Renter = player;
            Tables[table].RentTime = DateTime.Now.AddHours(1);
            Tables[table].Players.Add(player);

            player.SetData("POOL", table);

            Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, "Вы арендовали стол для бильярда. Используйте /poolmenu для управления", 5000);
        }

        public static void InvitePlayerToTable(Player player, Player target)
        {

            int table = player.GetData<int>("POOL");

            if (Tables[table].Started)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Вы должны закончить текущую игру", 3000);
                return;
            }

            if (target.HasData("PLAY_POOL") || target.HasData("POOL"))
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Игрок уже играет за другим столом", 3000);
                return;
            }

            if(target.Position.DistanceTo(player.Position) > 3)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Игрок должен находиться в радиусе трех метров", 3000);
                return;
            }

            Tables[table].Players.Add(target);

            Notify.Send(target, NotifyType.Success, NotifyPosition.CenterLeft, "Вы были приглашены за бильярдны стол", 3000);

            target.SetData("PLAY_POOL", table);
        }

        public static void StartHitting(Player player)
        {
            int table = -1;

            if (player.HasData("POOL"))
            {
                table = player.GetData<int>("POOL");
            }

            if (player.HasData("PLAY_POOL"))
            {
                table = player.GetData<int>("PLAY_POOL");
            }

            if (table == -1)
            {
                return;
            }

            if (Tables[table].Started)
            {
                StartGame(player);
                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Игра начата!", 3000);
            }

            if(Tables[table].HitterNow != null)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "На данный момент кто то бьет", 3000);
                return;
            }

            Tables[table].HitterNow = player;

            player.SetData("HITTING_NOW", true);

            Trigger.ClientEvent(player, "startHitting");
        }

        public static void StopHitting(Player player)
        {
            int table = -1;

            if (player.HasData("POOL"))
            {
                table = player.GetData<int>("POOL");
            }

            if (player.HasData("PLAY_POOL"))
            {
                table = player.GetData<int>("PLAY_POOL");
            }

            if(table == -1)
            {
                return;
            }

            if (!Tables[table].Started)
            {
                StartGame(player);
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Игра неначата!", 3000);
            }

            if (Tables[table].HitterNow != player)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "На данный момент кто то бьет", 3000);
                return;
            }

            player.ResetData("HITTING_NOW");

            Trigger.ClientEvent(player, "stopHitting");

            Tables[table].HitterNow = null;
        }

        public static void RemovePlayerFromTable(Player player, Player target)
        {
            int table = player.GetData<int>("POOL");

            if (player == target) return;

            if (!Tables[table].Players.Contains(target))
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Игрок не находится за вашим столом", 3000);
                return;
            }

            QuitGame(target);
        }

        public static void StartGame(Player player)
        {
            int table = player.GetData<int>("POOL");

            if (Tables[table].Started)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Игра уже начата", 3000);
                return;
            }

            Tables[table].Balls = Shuffle(Tables[table].Balls);

            Trigger.ClientEventToPlayers(Tables[table].Players.ToArray(), "startPoolGame", table, Tables[table].Balls.ToArray());

            Tables[table].Started = true;
            Tables[table].Message("Игра начата!");
        }

        public static void QuitGame(Player player)
        {
            if (player.HasData("POOL"))
            {
                int table = player.GetData<int>("POOL");

                Tables[table].Message($"Аренда стола окончена");

                Tables[table].Players.ForEach((p) =>
                {
                    if (p != player)
                    {
                        Trigger.ClientEvent(p, "quitPoolGame");
                        player.ResetData("HITTING_NOW");
                        player.ResetData("PLAY_POOL");
                    }
                });

                Trigger.ClientEvent(player, "quitPoolGame");

                player.ResetData("POOL");

                Tables[table].ResetTable();

            }
            else if (player.HasData("PLAY_POOL"))
            {
                int table = player.GetData<int>("PLAY_POOL");

                Tables[table].Message($"Игрок {player.Name} покидает стол");

                Trigger.ClientEvent(player, "quitPoolGame");

                if (player.HasData("HITTING_NOW"))
                {
                    Tables[table].HitterNow = null;
                    player.ResetData("HITTING_NOW");
                }

                if(Tables[table].Players.Contains(player))
                    Tables[table].Players.Remove(player);

                player.ResetData("PLAY_POOL");

            }
        }

        [RemoteEvent("pushPoolBall")]
        public static void PushBall(Player player, int table, float camx, float camy, float power)
        {
            Log.Debug($"{player.Name} {table} {camx} {camy} {power}");
            Trigger.ClientEventToPlayers(Tables[table].Players.Where((p) => p != player).ToArray(), "pushedBall", table, camx, camy, power);
        }

        [RemoteEvent("ballInAngle")]
        public static void BallInAngle(Player player, int table, int ballId)
        {
            Log.Debug($"{player.Name} {table} {ballId}");

            Tables[table].Message($"Игрок {player.Name} забивает шар #{Tables[table].Balls[ballId]}");

            Trigger.ClientEventToPlayers(Tables[table].Players.Where((p) => p != player).ToArray(), "shotBall", ballId);
        }

        [ServerEvent(Event.PlayerDisconnected)]
        public static void onPlayerDisconnectedhandler(Player player, DisconnectionType type, string reason)
        {
            QuitGame(player);
        }

        [ServerEvent(Event.PlayerDeath)]
        public void onPlayerDeathHandler(Player player, Player entityKiller, uint weapon)
        {
            QuitGame(player);
        }

        [Command("poolmenu")]
        public static void OpenPoolMenu(Player player)
        {
            if (!player.HasData("POOL")) return;

            Menu menu = new Menu("poolmenu", false, false);
            menu.Callback = callback_poolmenu;

            Menu.Item menuItem = new Menu.Item("header", Menu.MenuItem.Header);
            menuItem.Text = "Управление столом";
            menu.Add(menuItem);

            menuItem = new Menu.Item("invite", Menu.MenuItem.Button);
            menuItem.Text = $"Пригласить игрока";
            menu.Add(menuItem);

            menuItem = new Menu.Item("remove", Menu.MenuItem.Button);
            menuItem.Text = $"Выгнать игрока";
            menu.Add(menuItem);

            menuItem = new Menu.Item("reset", Menu.MenuItem.Button);
            menuItem.Text = $"Сбросить игру";
            menu.Add(menuItem);

            menuItem = new Menu.Item("finish", Menu.MenuItem.Button);
            menuItem.Text = $"Завершить аренду";
            menu.Add(menuItem);

            menuItem = new Menu.Item("back", Menu.MenuItem.Button);
            menuItem.Text = "Выход";
            menu.Add(menuItem);

            menu.Open(player);
        }

        private static void callback_poolmenu(Player player, Menu menu, Menu.Item item, string eventName, dynamic data)
        {
            int table = player.GetData<int>("POOL");

            int UUID = Main.Players[player].UUID;
            switch (item.ID)
            {
                case "invite":
                    if(Tables[table].Players.Count == 4)
                    {
                        Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Максимум 4 игрока", 3000);
                        return;
                    }
                    Trigger.ClientEvent(player, "openInput", "Введите ID игрока", "", 3, "player_invitepool");
                    return;
                case "remove":
                    Trigger.ClientEvent(player, "openInput", "Введите ID игрока", "", 3, "player_removepool");
                    return;
                case "reset":
                    Tables[table].ResetGame();
                    MenuManager.Close(player);
                    return;
                case "finish":
                    QuitGame(player);
                    MenuManager.Close(player);
                    return;
                case "back":
                    MenuManager.Close(player);
                    return;
            }
        }


    }

    class PoolTable
    {
        public int ID;

        public bool IsRent;

        public bool Started;

        public Player Renter;

        public DateTime RentTime;

        public Vector3 Position;

        public List<Player> Players;

        public Player HitterNow;

        public ColShape Shape;

        public List<int> Balls = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };

        public PoolTable(int id, Vector3 pos)
        {
            ID = id;
            IsRent = false;
            Started = false;
            RentTime = DateTime.Now;
            Position = pos;
            Players = new List<Player>();
            HitterNow = null;
            Renter = null;

            Shape = NAPI.ColShape.CreateCylinderColShape(Position, 2, 4);
            Shape.SetData("POOL_TABLE", ID);

            Shape.OnEntityEnterColShape += (shape, entity) =>
            {
                entity.SetData("PTABLE", shape.GetData<int>("POOL_TABLE"));
                entity.SetData("INTERACTIONCHECK", 1002);
            };
            Shape.OnEntityExitColShape += (shape, entity) =>
            {
                entity.SetData("INTERACTIONCHECK", 0);
                entity.ResetData("PTABLE");
            };

        }

        public void Message(string str)
        {
            Players.ForEach((p) => {

                Notify.Send(p, NotifyType.Info, NotifyPosition.BottomCenter, str, 3000);
            });
        }

        public void ResetGame()
        {
            Balls = Billiards.Shuffle(Balls);

            Message("Начата новая игра!");

            Players.ForEach((p) => {

                Trigger.ClientEvent(p, "resetPoolGame", Balls.ToArray());
            });
        }

        public void ResetTable()
        {
            Players.Clear();
            HitterNow = null;
            Renter = null;
            IsRent = false;
            Started = false;
            RentTime = DateTime.Now;
        }
    }
}
