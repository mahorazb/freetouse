using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;

namespace dayz.Peds
{
    enum PedType
    {
        Human = 1,
        Animal,
        AngryAnimal,
        Zombie
    }

    internal class Peds : Script
    {
        //
        public static Dictionary<int, Ped> SpawnedPeds = new Dictionary<int, Ped>();
        public static Random random = new Random();

        public static List<Vector3> Positions = new List<Vector3>()
        {
            new Vector3(-2679.9324, 3293.8843, 30.811714),
            new Vector3(-2705.9556, 3259.3193, 30.709526),
            new Vector3(-2549.2786, 3179.317, 30.78545),
            new Vector3(-2444.0164, 3116.064, 30.82256),
            new Vector3(-2385.1838, 3163.9832, 30.78567),
            new Vector3(-2335.8083, 3211.192, 30.759487),
            new Vector3(-2289.7344, 3283.7368, 30.862198),
            new Vector3(-2226.297, 3304.82, 30.704704),
            new Vector3(-2173.1118, 3302.463, 30.74065),
            new Vector3(-2117.904, 3316.5293, 30.78235),
            new Vector3(-2010.3416, 3252.0562, 30.690323),
            new Vector3(-1944.6033, 3215.1467, 30.76286),
            new Vector3(-1963.777, 3156.5728, 30.726814),
            new Vector3(-1938.0009, 3117.7344, 30.746933),
            new Vector3(-1876.0197, 3077.47, 30.74578),
            new Vector3(-1869.5491, 3012.2522, 30.768433),
            new Vector3(-1835.08, 2954.3318, 30.744286),
            new Vector3(-1804.5352, 2985.8198, 30.810898),
            new Vector3(-2123.704, 3240.3389, 30.756676),
            new Vector3(-2114.957, 3291.359, 30.708363),
            new Vector3(-2149.2422, 3272.629, 30.747284),
            new Vector3(-2165.0044, 3220.8672, 30.76358),
            new Vector3(-2166.7678, 3120.3213, 32.76287),
            new Vector3(-2100.3855, 3082.7441, 32.815063),
            new Vector3(-2002.8016, 3024.4922, 32.909237),
            new Vector3(-1940.3658, 2987.1272, 32.8696),
            new Vector3(-1927.1724, 2942.6008, 32.796967),
            new Vector3(-1956.8783, 2876.2952, 32.829983),
            new Vector3(-2021.2483, 2861.317, 32.919952),
            new Vector3(-2090.7827, 2849.5251, 32.831318),
            new Vector3(-2110.242, 2842.6104, 32.72703),
            new Vector3(-2148.7024, 2884.4783, 32.796),
            new Vector3(-2196.0464, 2934.0051, 32.842316),
            new Vector3(-2252.3896, 2971.721, 32.833492),
            new Vector3(-2320.079, 3011.7817, 32.86617),
            new Vector3(-2387.9302, 3061.736, 32.860416),
            new Vector3(-2416.865, 3027.1958, 32.7551),
            new Vector3(-2428.4644, 3000.91, 32.73573),
            new Vector3(-2444.3484, 2973.9895, 32.775764),
            new Vector3(-2458.0303, 2950.5405, 32.87579),
            new Vector3(-2461.1685, 2921.0847, 32.872658),
            new Vector3(-1574.8894, 2785.0276, 16.845898),
            new Vector3(-1684.0992, 2933.3103, 32.77144),
            new Vector3(-1716.8468, 3089.6584, 32.90021),
            new Vector3(-1782.8186, 3158.8176, 32.74532),
            new Vector3(-1882.0011, 3231.0627, 32.786438),
            new Vector3(-1996.5854, 3296.6863, 32.76469),
            new Vector3(-2168.1304, 3360.3342, 33.053375),
            new Vector3(-2287.53, 3380.8665, 31.07035),
            new Vector3(-2321.2778, 3402.4016, 30.56926),
            new Vector3(-2376.6118, 3476.6462, 24.184492),

        };

        public static List<Vector3> AnimalsSpawn = new List<Vector3>()
        {
            new Vector3(-2529.8032, 2656.4333, 2.8469362),
            new Vector3(-2579.731, 2738.4072, 2.8727355),
            new Vector3(-2541.899, 2837.9285, 3.0359905),
            new Vector3(-2429.7742, 2802.1887, 3.0036905),
            new Vector3(-2350.3135, 2788.537, 2.8272455),
            new Vector3(-2312.8481, 2726.6787, 2.969837),
            new Vector3(-2242.5215, 2684.6858, 3.9514632),
            new Vector3(-2278.2415, 2543.5261, 2.8353615),
            new Vector3(-2135.011, 2526.372, 3.0866857),
            new Vector3(-2010.9089, 2511.3564, 1.9845675),
            new Vector3(-1989.5159, 2623.3916, 2.886671),
            new Vector3(-1875.174, 2674.8691, 3.9279296),
            new Vector3(-1813.2269, 2642.513, 2.7926257),
            new Vector3(-1767.0559, 2581.446, 3.614329),
            new Vector3(-1693.1279, 2573.4712, 3.5198114),
            new Vector3(-2394.3406, 2517.7747, 5.086396),
            new Vector3(-2568.322, 2515.205, 2.2452357),
            new Vector3(-1584.4419, 2698.0442, 4.112765),
        };

