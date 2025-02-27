using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;

// prop_food_cb_donuts - пончики

namespace dayz.Items
{
    enum ItemType
    {
        EmptyBottle = 1,
        WaterBottle,
        Burger,
        Chips,
        Nuggets,
        Pizza,
        Cola,
        Pineapple,
        Champane,
        Beer,
        Vodka,
        WhiteWine,
        RoseWine,
        RedWine,
        Sprunk,
        Orang,
        CarBattery,
        Cigarets,
        Lighter,
        Pistol50,
        AssaultRifle,
        Scope2x,
        Scope4x,
        Scope8x,
        ToolBox,
        HealthKit,
        BigHealthKit,
        SmallBackpack,
        Gps,
        Wheel,
        VehicleDoor,
        Radiator,
        EnginePart,
        Knife,
        Hammer,
        Crowbar,
        Bat,
        Wrench,
        StoneHatchet,
        Watch,
    }
    internal class Items 
    {
        public static Dictionary<ItemType, Item> ServerItems = new Dictionary<ItemType, Item>()
        {
            {ItemType.EmptyBottle, new Item("Пустая бутылка", ItemType.EmptyBottle, "prop_ld_flow_bottle", new Vector3(0, 0, -0.88), new Vector3()) },
            {ItemType.WaterBottle, new Item("Бутылка воды", ItemType.WaterBottle, "prop_ld_flow_bottle", new Vector3(0, 0, -0.93), new Vector3()) },
            {ItemType.Burger, new Item("Бургер", ItemType.Burger, "prop_food_burg3", new Vector3(0, 0, -1), new Vector3()) },
            {ItemType.Chips, new Item("Картошка-фри", ItemType.Chips, "prop_food_chips", new Vector3(0, 0, -1), new Vector3()) },
            {ItemType.Nuggets, new Item("Наггетсы", ItemType.Nuggets, "prop_food_cb_nugets", new Vector3(0, 0, -1), new Vector3()) },
            {ItemType.Pizza, new Item("Пицца", ItemType.Pizza, "prop_pizza_box_02", new Vector3(0, 0, -1), new Vector3()) },
            {ItemType.Cola, new Item("eCola", ItemType.Cola, "prop_ecola_can", new Vector3(0, 0, -0.94), new Vector3()) },
            {ItemType.Pineapple, new Item("Ананас", ItemType.Pineapple, "prop_pineapple", new Vector3(0, 0, -0.90), new Vector3()) },
            {ItemType.Champane, new Item("Шампанское", ItemType.Champane, "ba_prop_battle_champ_closed", new Vector3(0, 0, -0.80), new Vector3()) },
            {ItemType.Beer, new Item("Пиво", ItemType.Beer, "prop_amb_beer_bottle", new Vector3(0, 0, -0.87), new Vector3()) },
            {ItemType.Vodka, new Item("Водка", ItemType.Vodka, "prop_vodka_bottle", new Vector3(0, 0, -1), new Vector3()) },
            {ItemType.WhiteWine, new Item("Вино белое", ItemType.WhiteWine, "prop_wine_white", new Vector3(0, 0, -1), new Vector3()) },
            {ItemType.RoseWine, new Item("Вино розовое", ItemType.RoseWine, "prop_wine_rose", new Vector3(0, 0, -1), new Vector3()) },
            {ItemType.RedWine, new Item("Вино красное", ItemType.RedWine, "prop_wine_red", new Vector3(0, 0, -1), new Vector3()) },
            {ItemType.Sprunk, new Item("Sprunk", ItemType.Sprunk, "prop_ld_can_01", new Vector3(0, 0, -0.94), new Vector3()) },
            {ItemType.Orang, new Item("Orang-Tang", ItemType.Orang, "prop_orang_can_01", new Vector3(0, 0, -0.94), new Vector3()) },
            {ItemType.CarBattery, new Item("Аккумулятор", ItemType.CarBattery, "prop_car_battery_01", new Vector3(0, 0, -0.9), new Vector3()) },
            {ItemType.Cigarets, new Item("Пачка сигарет", ItemType.Cigarets, "ng_proc_cigpak01a", new Vector3(0, 0, -1), new Vector3()) },
            {ItemType.Lighter, new Item("Зажигалка", ItemType.Cigarets, "ng_proc_ciglight01a", new Vector3(0, 0, -1), new Vector3()) },
            {ItemType.Pistol50, new Item("Пистолет", ItemType.Pistol50, "w_pi_pistol50", new Vector3(0, 0, -0.98), new Vector3(90, 0, 0)) },
            {ItemType.AssaultRifle, new Item("АК-74", ItemType.AssaultRifle, "w_ar_assaultrifle", new Vector3(0, 0, -0.98), new Vector3(90, 0, 0)) },
            {ItemType.Scope2x, new Item("Прицел 2x", ItemType.Scope2x, "w_at_scope_small", new Vector3(0, 0, -0.97), new Vector3(90, 0, 0)) },
            {ItemType.Scope4x, new Item("Прицел 4x", ItemType.Scope4x, "w_at_scope_medium", new Vector3(0, 0, -0.97), new Vector3(90, 0, 0)) },
            {ItemType.Scope8x, new Item("Прицел 8x", ItemType.Scope8x, "w_at_scope_large", new Vector3(0, 0, -0.97), new Vector3(90, 0, 0)) },
            {ItemType.ToolBox, new Item("Интсрументы", ItemType.ToolBox, "gr_prop_gr_tool_box_01a", new Vector3(0, 0, -0.95), new Vector3(90, 0, 0)) },
            {ItemType.HealthKit, new Item("Маленькая аптечка", ItemType.HealthKit, "prop_ld_health_pack", new Vector3(0, 0, -0.9), new Vector3(0, 0, 0)) },
            {ItemType.BigHealthKit, new Item("Большая аптечка", ItemType.BigHealthKit, "xm_prop_smug_crate_s_medical", new Vector3(0, 0, -0.85), new Vector3(0, 0, 0)) },
            {ItemType.SmallBackpack, new Item("Маленький рюкзак", ItemType.SmallBackpack, "vw_prop_vw_backpack_01a", new Vector3(0, 0, -1.02), new Vector3(90, 0, 0)) },
            {ItemType.Gps, new Item("GPS-Навигатор", ItemType.Gps, "prop_police_phone", new Vector3(0, 0, -0.99), new Vector3(-90, 0, 0)) },
            {ItemType.Wheel, new Item("Колесо", ItemType.Wheel, "prop_wheel_01", new Vector3(0, 0, -0.9), new Vector3(-90, 0, 0)) },
            {ItemType.VehicleDoor, new Item("Дверь автомобиля", ItemType.VehicleDoor, "imp_prop_impexp_car_door_01a", new Vector3(0, 0, -0.9), new Vector3(0, 90, 0)) },
            {ItemType.Radiator, new Item("Радиатор", ItemType.Radiator, "imp_prop_impexp_radiator_01", new Vector3(0, 0, -1.1), new Vector3(70, 0, 0)) },
            {ItemType.EnginePart, new Item("Части двигателя", ItemType.EnginePart, "imp_prop_impexp_exhaust_05", new Vector3(0, 0, -1), new Vector3(0, 0, 0)) },
            {ItemType.Knife, new Item("Нож", ItemType.Knife, "w_me_knife_01", new Vector3(0, 0, -0.98), new Vector3(90, 0, 0)) },
            {ItemType.Hammer, new Item("Молоток", ItemType.Hammer, "w_me_hammer", new Vector3(0, 0, -0.98), new Vector3(90, 0, 0)) },
            {ItemType.Crowbar, new Item("Лом", ItemType.Crowbar, "w_me_crowbar", new Vector3(0, 0, -0.98), new Vector3(90, 0, 0)) },
            {ItemType.Bat, new Item("Бита", ItemType.Bat, "w_me_bat", new Vector3(0, 0, -0.98), new Vector3(90, 0, 0)) },
            {ItemType.Wrench, new Item("Гаячный ключ", ItemType.Wrench, "w_me_wrench", new Vector3(0, 0, -0.98), new Vector3(90, 0, 0)) },
            {ItemType.StoneHatchet, new Item("Топорик", ItemType.StoneHatchet, "w_me_stonehatchet", new Vector3(0, 0, -0.98), new Vector3(90, 0, 0)) },
            {ItemType.Watch, new Item("Часы", ItemType.Watch, "p_watch_02", new Vector3(0, 0, -0.99), new Vector3(0, 0, 0)) },
            



        };
    }

    class Item
    {
        public ItemType Type { get; }
        public string Name { get; }
        public uint Model { get; }
        public Vector3 PositionOffset { get; }
        
        public Vector3 RotationOffset { get; }

        public Item(string name, ItemType type, string model, Vector3 posOffset, Vector3 rotOffset)
        {
            Type = type;
            Name = name;
            Model = NAPI.Util.GetHashKey(model);
            PositionOffset = posOffset;
            RotationOffset = rotOffset;
        }
    
    }
}
