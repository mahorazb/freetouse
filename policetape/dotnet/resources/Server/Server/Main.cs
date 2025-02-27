using System;
using System.IO;
using System.Text;
using GTANetworkAPI;

namespace Server
{
    public class Main : Script
    {

        [ServerEvent(Event.ResourceStart)]
        public static void OnServerStart()
        {
   
            NAPI.Server.SetAutoRespawnAfterDeath(false);
            NAPI.Server.SetAutoSpawnOnConnect(true);
            

            NAPI.World.SetTime(0, 0, 0);
            NAPI.World.SetWeather(Weather.CLEAR);
          
        }

        [ServerEvent(Event.ResourceStop)]
        public static void OnServerStop()
        {
            Console.WriteLine("Stopped");

        }

        [ServerEvent(Event.PlayerConnected)]
        public static void OnPlayerConnect(Player player)
        {

            player.SetSkin(PedHash.FreemodeMale01);

            player.SetClothes(8, 15, 1);
            player.SetClothes(11, 243, 0);
            player.SetClothes(4, 121, 0);
            player.SetClothes(6, 27, 0);
            player.SetClothes(1, 169, 0);

            player.SetAccessories(0, 2, 0);
            player.SetAccessories(1, 39, 0);
            player.SetAccessories(2, 2, 0);
            player.SetAccessories(6, 20, 0);
            player.SetClothes(10, 55, 0);
        }

        [ServerEvent(Event.PlayerSpawn)]
        public static void OnPlayerSpawn(Player player)
        {
            //player.Position = World.Positions.GetRandomSpawnPosition();

            player.Position = new Vector3(195.68951, 1164.36, 227.0361);
        }

        [Command("aveh")]
        public static void AVeh(Player player)
        {
            var veh = NAPI.Vehicle.CreateVehicle(VehicleHash.Oppressor2, player.Position - new Vector3(1, 1, 0), 6, 7, 0);
            veh.SetSharedData("V_ADMIN", true);
            veh.SetSharedData("V_ENGINE", false);

            veh.EngineStatus = false;
        }

        [Command("sc")]
        public static void SC(Player player, int draw, int text, int col)
        {
            player.SetClothes(draw, text, col);
        }

        [Command("tp")]
        public static void Tp(Player player)
        {
            player.Position = new Vector3(892.1839, 3657.5537, 33.919052);
        }

        [Command("gg")]
        public static void Givegun(Player player)
        {
            
            player.GiveWeapon(WeaponHash.Assaultrifle_mk2, 100);

            NAPI.Task.Run(() =>
            {
                player.TriggerEvent("giveGun");
            }, 1000);
        }

        [Command("save")]
        public static void saveCoords(Player player, string name)
        {

            Vector3 pos = NAPI.Entity.GetEntityPosition(player);
            Vector3 rot = NAPI.Entity.GetEntityRotation(player);
            if (NAPI.Player.IsPlayerInAnyVehicle(player))
            {
                Vehicle vehicle = player.Vehicle;
                pos = NAPI.Entity.GetEntityPosition(vehicle) + new Vector3(0, 0, 1.62);
                rot = NAPI.Entity.GetEntityRotation(vehicle);
            }
            try
            {
                StreamWriter saveCoords = new StreamWriter("savepos.txt", true, Encoding.UTF8);
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
                saveCoords.Write($"{name} Position: new Vector3({pos.X}, {pos.Y}, {pos.Z}),\r\n");
                saveCoords.Write($"{name} Rotation new Vector3({rot.X}, {rot.Y}, {rot.Z}),\r\n");
                saveCoords.Close();
            }
            catch (Exception error)
            {
                NAPI.Chat.SendChatMessageToPlayer(player, "Exeption: " + error);
            }
        }
    }
}
