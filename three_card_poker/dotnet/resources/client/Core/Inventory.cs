﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Linq;
using System.Threading;
using GTANetworkAPI;
using Newtonsoft.Json;
using Redage.SDK;
using NeptuneEvo.Core.Character;

namespace NeptuneEvo.Core
{
    class nInventory : Script
    {
        public static float maxPlayerWeight = 15.0f;

        public static Dictionary<int, string> ItemsNames = new Dictionary<int, string>
        {
            {-1, "Маска" },
            {-3, "Перчатки" },
            {-4, "Штаны"},
            {-5, "Рюкзак"},
            {-6, "Обувь"},
            {-7, "Аксессуар"},
            {-8, "Нижняя одежда"},
            {-9, "Бронежилет"},
            {-10, "Украшения"},
            {-11, "Верхняя одежда" },
            {-12, "Головной убор" },
            {-13, "Очки" },
            {-14, "Аксессуар" },
            {1, "Аптечка"},
            {2, "Канистра"},
            {3, "Чипсы"},
            {4, "Пиво"},
            {5, "Пицца"},
            {6, "Бургер"},
            {7, "Хот-Дог"},
            {8, "Сэндвич"},
            {9, "eCola"},
            {10, "Sprunk"},
            {11, "Отмычка для замков"},
            {12, "Сумка с деньгами"},
            {13, "Материалы"},
            {14, "Наркотики"},
            {15, "Сумка с дрелью"},
            {16, "Военная отмычка"},
            {17, "Мешок"},
            {18, "Стяжки"},
            {19, "Ключи от машины"},
            {40, "Подарок"},
            {41, "Связка ключей"},

            {20, "«На корке лимона»"},
            {21, "«На бруснике»"},
            {22, "«Русский стандарт»"},
            {23, "«Asahi»"},
            {24, "«Midori»"},
            {25, "«Yamazaki»"},
            {26, "«Martini Asti»"},
            {27, "«Sambuca»"},
            {28, "«Campari»"},
            {29, "«Дживан»"},
            {30, "«Арарат»"},
            {31, "«Noyan Tapan»"},

            {100, "Pistol" },
            {101, "Combat Pistol" },
            {102, "Pistol 50" },
            {103, "SNS Pistol" },
            {104, "Heavy Pistol" },
            {105, "Vintage Pistol" },
            {106, "Marksman Pistol" },
            {107, "Revolver" },
            {108, "AP Pistol" },
            {109, "Stun Gun" },
            {110, "Flare Gun" },
            {111, "Double Action" },
            {112, "Pistol Mk2" },
            {113, "SNSPistol Mk2" },
            {114, "Revolver Mk2" },

            {115, "Micro SMG" },
            {116, "Machine Pistol" },
            {117, "SMG" },
            {118, "Assault SMG" },
            {119, "Combat PDW" },
            {120, "MG" },
            {121, "Combat MG" },
            {122, "Gusenberg" },
            {123, "Mini SMG" },
            {124, "SMG Mk2" },
            {125, "Combat MG Mk2" },

            {126, "Assault Rifle" },
            {127, "Carbine Rifle" },
            {128, "Advanced Rifle" },
            {129, "Special Carbine" },
            {130, "Bullpup Rifle" },
            {131, "Compact Rifle" },
            {132, "Assault Rifle Mk2" },
            {133, "Carbine Rifle Mk2" },
            {134, "Special Carbine Mk2" },
            {135, "Bullpup Rifle Mk2" },

            {136, "Sniper Rifle" },
            {137, "Heavy Sniper" },
            {138, "Marksman Rifle" },
            {139, "Heavy Sniper Mk2" },
            {140, "Marksman Rifle Mk2" },

            {141, "Pump Shotgun" },
            {142, "SawnOff Shotgun" },
            {143, "Bullpup Shotgun" },
            {144, "Assault Shotgun" },
            {145, "Musket" },
            {146, "Heavy Shotgun" },
            {147, "Double Barrel Shotgun" },
            {148, "Sweeper Shotgun" },
            {149, "Pump Shotgun Mk2" },

            {180, "Нож" },
            {181, "Дубинка" },
            {182, "Молоток" },
            {183, "Бита" },
            {184, "Лом" },
            {185, "Гольф клюшка" },
            {186, "Бутылка" },
            {187, "Кинжал" },
            {188, "Топор" },
            {189, "Кастет" },
            {190, "Мачете" },
            {191, "Фонарик" },
            {192, "Швейцарский нож" },
            {193, "Кий" },
            {194, "Ключ" },
            {195, "Боевой топор" },

            {200, "Пистолетный калибр" },
            {201, "Малый калибр" },
            {202, "Автоматный калибр" },
            {203, "Снайперский калибр" },
            {204, "Дробь" },

            {205, "Удочка" },
            {206, "Улучшенная удочка" },
            {207, "Удочка MK2" },
            {208, "Наживка" },
            {209, "Корюшка" },
            {210, "Кунджа" },
            {211, "Лосось" },
            {212, "Окунь" },
            {213, "Осётр" },
            {214, "Скат" },
            {215, "Тунец" },
            {216, "Угорь" },
            {217, "Чёрный амур" },
            {218, "Щука" },

            {234, "Урожай" },
            {235, "Семена" },

            {238, "Фишки" },

            {556, "Сух.Паёк" },
        };
        public static Dictionary<int, string> ItemsDescriptions = new Dictionary<int, string>();
        public static Dictionary<ItemType, uint> ItemModels = new Dictionary<ItemType, uint>()
        {
            { ItemType.Hat, 1619813869 },
            { ItemType.Mask, 3887136870 },
            { ItemType.Gloves, 3125389411 },
            { ItemType.Leg, 2086911125 },
            { ItemType.Bag, 0000000 },
            { ItemType.Feet, 1682675077 },
            { ItemType.Jewelry, 2329969874 },
            { ItemType.Undershit, 578126062 },
            { ItemType.BodyArmor, 701173564 },
            { ItemType.Unknown, 0000000 },
            { ItemType.Top, 3038378640 },
            { ItemType.Glasses, 2329969874 },
            { ItemType.Accessories, 2329969874 },

            { ItemType.Drugs, 4293279169 },
            { ItemType.Material, 3045218749 },
            { ItemType.Debug, 0000000 },
            { ItemType.HealthKit, 678958360 },
            { ItemType.GasCan, 786272259 },
            { ItemType.Сrisps, 2564432314 },
            { ItemType.Beer, 1940235411 },
            { ItemType.Pizza, 604847691 },
            { ItemType.Burger, 2240524752 },
            { ItemType.HotDog, 2565741261 },
            { ItemType.Sandwich, 987331897 },
            { ItemType.eCola, 144995201 },
            { ItemType.Sprunk, 2973713592 },
            { ItemType.Lockpick, 977923025 },
            { ItemType.ArmyLockpick, 977923025 },
            { ItemType.Pocket, 3887136870 },
            { ItemType.Cuffs, 3887136870 },
            { ItemType.CarKey, 977923025 },
            { ItemType.Present, NAPI.Util.GetHashKey("prop_box_ammo07a") },
            { ItemType.KeyRing, 977923025 },

            { ItemType.RusDrink1, NAPI.Util.GetHashKey("prop_vodka_bottle") },
            { ItemType.RusDrink2, NAPI.Util.GetHashKey("prop_vodka_bottle") },
            { ItemType.RusDrink3, NAPI.Util.GetHashKey("prop_vodka_bottle") },
            { ItemType.YakDrink1, NAPI.Util.GetHashKey("prop_cs_beer_bot_02") },
            { ItemType.YakDrink2, NAPI.Util.GetHashKey("prop_wine_red") },
            { ItemType.YakDrink3, NAPI.Util.GetHashKey("p_whiskey_bottle_s") },
            { ItemType.LcnDrink1, NAPI.Util.GetHashKey("prop_wine_white") },
            { ItemType.LcnDrink2, NAPI.Util.GetHashKey("prop_vodka_bottle") },
            { ItemType.LcnDrink3, NAPI.Util.GetHashKey("prop_wine_red") },
            { ItemType.ArmDrink1, NAPI.Util.GetHashKey("prop_bottle_cognac") },
            { ItemType.ArmDrink2, NAPI.Util.GetHashKey("prop_bottle_cognac") },
            { ItemType.ArmDrink3, NAPI.Util.GetHashKey("prop_bottle_cognac") },

            { ItemType.Pistol, NAPI.Util.GetHashKey("w_pi_pistol") },
            { ItemType.CombatPistol, NAPI.Util.GetHashKey("w_pi_combatpistol") },
            { ItemType.Pistol50, NAPI.Util.GetHashKey("w_pi_pistol50") },
            { ItemType.SNSPistol, NAPI.Util.GetHashKey("w_pi_sns_pistol") },
            { ItemType.HeavyPistol, NAPI.Util.GetHashKey("w_pi_heavypistol") },
            { ItemType.VintagePistol, NAPI.Util.GetHashKey("w_pi_vintage_pistol") },
            { ItemType.MarksmanPistol, NAPI.Util.GetHashKey("w_pi_singleshot") },
            { ItemType.Revolver, NAPI.Util.GetHashKey("w_pi_revolver") },
            { ItemType.APPistol, NAPI.Util.GetHashKey("w_pi_appistol") },
            { ItemType.StunGun, NAPI.Util.GetHashKey("w_pi_stungun") },
            { ItemType.FlareGun, NAPI.Util.GetHashKey("w_pi_flaregun") },
            { ItemType.DoubleAction, NAPI.Util.GetHashKey("mk2") },
            { ItemType.PistolMk2, NAPI.Util.GetHashKey("w_pi_pistolmk2") },
            { ItemType.SNSPistolMk2, NAPI.Util.GetHashKey("w_pi_sns_pistolmk2") },
            { ItemType.RevolverMk2, NAPI.Util.GetHashKey("w_pi_revolvermk2") },

            { ItemType.MicroSMG, NAPI.Util.GetHashKey("w_sb_microsmg") },
            { ItemType.MachinePistol, NAPI.Util.GetHashKey("w_sb_compactsmg") },
            { ItemType.SMG, NAPI.Util.GetHashKey("w_sb_smg") },
            { ItemType.AssaultSMG, NAPI.Util.GetHashKey("w_sb_assaultsmg") },
            { ItemType.CombatPDW, NAPI.Util.GetHashKey("w_sb_pdw") },
            { ItemType.MG, NAPI.Util.GetHashKey("w_mg_mg") },
            { ItemType.CombatMG, NAPI.Util.GetHashKey("w_mg_combatmg") },
            { ItemType.Gusenberg, NAPI.Util.GetHashKey("w_sb_gusenberg") },
            { ItemType.MiniSMG, NAPI.Util.GetHashKey("w_sb_minismg") },
            { ItemType.SMGMk2, NAPI.Util.GetHashKey("w_sb_smgmk2") },
            { ItemType.CombatMGMk2, NAPI.Util.GetHashKey("w_mg_combatmgmk2") },

            { ItemType.AssaultRifle, NAPI.Util.GetHashKey("w_ar_assaultrifle") },
            { ItemType.CarbineRifle, NAPI.Util.GetHashKey("w_ar_carbinerifle") },
            { ItemType.AdvancedRifle, NAPI.Util.GetHashKey("w_ar_advancedrifle") },
            { ItemType.SpecialCarbine, NAPI.Util.GetHashKey("w_ar_specialcarbine") },
            { ItemType.BullpupRifle, NAPI.Util.GetHashKey("w_ar_bullpuprifle") },
            { ItemType.CompactRifle, NAPI.Util.GetHashKey("w_ar_assaultrifle_smg") },
            { ItemType.AssaultRifleMk2, NAPI.Util.GetHashKey("w_ar_assaultriflemk2") },
            { ItemType.CarbineRifleMk2, NAPI.Util.GetHashKey("w_ar_carbineriflemk2") },
            { ItemType.SpecialCarbineMk2, NAPI.Util.GetHashKey("w_ar_specialcarbinemk2") },
            { ItemType.BullpupRifleMk2, NAPI.Util.GetHashKey("w_ar_bullpupriflemk2") },

            { ItemType.SniperRifle, NAPI.Util.GetHashKey("w_sr_sniperrifle") },
            { ItemType.HeavySniper, NAPI.Util.GetHashKey("w_sr_heavysniper") },
            { ItemType.MarksmanRifle, NAPI.Util.GetHashKey("w_sr_marksmanrifle") },
            { ItemType.HeavySniperMk2, NAPI.Util.GetHashKey("w_sr_heavysnipermk2") },
            { ItemType.MarksmanRifleMk2, NAPI.Util.GetHashKey("w_sr_marksmanriflemk2") },

            { ItemType.PumpShotgun, NAPI.Util.GetHashKey("w_sg_pumpshotgun") },
            { ItemType.SawnOffShotgun, NAPI.Util.GetHashKey("w_sg_sawnoff") },
            { ItemType.BullpupShotgun, NAPI.Util.GetHashKey("w_sg_bullpupshotgun") },
            { ItemType.AssaultShotgun, NAPI.Util.GetHashKey("w_sg_assaultshotgun") },
            { ItemType.Musket, NAPI.Util.GetHashKey("w_ar_musket") },
            { ItemType.HeavyShotgun, NAPI.Util.GetHashKey("w_sg_heavyshotgun") },
            { ItemType.DoubleBarrelShotgun, NAPI.Util.GetHashKey("w_sg_doublebarrel") },
            { ItemType.SweeperShotgun, NAPI.Util.GetHashKey("mk2") },
            { ItemType.PumpShotgunMk2, NAPI.Util.GetHashKey("w_sg_pumpshotgunmk2") },

            { ItemType.Knife, NAPI.Util.GetHashKey("w_me_knife_01") },
            { ItemType.Nightstick, NAPI.Util.GetHashKey("w_me_nightstick") },
            { ItemType.Hammer, NAPI.Util.GetHashKey("w_me_hammer") },
            { ItemType.Bat, NAPI.Util.GetHashKey("w_me_bat") },
            { ItemType.Crowbar, NAPI.Util.GetHashKey("w_me_crowbar") },
            { ItemType.GolfClub, NAPI.Util.GetHashKey("w_me_gclub") },
            { ItemType.Bottle, NAPI.Util.GetHashKey("w_me_bottle") },
            { ItemType.Dagger, NAPI.Util.GetHashKey("w_me_dagger") },
            { ItemType.Hatchet, NAPI.Util.GetHashKey("w_me_hatchet") },
            { ItemType.KnuckleDuster, NAPI.Util.GetHashKey("w_me_knuckle") },
            { ItemType.Machete, NAPI.Util.GetHashKey("prop_ld_w_me_machette") },
            { ItemType.Flashlight, NAPI.Util.GetHashKey("w_me_flashlight") },
            { ItemType.SwitchBlade, NAPI.Util.GetHashKey("w_me_switchblade") },
            { ItemType.PoolCue, NAPI.Util.GetHashKey("prop_pool_cue") },
            { ItemType.Wrench, NAPI.Util.GetHashKey("prop_cs_wrench") },
            { ItemType.BattleAxe, NAPI.Util.GetHashKey("w_me_battleaxe") },

            { ItemType.PistolAmmo, NAPI.Util.GetHashKey("w_am_case") },
            { ItemType.RiflesAmmo, NAPI.Util.GetHashKey("w_am_case") },
            { ItemType.ShotgunsAmmo, NAPI.Util.GetHashKey("w_am_case") },
            { ItemType.SMGAmmo, NAPI.Util.GetHashKey("w_am_case") },
            { ItemType.SniperAmmo, NAPI.Util.GetHashKey("w_am_case") },

            /* Fishing */
            { ItemType.Rod, NAPI.Util.GetHashKey("prop_fishing_rod_01") },
            { ItemType.RodUpgrade, NAPI.Util.GetHashKey("prop_fishing_rod_01") },
            { ItemType.RodMK2, NAPI.Util.GetHashKey("prop_fishing_rod_01") },
            { ItemType.Naz, NAPI.Util.GetHashKey("ng_proc_paintcan02a") },
            { ItemType.Koroska, NAPI.Util.GetHashKey("prop_starfish_01") },
            { ItemType.Kyndja, NAPI.Util.GetHashKey("prop_starfish_01") },
            { ItemType.Lococ, NAPI.Util.GetHashKey("prop_starfish_01") },
            { ItemType.Okyn, NAPI.Util.GetHashKey("prop_starfish_01") },
            { ItemType.Ocetr, NAPI.Util.GetHashKey("prop_starfish_01") },
            { ItemType.Skat, NAPI.Util.GetHashKey("prop_starfish_01") },
            { ItemType.Tunec, NAPI.Util.GetHashKey("prop_starfish_01") },
            { ItemType.Ygol, NAPI.Util.GetHashKey("prop_starfish_01") },
            { ItemType.Amyr, NAPI.Util.GetHashKey("prop_starfish_01") },
            { ItemType.Chyka, NAPI.Util.GetHashKey("prop_starfish_01") },

            //Farmer Job Items
            { ItemType.Hay, NAPI.Util.GetHashKey("prop_haybale_01") },
            { ItemType.Seed, NAPI.Util.GetHashKey("ch_prop_ch_moneybag_01a") },

            { ItemType.Payek, NAPI.Util.GetHashKey("prop_ff_noodle_02") },

            { ItemType.CasinoChips, 3045218749 },
        };

