using System;
using System.Collections.Generic;
using System.Text;

namespace Redage.SDK
{
    public class ItemsWeight
    {
        private static nLog Log = new nLog("ItemsWeight");

        private static Dictionary<ItemType, float> ItemsWeights = new Dictionary<ItemType, float>()
        {
            { ItemType.BagWithMoney, 0.1f },
            { ItemType.Material, 0.1f },
            { ItemType.Drugs, 0.1f },
            { ItemType.BagWithDrill, 0.1f },
            { ItemType.Debug, 0.0f },
            { ItemType.HealthKit, 0.1f },
            { ItemType.GasCan, 0.1f },
            { ItemType.Сrisps, 0.1f },
            { ItemType.Beer, 0.1f },
            { ItemType.Pizza, 0.1f },
            { ItemType.Burger, 0.1f },
            { ItemType.HotDog, 0.1f },
            { ItemType.Sandwich, 0.1f },
            { ItemType.eCola, 0.1f },
            { ItemType.Sprunk, 0.1f },
            { ItemType.Lockpick, 0.1f },
            { ItemType.ArmyLockpick, 0.1f },
            { ItemType.Pocket, 0.1f },
            { ItemType.Cuffs, 0.1f },
            { ItemType.CarKey, 0.1f },
            { ItemType.Present, 0.1f },
            { ItemType.KeyRing, 0.1f },

            { ItemType.Mask, 0.1f },
            { ItemType.Gloves, 0.1f },
            { ItemType.Leg, 0.1f },
            { ItemType.Bag, 0.1f },
            { ItemType.Feet, 0.1f },
            { ItemType.Jewelry, 0.1f },
            { ItemType.Undershit, 0.1f },
            { ItemType.BodyArmor, 0.1f },
            { ItemType.Unknown, 0.1f },
            { ItemType.Top, 0.1f },
            { ItemType.Hat, 0.1f },
            { ItemType.Glasses, 0.1f },
            { ItemType.Accessories, 0.1f },

            { ItemType.RusDrink1, 0.1f },
            { ItemType.RusDrink2, 0.1f },
            { ItemType.RusDrink3, 0.1f },

            { ItemType.YakDrink1, 0.1f },
            { ItemType.YakDrink2, 0.1f },
            { ItemType.YakDrink3, 0.1f },

            { ItemType.LcnDrink1, 0.1f },
            { ItemType.LcnDrink2, 0.1f },
            { ItemType.LcnDrink3, 0.1f },

            { ItemType.ArmDrink1, 0.1f },
            { ItemType.ArmDrink2, 0.1f },
            { ItemType.ArmDrink3, 0.1f },

            { ItemType.Pistol, 0.1f },
            { ItemType.CombatPistol, 0.1f },
            { ItemType.Pistol50, 0.1f },
            { ItemType.SNSPistol, 0.1f },
            { ItemType.HeavyPistol, 0.1f },
            { ItemType.VintagePistol, 0.1f },
            { ItemType.MarksmanPistol, 0.1f },
            { ItemType.Revolver, 0.1f },
            { ItemType.APPistol, 0.1f },
            { ItemType.StunGun, 0.1f },
            { ItemType.FlareGun, 0.1f },
            { ItemType.DoubleAction, 0.1f },
            { ItemType.PistolMk2, 0.1f },
            { ItemType.SNSPistolMk2, 0.1f },
            { ItemType.RevolverMk2, 0.1f },

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

            { ItemType.PistolAmmo, 0.1f },
            { ItemType.RiflesAmmo, 0.1f },
            { ItemType.ShotgunsAmmo, 0.1f },
            { ItemType.SMGAmmo, 0.1f },
            { ItemType.SniperAmmo, 0.1f },

            /* Fishing */
            { ItemType.Rod, 1 },
            { ItemType.RodUpgrade, 1 },
            { ItemType.RodMK2, 1 },
            { ItemType.Naz, 0.1f },
            { ItemType.Koroska, 0.1f },
            { ItemType.Kyndja, 0.1f },
            { ItemType.Lococ, 0.1f },
            { ItemType.Okyn, 0.1f },
            { ItemType.Ocetr, 0.1f },
            { ItemType.Skat, 0.1f },
            { ItemType.Tunec, 0.1f },
            { ItemType.Ygol, 0.1f },
            { ItemType.Amyr, 0.1f },
            { ItemType.Chyka, 0.1f },

            //Farmer Job Items
            { ItemType.Hay, 0.1f }, //60 урожая всего в инвентаре
            { ItemType.Seed, 0.1f }, //100 семян всего в инвентаре (максимум)

            { ItemType.Payek, 0.1f },
            { ItemType.CasinoChips, 0.01f },
        };

        public static float GetItemWeight(ItemType item)
        {
            if (ItemsWeights.ContainsKey(item))
                return ItemsWeights[item];
            Log.Write($"Item {item} don't have weight", nLog.Type.Warn);
            return 0;
        }
    }
}
