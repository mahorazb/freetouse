namespace Golemo.Core
{
    using GTANetworkAPI;
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Timers;
    using GolemoSDK;
    using Golemo.GUI;

    public class Hunting : Script
    {
        private static nLog Log = new nLog("Hunting");

        public static Dictionary<int, int> SkinPrice = new Dictionary<int, int>();

        public static Dictionary<int, string> Animals = new Dictionary<int, string>()
        {
            { 1, "a_c_rabbit_01" },
            { 2, "a_c_deer" },
            { 3, "a_c_boar" },
            { 4, "a_c_coyote"},
            { 5, "a_c_mtlion"},
            { 6, "a_c_panther"},
        };

        public static Dictionary<int, ItemType> AnimalsSkin = new Dictionary<int, ItemType>()
        {
            { 1, ItemType.SkinRabbit },
            { 2, ItemType.SkinDeer },
            { 3, ItemType.SkinBoar },
            { 4, ItemType.SkinCoyote},
            { 5, ItemType.SkinLion},
            { 6, ItemType.SkinPanther},
        };

        public static Vector3[] AnimalSpawnPoints =
       {
            new Vector3(-1725.521, 4699.659, 33.80555),
            new Vector3(-1690.836, 4682.494, 24.47228),
            new Vector3(-1661.219, 4650.042, 26.12522),
            new Vector3(-1613.11, 4632.693, 46.37965),
            new Vector3(-1569.1, 4688.946, 48.04772),
            new Vector3(-1549.585, 4766.055, 60.47577),
            new Vector3(-1461.021, 4702.999, 39.26906),
            new Vector3(-1397.87, 4637.824, 72.12587),
            new Vector3(-617.851, 5762.557, 31.45378),
            new Vector3(-613.3984, 5863.435, 22.00531),
            new Vector3(-512.6949, 5940.441, 34.46115),
            new Vector3(-363.9753, 5921.967, 43.97315),
            new Vector3(-384.0528, 5866.263, 49.28809),
            new Vector3(-374.6584, 5798.462, 62.83068),
            new Vector3(-448.7513, 5565.69, 71.9878),
            new Vector3(-551.2087, 5167.825, 97.50465),
            new Vector3(-603.5089, 5154.867, 110.1652),
            new Vector3(-711.7279, 5149.544, 114.7229),
            new Vector3(-711.3442, 5075.649, 138.9036),
            new Vector3(-672.9759, 5042.516, 152.8032),
            new Vector3(-661.6283, 4974.586, 172.7258),
            new Vector3(-600.277, 4918.438, 176.7214),
            new Vector3(-588.3793, 4889.981, 181.3767),
            new Vector3(-549.8376, 4838.274, 183.2239),
            new Vector3(-478.639, 4831.655, 209.2594),
            new Vector3(-399.3954, 4865.303, 203.7752),
            new Vector3(-411.9441, 4946.082, 177.4535),
            new Vector3(-414.8653, 5074.294, 149.0627),
        };

        public static Vector3 SellPostion = new Vector3(-679.62646, 5833.967, 16.211308);

        public static Random rand = new Random();

        [ServerEvent(Event.ResourceStart)]
        public static void OnStart()
        {
            try
            {
                for (int i = 0; i < AnimalSpawnPoints.Length; i++)
                {
                    int type = rand.Next(1, Animals.Count);

                    new HuntingAnimal(i + 1, AnimalSpawnPoints[i], type, NAPI.Util.GetHashKey(Animals[type]));
                }

                ColShape col = NAPI.ColShape.CreateCylinderColShape(SellPostion, 1, 3, 0);
                col.OnEntityEnterColShape += (s, e) => {
                    try
                    {
                        e.SetData("INTERACTIONCHECK", 1001);
                    }
                    catch (Exception ex) { Log.Write("col.OnEntityEnterColShape: " + ex.Message, nLog.Type.Error); }
                };
                col.OnEntityExitColShape += (s, e) => {
                    try
                    {
                        e.SetData("INTERACTIONCHECK", 0);
                    }
                    catch (Exception ex) { Log.Write("col.OnEntityExitColShape: " + ex.Message, nLog.Type.Error); }
                };
                NAPI.Marker.CreateMarker(1, SellPostion - new Vector3(0, 0, 0.7), new Vector3(), new Vector3(), 1, new Color(0, 255, 255), false, 0);
                NAPI.TextLabel.CreateTextLabel(Main.StringToU16("~g~Продавец кожи"), SellPostion + new Vector3(0, 0, 0.3), 30f, 0.4f, 0, new Color(255, 255, 255), true, 0);


                for (int i = 1; i <= AnimalsSkin.Count; i++)
                {
                    SkinPrice.Add(i, rand.Next(1000, 3000));
                }

                Log.Write("Created " + SpawnedAnimals.Count + " animals.");
            }
            catch (Exception ex)
            {
                Log.Write(ex.Message);
            }
        }

        public static List<HuntingAnimal> SpawnedAnimals = new List<HuntingAnimal>();


        [RemoteEvent("peds_requireController")]
        public static void AController(Player player, int pedId)
        {
            try
            {
                if (pedId < 0 || pedId > SpawnedAnimals.Count - 1) return;

                SpawnedAnimals[pedId - 1].handle.Controller = player;

                return;
            }
            catch(Exception ex)
            {
                Log.Write(ex.Message);
            }
        }

        [RemoteEvent("peds_removeController")]
        public static void RController(Player player, int pedId)
        {
            try
            {
                if (pedId < 0 || pedId > SpawnedAnimals.Count - 1) return;

                if (SpawnedAnimals[pedId - 1].handle != null)
                    SpawnedAnimals[pedId - 1].handle.Controller = null;

                return;
            }
            catch(Exception ex)
            {
                Log.Write(ex.Message);
            }
        }

        [RemoteEvent("peds_death")]
        public static void Death(Player player, int pedId, double x, double y, double z)
        {
            try
            {
                if (pedId < 0 || pedId > SpawnedAnimals.Count - 1) return;

                SpawnedAnimals[pedId - 1].Death(new Vector3(x, y, z));

                return;
            }
            catch(Exception ex)
            {
                Log.Write(ex.Message);
            }
        }

        public static void HarvestAnimal(Player player)
        {
            try
            {
                int animalId = player.GetData<int>("ANIMAL_ID");

                int tryAdd = nInventory.TryAdd(player, new nItem(AnimalsSkin[SpawnedAnimals[animalId - 1].Type], 1));

                if (tryAdd != 0)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Нехватает места в инвентаре", 3000);
                    return;
                }

                if(player.GetData<ItemType>("LastActiveWeap") != ItemType.Knife)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "У вас в руках должен быть нож", 3000);
                    return;
                }

                player.TriggerEvent("client_hunting_anim");
                

                SpawnedAnimals[animalId - 1].Harvest(player);

                return;
            }
            catch(Exception ex)
            {
                Log.Write(ex.Message);
            }
        }

        public static void CheckAnimals()
        {
            foreach(HuntingAnimal anm in SpawnedAnimals)
            {
                if (!anm.Alive)
                {
                    anm.Respawn();
                }
            }

            SkinPrice.Clear();

            for (int i = 1; i <= AnimalsSkin.Count; i++)
            {
                SkinPrice.Add(i, rand.Next(1000, 3000));
            }
        }

        public static void OpenSellMenu(Player player)
        {
            Menu menu = new Menu("sellskin", false, false);
            menu.Callback = callback_sellskin;

            Menu.Item menuItem = new Menu.Item("header", Menu.MenuItem.Header);
            menuItem.Text = "Продажа кожи";
            menu.Add(menuItem);
        
            menuItem = new Menu.Item("sell1", Menu.MenuItem.Button);
            menuItem.Text = $"Кролик - {SkinPrice[1]}$";
            menu.Add(menuItem);

            menuItem = new Menu.Item("sell2", Menu.MenuItem.Button);
            menuItem.Text = $"Олень - {SkinPrice[2]}$";
            menu.Add(menuItem);

            menuItem = new Menu.Item("sell3", Menu.MenuItem.Button);
            menuItem.Text = $"Кабан - {SkinPrice[3]}$";
            menu.Add(menuItem);

            menuItem = new Menu.Item("sell4", Menu.MenuItem.Button);
            menuItem.Text = $"Койот - {SkinPrice[4]}$";
            menu.Add(menuItem);

            menuItem = new Menu.Item("sell5", Menu.MenuItem.Button);
            menuItem.Text = $"Лев - {SkinPrice[5]}$";
            menu.Add(menuItem);

            menuItem = new Menu.Item("sell6", Menu.MenuItem.Button);
            menuItem.Text = $"Пантера - {SkinPrice[6]}$";
            menu.Add(menuItem);

            menuItem = new Menu.Item("back", Menu.MenuItem.Button);
            menuItem.Text = "Выход";
            menu.Add(menuItem);

            menu.Open(player);
        }

        private static void callback_sellskin(Player player, Menu menu, Menu.Item item, string eventName, dynamic data)
        {
            int UUID = Main.Players[player].UUID;
            switch (item.ID)
            {
                case "sell1":
                    List<nItem> skin = nInventory.FindAll(UUID, ItemType.SkinRabbit);

                    if(skin.Count == 0)
                    {
                        Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "У вас нет кожи этого животного", 3000);
                        return;
                    }

                    int allPrice = 0;

                    for(int i = 0; i < skin.Count; i++)
                    {
                        int skinPrice = Convert.ToInt32(SkinPrice[1] / 100) * Convert.ToInt32(skin[i].Data);
                        allPrice += skinPrice;
                        nInventory.Remove(player, new nItem(ItemType.SkinRabbit, 1));
                    }

                    MoneySystem.Wallet.Change(player, allPrice);

                    Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, $"Вы продали кожу Кролика на {allPrice}$");
                    GameLog.Money("server", player.Name, allPrice, "hunting");
                    return;
                case "sell2":
                    skin = nInventory.FindAll(UUID, ItemType.SkinDeer);

                    if (skin.Count == 0)
                    {
                        Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "У вас нет кожи этого животного", 3000);
                        return;
                    }

                    allPrice = 0;

                    for (int i = 0; i < skin.Count; i++)
                    {
                        int skinPrice = Convert.ToInt32(SkinPrice[2] / 100) * Convert.ToInt32(skin[i].Data);
                        allPrice += skinPrice;
                        nInventory.Remove(player, new nItem(ItemType.SkinDeer, 1));
                    }

                    MoneySystem.Wallet.Change(player, allPrice);

                    Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, $"Вы продали кожу Оленя на {allPrice}$");
                    GameLog.Money("server", player.Name, allPrice, "hunting");
                    return;
                case "sell3":
                    skin = nInventory.FindAll(UUID, ItemType.SkinBoar);

                    if (skin.Count == 0)
                    {
                        Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "У вас нет кожи этого животного", 3000);
                        return;
                    }

                    allPrice = 0;

                    for (int i = 0; i < skin.Count; i++)
                    {
                        int skinPrice = Convert.ToInt32(SkinPrice[3] / 100) * Convert.ToInt32(skin[i].Data);
                        allPrice += skinPrice;
                        nInventory.Remove(player, new nItem(ItemType.SkinBoar, 1));
                    }

                    MoneySystem.Wallet.Change(player, allPrice);

                    Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, $"Вы продали кожу Кабана на {allPrice}$");
                    GameLog.Money("server", player.Name, allPrice, "hunting");
                    return;
                case "sell4":
                    skin = nInventory.FindAll(UUID, ItemType.SkinCoyote);

                    if (skin.Count == 0)
                    {
                        Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "У вас нет кожи этого животного", 3000);
                        return;
                    }

                    allPrice = 0;

                    for (int i = 0; i < skin.Count; i++)
                    {
                        int skinPrice = Convert.ToInt32(SkinPrice[4] / 100) * Convert.ToInt32(skin[i].Data);
                        allPrice += skinPrice;
                        nInventory.Remove(player, new nItem(ItemType.SkinCoyote, 1));
                    }

                    MoneySystem.Wallet.Change(player, allPrice);

                    Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, $"Вы продали кожу Койота на {allPrice}$");
                    GameLog.Money("server", player.Name, allPrice, "hunting");
                    return;
                case "sell5":
                    skin = nInventory.FindAll(UUID, ItemType.SkinLion);

                    if (skin.Count == 0)
                    {
                        Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "У вас нет кожи этого животного", 3000);
                        return;
                    }

                    allPrice = 0;

                    for (int i = 0; i < skin.Count; i++)
                    {
                        int skinPrice = Convert.ToInt32(SkinPrice[5] / 100) * Convert.ToInt32(skin[i].Data);
                        allPrice += skinPrice;
                        nInventory.Remove(player, new nItem(ItemType.SkinLion, 1));
                    }

                    MoneySystem.Wallet.Change(player, allPrice);

                    Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, $"Вы продали кожу Льва на {allPrice}$");
                    GameLog.Money("server", player.Name, allPrice, "hunting");
                    return;
                case "sell6":
                    skin = nInventory.FindAll(UUID, ItemType.SkinPanther);

                    if (skin.Count == 0)
                    {
                        Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "У вас нет кожи этого животного", 3000);
                        return;
                    }

                    allPrice = 0;

                    for (int i = 0; i < skin.Count; i++)
                    {
                        int skinPrice = Convert.ToInt32(SkinPrice[6] / 100) * Convert.ToInt32(skin[i].Data);
                        allPrice += skinPrice;
                        nInventory.Remove(player, new nItem(ItemType.SkinPanther, 1));
                    }

                    MoneySystem.Wallet.Change(player, allPrice);

                    Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, $"Вы продали кожу Пантеры на {allPrice}$");
                    GameLog.Money("server", player.Name, allPrice, "hunting");
                    return;
                case "back":
                    MenuManager.Close(player);
                    return;
            }
        }


        public class HuntingAnimal
        {
            public Ped handle;
            public int ID;
            public Vector3 Spawn;
            public int Type;
            public ColShape Shape;

            public bool Alive = true;
            // Handles each animal and it's current state.
            public HuntingAnimal(int id, Vector3 spawn, int type, uint hash)
            {

                ID = id;
                Type = type;

                handle = NAPI.Ped.CreatePed(hash, spawn, 0, true, false, false);
                handle.Controller = null;

                Dictionary<string, object> dict = new Dictionary<string, object>();

                dict.Add("staticDead", false);
                dict.Add("controllerScript", "ANIMAL_HUNTING");
                dict.Add("autoControl", true);
                dict.Add("pedId", ID);
                handle.SetSharedData(dict);

                Spawn = spawn;

                SpawnedAnimals.Add(this);

            }

            public void Harvest(Player player)
            {
                try
                {
                    Trigger.ClientEventToPlayers(NAPI.Pools.GetAllPlayers().ToArray(), "client_ped_destroy", ID);

                    Alive = false;
                    NAPI.Task.Run(() =>
                    {
                        Shape.Delete();
                        player.SetData("INTERACTIONCHECK", 0);
                        Trigger.ClientEvent(player, "playerInteractionCheck", false);
                    });

                    NAPI.Task.Run(() =>
                    {
                        
                        handle.Delete();
                        handle = null;

                        int sost = rand.Next(5, 10) * 10;

                        nInventory.Add(player, new nItem(AnimalsSkin[Type], 1, sost));

                        Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, $"Вы получили {sost}% {nInventory.ItemsNames[(int)AnimalsSkin[Type]]} ");

                    }, 5000);
                }
                catch(Exception ex)
                {
                    Log.Write(ex.Message);
                }
            }

            public void Respawn()
            {
                try
                {
                    if (handle == null)
                    {
                        handle = NAPI.Ped.CreatePed(NAPI.Util.GetHashKey(Animals[Type]), Spawn, 0, true, false, false);

                        Dictionary<string, object> dict = new Dictionary<string, object>();

                        dict.Add("staticDead", false);
                        dict.Add("controllerScript", "ANIMAL_HUNTING");
                        dict.Add("autoControl", true);
                        dict.Add("pedId", ID);

                        handle.SetSharedData(dict);
                    }
                }
                catch(Exception ex)
                {
                    Log.Write(ex.Message);
                }
            }

            public void Death(Vector3 pos)
            {
                try
                {
                    handle.SetSharedData("staticDead", true);

                    Shape = NAPI.ColShape.CreateCylinderColShape(pos, 1, 2);
                    Shape.SetData("A_ID", ID);
                    Shape.OnEntityEnterColShape += (shape, entity) =>
                    {
                        entity.SetData("INTERACTIONCHECK", 1000);
                        entity.SetData("ANIMAL_ID", Shape.GetData<int>("A_ID"));
                    };
                    Shape.OnEntityExitColShape += (shape, entity) =>
                    {
                        entity.SetData("INTERACTIONCHECK", 0);
                        entity.ResetData("ANIMAL_ID");
                    };
                }
                catch(Exception ex)
                {
                    Log.Write(ex.Message);
                }
            }

        }
    }
}