        public static Dictionary<ItemType, Vector3> ItemsPosOffset = new Dictionary<ItemType, Vector3>()
        {
            { ItemType.Hat, new Vector3(0, 0, -0.93) },
            { ItemType.Mask, new Vector3(0, 0, -1) },
            { ItemType.Gloves, new Vector3(0, 0, -1) },
            { ItemType.Leg, new Vector3(0, 0, -0.85) },
            { ItemType.Bag, new Vector3() },
            { ItemType.Feet, new Vector3(0, 0, -0.95) },
            { ItemType.Jewelry, new Vector3(0, 0, -0.98) },
            { ItemType.Undershit, new Vector3(0, 0, -0.98) },
            { ItemType.BodyArmor, new Vector3(0, 0, -0.88) },
            { ItemType.Unknown, new Vector3() },
            { ItemType.Top, new Vector3(0, 0, -0.96) },
            { ItemType.Glasses, new Vector3(0, 0, -0.98) },
            { ItemType.Accessories, new Vector3(0, 0, -0.98) },

            { ItemType.Drugs, new Vector3(0, 0, -0.95) },
            { ItemType.Material, new Vector3(0, 0, -0.6) },
            { ItemType.Debug, new Vector3() },
            { ItemType.HealthKit, new Vector3(0, 0, -0.9) },
            { ItemType.GasCan, new Vector3(0, 0, -1) },
            { ItemType.Сrisps, new Vector3(0, 0, -1) },
            { ItemType.Beer, new Vector3(0, 0, -1) },
            { ItemType.Pizza, new Vector3(0, 0, -1) },
            { ItemType.Burger, new Vector3(0, 0, -0.97) },
            { ItemType.HotDog, new Vector3(0, 0, -0.97) },
            { ItemType.Sandwich, new Vector3(0, 0, -0.99) },
            { ItemType.eCola, new Vector3(0, 0, -1) },
            { ItemType.Sprunk, new Vector3(0, 0, -1) },
            { ItemType.Lockpick, new Vector3(0, 0, -0.98) },
            { ItemType.ArmyLockpick, new Vector3(0, 0, -0.98) },
            { ItemType.Pocket, new Vector3(0, 0, -0.98) },
            { ItemType.Cuffs, new Vector3(0, 0, -0.98) },
            { ItemType.CarKey, new Vector3(0, 0, -0.98) },
            { ItemType.Present, new Vector3(0, 0, -0.98) },
            { ItemType.KeyRing, new Vector3(0, 0, -0.98) },

            { ItemType.RusDrink1, new Vector3(0, 0, -1) },
            { ItemType.RusDrink2, new Vector3(0, 0, -1) },
            { ItemType.RusDrink3, new Vector3(0, 0, -1) },
            { ItemType.YakDrink1, new Vector3(0, 0, -0.87) },
            { ItemType.YakDrink2, new Vector3(0, 0, -1) },
            { ItemType.YakDrink3, new Vector3(0, 0, -0.87) },
            { ItemType.LcnDrink1, new Vector3(0, 0, -1) },
            { ItemType.LcnDrink2, new Vector3(0, 0, -1) },
            { ItemType.LcnDrink3, new Vector3(0, 0, -1) },
            { ItemType.ArmDrink1, new Vector3(0, 0, -1) },
            { ItemType.ArmDrink2, new Vector3(0, 0, -1) },
            { ItemType.ArmDrink3, new Vector3(0, 0, -1) },

            { ItemType.Pistol, new Vector3(0, 0, -0.99) },
            { ItemType.CombatPistol, new Vector3(0, 0, -0.99) },
            { ItemType.Pistol50, new Vector3(0, 0, -0.99) },
            { ItemType.SNSPistol, new Vector3(0, 0, -0.99) },
            { ItemType.HeavyPistol, new Vector3(0, 0, -0.99) },
            { ItemType.VintagePistol, new Vector3(0, 0, -0.99) },
            { ItemType.MarksmanPistol, new Vector3(0, 0, -0.99) },
            { ItemType.Revolver, new Vector3(0, 0, -0.99) },
            { ItemType.APPistol, new Vector3(0, 0, -0.99) },
            { ItemType.StunGun, new Vector3(0, 0, -0.99) },
            { ItemType.FlareGun, new Vector3(0, 0, -0.99) },
            { ItemType.DoubleAction, new Vector3(0, 0, -0.99) },
            { ItemType.PistolMk2, new Vector3(0, 0, -0.99) },
            { ItemType.SNSPistolMk2, new Vector3(0, 0, -0.99) },
            { ItemType.RevolverMk2, new Vector3(0, 0, -0.99) },

            { ItemType.MicroSMG, new Vector3(0, 0, -0.99) },
            { ItemType.MachinePistol, new Vector3(0, 0, -0.99) },
            { ItemType.SMG, new Vector3(0, 0, -0.99) },
            { ItemType.AssaultSMG, new Vector3(0, 0, -0.99) },
            { ItemType.CombatPDW, new Vector3(0, 0, -0.99) },
            { ItemType.MG, new Vector3(0, 0, -0.99) },
            { ItemType.CombatMG, new Vector3(0, 0, -0.99) },
            { ItemType.Gusenberg, new Vector3(0, 0, -0.99) },
            { ItemType.MiniSMG, new Vector3(0, 0, -0.99) },
            { ItemType.SMGMk2, new Vector3(0, 0, -0.99) },
            { ItemType.CombatMGMk2, new Vector3(0, 0, -0.99) },

            { ItemType.AssaultRifle, new Vector3(0, 0, -0.99) },
            { ItemType.CarbineRifle, new Vector3(0, 0, -0.99) },
            { ItemType.AdvancedRifle, new Vector3(0, 0, -0.99) },
            { ItemType.SpecialCarbine, new Vector3(0, 0, -0.99) },
            { ItemType.BullpupRifle, new Vector3(0, 0, -0.99) },
            { ItemType.CompactRifle, new Vector3(0, 0, -0.99) },
            { ItemType.AssaultRifleMk2, new Vector3(0, 0, -0.99) },
            { ItemType.CarbineRifleMk2, new Vector3(0, 0, -0.99) },
            { ItemType.SpecialCarbineMk2, new Vector3(0, 0, -0.99) },
            { ItemType.BullpupRifleMk2, new Vector3(0, 0, -0.99) },

            { ItemType.SniperRifle, new Vector3(0, 0, -0.99) },
            { ItemType.HeavySniper, new Vector3(0, 0, -0.99) },
            { ItemType.MarksmanRifle, new Vector3(0, 0, -0.99) },
            { ItemType.HeavySniperMk2, new Vector3(0, 0, -0.99) },
            { ItemType.MarksmanRifleMk2, new Vector3(0, 0, -0.99) },

            { ItemType.PumpShotgun, new Vector3(0, 0, -0.99) },
            { ItemType.SawnOffShotgun, new Vector3(0, 0, -0.99) },
            { ItemType.BullpupShotgun, new Vector3(0, 0, -0.99) },
            { ItemType.AssaultShotgun, new Vector3(0, 0, -0.99) },
            { ItemType.Musket, new Vector3(0, 0, -0.99) },
            { ItemType.HeavyShotgun, new Vector3(0, 0, -0.99) },
            { ItemType.DoubleBarrelShotgun, new Vector3(0, 0, -0.99) },
            { ItemType.SweeperShotgun, new Vector3(0, 0, -0.99) },
            { ItemType.PumpShotgunMk2, new Vector3(0, 0, -0.99) },

            { ItemType.Knife, new Vector3(0, 0, -0.99) },
            { ItemType.Nightstick, new Vector3(0, 0, -0.99) },
            { ItemType.Hammer, new Vector3(0, 0, -0.99) },
            { ItemType.Bat, new Vector3(0, 0, -0.99) },
            { ItemType.Crowbar, new Vector3(0, 0, -0.99) },
            { ItemType.GolfClub, new Vector3(0, 0, -0.99) },
            { ItemType.Bottle, new Vector3(0, 0, -0.99) },
            { ItemType.Dagger, new Vector3(0, 0, -0.99) },
            { ItemType.Hatchet, new Vector3(0, 0, -0.99) },
            { ItemType.KnuckleDuster, new Vector3(0, 0, -0.99) },
            { ItemType.Machete, new Vector3(0, 0, -0.99) },
            { ItemType.Flashlight, new Vector3(0, 0, -0.99) },
            { ItemType.SwitchBlade, new Vector3(0, 0, -0.99) },
            { ItemType.PoolCue, new Vector3(0, 0, -0.99) },
            { ItemType.Wrench, new Vector3(0, 0, -0.985) },
            { ItemType.BattleAxe, new Vector3(0, 0, -0.99) },

            { ItemType.PistolAmmo, new Vector3(0, 0, -1) },
            { ItemType.RiflesAmmo, new Vector3(0, 0, -1) },
            { ItemType.ShotgunsAmmo, new Vector3(0, 0, -1) },
            { ItemType.SMGAmmo, new Vector3(0, 0, -1) },
            { ItemType.SniperAmmo, new Vector3(0, 0, -1) },

            /* Fishing */
            { ItemType.Rod, new Vector3(0, 0, -0.99) },
            { ItemType.RodUpgrade, new Vector3(0, 0, -0.99) },
            { ItemType.RodMK2, new Vector3(0, 0, -0.99) },
            { ItemType.Naz, new Vector3(0, 0, -0.99) },
            { ItemType.Koroska, new Vector3(0, 0, -0.99) },
            { ItemType.Kyndja, new Vector3(0, 0, -0.99) },
            { ItemType.Lococ, new Vector3(0, 0, -0.99) },
            { ItemType.Okyn, new Vector3(0, 0, -0.99) },
            { ItemType.Ocetr, new Vector3(0, 0, -0.99) },
            { ItemType.Skat, new Vector3(0, 0, -0.99) },
            { ItemType.Tunec, new Vector3(0, 0, -0.99) },
            { ItemType.Ygol, new Vector3(0, 0, -0.99) },
            { ItemType.Amyr, new Vector3(0, 0, -0.99) },
            { ItemType.Chyka, new Vector3(0, 0, -0.99) },

            //Farmer Job Items
            { ItemType.Hay, new Vector3(0, 0, -0.99) },
            { ItemType.Seed, new Vector3(0, 0, -0.99) },

            { ItemType.Payek, new Vector3(0, 0, -1) },

            { ItemType.CasinoChips, new Vector3(0, 0, -0.6) },
        };
        public static Dictionary<ItemType, Vector3> ItemsRotOffset = new Dictionary<ItemType, Vector3>()
        {
            { ItemType.Hat, new Vector3() },
            { ItemType.Mask, new Vector3() },
            { ItemType.Gloves, new Vector3(90, 0, 0) },
            { ItemType.Leg, new Vector3() },
            { ItemType.Bag, new Vector3() },
            { ItemType.Feet, new Vector3() },
            { ItemType.Jewelry, new Vector3() },
            { ItemType.Undershit, new Vector3() },
            { ItemType.BodyArmor, new Vector3(90, 90, 0) },
            { ItemType.Unknown, new Vector3() },
            { ItemType.Top, new Vector3() },
            { ItemType.Glasses, new Vector3() },
            { ItemType.Accessories, new Vector3() },

            { ItemType.Drugs, new Vector3() },
            { ItemType.Material, new Vector3() },
            { ItemType.Debug, new Vector3() },
            { ItemType.HealthKit, new Vector3() },
            { ItemType.GasCan, new Vector3() },
            { ItemType.Сrisps, new Vector3(90, 90, 0) },
            { ItemType.Beer, new Vector3() },
            { ItemType.Pizza, new Vector3() },
            { ItemType.Burger, new Vector3() },
            { ItemType.HotDog, new Vector3() },
            { ItemType.Sandwich, new Vector3() },
            { ItemType.eCola, new Vector3() },
            { ItemType.Sprunk, new Vector3() },
            { ItemType.Lockpick, new Vector3() },
            { ItemType.ArmyLockpick, new Vector3() },
            { ItemType.Pocket, new Vector3() },
            { ItemType.Cuffs, new Vector3() },
            { ItemType.CarKey, new Vector3() },
            { ItemType.Present, new Vector3() },
            { ItemType.KeyRing, new Vector3() },

            { ItemType.RusDrink1, new Vector3() },
            { ItemType.RusDrink2, new Vector3() },
            { ItemType.RusDrink3, new Vector3() },
            { ItemType.YakDrink1, new Vector3() },
            { ItemType.YakDrink2, new Vector3() },
            { ItemType.YakDrink3, new Vector3() },
            { ItemType.LcnDrink1, new Vector3() },
            { ItemType.LcnDrink2, new Vector3() },
            { ItemType.LcnDrink3, new Vector3() },
            { ItemType.ArmDrink1, new Vector3() },
            { ItemType.ArmDrink2, new Vector3() },
            { ItemType.ArmDrink3, new Vector3() },

            { ItemType.Pistol, new Vector3(90, 0, 0) },
            { ItemType.CombatPistol, new Vector3(90, 0, 0) },
            { ItemType.Pistol50, new Vector3(90, 0, 0) },
            { ItemType.SNSPistol, new Vector3(90, 0, 0) },
            { ItemType.HeavyPistol, new Vector3(90, 0, 0) },
            { ItemType.VintagePistol, new Vector3(90, 0, 0) },
            { ItemType.MarksmanPistol, new Vector3(90, 0, 0) },
            { ItemType.Revolver, new Vector3(90, 0, 0) },
            { ItemType.APPistol, new Vector3(90, 0, 0) },
            { ItemType.StunGun, new Vector3(90, 0, 0) },
            { ItemType.FlareGun, new Vector3(90, 0, 0) },
            { ItemType.DoubleAction, new Vector3(90, 0, 0) },
            { ItemType.PistolMk2, new Vector3(90, 0, 0) },
            { ItemType.SNSPistolMk2, new Vector3(90, 0, 0) },
            { ItemType.RevolverMk2, new Vector3(90, 0, 0) },

            { ItemType.MicroSMG, new Vector3(90, 0, 0) },
            { ItemType.MachinePistol, new Vector3(90, 0, 0) },
            { ItemType.SMG, new Vector3(90, 0, 0) },
            { ItemType.AssaultSMG, new Vector3(90, 0, 0) },
            { ItemType.CombatPDW, new Vector3(90, 0, 0) },
            { ItemType.MG, new Vector3(90, 0, 0) },
            { ItemType.CombatMG, new Vector3(90, 0, 0) },
            { ItemType.Gusenberg, new Vector3(90, 0, 0) },
            { ItemType.MiniSMG, new Vector3(90, 0, 0) },
            { ItemType.SMGMk2, new Vector3(90, 0, 0) },
            { ItemType.CombatMGMk2, new Vector3(90, 0, 0) },

            { ItemType.AssaultRifle, new Vector3(90, 0, 0) },
            { ItemType.CarbineRifle, new Vector3(90, 0, 0) },
            { ItemType.AdvancedRifle, new Vector3(90, 0, 0) },
            { ItemType.SpecialCarbine, new Vector3(90, 0, 0) },
            { ItemType.BullpupRifle, new Vector3(90, 0, 0) },
            { ItemType.CompactRifle, new Vector3(90, 0, 0) },
            { ItemType.AssaultRifleMk2, new Vector3(90, 0, 0) },
            { ItemType.CarbineRifleMk2, new Vector3(90, 0, 0) },
            { ItemType.SpecialCarbineMk2, new Vector3(90, 0, 0) },
            { ItemType.BullpupRifleMk2, new Vector3(90, 0, 0) },

            { ItemType.SniperRifle, new Vector3(90, 0, 0) },
            { ItemType.HeavySniper, new Vector3(90, 0, 0) },
            { ItemType.MarksmanRifle, new Vector3(90, 0, 0) },
            { ItemType.HeavySniperMk2, new Vector3(90, 0, 0) },
            { ItemType.MarksmanRifleMk2, new Vector3(90, 0, 0) },

            { ItemType.PumpShotgun, new Vector3(90, 0, 0) },
            { ItemType.SawnOffShotgun, new Vector3(90, 0, 0) },
            { ItemType.BullpupShotgun, new Vector3(90, 0, 0) },
            { ItemType.AssaultShotgun, new Vector3(90, 0, 0) },
            { ItemType.Musket, new Vector3(90, 0, 0) },
            { ItemType.HeavyShotgun, new Vector3(90, 0, 0) },
            { ItemType.DoubleBarrelShotgun, new Vector3(90, 0, 0) },
            { ItemType.SweeperShotgun, new Vector3(90, 0, 0) },
            { ItemType.PumpShotgunMk2, new Vector3(90, 0, 0) },

            { ItemType.Knife, new Vector3(90, 0, 0) },
            { ItemType.Nightstick, new Vector3(90, 0, 0) },
            { ItemType.Hammer, new Vector3(90, 0, 0) },
            { ItemType.Bat, new Vector3(90, 0, 0) },
            { ItemType.Crowbar, new Vector3(90, 0, 0) },
            { ItemType.GolfClub, new Vector3(90, 0, 0) },
            { ItemType.Bottle, new Vector3(90, 0, 0) },
            { ItemType.Dagger, new Vector3(90, 0, 0) },
            { ItemType.Hatchet, new Vector3(90, 0, 0) },
            { ItemType.KnuckleDuster, new Vector3(90, 0, 0) },
            { ItemType.Machete, new Vector3(90, 0, 0) },
            { ItemType.Flashlight, new Vector3(90, 0, 0) },
            { ItemType.SwitchBlade, new Vector3(90, 0, 0) },
            { ItemType.PoolCue, new Vector3(90, 0, 0) },
            { ItemType.Wrench, new Vector3(-12, 0, 0) },
            { ItemType.BattleAxe, new Vector3(90, 0, 0) },

            { ItemType.PistolAmmo, new Vector3(90, 0, 0) },
            { ItemType.RiflesAmmo, new Vector3(90, 0, 0) },
            { ItemType.ShotgunsAmmo, new Vector3(90, 0, 0) },
            { ItemType.SMGAmmo, new Vector3(90, 0, 0) },
            { ItemType.SniperAmmo, new Vector3(90, 0, 0) },

            /* Fishing */
            { ItemType.Rod, new Vector3(90, 0, 0) },
            { ItemType.RodUpgrade, new Vector3(90, 0, 0) },
            { ItemType.RodMK2, new Vector3(90, 0, 0) },
            { ItemType.Naz, new Vector3(90, 0, 0) },
            { ItemType.Koroska, new Vector3(90, 0, 0) },
            { ItemType.Kyndja, new Vector3(90, 0, 0) },
            { ItemType.Lococ, new Vector3(90, 0, 0) },
            { ItemType.Okyn, new Vector3(90, 0, 0) },
            { ItemType.Ocetr, new Vector3(90, 0, 0) },
            { ItemType.Skat, new Vector3(90, 0, 0) },
            { ItemType.Tunec, new Vector3(90, 0, 0) },
            { ItemType.Ygol, new Vector3(90, 0, 0) },
            { ItemType.Amyr, new Vector3(90, 0, 0) },
            { ItemType.Chyka, new Vector3(90, 0, 0) },

            //Farmer Job Items
            { ItemType.Hay, new Vector3(0, 0, 0) },
            { ItemType.Seed, new Vector3(0, 0, 0) },

            { ItemType.Payek, new Vector3() },

            { ItemType.CasinoChips, new Vector3() },
        };

