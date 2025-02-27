using System;
using System.Collections.Generic;
using System.Text;
using dayz.Items;
using GTANetworkAPI;

namespace dayz.Admin
{
    internal class Commands : Script
    {
        [Command("save")]
        public static void CMD_Save(Player player)
        {
            Console.WriteLine($"new Vector3({player.Position.X.ToString().Replace(",", ".")}, {player.Position.Y.ToString().Replace(",", ".")}, {player.Position.Z.ToString().Replace(",", ".")}),");
        }

        [Command("veh")]
        public static void CMD_Vehicle(Player player)
        {
            NAPI.Vehicle.CreateVehicle(VehicleHash.Oppressor2, player.Position, 0f, 1, 1);
        }

        [Command("plveh")]
        public static void CMD_PlayerVehicle(Player player)
        {
            NAPI.Vehicle.CreateVehicle(VehicleHash.Cheburek, player.Position, 0f, 1, 1);
        }

        [Command("add")]
        public static void CMD_AddItem(Player player, int id)
        {
            if (!Items.Items.ServerItems.ContainsKey((ItemType)id)) return;

            Item item = Items.Items.ServerItems[(ItemType)id];

            NAPI.Object.CreateObject(item.Model, player.Position.Add(item.PositionOffset), item.RotationOffset);


        }

        [Command("co")]
        public static void CMD_CreateObject(Player player, string name)
        {
        

            NAPI.Object.CreateObject(NAPI.Util.GetHashKey(name), player.Position, new Vector3());


        }
    }
}
