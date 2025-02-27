using System;
using GTANetworkAPI;

namespace dayz
{
    public class Main : Script
    {
        [ServerEvent(Event.ResourceStart)]
        public static void OnResourceStart()
        {
            //NAPI.World.SetWeather(Weather.FOGGY);
            NAPI.World.SetTime(12, 0, 0);
        }

        [ServerEvent(Event.PlayerConnected)]
        public static void PlayerConnect(Player player)
        {
            player.TriggerEvent("serverWorldDataReady");

            player.SetSkin(PedHash.FreemodeMale01);

           // player.SetClothes(1, 129, 1);
            //player.SetClothes(2, 30, 0);
            player.SetClothes(3, 15, 0);
            player.SetClothes(4, 12, 0);
            player.SetClothes(6, 34, 0);
            player.SetClothes(8, 15, 0);
            player.SetClothes(10, 19, 0);
            player.SetClothes(11, 15, 0);

            NAPI.Entity.SetEntityPosition(player, new Vector3(-1566.2853, 2770.2112, 17.41528));

        }

        [ServerEvent(Event.PlayerSpawn)]
        public static void PlayerSpawn(Player player)
        {
       
            NAPI.Entity.SetEntityPosition(player, new Vector3(-632.4209, 838.7523, 207.0724));

            player.GiveWeapon(WeaponHash.Sniperrifle, 100);
            player.GiveWeapon(WeaponHash.Assaultrifle_mk2, 1000);
            player.GiveWeapon(WeaponHash.CeramicPistol, 1000);
            player.GiveWeapon(WeaponHash.Stungun, 1000);

        }
    }
}