        public static Dictionary<ItemType, int> ItemsStacks = new Dictionary<ItemType, int>()
        {
            { ItemType.BagWithMoney, 1 },
            { ItemType.Material, 300 },
            { ItemType.Drugs, 50 },
            { ItemType.BagWithDrill, 1 },
            { ItemType.Debug, 10000 },
            { ItemType.HealthKit, 5 },
            { ItemType.GasCan, 2 },
            { ItemType.Сrisps, 4 },
            { ItemType.Beer, 5 },
            { ItemType.Pizza, 3 },
            { ItemType.Burger, 4 },
            { ItemType.HotDog, 5 },
            { ItemType.Sandwich, 7 },
            { ItemType.eCola, 5 },
            { ItemType.Sprunk, 5 },
            { ItemType.Lockpick, 10 },
            { ItemType.ArmyLockpick, 10 },
            { ItemType.Pocket, 5 },
            { ItemType.Cuffs, 5 },
            { ItemType.CarKey, 1 },
            { ItemType.Present, 1 },
            { ItemType.KeyRing, 1 },

            { ItemType.Mask, 1 },
            { ItemType.Gloves, 1 },
            { ItemType.Leg, 1 },
            { ItemType.Bag, 1 },
            { ItemType.Feet, 1 },
            { ItemType.Jewelry, 1 },
            { ItemType.Undershit, 1 },
            { ItemType.BodyArmor, 1 },
            { ItemType.Unknown, 1 },
            { ItemType.Top, 1 },
            { ItemType.Hat, 1 },
            { ItemType.Glasses, 1 },
            { ItemType.Accessories, 1 },

            { ItemType.RusDrink1, 5 },
            { ItemType.RusDrink2, 5 },
            { ItemType.RusDrink3, 5 },

            { ItemType.YakDrink1, 5 },
            { ItemType.YakDrink2, 5 },
            { ItemType.YakDrink3, 5 },

            { ItemType.LcnDrink1, 5 },
            { ItemType.LcnDrink2, 5 },
            { ItemType.LcnDrink3, 5 },

            { ItemType.ArmDrink1, 5 },
            { ItemType.ArmDrink2, 5 },
            { ItemType.ArmDrink3, 5 },

            { ItemType.Pistol, 1 },
            { ItemType.CombatPistol, 1 },
            { ItemType.Pistol50, 1 },
            { ItemType.SNSPistol, 1 },
            { ItemType.HeavyPistol, 1 },
            { ItemType.VintagePistol, 1 },
            { ItemType.MarksmanPistol, 1 },
            { ItemType.Revolver, 1 },
            { ItemType.APPistol, 1 },
            { ItemType.StunGun, 1 },
            { ItemType.FlareGun, 1 },
            { ItemType.DoubleAction, 1 },
            { ItemType.PistolMk2, 1 },
            { ItemType.SNSPistolMk2, 1 },
            { ItemType.RevolverMk2, 1 },

            { ItemType.MicroSMG, 1 },
            { ItemType.MachinePistol, 1 },
            { ItemType.SMG, 1 },
            { ItemType.AssaultSMG, 1 },
            { ItemType.CombatPDW, 1 },
            { ItemType.MG, 1 },
            { ItemType.CombatMG, 1 },
            { ItemType.Gusenberg, 1 },
            { ItemType.MiniSMG, 1 },
            { ItemType.SMGMk2, 1 },
            { ItemType.CombatMGMk2, 1 },

            { ItemType.AssaultRifle, 1 },
            { ItemType.CarbineRifle, 1 },
            { ItemType.AdvancedRifle, 1 },
            { ItemType.SpecialCarbine, 1 },
            { ItemType.BullpupRifle, 1 },
            { ItemType.CompactRifle, 1 },
            { ItemType.AssaultRifleMk2, 1 },
            { ItemType.CarbineRifleMk2, 1 },
            { ItemType.SpecialCarbineMk2, 1 },
            { ItemType.BullpupRifleMk2, 1 },

            { ItemType.SniperRifle, 1 },
            { ItemType.HeavySniper, 1 },
            { ItemType.MarksmanRifle, 1 },
            { ItemType.HeavySniperMk2, 1 },
            { ItemType.MarksmanRifleMk2, 1 },

            { ItemType.PumpShotgun, 1 },
            { ItemType.SawnOffShotgun, 1 },
            { ItemType.BullpupShotgun, 1 },
            { ItemType.AssaultShotgun, 1 },
            { ItemType.Musket, 1 },
            { ItemType.HeavyShotgun, 1 },
            { ItemType.DoubleBarrelShotgun, 1 },
            { ItemType.SweeperShotgun, 1 },
            { ItemType.PumpShotgunMk2, 1 },

            { ItemType.Knife, 1 },
            { ItemType.Nightstick, 1 },
            { ItemType.Hammer, 1 },
            { ItemType.Bat, 1 },
            { ItemType.Crowbar, 1 },
            { ItemType.GolfClub, 1 },
            { ItemType.Bottle, 1 },
            { ItemType.Dagger, 1 },
            { ItemType.Hatchet, 1 },
            { ItemType.KnuckleDuster, 1 },
            { ItemType.Machete, 1 },
            { ItemType.Flashlight, 1 },
            { ItemType.SwitchBlade, 1 },
            { ItemType.PoolCue, 1 },
            { ItemType.Wrench, 1 },
            { ItemType.BattleAxe, 1 },

            { ItemType.PistolAmmo, 120 },
            { ItemType.RiflesAmmo, 200 },
            { ItemType.ShotgunsAmmo, 100 },
            { ItemType.SMGAmmo, 200 },
            { ItemType.SniperAmmo, 20 },

            /* Fishing */
            { ItemType.Rod, 1 },
            { ItemType.RodUpgrade, 1 },
            { ItemType.RodMK2, 1 },
            { ItemType.Naz, 100 },
            { ItemType.Koroska, 30 },
            { ItemType.Kyndja, 30 },
            { ItemType.Lococ, 30 },
            { ItemType.Okyn, 30 },
            { ItemType.Ocetr, 30 },
            { ItemType.Skat, 30 },
            { ItemType.Tunec, 30 },
            { ItemType.Ygol, 30 },
            { ItemType.Amyr, 30 },
            { ItemType.Chyka, 30 },

            //Farmer Job Items
            { ItemType.Hay, 60 }, //60 урожая всего в инвентаре
            { ItemType.Seed, 100 }, //100 семян всего в инвентаре (максимум)

            { ItemType.Payek, 2 },

            { ItemType.CasinoChips, 10000000 },
        };

        public static List<ItemType> ClothesItems = new List<ItemType>()
        {
            ItemType.Mask,
            ItemType.Gloves,
            ItemType.Leg,
            ItemType.Bag,
            ItemType.Feet,
            ItemType.Jewelry,
            ItemType.Undershit,
            ItemType.BodyArmor,
            ItemType.Unknown,
            ItemType.Top,
            ItemType.Hat,
            ItemType.Glasses,
            ItemType.Accessories,
        };
        public static List<ItemType> WeaponsItems = new List<ItemType>()
        {
            ItemType.Pistol,
            ItemType.CombatPistol,
            ItemType.Pistol50,
            ItemType.SNSPistol,
            ItemType.HeavyPistol,
            ItemType.VintagePistol,
            ItemType.MarksmanPistol,
            ItemType.Revolver,
            ItemType.APPistol,
            ItemType.FlareGun,
            ItemType.DoubleAction,
            ItemType.PistolMk2,
            ItemType.SNSPistolMk2,
            ItemType.RevolverMk2,

            ItemType.MicroSMG,
            ItemType.MachinePistol,
            ItemType.SMG,
            ItemType.AssaultSMG,
            ItemType.CombatPDW,
            ItemType.MG,
            ItemType.CombatMG,
            ItemType.Gusenberg,
            ItemType.MiniSMG,
            ItemType.SMGMk2,
            ItemType.CombatMGMk2,

            ItemType.AssaultRifle,
            ItemType.CarbineRifle,
            ItemType.AdvancedRifle,
            ItemType.SpecialCarbine,
            ItemType.BullpupRifle,
            ItemType.CompactRifle,
            ItemType.AssaultRifleMk2,
            ItemType.CarbineRifleMk2,
            ItemType.SpecialCarbineMk2,
            ItemType.BullpupRifleMk2,

            ItemType.SniperRifle,
            ItemType.HeavySniper,
            ItemType.MarksmanRifle,
            ItemType.HeavySniperMk2,
            ItemType.MarksmanRifleMk2,

            ItemType.PumpShotgun,
            ItemType.SawnOffShotgun,
            ItemType.BullpupShotgun,
            ItemType.AssaultShotgun,
            ItemType.Musket,
            ItemType.HeavyShotgun,
            ItemType.DoubleBarrelShotgun,
            ItemType.SweeperShotgun,
            ItemType.PumpShotgunMk2,
        };
        public static List<ItemType> MeleeWeaponsItems = new List<ItemType>()
        {
            ItemType.Knife,
            ItemType.Nightstick,
            ItemType.Hammer,
            ItemType.Bat,
            ItemType.Crowbar,
            ItemType.GolfClub,
            ItemType.Bottle,
            ItemType.Dagger,
            ItemType.Hatchet,
            ItemType.KnuckleDuster,
            ItemType.Machete,
            ItemType.Flashlight,
            ItemType.SwitchBlade,
            ItemType.PoolCue,
            ItemType.Wrench,
            ItemType.BattleAxe,
            ItemType.StunGun,
        };
        public static List<ItemType> AmmoItems = new List<ItemType>()
        {
            ItemType.PistolAmmo,
            ItemType.RiflesAmmo,
            ItemType.ShotgunsAmmo,
            ItemType.SMGAmmo,
            ItemType.SniperAmmo
        };
        public static List<ItemType> AlcoItems = new List<ItemType>()
        {
            ItemType.LcnDrink1,
            ItemType.LcnDrink2,
            ItemType.LcnDrink3,
            ItemType.RusDrink1,
            ItemType.RusDrink2,
            ItemType.RusDrink3,
            ItemType.YakDrink1,
            ItemType.YakDrink2,
            ItemType.YakDrink3,
            ItemType.ArmDrink1,
            ItemType.ArmDrink2,
            ItemType.ArmDrink3,
        };

        #region ItemType.Present
        public static readonly List<Tuple<int, int>> PresentsTypes = new List<Tuple<int, int>>()
        {
            new Tuple<int, int>(0, 5),
            new Tuple<int, int>(1, 4),
            new Tuple<int, int>(2, 3),
            new Tuple<int, int>(5, 0),
            new Tuple<int, int>(4, 1),
            new Tuple<int, int>(3, 2),
        };
        public static readonly List<int> TypesCounts = new List<int>()
        {
            10, 25, 50, 1000, 5000, 10000
        };
        #endregion
        // UUID, Items by index
        public static Dictionary<int, List<nItem>> Items = new Dictionary<int, List<nItem>>();
        public static Dictionary<int, List<nItem>> ActiveClothes = new Dictionary<int, List<nItem>>();
        public static Dictionary<int, List<nItem>> ActiveWeapons = new Dictionary<int, List<nItem>>();

        public static int maxSlots = 17;
        public static int maxClothesSlots = 15;
        public static int maxWeaponsSlots = 3;

        private static nLog Log = new nLog("nInventory");
        private static Timer SaveTimer;

        #region Constructor
        [ServerEvent(Event.ResourceStart)]
        public void onResourceStart()
        {
            try
            {
                Log.Write("Loading player items...", nLog.Type.Info);
                // // //
                var result = MySQL.QueryRead($"SELECT * FROM `inventory`");
                if (result == null || result.Rows.Count == 0)
                {
                    Log.Write("DB return null result", nLog.Type.Warn);
                    return;
                }
                foreach (DataRow Row in result.Rows)
                {
                    int UUID = Convert.ToInt32(Row["uuid"]);
                    string json = Convert.ToString(Row["items"]);
                    string weapon = Convert.ToString(Row["weapons"]);
                    string clothe = Convert.ToString(Row["clothes"]);

                    List<nItem> items = JsonConvert.DeserializeObject<List<nItem>>(json);
                    List<nItem> weapons = JsonConvert.DeserializeObject<List<nItem>>(weapon);
                    List<nItem> clothes = JsonConvert.DeserializeObject<List<nItem>>(clothe);

                    Items.Add(UUID, items);
                    ActiveClothes.Add(UUID, clothes);
                    ActiveWeapons.Add(UUID, weapons);
                }
                SaveTimer = new Timer(new TimerCallback(SaveAll), null, 0, 1800000);
                Log.Write("Items loaded.", nLog.Type.Success);
            }
            catch (Exception e)
            {
                Log.Write("EXCEPTION AT \"INVENTORY_CONSTRUCT\":\n" + e.ToString(), nLog.Type.Error);
            }
        }
        #endregion

        #region Add/Remove item
        public static void Add(Player player, nItem item, float weight = 0.0f, int toIndex = -1)
        {
            try
            {
                int UUID = Main.Players[player].UUID;
                //int index = FindIndex(UUID, item.Type);
                int emptyIndex = FindIndex(UUID, ItemType.Debug);

                if (toIndex != -1)
                    emptyIndex = toIndex;

                if (ClothesItems.Contains(item.Type) || WeaponsItems.Contains(item.Type) || item.Type == ItemType.CarKey || item.Type == ItemType.KeyRing)
                {
                    if (emptyIndex != -1)
                    {
                        if (Items[UUID][emptyIndex].Type == ItemType.Debug)
                        {
                            Items[UUID][emptyIndex] = item;
                            GUI.Dashboard.Update(player, item, emptyIndex);

                        }
                    }
                }
                else
                {
                    /*if (index != -1)
                    {
                        int count = Items[UUID][index].Count;
                        Items[UUID][index].Count = count + item.Count;

                        if (weight == 0f)
                            Items[UUID][index].Weight = Items[UUID][index].Count * ItemsWeight[Items[UUID][index].Type];
                        else
                            Items[UUID][index].Weight += weight;

                        GUI.Dashboard.Update(player, Items[UUID][index], index);
                        //Log.Debug($"Added existing item! {UUID.ToString()}:{index.ToString()}");
                    }*/
                    // else
                    // {
                    if (emptyIndex != -1)
                    {
                        Dictionary<int, int> pairs = FindFree(UUID, item.Type);

                        int counts = item.Count;

                        if (pairs.Count != 0)
                        {
                        
                            foreach (KeyValuePair<int, int> pair in pairs)
                            {
                                if (counts <= 0)
                                    continue;

                                Items[UUID][pair.Key].Count += counts > pair.Value ? pair.Value : counts;
                                Items[UUID][pair.Key].Weight += counts > pair.Value ? pair.Value * ItemsWeight.GetItemWeight(Items[UUID][pair.Key].Type) : counts * ItemsWeight.GetItemWeight(Items[UUID][pair.Key].Type);

                                counts -= pair.Value;

                                GUI.Dashboard.Update(player, Items[UUID][pair.Key], pair.Key);
                            }
                        }

                        if (counts > 0)
                        {
                            if (Items[UUID][emptyIndex].Type == ItemType.Debug)
                            {
                                if (weight == 0.0f)
                                    item.Weight = item.Count * ItemsWeight.GetItemWeight(item.Type);
                                else
                                    item.Weight = weight;

                                Items[UUID][emptyIndex] = new nItem(item.Type, item.Count, item.Data);

                                GUI.Dashboard.Update(player, item, emptyIndex);
                            }
                        }
                    }
                    else
                    {
                        Dictionary<int, int> pairs = FindFree(UUID, item.Type);

                        if (pairs.Count != 0)
                        {
                            int counts = item.Count;

                            foreach (KeyValuePair<int, int> pair in pairs)
                            {
                                if (counts <= 0)
                                    continue;

                                Items[UUID][pair.Key].Count += counts > pair.Value ? pair.Value : counts;
                                Items[UUID][pair.Key].Weight += counts > pair.Value ? pair.Value * ItemsWeight.GetItemWeight(Items[UUID][pair.Key].Type) : counts * ItemsWeight.GetItemWeight(Items[UUID][pair.Key].Type);

                                counts -= pair.Value;

                                GUI.Dashboard.Update(player, Items[UUID][pair.Key], pair.Key);
                            }
                        }
                    }
                    //GUI.Dashboard.sendItems(player);
                    // }
                }
                Log.Debug($"Item added. {UUID.ToString()}:{toIndex.ToString()}");
            }
            catch (Exception e)
            {
                Log.Write("EXCEPTION AT \"INVENTORY_ADD\":\n" + e.ToString(), nLog.Type.Error);
            }
        }