        public static string[] ZombieSkins = new string[] {
            "U_M_Y_Zombie_01",
            "u_f_m_corpse_01",
            "csb_stripper_01",
            "s_m_m_scientist_01",
            "s_m_y_swat_01",
            "s_m_y_blackops_02",
        };

        public static string[] AnimalSkins = new string[] {
            "a_c_boar",
            "a_c_deer",
            "a_c_rabbit_01",
        };

        public static string[] MeleeWeapons = new string[]
        {
            "weapon_hammer",
            "weapon_crowbar",
            "weapon_bat",
            "weapon_wrench",
            "weapon_stone_hatchet"
        };

        [ServerEvent(Event.ResourceStart)]
        public static void OnResourceStart()
        {
            int num = 1;

            foreach(Vector3 pos in Positions)
            {
                new Ped(num, PedType.Zombie, ZombieSkins[random.Next(0, ZombieSkins.Length)], pos);
                num++;
            }

            foreach (Vector3 pos in AnimalsSpawn)
            {
                new Ped(num, PedType.Animal, AnimalSkins[random.Next(0, AnimalSkins.Length)], pos);
                num++;
            }
        }

        [RemoteEvent("server:peds:require_controller")]
        public static void RequireController(Player player, int pedId)
        {
            if (player == null) return;
            if (!SpawnedPeds.ContainsKey(pedId)) return;

            SpawnedPeds[pedId].SetController(player);
        }

        [RemoteEvent("server:peds:remove_controller")]
        public static void RemoveController(Player player, int pedId)
        {
            if (player == null) return;
            if (!SpawnedPeds.ContainsKey(pedId)) return;

            SpawnedPeds[pedId].RemoveController();
        }

        [RemoteEvent("server:peds:died")]
        public static void PedDied(Player player, int pedId, double x, double y, double z)
        {
            if (player == null) return;
            if (!SpawnedPeds.ContainsKey(pedId)) return;

            SpawnedPeds[pedId].Death();
        }

        [RemoteEvent("server:peds:kill")]
        public static void PedKill(Player player, int pedId, Player target)
        {
            if (player == null) return;
            if (!SpawnedPeds.ContainsKey(pedId)) return;
        }

        internal class Ped
        {
            public int Id { get; }
            public PedType Type { get; }
            public string Model { get; }
            public bool IsDied { get; set; }
            public Vector3 SpawnPosition { get; set; }

            private GTANetworkAPI.Ped Handle { get; }

            public Ped(int id, PedType type, string model, Vector3 spawnPos)
            {
                Id = id;
                Type = type;
                Model = model;
                SpawnPosition = spawnPos;

                IsDied = false;

                Handle = NAPI.Ped.CreatePed(NAPI.Util.GetHashKey(Model), SpawnPosition, 0f, true, false, false);

                if(Type == PedType.Zombie)
                    Handle.SetSharedData("controllerScript", "ZOMBIE");
                else
                    Handle.SetSharedData("controllerScript", "ANIMAL");

                Handle.SetSharedData("autoControl", true);

                if (random.Next(0, 3) == 0 && Type == PedType.Zombie)
                {
                    int rand = random.Next(0, 5);
                    if(rand >= 4) 
                    {
                        Handle.SetSharedData("weapon", "weapon_pistol");
                    }
                    else
                    {
                        Handle.SetSharedData("meleeWeapon", MeleeWeapons[random.Next(0, MeleeWeapons.Length)]);
                    }
                }

                Handle.SetSharedData("staticDead", false);
                Handle.SetSharedData("pedId", Id);

                SpawnedPeds.Add(Id, this);
            }

            public void Delete()
            {
                NAPI.Task.Run(() =>
                {
                    Handle.Delete();
                });
            }

            public void Death()
            {
                IsDied = true;
                Handle.SetSharedData("staticDead", true);
            }

            public void SetController(Player player)
            {
                Handle.Controller = player;
            }

            public void RemoveController()
            {
                Handle.Controller = null;
            }

            /*public Pl GetController(Player player)
            {
                Handle.Contr;
            }*/

        }
    }

}