        public static float GetTotalWeight(Player player)
        {
            return GetTotalWeight(nInventory.Items[Main.Players[player].UUID]);
        }

        public static float GetTotalWeight(List<nItem> items)
        {
            float weight = 0f;

            items.ForEach((nItem item) => weight += item.Type != ItemType.Debug ? item.Weight : 0.0f);

            return weight;
        }


        public static int TryAdd(Player client, nItem item)
        {
            try
            {
                int UUID = Main.Players[client].UUID;

                int index = FindIndex(UUID, item.Type);

                int tail = 0;

                //Log.Write($"{item.Type} {item.Count} {item.Weight}");

                if (ClothesItems.Contains(item.Type) || item.Type == ItemType.CarKey || item.Type == ItemType.KeyRing)
                {
                    if (isFull(UUID))
                        return -1;
                }
                else if (WeaponsItems.Contains(item.Type) || MeleeWeaponsItems.Contains(item.Type))
                {
                    if (isFull(UUID))
                        return -1;
                    float weight = GetTotalWeight(client);
                    if (weight + (item.Weight) >= maxPlayerWeight)
                    {
                        return -2;
                    }
                    /*var ammoType = Weapons.WeaponsAmmoTypes[item.Type];
                    var sameTypeWeapon = Items[UUID].FirstOrDefault(i => WeaponsItems.Contains(i.Type) && Weapons.WeaponsAmmoTypes[i.Type] == ammoType);
                    if (sameTypeWeapon != null) return -1;*/
                }
                else
                {
                    if (index != -1)
                    {
                        int max = (ItemsStacks.ContainsKey(item.Type)) ? ItemsStacks[item.Type] : 1;
                        int count = Items[UUID][index].Count;
                        int temp = count + item.Count;
                        if (temp > max)
                        {
                            int emptyIndex = FindIndex(UUID, ItemType.Debug);
                            if (emptyIndex == -1)
                            {
                                tail = temp - max;

                                Dictionary<int, int> dict = new Dictionary<int, int>();
                                dict = FindFree(UUID, item.Type);

                                if (dict.Count != 0)
                                {
                                    foreach (KeyValuePair<int, int> pair in dict)
                                    {
                                        if (tail == 0)
                                            break;
                                        tail -= tail - pair.Value < 0 ? tail : pair.Value;
                                    }

                                    if (tail > 0)
                                        return tail;

                                }
                                else
                                {
                                    return tail;
                                }
                            }
                        }

                        float weight = GetTotalWeight(client);
                        //Log.Write($"{weight}");
                        if (weight + (item.Weight) >= maxPlayerWeight)
                        {
                            return -2;
                        }
                    }
                    else
                    {
                        int emptyIndex = FindIndex(UUID, ItemType.Debug);

                        if (emptyIndex == -1)
                        {
                            return -1;
                        }

                        if (item.Count > ItemsStacks[item.Type])
                        {
                            tail = item.Count - ItemsStacks[item.Type];
                            return tail;
                        }

                        float weight = GetTotalWeight(client);
                        if (weight + (item.Weight) >= maxPlayerWeight)
                        {
                            return -2;
                        }

                    }
                }
                return tail;
            }
            catch (Exception e)
            {
                Log.Write("EXCEPTION AT \"INVENTORY_ADD\":\n" + e.ToString() + "\n" + e.StackTrace, nLog.Type.Error);
                return 0;
            }
        }
        public static void Remove(Player player, ItemType type, int count)
        {
            try
            {
                int UUID = Main.Players[player].UUID;
                int Index = FindIndex(UUID, type);
                if (Index != -1)
                {
                    int temp = Items[UUID][Index].Count - count;
                    if (temp > 0)
                    {
                        Items[UUID][Index].Count = temp;
                        GUI.Dashboard.Update(player, Items[UUID][Index], Index);
                    }
                    else
                    {
                        Items[UUID][Index] = new nItem(ItemType.Debug);
                        GUI.Dashboard.sendItems(player);
                    }
                }
                Log.Debug($"Item removed. {UUID.ToString()}:{Index.ToString()}");
                return;
            }
            catch (Exception e)
            {
                Log.Write("EXCEPTION AT \"INVENTORY_REMOVE\":\n" + e.ToString(), nLog.Type.Error);
            }

        }
        public static void Remove(Player player, nItem item)
        {
            try
            {
                int UUID = Main.Players[player].UUID;

                if (ClothesItems.Contains(item.Type) || WeaponsItems.Contains(item.Type) || MeleeWeaponsItems.Contains(item.Type) || item.Type == ItemType.BagWithDrill
                    || item.Type == ItemType.BagWithMoney || item.Type == ItemType.CarKey || item.Type == ItemType.KeyRing)
                {

                    int Index = FindIndex(UUID, item.Type);
                    if (item.Type == ItemType.CarKey)
                    {
                        if (Items[UUID][Index].Data != item.Data)
                            return;
                    }

                    Items[UUID][Index] = new nItem(ItemType.Debug, 1);
                    GUI.Dashboard.sendItems(player);
                    Log.Debug($"Item removed. {UUID.ToString()}:TYPE {(int)item.Type}");
                }
                else
                {
                    int Index = FindIndex(UUID, item.Type);
                    if (Index != -1)
                    {
                        int temp = Items[UUID][Index].Count - item.Count;
                        if (temp > 0)
                        {
                            Items[UUID][Index].Count = temp;
                            Items[UUID][Index].Weight = temp * ItemsWeight.GetItemWeight(Items[UUID][Index].Type);
                            //GUI.Dashboard.Update(player, Items[UUID][Index], Index);
                            GUI.Dashboard.sendItems(player);
                        }
                        else
                        {
                            Items[UUID][Index] = new nItem(ItemType.Debug, 1);
                            GUI.Dashboard.sendItems(player);
                        }
                    }
                    Log.Debug($"Item removed. {UUID.ToString()}:{Index.ToString()}");
                }
                return;
            }
            catch (Exception e)
            {
                Log.Write("EXCEPTION AT \"INVENTORY_REMOVE\":\n" + e.ToString(), nLog.Type.Error);
            }

        }

        public static void Remove(Player player, int index)
        {
            int UUID = Main.Players[player].UUID;
            Items[UUID][index] = new nItem(ItemType.Debug, 1);
            GUI.Dashboard.sendItems(player);
        }

        public static void Split(Player player, int index, int count)
        {
            try
            {
                int UUID = Main.Players[player].UUID;
                if (index != -1)
                {
                    int temp = Items[UUID][index].Count - count;
                    if (temp > 0)
                    {
                        int toIndex = FindIndex(UUID, ItemType.Debug);

                        if (toIndex == -1)
                            return;

                        Items[UUID][toIndex] = new nItem(Items[UUID][index].Type, count);
                        //GUI.Dashboard.Update(player, Items[UUID][Items[UUID].Count - 2], Items[UUID].Count - 2);

                        Items[UUID][index].Count -= count;
                        Items[UUID][index].Weight = Items[UUID][index].Count * ItemsWeight.GetItemWeight(Items[UUID][index].Type);

                        //GUI.Dashboard.Update(player, Items[UUID][index], index);
                        GUI.Dashboard.sendItems(player);
                        //Log.Write($"Item split {UUID.ToString()}:{index.ToString()}  {temp} | {count}");
                    }
                }
                Log.Debug($"Item split {UUID.ToString()}:{index.ToString()} ");
                return;
            }
            catch (Exception e)
            {
                Log.Write("EXCEPTION AT \"INVENTORY_SPLIT\":\n" + e.ToString(), nLog.Type.Error);
            }
        }
        public static void Stack(Player player, int index, int toIndex)
        {
            try
            {
                int UUID = Main.Players[player].UUID;
                if ((index != -1 && toIndex != -1) && index != toIndex && Items[UUID][index].Type == Items[UUID][toIndex].Type)
                {
                    var fromItem = Items[UUID][index];
                    var toItem = Items[UUID][toIndex];

                    int max = (ItemsStacks.ContainsKey(toItem.Type)) ? ItemsStacks[toItem.Type] : 1;
                    int firstCount = fromItem.Count + toItem.Count;
                    int secondCount = 0;

                    if (firstCount > max)
                    {
                        secondCount = firstCount - max;
                        firstCount = max;
                    }

                    Items[UUID][toIndex].Count = firstCount;
                    Items[UUID][toIndex].Weight = firstCount * ItemsWeight.GetItemWeight(toItem.Type);

                    if (secondCount > 0)
                    {
                        Items[UUID][index].Count = secondCount;
                        Items[UUID][index].Weight = secondCount * ItemsWeight.GetItemWeight(toItem.Type);
                        GUI.Dashboard.Update(player, Items[UUID][index], index);
                    }
                    else
                    {
                        Items[UUID][index] = new nItem(ItemType.Debug, 1);
                    }

                    GUI.Dashboard.sendItems(player);
                }
            }
            catch (Exception e)
            {
                Log.Write("Stack: " + e.ToString(), nLog.Type.Error);
            }
        }
        public static void Swap(Player player, int indexFrom, int indexTo)
        {
            try
            {
                int UUID = Main.Players[player].UUID;

                if (indexFrom == indexTo)
                {

                    GUI.Dashboard.Update(player, Items[UUID][indexFrom], indexFrom);
                    return;
                }
                if (indexFrom != -1 && indexTo != -1)
                {
                    nItem temp = (nItem)Items[UUID][indexTo].Clone();
                    Items[UUID][indexTo] = Items[UUID][indexFrom];
                    Items[UUID][indexFrom] = temp;

                    //GUI.Dashboard.Update(player, Items[UUID][indexTo], indexTo);
                    //GUI.Dashboard.Update(player, Items[UUID][indexFrom], indexFrom);
                }
            }
            catch (Exception e)
            {
                Log.Write("Inventory_Swap: " + e.Message);
            }
        }

        public static void Remove(Player player, int index, int count)
        {
            try
            {
                int UUID = Main.Players[player].UUID;
                var item = Items[UUID][index];

                if (ClothesItems.Contains(item.Type) || WeaponsItems.Contains(item.Type) || MeleeWeaponsItems.Contains(item.Type) || item.Type == ItemType.BagWithDrill
                    || item.Type == ItemType.BagWithMoney || item.Type == ItemType.CarKey || item.Type == ItemType.KeyRing)
                {
                    Items[UUID][index] = new nItem(ItemType.Debug, 1);
                    GUI.Dashboard.sendItems(player);
                    Log.Debug($"Item removed. {UUID.ToString()}:TYPE {(int)item.Type}");
                }
                else
                {
                    if (index != -1)
                    {
                        int temp = Items[UUID][index].Count - count;
                        if (temp > 0)
                        {
                            Items[UUID][index].Count = temp;
                            Items[UUID][index].Weight = temp * ItemsWeight.GetItemWeight(Items[UUID][index].Type);
                            //GUI.Dashboard.Update(player, Items[UUID][index], index);
                            GUI.Dashboard.sendItems(player);
                        }
                        else
                        {
                            //Items[UUID].RemoveAt(index);
                            Items[UUID][index] = new nItem(ItemType.Debug, 1);
                            GUI.Dashboard.sendItems(player);
                        }
                    }
                    Log.Debug($"Item removed. {UUID.ToString()}:{index.ToString()}");
                }
                return;
            }
            catch (Exception e)
            {
                Log.Write("EXCEPTION AT \"INVENTORY_REMOVE\":\n" + e.ToString(), nLog.Type.Error);
            }

        }

        public static void SwapFromVehicle(Player player, Vehicle veh, int indexFrom, int indexTo)
        {
            try
            {
                if (!veh.HasData("ITEMS"))
                    return;

                List<nItem> vehicleItems = veh.GetData<List<nItem>>("ITEMS");

                int UUID = Main.Players[player].UUID;
                //Log.Write($"{indexFrom}, {indexTo}");

                if (indexFrom != -1 && indexTo != -1)
                {
                    //Log.Write($"Before: ({Items[UUID][indexFrom].Type},{Items[UUID][indexFrom].ID},{Items[UUID][indexFrom].Weight}) ({Items[UUID][indexTo].Type},{Items[UUID][indexTo].ID},{Items[UUID][indexTo].Weight})");

                    nItem temp = (nItem)vehicleItems[indexFrom].Clone();
                    vehicleItems[indexFrom] = (nItem)Items[UUID][indexTo].Clone();
                    Items[UUID][indexTo] = temp;

                    veh.SetData("ITEMS", vehicleItems);
                    //Log.Write($"After: ({Items[UUID][indexFrom].Type},{Items[UUID][indexFrom].ID},{Items[UUID][indexFrom].Weight}) ({Items[UUID][indexTo].Type},{Items[UUID][indexTo].ID},{Items[UUID][indexTo].Weight})");

                    GUI.Dashboard.sendItems(player);
                    //GUI.Dashboard.Update(player, Items[UUID][indexTo], indexTo);
                    GUI.Dashboard.OpenOut(player, veh.GetData<List<nItem>>("ITEMS"), "Багажник", 2);
                    //Trigger.PlayerEvent(player, "board", "itemSwitch", indexFrom, indexTo, Items[UUID][indexFrom], Items[UUID][indexTo]);
                }
            }
            catch (Exception e)
            {
                Log.Write("Inventory_Swap: " + e.Message);
            }
        }
        #endregion

        #region Save items to db
        public static void SaveAll(object state = null)
        {
            try
            {
                Log.Write("Saving items...", nLog.Type.Info);
                if (Items.Count == 0) return;
                Dictionary<int, List<nItem>> cItems = new Dictionary<int, List<nItem>>(Items);

                foreach (KeyValuePair<int, List<nItem>> kvp in cItems)
                {
                    int UUID = kvp.Key;
                    string json = JsonConvert.SerializeObject(kvp.Value);
                    MySQL.Query($"UPDATE `inventory` SET items='{json}' WHERE uuid={UUID}");
                }
                Log.Write("Items has been saved to DB.", nLog.Type.Success);
            }
            catch (Exception e)
            {
                Log.Write("EXCEPTION AT \"INVENTORY_SAVEALL\":\n" + e.ToString(), nLog.Type.Error);
            }
        }
        public static void Save(int UUID)
        {
            try
            {
                if (!Items.ContainsKey(UUID)) return;
                Log.Write($"Saving items for {UUID}", nLog.Type.Info);

                string items = JsonConvert.SerializeObject(Items[UUID]);
                string clothes = JsonConvert.SerializeObject(ActiveClothes[UUID]);
                string weapons = JsonConvert.SerializeObject(ActiveWeapons[UUID]);

                MySqlConnector.MySqlCommand cmd = new MySqlConnector.MySqlCommand();
                cmd.CommandText = "UPDATE `inventory` SET items=@items, weapons=@weapons, clothes=@clothes WHERE uuid=@uuid";
                cmd.Parameters.AddWithValue("@items", items);
                cmd.Parameters.AddWithValue("@weapons", weapons);
                cmd.Parameters.AddWithValue("@clothes", clothes);
                cmd.Parameters.AddWithValue("@uuid", UUID);
                MySQL.Query(cmd);

                //MySQL.Query($"UPDATE `inventory` SET items='{items}', weapons = '{weapons}', clothes = '{clothes}'  WHERE uuid={UUID}");

                Log.Write("Items has been saved to DB.", nLog.Type.Success);
            }
            catch (Exception e)
            {
                Log.Write("EXCEPTION AT \"INVENTORY_SAVE\":\n" + e.ToString(), nLog.Type.Error);
            }
        }
        #endregion

        #region SPECIAL
        public static nItem Find(int UUID, ItemType type)
        {
            List<nItem> items = Items[UUID];
            nItem result = items.Find(i => i.Type == type);
            return result;
        }
        public static int FindIndex(int UUID, ItemType type)
        {
            List<nItem> items = Items[UUID];
            int result = items.FindIndex(i => i.Type == type);
            return result;
        }

        public static Dictionary<int, int> FindFree(int UUID, ItemType type)
        {
            List<nItem> items = Items[UUID];

            Dictionary<int, int> dict = new Dictionary<int, int>();

            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].Type == type && items[i].Count < ItemsStacks[type])
                    dict.Add(i, ItemsStacks[type] - items[i].Count);

            }

            return dict;

        }

        public static bool isFull(int UUID)
        {

            if (FindIndex(UUID, ItemType.Debug) == -1) return true;
            else return false;
        }

        public static void InitInventory(int uuid)
        {
            Items[uuid].Clear();
            ActiveClothes[uuid].Clear();
            ActiveWeapons[uuid].Clear();

            for (int i = 0; i < maxSlots; i++)
                Items[uuid].Add(new nItem(ItemType.Debug, 1));

            for (int i = 0; i < maxClothesSlots; i++)
                ActiveClothes[uuid].Add(new nItem(ItemType.Debug, 1));

            for (int i = 0; i < maxWeaponsSlots; i++)
                ActiveWeapons[uuid].Add(new nItem(ItemType.Debug, 1));
        }

        public static void Check(int uuid)
        { //if items dict does not contains account uuid, then add him
            if (!Items.ContainsKey(uuid))
            {
                Items.Add(uuid, new List<nItem>());
                ActiveClothes.Add(uuid, new List<nItem>());
                ActiveWeapons.Add(uuid, new List<nItem>());

                InitInventory(uuid);
                MySQL.Query($"INSERT INTO `inventory`(`uuid`,`items`) VALUES ({uuid},'{JsonConvert.SerializeObject(Items[uuid])}')");
                nInventory.Save(uuid);
                Log.Debug("Player added");
            }
        }

        public static void UnActiveItem(Player player, ItemType type)
        {
            var items = Items[Main.Players[player].UUID];
            foreach (var i in items)
                if (i.Type == type && i.IsActive)
                {
                    i.IsActive = false;
                    GUI.Dashboard.Update(player, i, items.IndexOf(i));
                }
            Items[Main.Players[player].UUID] = items;
        }
        public static void ClearWithoutClothes(Player player)
        {
            try
            {
                int uuid = Main.Players[player].UUID;
                List<nItem> items = Items[uuid];
                for (int i = 0; i < items.Count; i++)
                {
                    if (!ClothesItems.Contains(items[i].Type) && items[i].Type != ItemType.CarKey && items[i].Type != ItemType.KeyRing)
                        items[i] = new nItem(ItemType.Debug);
                }

                Items[uuid] = items;
                GUI.Dashboard.sendItems(player);
            }
            catch (Exception e)
            {
                Log.Write(e.ToString(), nLog.Type.Error);
            }
        }
        public static void ClearAllClothes(Player client)
        {
            try
            {
                int uuid = Main.Players[client].UUID;
                List<nItem> items = Items[uuid];
                List<nItem> upd = new List<nItem>();
                for (int i = 0; i < items.Count; i++)
                {
                    if (ClothesItems.Contains(items[i].Type))
                        items[i] = new nItem(ItemType.Debug);
                }

                Items[uuid] = items;
                GUI.Dashboard.sendItems(client);
            }
            catch (Exception e)
            {
                Log.Write(e.ToString(), nLog.Type.Error);
            }
        }
        #endregion
    }
    class Items : Script
    {
        private static nLog Log = new nLog("Items");

        public static List<int> ItemsDropped = new List<int>();
        public static List<int> InProcessering = new List<int>();
        [ServerEvent(Event.EntityDeleted)]
        public void Event_OnEntityDeleted(Entity entity)
        {
            try
            {
                if (NAPI.Entity.GetEntityType(entity) == EntityType.Object && NAPI.Data.HasEntityData(entity, "DELETETIMER"))
                {
                    Timers.Stop(NAPI.Data.GetEntityData(entity, "DELETETIMER"));
                    ItemsDropped.Remove(NAPI.Data.GetEntityData(entity, "ID"));
                    InProcessering.Remove(NAPI.Data.GetEntityData(entity, "ID"));
                }
            }
            catch (Exception e)
            {
                Log.Write("Event_OnEntityDeleted: " + e.Message, nLog.Type.Error);
            }
        }

        public static void deleteObject(GTANetworkAPI.Object obj)
        {
            NAPI.Task.Run(() =>
            {
                try
                {
                    //Main.StopT(obj.GetData<object>("DELETETIMER"), "timer_33");
                    obj.ResetData("DELETETIMER");
                    ItemsDropped.Remove(obj.GetData<int>("ID"));
                    InProcessering.Remove(obj.GetData<int>("ID"));
                    obj.Delete();
                }
                catch (Exception e)
                {
                    Log.Write("UpdateObject: " + e.Message, nLog.Type.Error);
                }
            }, 0);
        }

        [RemoteEvent("changeWeaponSlot")]
        public static void ChangeWeaponSlot(Player player, int ind, int outIndex)
        {
            Log.Write($"dfd {ind} {outIndex}");

            if (!Main.Players.ContainsKey(player))
            {
                // GUI.Dashboard.WeaponsUpdate(player, nInventory.ActiveWeapons[UUID][ind], ind);
                return;
            }

            int UUID = Main.Players[player].UUID;

            if ((ind < 0 || ind > 2) || (outIndex < 0 || outIndex > 2))
            {
                GUI.Dashboard.WeaponsUpdate(player, nInventory.ActiveWeapons[UUID][ind], ind);
                return;
            }

            if (!nInventory.ActiveWeapons.ContainsKey(UUID))
            {
                GUI.Dashboard.WeaponsUpdate(player, nInventory.ActiveWeapons[UUID][ind], ind);
                return;
            }

            if (nInventory.ActiveWeapons[UUID][ind].Type != ItemType.Debug && !nInventory.ActiveWeapons[UUID][ind].IsActive)
            {
                nItem itemFrom = (nItem)nInventory.ActiveWeapons[UUID][ind].Clone();

                nInventory.ActiveWeapons[UUID][ind] = (nItem)nInventory.ActiveWeapons[UUID][outIndex].Clone();
                nInventory.ActiveWeapons[UUID][outIndex] = itemFrom;

                GUI.Dashboard.WeaponsUpdate(player, nInventory.ActiveWeapons[UUID][ind], ind);
                GUI.Dashboard.WeaponsUpdate(player, nInventory.ActiveWeapons[UUID][outIndex], outIndex);

                return;
            }
            else
            {
                GUI.Dashboard.WeaponsUpdate(player, nInventory.ActiveWeapons[UUID][ind], ind);
                GUI.Dashboard.WeaponsUpdate(player, nInventory.ActiveWeapons[UUID][outIndex], outIndex);
                return;
            }
        }

        public static void onWeaponUse(Player player, nItem item, int index)
        {
            int UUID = Main.Players[player].UUID;

            /*if (item.IsActive)
            {
                var wHash = Weapons.GetHash(item.Type.ToString());
                //Trigger.PlayerEvent(player, "takeOffWeapon", (int)wHash);
                Trigger.PlayerEvent(player, "serverTakeOffWeapon", (int)wHash);
                //Commands.RPChat("me", player, $"убрал(а) {nInventory.ItemsNames[(int)item.Type]}");
                nInventory.ActiveWeapons[UUID][index].IsActive = false;
                GUI.Dashboard.WeaponsUpdate(player, nInventory.ActiveWeapons[UUID][index], index);
            }
            else
            {
                var oldwItem = nInventory.ActiveWeapons[UUID].FirstOrDefault(i => (nInventory.WeaponsItems.Contains(i.Type) || nInventory.MeleeWeaponsItems.Contains(i.Type)) && i.IsActive);
                if (oldwItem != null)
                {
                    var oldwHash = Weapons.GetHash(oldwItem.Type.ToString());
                    Trigger.PlayerEvent(player, "serverTakeOffWeapon", (int)oldwHash);
                    oldwItem.IsActive = false;
                    //GUI.Dashboard.Update(player, oldwItem, nInventory.ActiveWeapons[UUID].IndexOf(oldwItem));
                    // Commands.RPChat("me", player, $"убрал(а) {nInventory.ItemsNames[(int)oldwItem.Type]}");
                }

                var wHash = Weapons.GetHash(item.Type.ToString());
                if (Weapons.WeaponsAmmoTypes.ContainsKey(item.Type))
                {
                    var ammoItem = nInventory.Find(UUID, Weapons.WeaponsAmmoTypes[item.Type]);
                    var ammo = (ammoItem == null) ? 0 : ammoItem.Count;
                    if (ammo > Weapons.WeaponsClipsMax[item.Type]) ammo = Weapons.WeaponsClipsMax[item.Type];
                    if (ammoItem != null) nInventory.Remove(player, ammoItem.Type, ammo);
                    Trigger.PlayerEvent(player, "wgive", (int)wHash, ammo, false, true);
                }
                else
                {
                    Trigger.PlayerEvent(player, "wgive", (int)wHash, 1, false, true);
                }

                //Commands.RPChat("me", player, $"достал(а) {nInventory.ItemsNames[(int)item.Type]}");
                nInventory.ActiveWeapons[UUID][index].IsActive = true;
                player.SetData("LastActiveWeap", item.Type);
                //GUI.Dashboard.Update(player, item, index);
                GUI.Dashboard.WeaponsUpdate(player, nInventory.ActiveWeapons[UUID][index], index);
                //GUI.Dashboard.Close(player);
                return;
            }*/

/*            #region MyRegion

            if (item.Data.ToString().Split("_").Length == 1)
                item.Data = $"{item.Data}_0_[]";
            #endregion
*/
            if (item.IsActive)
            {
                var wHash = Weapons.GetHash(item.Type.ToString());
                //Trigger.PlayerEvent(player, "takeOffWeapon", (int)wHash);
                Trigger.ClientEvent(player, "serverTakeOffWeapon", (int)wHash);
                player.SetData("CURWEAP", WeaponHash.Unarmed);
                player.SetData("CURTYPE", ItemType.Unknown);
                item.IsActive = false;
                Commands.RPChat("me", player, $"убрал(а) {nInventory.ItemsNames[(int)item.Type]}");
                //WeaponAttachment.ShowWeapon(player, (uint)wHash, JsonConvert.DeserializeObject<List<uint>>(Convert.ToString(item.Data.ToString().Split('_')[2])), Convert.ToInt32(item.Data.ToString().Split('_')[1]));
            }
            else
            {
                var oldwItem = nInventory.Items[UUID].FirstOrDefault(i => (nInventory.WeaponsItems.Contains(i.Type) || nInventory.MeleeWeaponsItems.Contains(i.Type)) && i.IsActive);
                if (oldwItem != null)
                {
                    var oldwHash = Weapons.GetHash(oldwItem.Type.ToString());
                    Trigger.ClientEvent(player, "serverTakeOffWeapon", (int)oldwHash);
                    oldwItem.IsActive = false;
                    GUI.Dashboard.Update(player, oldwItem, nInventory.Items[UUID].IndexOf(oldwItem));
                    //Commands.RPChat("me", player, $"убрал(а) {nInventory.ItemsNames[(int)oldwItem.Type]}");
                    //WeaponAttachment.ShowWeapon(player, (uint)oldwHash, JsonConvert.DeserializeObject<List<uint>>(Convert.ToString(oldwItem.Data.ToString().Split('_')[2])), Convert.ToInt32(oldwItem.Data.ToString().Split('_')[1]));
                }

                var wHash = Weapons.GetHash(item.Type.ToString());

                if (Weapons.WeaponsAmmoTypes.ContainsKey(item.Type))
                {
                    var ammoItem = nInventory.Find(UUID, Weapons.WeaponsAmmoTypes[item.Type]);
                    var ammo = (ammoItem == null) ? 0 : ammoItem.Count;
                    if (ammo > Weapons.WeaponsClipsMax[item.Type]) ammo = Weapons.WeaponsClipsMax[item.Type];
                    if (ammoItem != null) nInventory.Remove(player, ammoItem.Type, ammo);
                    Trigger.ClientEvent(player, "wgive", (int)wHash, ammo, false, true); //, Convert.ToInt32(item.Data.ToString().Split('_')[0]));
                }
                else
                {
                    Trigger.ClientEvent(player, "wgive", (int)wHash, 1, false, true); //, item.Data == null ? 0 : Convert.ToInt32(item.Data.ToString().Split('_')[0]));
                }



                //Commands.RPChat("me", player, $"достал(а) {nInventory.ItemsNames[(int)item.Type]}");
                item.IsActive = true;


                player.SetData("LastActiveWeap", item.Type);
                player.SetData("CURWEAP", (WeaponHash)wHash);
                player.SetData("CURTYPE", item.Type);
                GUI.Dashboard.Update(player, item, index);
                GUI.Dashboard.Close(player);
            }
            return;
        }

        public static void onUse(Player player, nItem item, int index, int toIndex = -1)
        {
            try
            {
                var UUID = Main.Players[player].UUID;
                if (nInventory.ClothesItems.Contains(item.Type) && item.Type != ItemType.BodyArmor && item.Type != ItemType.Mask)
                {
                    var data = (string)item.Data;
                    var clothesGender = Convert.ToBoolean(data.Split('_')[2]);
                    if (clothesGender != Main.Players[player].Gender)
                    {
                        var error_gender = (clothesGender) ? "мужская" : "женская";
                        Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Это {error_gender} одежда", 3000);
                        GUI.Dashboard.Close(player);
                        return;
                    }
                    if ((player.GetData<bool>("ON_DUTY") && Fractions.Manager.FractionTypes[Main.Players[player].FractionID] == 2 && Main.Players[player].FractionID != 9) || player.GetData<bool>("ON_WORK"))
                    {
                        Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Вы не можете использовать это сейчас", 3000);
                        GUI.Dashboard.Close(player);
                        GUI.Dashboard.sendItems(player);
                        return;
                    }
                }

                /*if (nInventory.WeaponsItems.Contains(item.Type) || nInventory.MeleeWeaponsItems.Contains(item.Type))
                {
                    if (item.IsActive)
                    {
                        var wHash = Weapons.GetHash(item.Type.ToString());
                        Trigger.ClientEvent(player, "takeOffWeapon", (int)wHash);
                        Commands.RPChat("me", player, $"убрал(а) {nInventory.ItemsNames[(int)item.Type]}");
                    }
                    else
                    {
                        var oldwItem = nInventory.Items[UUID].FirstOrDefault(i => (nInventory.WeaponsItems.Contains(i.Type) || nInventory.MeleeWeaponsItems.Contains(i.Type)) && i.IsActive);
                        if (oldwItem != null)
                        {
                            var oldwHash = Weapons.GetHash(oldwItem.Type.ToString());
                            Trigger.ClientEvent(player, "serverTakeOffWeapon", (int)oldwHash);
                            oldwItem.IsActive = false;
                            GUI.Dashboard.Update(player, oldwItem, nInventory.Items[UUID].IndexOf(oldwItem));
                            Commands.RPChat("me", player, $"убрал(а) {nInventory.ItemsNames[(int)oldwItem.Type]}");
                        }

                        var wHash = Weapons.GetHash(item.Type.ToString());
                        if (Weapons.WeaponsAmmoTypes.ContainsKey(item.Type))
                        {
                            var ammoItem = nInventory.Find(UUID, Weapons.WeaponsAmmoTypes[item.Type]);
                            var ammo = (ammoItem == null) ? 0 : ammoItem.Count;
                            if (ammo > Weapons.WeaponsClipsMax[item.Type]) ammo = Weapons.WeaponsClipsMax[item.Type];
                            if (ammoItem != null) nInventory.Remove(player, ammoItem.Type, ammo);
                            Trigger.ClientEvent(player, "wgive", (int)wHash, ammo, false, true);
                        }
                        else
                        {
                            Trigger.ClientEvent(player, "wgive", (int)wHash, 1, false, true);
                        }

                        Commands.RPChat("me", player, $"достал(а) {nInventory.ItemsNames[(int)item.Type]}");
                        item.IsActive = true;
                        player.SetData("LastActiveWeap", item.Type);
                        GUI.Dashboard.Update(player, item, index);
                        GUI.Dashboard.Close(player);
                    }
                    return;
                }*/

                if (nInventory.WeaponsItems.Contains(item.Type) || nInventory.MeleeWeaponsItems.Contains(item.Type))
                {
                    if (toIndex == -1)
                    {
                        Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Вы не можете использовать это сейчас", 3000);
                        GUI.Dashboard.Close(player);
                        return;
                    }

                    nInventory.ActiveWeapons[UUID][toIndex] = new nItem(item.Type, 1, item.Data, false);
                    nInventory.Remove(player, index, 1);

                    //GUI.Dashboard.WeaponsUpdate(player, nInventory.ActiveWeapons[UUID][toIndex], toIndex);
                    GUI.Dashboard.Update(player, nInventory.Items[UUID][index], index);
                    GUI.Dashboard.WeaponsUpdate(player, nInventory.ActiveWeapons[UUID][toIndex], toIndex);

                    Log.Write($"{nInventory.ActiveWeapons[UUID][toIndex].Type}");

                    return;
                }

                if (nInventory.AmmoItems.Contains(item.Type)) return;

                if (nInventory.AlcoItems.Contains(item.Type))
                {
                    int stage = Convert.ToInt32(item.Type.ToString().Split("Drink")[1]);
                    int curStage = player.GetData<int>("RESIST_STAGE");

                    if (player.HasData("RESIST_BAN"))
                    {
                        Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Вы пьяны до такой степени, что не можете открыть бутылку", 3000);
                        return;
                    }

                    var stageTimes = new List<int>() { 0, 300, 420, 600 };

                    if (curStage == 0 || curStage == stage)
                    {
                        player.SetData("RESIST_STAGE", stage);
                        player.SetData("RESIST_TIME", player.GetData<int>("RESIST_TIME") + stageTimes[stage]);
                    }
                    else if (curStage < stage)
                    {
                        player.SetData("RESIST_STAGE", stage);
                    }
                    else if (curStage > stage)
                    {
                        player.SetData("RESIST_TIME", player.GetData<int>("RESIST_TIME") + stageTimes[stage]);
                    }

                    if (player.GetData<int>("RESIST_TIME") >= 1500)
                        player.SetData("RESIST_BAN", true);

                    Trigger.ClientEvent(player, "setResistStage", player.GetData<int>("RESIST_STAGE"));
                    BasicSync.AttachObjectToPlayer(player, nInventory.ItemModels[item.Type], 57005, Fractions.AlcoFabrication.AlcoPosOffset[item.Type], Fractions.AlcoFabrication.AlcoRotOffset[item.Type]);

                    Main.OnAntiAnim(player);
                    player.PlayAnimation("amb@world_human_drinking@beer@male@idle_a", "idle_c", 49);
                    NAPI.Task.Run(() => {
                        try
                        {
                            if (player != null)
                            {
                                if (!player.IsInVehicle) player.StopAnimation();
                                else player.SetData("ToResetAnimPhone", true);
                                Main.OffAntiAnim(player);
                                Trigger.ClientEvent(player, "startScreenEffect", "PPFilter", player.GetData<int>("RESIST_TIME") * 1000, false);
                                BasicSync.DetachObject(player);
                            }
                        } catch { }
                    }, 5000);

                    /*if (!player.HasData("RESIST_TIMER"))
                        player.SetData("RESIST_TIMER", Timers.Start(1000, () => Fractions.AlcoFabrication.ResistTimer(player.Name)));*/

                    Commands.RPChat("me", player, "выпил бутылку " + nInventory.ItemsNames[(int)item.Type]);
                    GameLog.Items($"player({Main.Players[player].UUID})", "use", Convert.ToInt32(item.Type), 1, $"{item.Data}");
                }

                var gender = Main.Players[player].Gender;
                Log.Debug("item used");
                switch (item.Type)
                {
                    #region Clothes
                    case ItemType.Glasses:
                        {
                            if (item.IsActive)
                            {

                                Customization.CustomPlayerData[Main.Players[player].UUID].Accessory.Glasses.Variation = -1;
                                player.ClearAccessory(1);
                                nInventory.Items[UUID][index].IsActive = false;

                                //var variation = Customization.CustomPlayerData[Main.Players[player].UUID].Accessory.Glasses.Variation;
                                //var texture = Customization.CustomPlayerData[Main.Players[player].UUID].Accessory.Glasses.Texture;

                                //var itemInc = new nItem(item.Type, 1, $"{variation}_{texture}_{Main.Players[player].Gender}", false, weight: nInventory.ItemsWeight[item.Type]);
                                //nInventory.Add(player, item);

                                //nInventory.ActiveClothes[UUID][((int)item.Type * -1) - 1] = new nItem(ItemType.Debug, 1);

                                //GUI.Dashboard.Update(player, item, index);
                                GUI.Dashboard.sendItems(player);
                            }
                            else
                            {
                                var mask = Customization.CustomPlayerData[Main.Players[player].UUID].Clothes.Mask.Variation;
                                if (Customization.MaskTypes.ContainsKey(mask) && Customization.MaskTypes[mask].Item3)
                                {
                                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Вы не можете надеть эти очки с маской", 3000);
                                    GUI.Dashboard.Update(player, item, index);
                                    GUI.Dashboard.sendItems(player);
                                    return;
                                }
                                var itemData = (string)item.Data;
                                var variation = Convert.ToInt32(itemData.Split('_')[0]);
                                var texture = Convert.ToInt32(itemData.Split('_')[1]);
                                Customization.CustomPlayerData[Main.Players[player].UUID].Accessory.Glasses = new ComponentItem(variation, texture);
                                player.SetAccessories(1, variation, texture);

                                //  nInventory.Remove(player, index, 1);

                                nInventory.ActiveClothes[UUID][((int)item.Type * -1) - 1] = new nItem(item.Type, 1, item.Data, true);

                                nInventory.UnActiveItem(player, item.Type);
                                nInventory.Items[UUID][index].IsActive = true;

                                nInventory.Remove(player, index, 1);

                                GUI.Dashboard.Update(player, item, index);
                                //GUI.Dashboard.sendItems(player);
                                GUI.Dashboard.ClothesUpdate(player, nInventory.ActiveClothes[UUID][((int)item.Type * -1) - 1], ((int)item.Type * -1) - 1);
                            }
                            return;
                        }
                    case ItemType.Hat:
                        {
                            if (item.IsActive)
                            {
                                Customization.CustomPlayerData[Main.Players[player].UUID].Accessory.Hat.Variation = -1;

                                nInventory.Items[UUID][index].IsActive = false;
                                GUI.Dashboard.Update(player, item, index);
                                GUI.Dashboard.sendItems(player);
                            }
                            else
                            {
                                var mask = Customization.CustomPlayerData[Main.Players[player].UUID].Clothes.Mask.Variation;
                                if (Customization.MaskTypes.ContainsKey(mask) && Customization.MaskTypes[mask].Item2)
                                {
                                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Вы не можете надеть этот головной убор с маской", 3000);
                                    GUI.Dashboard.Update(player, item, index);
                                    GUI.Dashboard.sendItems(player);
                                    return;
                                }
                                var itemData = (string)item.Data;
                                var variation = Convert.ToInt32(itemData.Split('_')[0]);
                                var texture = Convert.ToInt32(itemData.Split('_')[1]);
                                Customization.CustomPlayerData[Main.Players[player].UUID].Accessory.Hat = new ComponentItem(variation, texture);

                                Log.Debug($"{player.Name} dressed hat variation: {variation} texture: {texture}");

                                nInventory.ActiveClothes[UUID][((int)item.Type * -1) - 1] = new nItem(item.Type, 1, item.Data, true);

                                nInventory.UnActiveItem(player, item.Type);
                                nInventory.Items[UUID][index].IsActive = true;
                                nInventory.Remove(player, index, 1);

                                GUI.Dashboard.Update(player, item, index);
                                //GUI.Dashboard.sendItems(player);
                                GUI.Dashboard.ClothesUpdate(player, nInventory.ActiveClothes[UUID][((int)item.Type * -1) - 1], ((int)item.Type * -1) - 1);
                            }
                            Customization.SetHat(player, Customization.CustomPlayerData[Main.Players[player].UUID].Accessory.Hat.Variation, Customization.CustomPlayerData[Main.Players[player].UUID].Accessory.Hat.Texture);
                            return;
                        }
                    case ItemType.Mask:
                        {
                            if (item.IsActive)
                            {
                                Customization.CustomPlayerData[Main.Players[player].UUID].Clothes.Mask = new ComponentItem(Customization.EmtptySlots[gender][1], 0);

                                nInventory.Items[UUID][index].IsActive = false;
                                GUI.Dashboard.Update(player, item, index);
                                GUI.Dashboard.sendItems(player);
                                player.SetSharedData("IS_MASK", false);
                            }
                            else
                            {
                                var itemData = (string)item.Data;
                                var variation = Convert.ToInt32(itemData.Split('_')[0]);
                                var texture = Convert.ToInt32(itemData.Split('_')[1]);
                                Customization.CustomPlayerData[Main.Players[player].UUID].Clothes.Mask = new ComponentItem(variation, texture);

                                if (Customization.MaskTypes.ContainsKey(variation))
                                {
                                    if (Customization.MaskTypes[variation].Item1)
                                    {
                                        player.SetClothes(2, 0, 0);
                                    }
                                    if (Customization.MaskTypes[variation].Item2)
                                    {
                                        Customization.CustomPlayerData[Main.Players[player].UUID].Accessory.Hat.Variation = -1;
                                        nInventory.UnActiveItem(player, ItemType.Hat);
                                        Customization.SetHat(player, -1, 0);
                                    }
                                    if (Customization.MaskTypes[variation].Item3)
                                    {
                                        Customization.CustomPlayerData[Main.Players[player].UUID].Accessory.Glasses.Variation = -1;
                                        nInventory.UnActiveItem(player, ItemType.Glasses);
                                        player.ClearAccessory(1);
                                    }
                                }
                                else
                                    Log.Write($"UseMask: {variation} not on the dictionaly \"Customization.MaskTypes\"", nLog.Type.Warn);

                                nInventory.ActiveClothes[UUID][((int)item.Type * -1) - 1] = new nItem(item.Type, 1, item.Data, true);

                                nInventory.UnActiveItem(player, item.Type);
                                nInventory.Items[UUID][index].IsActive = true;
                                nInventory.Remove(player, index, 1);

                                GUI.Dashboard.Update(player, item, index);
                                //GUI.Dashboard.sendItems(player);
                                GUI.Dashboard.ClothesUpdate(player, nInventory.ActiveClothes[UUID][((int)item.Type * -1) - 1], ((int)item.Type * -1) - 1);
                                player.SetSharedData("IS_MASK", true);
                            }
                            Customization.SetMask(player, Customization.CustomPlayerData[Main.Players[player].UUID].Clothes.Mask.Variation, Customization.CustomPlayerData[Main.Players[player].UUID].Clothes.Mask.Texture);
                            return;
                        }
                    case ItemType.Gloves:
                        {
                            if (item.IsActive)
                            {
                                Customization.CustomPlayerData[Main.Players[player].UUID].Clothes.Gloves = new ComponentItem(0, 0);
                                Customization.CustomPlayerData[Main.Players[player].UUID].Clothes.Torso = new ComponentItem(Customization.CorrectTorso[gender][Customization.CustomPlayerData[Main.Players[player].UUID].Clothes.Top.Variation], 0);

                                nInventory.Items[UUID][index].IsActive = false;
                                GUI.Dashboard.Update(player, item, index);
                                GUI.Dashboard.sendItems(player);
                            }
                            else
                            {
                                var itemData = (string)item.Data;
                                var variation = Convert.ToInt32(itemData.Split('_')[0]);
                                var texture = Convert.ToInt32(itemData.Split('_')[1]);
                                if (!Customization.CorrectGloves[gender][variation].ContainsKey(Customization.CustomPlayerData[Main.Players[player].UUID].Clothes.Torso.Variation)) return;
                                Customization.CustomPlayerData[Main.Players[player].UUID].Clothes.Gloves = new ComponentItem(variation, texture);
                                Customization.CustomPlayerData[Main.Players[player].UUID].Clothes.Torso = new ComponentItem(Customization.CorrectGloves[gender][variation][Customization.CustomPlayerData[Main.Players[player].UUID].Clothes.Torso.Variation], texture);

                                nInventory.ActiveClothes[UUID][((int)item.Type * -1) - 1] = new nItem(item.Type, 1, item.Data, true);

                                nInventory.UnActiveItem(player, item.Type);
                                nInventory.Items[UUID][index].IsActive = true;
                                nInventory.Remove(player, index, 1);

                                GUI.Dashboard.Update(player, item, index);
                                //GUI.Dashboard.sendItems(player);
                                GUI.Dashboard.ClothesUpdate(player, nInventory.ActiveClothes[UUID][((int)item.Type * -1) - 1], ((int)item.Type * -1) - 1);
                            }
                            player.SetClothes(3, Customization.CustomPlayerData[Main.Players[player].UUID].Clothes.Torso.Variation, Customization.CustomPlayerData[Main.Players[player].UUID].Clothes.Torso.Texture);
                            return;
                        }
                    case ItemType.Leg:
                        {
                            if (item.IsActive)
                            {
                                Customization.CustomPlayerData[Main.Players[player].UUID].Clothes.Leg = new ComponentItem(Customization.EmtptySlots[gender][4], 0);

                                nInventory.Items[UUID][index].IsActive = false;
                                GUI.Dashboard.Update(player, item, index);
                                GUI.Dashboard.sendItems(player);
                            }
                            else
                            {
                                var itemData = (string)item.Data;
                                var variation = Convert.ToInt32(itemData.Split('_')[0]);
                                var texture = Convert.ToInt32(itemData.Split('_')[1]);
                                Customization.CustomPlayerData[Main.Players[player].UUID].Clothes.Leg = new ComponentItem(variation, texture);

                                nInventory.ActiveClothes[UUID][((int)item.Type * -1) - 1] = new nItem(item.Type, 1, item.Data, true);

                                nInventory.UnActiveItem(player, item.Type);
                                nInventory.Items[UUID][index].IsActive = true;
                                nInventory.Remove(player, index, 1);

                                GUI.Dashboard.Update(player, item, index);
                                //GUI.Dashboard.sendItems(player);
                                GUI.Dashboard.ClothesUpdate(player, nInventory.ActiveClothes[UUID][((int)item.Type * -1) - 1], ((int)item.Type * -1) - 1);
                            }
                            player.SetClothes(4, Customization.CustomPlayerData[Main.Players[player].UUID].Clothes.Leg.Variation, Customization.CustomPlayerData[Main.Players[player].UUID].Clothes.Leg.Texture);
                            return;
                        }
                    case ItemType.Bag:
                        {
                            if (item.IsActive)
                            {
                                Customization.CustomPlayerData[Main.Players[player].UUID].Clothes.Bag = new ComponentItem(Customization.EmtptySlots[gender][5], 0);

                                nInventory.Items[UUID][index].IsActive = false;
                                GUI.Dashboard.Update(player, item, index);
                                GUI.Dashboard.sendItems(player);
                            }
                            else
                            {
                                var itemData = (string)item.Data;
                                var variation = Convert.ToInt32(itemData.Split('_')[0]);
                                var texture = Convert.ToInt32(itemData.Split('_')[1]);
                                Customization.CustomPlayerData[Main.Players[player].UUID].Clothes.Bag = new ComponentItem(variation, texture);

                                nInventory.ActiveClothes[UUID][((int)item.Type * -1) - 1] = new nItem(item.Type, 1, item.Data, true);

                                nInventory.UnActiveItem(player, item.Type);
                                nInventory.Items[UUID][index].IsActive = true;
                                nInventory.Remove(player, index, 1);

                                GUI.Dashboard.Update(player, item, index);
                                //GUI.Dashboard.sendItems(player);
                                GUI.Dashboard.ClothesUpdate(player, nInventory.ActiveClothes[UUID][((int)item.Type * -1) - 1], ((int)item.Type * -1) - 1);
                            }
                            player.SetClothes(5, Customization.CustomPlayerData[Main.Players[player].UUID].Clothes.Bag.Variation, Customization.CustomPlayerData[Main.Players[player].UUID].Clothes.Bag.Texture);
                            return;
                        }
                    case ItemType.Feet:
                        {
                            if (item.IsActive)
                            {
                                Customization.CustomPlayerData[Main.Players[player].UUID].Clothes.Feet = new ComponentItem(Customization.EmtptySlots[gender][6], 0);
                                nInventory.Items[UUID][index].IsActive = false;

                                GUI.Dashboard.Update(player, item, index);
                                GUI.Dashboard.sendItems(player);
                            }
                            else
                            {
                                var itemData = (string)item.Data;
                                var variation = Convert.ToInt32(itemData.Split('_')[0]);
                                var texture = Convert.ToInt32(itemData.Split('_')[1]);
                                Customization.CustomPlayerData[Main.Players[player].UUID].Clothes.Feet = new ComponentItem(variation, texture);

                                nInventory.ActiveClothes[UUID][((int)item.Type * -1) - 1] = new nItem(item.Type, 1, item.Data, true);

                                nInventory.UnActiveItem(player, item.Type);
                                nInventory.Items[UUID][index].IsActive = true;
                                nInventory.Remove(player, index, 1);

                                GUI.Dashboard.Update(player, item, index);
                                //GUI.Dashboard.sendItems(player);
                                GUI.Dashboard.ClothesUpdate(player, nInventory.ActiveClothes[UUID][((int)item.Type * -1) - 1], ((int)item.Type * -1) - 1);
                            }
                            player.SetClothes(6, Customization.CustomPlayerData[Main.Players[player].UUID].Clothes.Feet.Variation, Customization.CustomPlayerData[Main.Players[player].UUID].Clothes.Feet.Texture);
                            return;
                        }
                    case ItemType.Jewelry:
                        {
                            if (item.IsActive)
                            {
                                Customization.CustomPlayerData[Main.Players[player].UUID].Clothes.Accessory = new ComponentItem(Customization.EmtptySlots[gender][7], 0);

                                nInventory.Items[UUID][index].IsActive = false;
                                GUI.Dashboard.Update(player, item, index);
                                GUI.Dashboard.sendItems(player);
                            }
                            else
                            {
                                var itemData = (string)item.Data;
                                var variation = Convert.ToInt32(itemData.Split('_')[0]);
                                var texture = Convert.ToInt32(itemData.Split('_')[1]);
                                Customization.CustomPlayerData[Main.Players[player].UUID].Clothes.Accessory = new ComponentItem(variation, texture);

                                nInventory.ActiveClothes[UUID][((int)item.Type * -1) - 1] = new nItem(item.Type, 1, item.Data, true);

                                nInventory.UnActiveItem(player, item.Type);
                                nInventory.Items[UUID][index].IsActive = true;
                                nInventory.Remove(player, index, 1);

                                GUI.Dashboard.Update(player, item, index);
                                //GUI.Dashboard.sendItems(player);
                                GUI.Dashboard.ClothesUpdate(player, nInventory.ActiveClothes[UUID][((int)item.Type * -1) - 1], ((int)item.Type * -1) - 1);
                            }
                            player.SetClothes(7, Customization.CustomPlayerData[Main.Players[player].UUID].Clothes.Accessory.Variation, Customization.CustomPlayerData[Main.Players[player].UUID].Clothes.Accessory.Texture);
                            return;
                        }
                    case ItemType.Accessories:
                        {
                            var itemData = (string)item.Data;
                            var variation = Convert.ToInt32(itemData.Split('_')[0]);
                            var texture = Convert.ToInt32(itemData.Split('_')[1]);

                            if (item.IsActive)
                            {
                                var watchesSlot = Customization.CustomPlayerData[Main.Players[player].UUID].Accessory.Watches;
                                if (watchesSlot.Variation == variation && watchesSlot.Texture == texture)
                                {
                                    Customization.CustomPlayerData[Main.Players[player].UUID].Accessory.Watches = new ComponentItem(-1, 0);
                                    player.ClearAccessory(6);
                                }
                                else
                                {
                                    Customization.CustomPlayerData[Main.Players[player].UUID].Accessory.Bracelets = new ComponentItem(-1, 0);
                                    player.ClearAccessory(7);
                                }

                                //nInventory.ActiveClothes[UUID][((int)item.Type * -1) - 1] = new nItem(item.Type, 1, item.Data, true, weight: nInventory.ItemsWeight[item.Type]);
                                nInventory.Items[UUID][index].IsActive = false;


                                GUI.Dashboard.Update(player, item, index);
                                GUI.Dashboard.sendItems(player);
                            }
                            else
                            {
                                if (Customization.CustomPlayerData[Main.Players[player].UUID].Accessory.Watches.Variation == -1)
                                {
                                    Customization.CustomPlayerData[Main.Players[player].UUID].Accessory.Watches = new ComponentItem(variation, texture);
                                    player.SetAccessories(6, variation, texture);

                                    nInventory.ActiveClothes[UUID][((int)item.Type * -1) - 1] = new nItem(item.Type, 1, item.Data, true);

                                    nInventory.Items[UUID][index].IsActive = true;
                                    nInventory.Remove(player, index, 1);

                                    GUI.Dashboard.Update(player, item, index);
                                    GUI.Dashboard.sendItems(player);
                                }
                                else if (Customization.AccessoryRHand[gender].ContainsKey(variation))
                                {
                                    if (Customization.CustomPlayerData[Main.Players[player].UUID].Accessory.Bracelets.Variation == -1)
                                    {
                                        Customization.CustomPlayerData[Main.Players[player].UUID].Accessory.Bracelets = new ComponentItem(Customization.AccessoryRHand[gender][variation], texture);
                                        player.SetAccessories(7, Customization.AccessoryRHand[gender][variation], texture);

                                        nInventory.ActiveClothes[UUID][((int)item.Type * -1) - 1] = new nItem(item.Type, 1, item.Data, true);

                                        nInventory.Items[UUID][index].IsActive = true;
                                        nInventory.Remove(player, index, 1);

                                        GUI.Dashboard.Update(player, item, index);
                                        //GUI.Dashboard.sendItems(player);
                                        GUI.Dashboard.ClothesUpdate(player, nInventory.ActiveClothes[UUID][((int)item.Type * -1) - 1], ((int)item.Type * -1) - 1);
                                    }
                                    else
                                    {
                                        Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Заняты обе руки", 3000);
                                        GUI.Dashboard.Update(player, item, index);
                                        GUI.Dashboard.sendItems(player);
                                        return;
                                    }
                                }
                                else
                                {
                                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Левая рука занята, а на правой никто часы не носит", 3000);
                                    GUI.Dashboard.Update(player, item, index);
                                    GUI.Dashboard.sendItems(player);
                                    return;
                                }
                            }
                            return;
                        }
                    case ItemType.Undershit:
                        {
                            var itemData = (string)item.Data;
                            var underwearID = Convert.ToInt32(itemData.Split('_')[0]);
                            var underwear = Customization.Underwears[gender][underwearID];
                            var texture = Convert.ToInt32(itemData.Split('_')[1]);
                            if (item.IsActive)
                            {
                                if (underwear.Top == Customization.CustomPlayerData[Main.Players[player].UUID].Clothes.Top.Variation)
                                    Customization.CustomPlayerData[Main.Players[player].UUID].Clothes.Top = new ComponentItem(Customization.EmtptySlots[gender][11], 0);
                                Customization.CustomPlayerData[Main.Players[player].UUID].Clothes.Undershit = new ComponentItem(Customization.EmtptySlots[gender][8], 0);

                                //nInventory.ActiveClothes[UUID][((int)item.Type * -1) - 1] = new nItem(item.Type, 1, item.Data, true, weight: nInventory.ItemsWeight[item.Type]);

                                nInventory.Items[UUID][index].IsActive = false;

                                GUI.Dashboard.Update(player, item, index);
                                GUI.Dashboard.sendItems(player);
                            }
                            else
                            {
                                if (Customization.CustomPlayerData[Main.Players[player].UUID].Clothes.Top.Variation == Customization.EmtptySlots[gender][11]) // Если нету верхней одежды
                                {
                                    if (underwear.Top == -1)
                                    {
                                        Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Эту одежду можно одеть только под низ верхней", 3000);
                                        GUI.Dashboard.Update(player, item, index);
                                        GUI.Dashboard.sendItems(player);
                                        return;
                                    }
                                    Customization.CustomPlayerData[Main.Players[player].UUID].Clothes.Top = new ComponentItem(underwear.Top, texture);

                                    nInventory.ActiveClothes[UUID][((int)item.Type * -1) - 1] = new nItem(item.Type, 1, item.Data, true);

                                    // nInventory.UnActiveItem(player, item.Type);
                                    nInventory.Items[UUID][index].IsActive = true;
                                    nInventory.Remove(player, index, 1);

                                    GUI.Dashboard.Update(player, item, index);
                                    //GUI.Dashboard.sendItems(player);
                                    GUI.Dashboard.ClothesUpdate(player, nInventory.ActiveClothes[UUID][((int)item.Type * -1) - 1], ((int)item.Type * -1) - 1);

                                }
                                else // если есть верхняя одежда
                                {
                                    var nowTop = Customization.Tops[gender].FirstOrDefault(t => t.Variation == Customization.CustomPlayerData[Main.Players[player].UUID].Clothes.Top.Variation);
                                    if (nowTop != null)
                                    {
                                        var topType = nowTop.Type;
                                        if (!underwear.UndershirtIDs.ContainsKey(topType))
                                        {
                                            Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Эта одежда несовместима с Вашей верхней одеждой", 3000);
                                            GUI.Dashboard.Update(player, item, index);
                                            GUI.Dashboard.sendItems(player);
                                            return;
                                        }
                                        Customization.CustomPlayerData[Main.Players[player].UUID].Clothes.Undershit = new ComponentItem(underwear.UndershirtIDs[topType], texture);

                                        nInventory.ActiveClothes[UUID][((int)item.Type * -1) - 1] = new nItem(item.Type, 1, item.Data, true);

                                        nInventory.UnActiveItem(player, item.Type);
                                        nInventory.Items[UUID][index].IsActive = true;
                                        nInventory.Remove(player, index, 1);

                                        GUI.Dashboard.Update(player, item, index);
                                        //GUI.Dashboard.sendItems(player);
                                        GUI.Dashboard.ClothesUpdate(player, nInventory.ActiveClothes[UUID][((int)item.Type * -1) - 1], ((int)item.Type * -1) - 1);
                                    }
                                    else
                                    {
                                        if (underwear.Top == -1)
                                        {
                                            Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Эту одежду можно одеть только под низ верхней", 3000);
                                            GUI.Dashboard.Update(player, item, index);
                                            GUI.Dashboard.sendItems(player);
                                            return;
                                        }
                                        Customization.CustomPlayerData[Main.Players[player].UUID].Clothes.Top = new ComponentItem(underwear.Top, texture);

                                        nInventory.ActiveClothes[UUID][((int)item.Type * -1) - 1] = new nItem(item.Type, 1, item.Data, true);

                                        nInventory.UnActiveItem(player, item.Type);
                                        nInventory.Items[UUID][index].IsActive = true;
                                        nInventory.Remove(player, index, 1);
                                        GUI.Dashboard.Update(player, item, index);
                                        GUI.Dashboard.ClothesUpdate(player, nInventory.ActiveClothes[UUID][((int)item.Type * -1) - 1], ((int)item.Type * -1) - 1);
                                        // GUI.Dashboard.sendItems(player);
                                    }
                                }
                            }

                            var gloves = Customization.CustomPlayerData[Main.Players[player].UUID].Clothes.Gloves.Variation;
                            if (gloves != 0 &&
                                !Customization.CorrectGloves[gender][gloves].ContainsKey(Customization.CorrectTorso[gender][Customization.CustomPlayerData[Main.Players[player].UUID].Clothes.Top.Variation]))
                            {
                                nInventory.UnActiveItem(player, ItemType.Gloves);
                                Customization.CustomPlayerData[Main.Players[player].UUID].Clothes.Gloves = new ComponentItem(0, 0);
                            }

                            player.SetClothes(8, Customization.CustomPlayerData[Main.Players[player].UUID].Clothes.Undershit.Variation, Customization.CustomPlayerData[Main.Players[player].UUID].Clothes.Undershit.Texture);
                            player.SetClothes(11, Customization.CustomPlayerData[Main.Players[player].UUID].Clothes.Top.Variation, Customization.CustomPlayerData[Main.Players[player].UUID].Clothes.Top.Texture);
                            var noneGloves = Customization.CorrectTorso[gender][Customization.CustomPlayerData[Main.Players[player].UUID].Clothes.Top.Variation];
                            if (Customization.CustomPlayerData[Main.Players[player].UUID].Clothes.Gloves.Variation == 0)
                            {
                                Customization.CustomPlayerData[Main.Players[player].UUID].Clothes.Torso = new ComponentItem(noneGloves, 0);
                                player.SetClothes(3, noneGloves, 0);
                            }
                            else
                                player.SetClothes(3, Customization.CorrectGloves[gender][Customization.CustomPlayerData[Main.Players[player].UUID].Clothes.Gloves.Variation][noneGloves], Customization.CustomPlayerData[Main.Players[player].UUID].Clothes.Gloves.Texture);
                            return;
                        }
                    case ItemType.BodyArmor:
                        {
                            if (item.IsActive)
                            {
                                item.Data = player.Armor.ToString();
                                

                                player.Armor = 0;
                                player.ResetSharedData("HASARMOR");

                                //nInventory.ActiveClothes[UUID][((int)item.Type * -1) - 1] = new nItem(item.Type, 1, item.Data, true, weight: nInventory.ItemsWeight[item.Type]);

                                nInventory.Items[UUID][index].IsActive = false;
                                GUI.Dashboard.Update(player, item, index);

                                GUI.Dashboard.sendItems(player);

                                Customization.CustomPlayerData[UUID].Clothes.Bodyarmor = new ComponentItem(0, 0);
                            }
                            else
                            {
                                if (nInventory.ActiveClothes[UUID][8].IsActive == true)
                                    return;
                                player.Armor = Convert.ToInt32(item.Data);
                                player.SetSharedData("HASARMOR", true);

                                nInventory.ActiveClothes[UUID][((int)item.Type * -1) - 1] = new nItem(item.Type, 1, item.Data, true);

                                nInventory.UnActiveItem(player, item.Type);
                                nInventory.Items[UUID][index].IsActive = true;
                                nInventory.Remove(player, index, 1);

                                GUI.Dashboard.Update(player, item, index);
                                //GUI.Dashboard.sendItems(player);
                                GUI.Dashboard.ClothesUpdate(player, nInventory.ActiveClothes[UUID][((int)item.Type * -1) - 1], ((int)item.Type * -1) - 1);
                            }
                            return;
                        }
                    case ItemType.Unknown:
                        {
                            if (item.IsActive)
                            {
                                Customization.CustomPlayerData[Main.Players[player].UUID].Clothes.Decals = new ComponentItem(Customization.EmtptySlots[gender][10], 0);
                            }
                            else
                            {
                                var itemData = (string)item.Data;
                                var variation = Convert.ToInt32(itemData.Split('_')[0]);
                                var texture = Convert.ToInt32(itemData.Split('_')[1]);
                                Customization.CustomPlayerData[Main.Players[player].UUID].Clothes.Decals = new ComponentItem(variation, texture);
                            }
                            player.SetClothes(10, Customization.CustomPlayerData[Main.Players[player].UUID].Clothes.Decals.Variation, Customization.CustomPlayerData[Main.Players[player].UUID].Clothes.Decals.Texture);
                            return;
                        }
                    case ItemType.Top:
                        {
                            if (item.IsActive)
                            {
                                if (Customization.CustomPlayerData[Main.Players[player].UUID].Clothes.Undershit.Variation == Customization.EmtptySlots[gender][8] || (!gender && Customization.CustomPlayerData[Main.Players[player].UUID].Clothes.Undershit.Variation == 15))
                                    Customization.CustomPlayerData[Main.Players[player].UUID].Clothes.Top = new ComponentItem(Customization.EmtptySlots[gender][11], 0);
                                else
                                {


                                    var underwearID = Customization.Undershirts[gender][Customization.CustomPlayerData[UUID].Clothes.Undershit.Variation];
                                    var underwear = Customization.Underwears[gender][underwearID];
                                    if (underwear.Top == -1)
                                    {
                                        Customization.CustomPlayerData[Main.Players[player].UUID].Clothes.Top = new ComponentItem(Customization.EmtptySlots[gender][11], 0);
                                        var uItem = nInventory.ActiveClothes[UUID][((int)item.Type * -1) - 1];
                                        nInventory.Add(player, new nItem(uItem.Type, 1, uItem.Data, false));
                                        nInventory.ActiveClothes[UUID][((int)item.Type * -1) - 1] = new nItem(ItemType.Debug, 1);
                                        GUI.Dashboard.ClothesUpdate(player, nInventory.ActiveClothes[UUID][((int)item.Type * -1) - 1], ((int)item.Type * -1) - 1);
                                        //RemoveClothes(player, )
                                    }
                                    else
                                        Customization.CustomPlayerData[Main.Players[player].UUID].Clothes.Top = new ComponentItem(underwear.Top, Customization.CustomPlayerData[Main.Players[player].UUID].Clothes.Undershit.Texture);
                                    Customization.CustomPlayerData[Main.Players[player].UUID].Clothes.Undershit = new ComponentItem(Customization.EmtptySlots[gender][8], 0);

                                }

                                nInventory.Items[UUID][index].IsActive = false;

                                GUI.Dashboard.Update(player, item, index);
                                GUI.Dashboard.sendItems(player);
                            }
                            else // Одеть
                            {
                                var itemData = (string)item.Data;
                                var variation = Convert.ToInt32(itemData.Split('_')[0]);
                                var texture = Convert.ToInt32(itemData.Split('_')[1]);

                                if (Customization.Tops[gender].FirstOrDefault(t => t.Variation == Customization.CustomPlayerData[Main.Players[player].UUID].Clothes.Top.Variation) != null || Customization.CustomPlayerData[Main.Players[player].UUID].Clothes.Top.Variation == Customization.EmtptySlots[gender][11])
                                {
                                    // Если нету футболки
                                    if (Customization.CustomPlayerData[Main.Players[player].UUID].Clothes.Undershit.Variation == Customization.EmtptySlots[gender][8] || (!gender && Customization.CustomPlayerData[Main.Players[player].UUID].Clothes.Undershit.Variation == 15))
                                        Customization.CustomPlayerData[Main.Players[player].UUID].Clothes.Top = new ComponentItem(variation, texture);
                                    else // если есть футболка
                                    {

                                        var underwearID = Customization.Undershirts[gender][Customization.CustomPlayerData[Main.Players[player].UUID].Clothes.Undershit.Variation];
                                        // var underwearID = 0;
                                        var underwear = Customization.Underwears[gender][underwearID];
                                        var underwearTexture = Customization.CustomPlayerData[Main.Players[player].UUID].Clothes.Undershit.Texture;
                                        var topType = Customization.Tops[gender].FirstOrDefault(t => t.Variation == variation).Type;
                                        Log.Debug($"UnderwearID: {underwearID} | TopType: {topType}");
                                        if (!underwear.UndershirtIDs.ContainsKey(topType)) // Если футболка не может быть одета под етот верх
                                        {
                                            Customization.CustomPlayerData[Main.Players[player].UUID].Clothes.Undershit = new ComponentItem(Customization.EmtptySlots[gender][8], 0);
                                            nInventory.UnActiveItem(player, ItemType.Undershit);
                                        }
                                        else
                                            Customization.CustomPlayerData[Main.Players[player].UUID].Clothes.Undershit = new ComponentItem(underwear.UndershirtIDs[topType], underwearTexture);
                                        Customization.CustomPlayerData[Main.Players[player].UUID].Clothes.Top = new ComponentItem(variation, texture);
                                    }
                                }
                                else // Если одета футболка Без верхней одежды
                                {
                                    var underwearID = 0;
                                    var underwear = Customization.Underwears[gender].Values.FirstOrDefault(u => u.Top == Customization.CustomPlayerData[Main.Players[player].UUID].Clothes.Top.Variation);
                                    var underwearTexture = Customization.CustomPlayerData[Main.Players[player].UUID].Clothes.Top.Texture;
                                    if (underwear != null)
                                    {
                                        var topType = Customization.Tops[gender].FirstOrDefault(t => t.Variation == variation).Type;
                                        Log.Debug($"UnderwearID: {underwearID} | TopType: {topType}");
                                        if (!underwear.UndershirtIDs.ContainsKey(topType))
                                        {
                                            Customization.CustomPlayerData[Main.Players[player].UUID].Clothes.Undershit = new ComponentItem(Customization.EmtptySlots[gender][8], 0);
                                            nInventory.UnActiveItem(player, ItemType.Undershit);
                                        }
                                        else
                                            Customization.CustomPlayerData[Main.Players[player].UUID].Clothes.Undershit = new ComponentItem(underwear.UndershirtIDs[topType], underwearTexture);
                                    }
                                    Customization.CustomPlayerData[Main.Players[player].UUID].Clothes.Top = new ComponentItem(variation, texture);
                                }

                                nInventory.ActiveClothes[UUID][((int)item.Type * -1) - 1] = new nItem(item.Type, 1, item.Data, true);

                                nInventory.UnActiveItem(player, item.Type);
                                nInventory.Items[UUID][index].IsActive = true;
                                nInventory.Remove(player, index, 1);

                                GUI.Dashboard.Update(player, item, index);
                                //GUI.Dashboard.sendItems(player);
                                GUI.Dashboard.ClothesUpdate(player, nInventory.ActiveClothes[UUID][((int)item.Type * -1) - 1], ((int)item.Type * -1) - 1);
                            }

                            var gloves = Customization.CustomPlayerData[Main.Players[player].UUID].Clothes.Gloves.Variation;
                            if (gloves != 0 &&
                                !Customization.CorrectGloves[gender][gloves].ContainsKey(Customization.CorrectTorso[gender][Customization.CustomPlayerData[Main.Players[player].UUID].Clothes.Top.Variation]))
                            {
                                nInventory.UnActiveItem(player, ItemType.Gloves);
                                Customization.CustomPlayerData[Main.Players[player].UUID].Clothes.Gloves = new ComponentItem(0, 0);
                            }

                            player.SetClothes(8, Customization.CustomPlayerData[Main.Players[player].UUID].Clothes.Undershit.Variation, Customization.CustomPlayerData[Main.Players[player].UUID].Clothes.Undershit.Texture);
                            player.SetClothes(11, Customization.CustomPlayerData[Main.Players[player].UUID].Clothes.Top.Variation, Customization.CustomPlayerData[Main.Players[player].UUID].Clothes.Top.Texture);
                            var noneGloves = Customization.CorrectTorso[gender][Customization.CustomPlayerData[Main.Players[player].UUID].Clothes.Top.Variation];
                            if (Customization.CustomPlayerData[Main.Players[player].UUID].Clothes.Gloves.Variation == 0)
                            {
                                Customization.CustomPlayerData[Main.Players[player].UUID].Clothes.Torso = new ComponentItem(noneGloves, 0);
                                player.SetClothes(3, noneGloves, 0);
                            }
                            else
                                player.SetClothes(3, Customization.CorrectGloves[gender][Customization.CustomPlayerData[Main.Players[player].UUID].Clothes.Gloves.Variation][noneGloves], Customization.CustomPlayerData[Main.Players[player].UUID].Clothes.Gloves.Texture);
                            return;
                        }
                    #endregion
                    case ItemType.BagWithDrill:
                    case ItemType.BagWithMoney:
                    case ItemType.Pocket:
                    case ItemType.Cuffs:
                    case ItemType.CarKey:
                        return;
                    case ItemType.KeyRing:
                        List<nItem> items = new List<nItem>();
                        string data = item.Data;
                        List<string> keys = (data.Length == 0) ? new List<string>() : new List<string>(data.Split('/'));
                        if (keys.Count > 0 && string.IsNullOrEmpty(keys[keys.Count - 1]))
                            keys.RemoveAt(keys.Count - 1);

                        foreach (var key in keys)
                            items.Add(new nItem(ItemType.CarKey, 1, key));
                        player.SetData("KEYRING", nInventory.Items[Main.Players[player].UUID].IndexOf(item));
                        GUI.Dashboard.OpenOut(player, items, "Связка ключей", 7);
                        return;
                    case ItemType.Material:
                        Trigger.ClientEvent(player, "board", "close");
                        GUI.Dashboard.isopen[player] = false;
                        GUI.Dashboard.Close(player);
                        Fractions.Manager.OpenGunCraftMenu(player);
                        return;
                    case ItemType.Beer:
                        EatManager.AddWater(player, 12);
                        EatManager.AddEat(player, 2);
                        Commands.RPChat("me", player, $"выпил(а) {nInventory.ItemsNames[(int)item.Type]}");
                        break;
                    case ItemType.Burger:
                        player.Health = (player.Health + 30 > 100) ? 100 : player.Health + 30;
                        EatManager.AddEat(player, 15);
                        if (player.GetData<int>("RESIST_TIME") < 600) Trigger.ClientEvent(player, "stopScreenEffect", "PPFilter");
                        Commands.RPChat("me", player, $"съел(а) {nInventory.ItemsNames[(int)item.Type]}");
                        break;
                    case ItemType.eCola:
                        EatManager.AddWater(player, 15);
                        EatManager.AddEat(player, 2);
                        Commands.RPChat("me", player, $"выпил(а) {nInventory.ItemsNames[(int)item.Type]}");
                        break;
                    case ItemType.HotDog:
                        EatManager.AddWater(player, -10);
                        EatManager.AddEat(player, 14);
                        if (player.GetData<int>("RESIST_TIME") < 600) Trigger.ClientEvent(player, "stopScreenEffect", "PPFilter");
                        Commands.RPChat("me", player, $"съел(а) {nInventory.ItemsNames[(int)item.Type]}");
                        break;
                    case ItemType.Pizza:
                        EatManager.AddWater(player, -10);
                        EatManager.AddEat(player, 30);
                        if (player.GetData<int>("RESIST_TIME") < 600) Trigger.ClientEvent(player, "stopScreenEffect", "PPFilter");
                        Commands.RPChat("me", player, $"съел(а) {nInventory.ItemsNames[(int)item.Type]}");
                        break;
                    case ItemType.Sandwich:
                        EatManager.AddWater(player, -5);
                        EatManager.AddEat(player, 8);
                        if (player.GetData<int>("RESIST_TIME") < 600) Trigger.ClientEvent(player, "stopScreenEffect", "PPFilter");
                        Commands.RPChat("me", player, $"съел(а) {nInventory.ItemsNames[(int)item.Type]}");
                        break;
                    case ItemType.Sprunk:
                        EatManager.AddWater(player, 25);
                        EatManager.AddEat(player, 2);
                        if (player.GetData<int>("RESIST_TIME") < 600) Trigger.ClientEvent(player, "stopScreenEffect", "PPFilter");
                        Commands.RPChat("me", player, $"выпил(а) {nInventory.ItemsNames[(int)item.Type]}");
                        break;
                    case ItemType.Сrisps:
                        EatManager.AddWater(player, -10);
                        EatManager.AddEat(player, 15);
                        if (player.GetData<int>("RESIST_TIME") < 600) Trigger.ClientEvent(player, "stopScreenEffect", "PPFilter");
                        Commands.RPChat("me", player, $"съел(а) {nInventory.ItemsNames[(int)item.Type]}");
                        break;
                    case ItemType.Rod:
                        RodManager.useInventory(player, 1);
                        break;
                    case ItemType.RodUpgrade:
                        RodManager.useInventory(player, 2);
                        break;
                    case ItemType.RodMK2:
                        RodManager.useInventory(player, 3);
                        break;
                    case ItemType.Drugs:
                        if (!player.HasData("USE_DRUGS") || DateTime.Now > player.GetData<DateTime>("USE_DRUGS"))
                        {
                            player.Health = (player.Health + 50 > 100) ? 100 : player.Health + 50;
                            Trigger.ClientEvent(player, "startScreenEffect", "DrugsTrevorClownsFight", 300000, false);
                            Commands.RPChat("me", player, $"закурил(а) косяк");
                            player.SetData("USE_DRUGS", DateTime.Now.AddMinutes(3));
                        }
                        else
                        {
                            Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Попробуйте использовать позже", 3000);
                            return;
                        }
                        break;
                    case ItemType.GasCan:
                        if (!player.IsInVehicle)
                        {
                            Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Вы должны находиться в машине", 3000);
                            GUI.Dashboard.Close(player);
                            return;
                        }
                        var veh = player.Vehicle;
                        if (!veh.HasSharedData("PETROL")) return;
                        var fuel = veh.GetSharedData<int>("PETROL");
                        if (fuel == VehicleManager.VehicleTank[veh.Class])
                        {
                            Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"В машине полный бак", 3000);
                            GUI.Dashboard.Close(player);
                            return;
                        }
                        fuel += 30;
                        if (fuel > VehicleManager.VehicleTank[veh.Class]) fuel = VehicleManager.VehicleTank[veh.Class];
                        veh.SetSharedData("PETROL", fuel);
                        if (player.Vehicle.HasData("ACCESS") && player.Vehicle.GetData<string>("ACCESS") == "GARAGE")
                        {
                            var number = player.Vehicle.NumberPlate;
                            VehicleManager.Vehicles[number].Fuel = fuel;
                        }
                        break;
                    case ItemType.HealthKit:
                        if (!player.HasData("USE_MEDKIT") || DateTime.Now > player.GetData<DateTime>("USE_MEDKIT"))
                        {
                            player.Health = 100;
                            player.SetData("USE_MEDKIT", DateTime.Now.AddMinutes(5));
                            Main.OnAntiAnim(player);
                            player.PlayAnimation("amb@code_human_wander_texting_fat@female@enter", "enter", 49);
                            NAPI.Task.Run(() => {
                                try
                                {
                                    if (player == null) return;
                                    if (!player.IsInVehicle) player.StopAnimation();
                                    else player.SetData("ToResetAnimPhone", true);
                                    Main.OffAntiAnim(player);
                                    Trigger.ClientEvent(player, "stopScreenEffect", "PPFilter");
                                } catch { }
                            }, 5000);
                            Commands.RPChat("me", player, $"использовал(а) аптечку");
                        }
                        else
                        {
                            Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Попробуйте использовать позже", 3000);
                            return;
                        }
                        break;
                    case ItemType.Lockpick:
                        if (player.GetData<int>("INTERACTIONCHECK") != 3)
                        {
                            Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Невозможно использовать в данный момент", 3000);
                            GUI.Dashboard.Close(player);
                            return;
                        }
                        //player.SetData("LOCK_TIMER", Main.StartT(10000, 999999, (o) => SafeMain.lockCrack(player, player.Name), "LOCK_TIMER"));
                        player.SetData("LOCK_TIMER", Timers.StartOnce(10000, () => SafeMain.lockCrack(player, player.Name)));
                        //player.FreezePosition = true;
                        Trigger.ClientEvent(player, "showLoader", "Идёт взлом", 1);
                        Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"Вы начали взламывать дверь", 3000);
                        break;
                    /*case ItemType.ArmyLockpick:
                        Houses.House house = Houses.HouseManager.Houses.FirstOrDefault(h => h.ID == player.GetData<int>("HOUSEID"));
                        if (house == null || !Houses.HouseManager.Houses.Contains(house)) return;
                        Random rnd = new Random();
                        //HijackingHouse.HijackingHouseData dataHij = HijackingHouse.HijackingHouseData.HijackingHouseDic.FirstOrDefault(i => i.Player == player);
                        if (dataHij != null)
                        {
                            if (dataHij.House.ID == house.ID && dataHij.House.Type == house.Type)
                            {
                                if (!house.Locked)
                                {
                                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Дом открыт", 3000);
                                    return;
                                }
                                else
                                {
                                    Fractions.Manager.sendFractionMessage(7, $"Взламывается дом с номером - {house.ID}", true);
                                    Fractions.Manager.sendFractionMessage(9, $"Взламывается дом с номером - {house.ID}", true);
                                    int random = rnd.Next(0, 12);
                                    if (random == 8)
                                    {
                                        house.SetLock(false);
                                        Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Вы открыли дом", 3000);
                                    }
                                    else
                                    {
                                        Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Попробуйте еще раз", 3000);
                                        nInventory.Remove(player, new nItem(ItemType.ArmyLockpick));
                                        return;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (!player.IsInVehicle || player.Vehicle.DisplayName != "BARRACKS" || player.VehicleSeat != 0)
                            {
                                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, $"Вы должны находиться в военном перевозчике материалов", 3000);
                                return;
                            }
                            if (VehicleStreaming.GetEngineState(player.Vehicle))
                            {
                                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, $"Машину уже заведена", 3000);
                                return;
                            }
                            int lucky = rnd.Next(0, 6);
                            if (lucky == 5 || lucky == 1)
                            {
                                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"У Вас не получилось завести машину. Попробуйте ещё раз", 3000);
                                return;
                            }
                            else
                            {
                                VehicleStreaming.SetEngineState(player.Vehicle, true);
                                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, $"У Вас получилось завести машину", 3000);
                            }
                        }
                        break;*/
                    case ItemType.Present:
                        player.Health = (player.Health + 10 > 100) ? 100 : player.Health + 10;
                        Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, $"Вы открыли подарок, в нём были:", 3000);

                        Tuple<int, int> types = nInventory.PresentsTypes[Convert.ToInt32(item.Data)];
                        if (types.Item1 <= 2)
                        {
                            Main.Players[player].EXP += nInventory.TypesCounts[types.Item1];
                            if (Main.Players[player].EXP >= 3 + Main.Players[player].LVL * 3)
                            {
                                Main.Players[player].EXP = Main.Players[player].EXP - (3 + Main.Players[player].LVL * 3);
                                Main.Players[player].LVL += 1;
                            }

                            Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"{nInventory.TypesCounts[types.Item1]} EXP", 3000);

                            MoneySystem.Wallet.Change(player, nInventory.TypesCounts[types.Item2]);

                            Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"$ {nInventory.TypesCounts[types.Item2]}", 3000);
                        }
                        else
                        {
                            MoneySystem.Wallet.Change(player, nInventory.TypesCounts[types.Item1]);

                            Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"$ {nInventory.TypesCounts[types.Item1]}", 3000);

                            Main.Players[player].EXP += nInventory.TypesCounts[types.Item2];
                            if (Main.Players[player].EXP >= 3 + Main.Players[player].LVL * 3)
                            {
                                Main.Players[player].EXP = Main.Players[player].EXP - (3 + Main.Players[player].LVL * 3);
                                Main.Players[player].LVL += 1;
                            }

                            Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"{nInventory.TypesCounts[types.Item2]} EXP", 3000);
                        }

                        Commands.RPChat("me", player, $"открыл(а) подарок {types.Item1} + {types.Item2}");
                        break;
                    case ItemType.Payek:
                        if (player.VehicleSeat == 0)
                        {
                            Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Вы не можете сьесть Сух.Паёк находясь за рулем.", 3000);
                            return;
                        }
                        if (!player.HasData("USE_MEDKIT") || DateTime.Now > player.GetData<DateTime>("USE_MEDKIT"))
                        {
                            EatManager.SetEat(player, 100);
                            EatManager.SetWater(player, 100);
                            NAPI.Task.Run(() => {
                                try
                                {
                                    if (player == null) return;
                                    else player.SetData("ToResetAnimPhone", true);
                                    Trigger.ClientEvent(player, "stopScreenEffect", "PPFilter");
                                }
                                catch { }
                            }, 5000);
                            Commands.RPChat("me", player, $"сьел(а) Сух.Паёк");
                        }
                        else
                        {
                            Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Попробуйте использовать позже", 3000);
                            return;
                        }
                        break;
                }
                nInventory.Remove(player, item.Type, 1);
                Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"Вы использовали {nInventory.ItemsNames[item.ID]}", 3000);
                GameLog.Items($"player({Main.Players[player].UUID})", "use", Convert.ToInt32(item.Type), 1, $"{item.Data}");
                GUI.Dashboard.Close(player);
            }
            catch (Exception e)
            {
                Log.Write($"EXCEPTION AT\"ITEM_USE\"/{item.Type}/{index}/{player.Name}/:\n" + e.ToString(), nLog.Type.Error);
            }
        }
        // TO DELETE
        private static List<int> TypesCounts = new List<int>()
        {
            5, 10, 15, 3000, 5000, 10000
        };
        private static List<Tuple<int, int>> PresentsTypes = new List<Tuple<int, int>>()
        {
            new Tuple<int, int>(0, 5),
            new Tuple<int, int>(1, 4),
            new Tuple<int, int>(2, 3),
            new Tuple<int, int>(5, 0),
            new Tuple<int, int>(4, 1),
            new Tuple<int, int>(3, 2),
        };
        //
        public static void onDrop(Player player, nItem item, dynamic data)
        {
            try
            {
                var rnd = new Random();
                if (data != null && (int)data != 1)
                    Commands.RPChat("me", player, $"выбросил(а) {nInventory.ItemsNames[(int)item.Type]}");

                GameLog.Items($"player({Main.Players[player].UUID})", "ground", Convert.ToInt32(item.Type), 1, $"{item.Data}");

                if (!nInventory.ClothesItems.Contains(item.Type) && !nInventory.WeaponsItems.Contains(item.Type) && item.Type != ItemType.CarKey && item.Type != ItemType.KeyRing)
                {
                    foreach (var o in NAPI.Pools.GetAllObjects())
                    {
                        if (player.Position.DistanceTo(o.Position) > 2) continue;
                        if (!o.HasSharedData("TYPE") || o.GetSharedData<string>("TYPE") != "DROPPED" || !o.HasData("ITEM")) continue;
                        nItem oItem = o.GetData<nItem>("ITEM");
                        if (oItem.Type == item.Type)
                        {
                            oItem.Count += item.Count;
                            o.SetData("ITEM", oItem);
                            o.SetData("WILL_DELETE", DateTime.Now.AddMinutes(2));
                            return;
                        }
                    }
                }
                item.IsActive = false;

                
                var xrnd = rnd.NextDouble();
                var yrnd = rnd.NextDouble();
                var obj = NAPI.Object.CreateObject(nInventory.ItemModels[item.Type], player.Position + nInventory.ItemsPosOffset[item.Type] + new Vector3(xrnd, yrnd, 0), player.Rotation + nInventory.ItemsRotOffset[item.Type], 255, player.Dimension);
                obj.SetSharedData("TYPE", "DROPPED");
                obj.SetSharedData("PICKEDT", false);
                obj.SetData("ITEM", item);
                var id = rnd.Next(100000, 999999);
                while (ItemsDropped.Contains(id)) id = rnd.Next(100000, 999999);
                obj.SetData("ID", id);
                //obj.SetData("DELETETIMER", Main.StartT(14400000, 99999999, (o) => deleteObject(obj), "ODELETE_TIMER"));
                obj.SetData("DELETETIMER", Timers.StartOnce(14400000, () => deleteObject(obj)));
            }
            catch (Exception e) { Log.Write("onDrop: " + e.Message, nLog.Type.Error); }
        }
        public static void onTransfer(Player player, nItem item, dynamic data)
        {
            //
        }
    }
}
