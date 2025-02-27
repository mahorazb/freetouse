using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using GTANetworkAPI;
using Newtonsoft.Json;
using Server.Core;
using Server.World;

namespace Server.Vehicles
{
    class VehicleSync : Script
    {
        private const string VEHICLE_ID_VAR = "V_ID";
        private const string VEHICLE_DOORS_VAR = "V_DOORS";
        private const string VEHICLE_WHEELS_VAR = "V_WHEELS";
        private const string VEHICLE_ENGINE_VAR = "V_ENGINE";

        private static Random random = new Random();
        
        public static List<VehicleHash> VehicleModels = new List<VehicleHash>() 
        {
            VehicleHash.Emperor2,
            VehicleHash.Glendale,
            VehicleHash.Primo,
            VehicleHash.Voodoo2,
            VehicleHash.Tornado4,
            VehicleHash.Manchez,
            VehicleHash.Burrito, 
        };

        public static Dictionary<int, Vehicle> Vehicles = new Dictionary<int, Vehicle>();

        [ServerEvent(Event.ResourceStart)]
        public static void ResourceStart()
        {
            foreach(VehiclePosition pos in Positions.VehicleSpawn)
            {
                CreateRandomVehicle(pos.Position, pos.Rotation);
            }
        }

        public static void CreateRandomVehicle(Vector3 pos, float rot)
        {
            
            GTANetworkAPI.Vehicle veh = NAPI.Vehicle.CreateVehicle(VehicleModels[random.Next(0, VehicleModels.Count)], pos, rot, random.Next(0, 100), random.Next(0, 100), GenerateRandomNumber(9));

            List<int> doors = Enumerable.Repeat(0, 4).Select(i => random.Next(0, 2)).ToList();
            List<int> wheels = Enumerable.Repeat(0, 6).Select(i => random.Next(0, 2)).ToList();

            SetVehicleDoors(veh, doors);
            SetVehicleWheels(veh, wheels);
            SetVehicleEngineStatus(veh, false);


            veh.SetSharedData(VEHICLE_ID_VAR, JsonConvert.SerializeObject(Vehicles.Count));

            Vehicles.Add(Vehicles.Count, veh);
        }

        public static string GenerateRandomNumber(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        [RemoteEvent("engineStatus")]
        public static void ChangeEngineStatus(Player player)
        {
            Vehicle vehicle = player.Vehicle;

            if (GetVehicleEngineStatus(vehicle))
            {
                SetVehicleEngineStatus(vehicle, false);
            }
            else
            {
                SetVehicleEngineStatus(vehicle, true);
            }
        }

        public static void SetVehicleEngineStatus(Vehicle vehicle, bool toggle)
        {
            try
            {
                if (vehicle == null)
                {
                    throw new Exception("Parameter `vehicle` is null");
                }

                vehicle.SetSharedData(VEHICLE_ENGINE_VAR, toggle);
                vehicle.EngineStatus = toggle;
            }
            catch (Exception ex)
            {
                Log.MethodException(ex.TargetSite, ex.Message);
            }
        }

        public static bool GetVehicleEngineStatus(Vehicle vehicle)
        {
            try
            {
                if (vehicle == null)
                {
                    throw new Exception("Parameter `vehicle` is null");
                }

                if (!vehicle.HasSharedData(VEHICLE_ENGINE_VAR))
                {
                    throw new Exception($"Vehicle doesn`t have {VEHICLE_ENGINE_VAR} var");
                }

                return vehicle.GetSharedData<bool>(VEHICLE_ENGINE_VAR);
            }
            catch (Exception ex)
            {
                Log.MethodException(ex.TargetSite, ex.Message);
                return false;
            }
        }

        public static void SetVehicleDoors(Vehicle vehicle, List<int> doors)
        {
            try
            {
                if (vehicle == null)
                {
                    throw new Exception("Parameter `vehicle` is null");
                }

                if (doors.Count != 4)
                {
                    throw new Exception("Parameter `doors` not true length");
                }

                vehicle.SetSharedData(VEHICLE_DOORS_VAR, JsonConvert.SerializeObject(doors));
            }
            catch(Exception ex)
            {
                Log.MethodException(ex.TargetSite, ex.Message);
            }
        }


        public static List<int> GetVehicleDoors(Vehicle vehicle)
        {
            try
            {
                if (vehicle == null)
                {
                    throw new Exception("Parameter `vehicle` is null");
                }

                if (!vehicle.HasSharedData(VEHICLE_DOORS_VAR))
                {
                    throw new Exception($"Vehicle doesn`t have {VEHICLE_DOORS_VAR} var");
                }

                return vehicle.GetSharedData<List<int>>(VEHICLE_DOORS_VAR);
            }
            catch (Exception ex)
            {
                Log.MethodException(ex.TargetSite, ex.Message);
                return new List<int>(6);
            }
        }
        public static void SetVehicleWheels(Vehicle vehicle, List<int> wheels)
        {
            try
            {
                if (vehicle == null)
                {
                    throw new Exception("Parameter `vehicle` is null");
                }

                if (wheels.Count != 6)
                {
                    throw new Exception("Parameter `wheels` not true length");
                }

                vehicle.SetSharedData(VEHICLE_WHEELS_VAR, JsonConvert.SerializeObject(wheels));
            }
            catch (Exception ex)
            {
                Log.MethodException(ex.TargetSite, ex.Message);
            }
        }
        public static List<int> GetVehicleWheels(Vehicle vehicle)
        {
            try
            {
                if (vehicle == null)
                {
                    throw new Exception("Parameter `vehicle` is null");
                }

                if (!vehicle.HasSharedData(VEHICLE_WHEELS_VAR))
                {
                    throw new Exception($"Vehicle doesn`t have {VEHICLE_WHEELS_VAR} var");
                }

                return vehicle.GetSharedData<List<int>>(VEHICLE_WHEELS_VAR);
            }
            catch (Exception ex)
            {
                Log.MethodException(ex.TargetSite, ex.Message);
                return new List<int>(6);
            }
        }
        public static int GetVehicleId(Vehicle vehicle)
        {
            try
            {
                if (vehicle == null)
                {
                    throw new Exception("Parameter `vehicle` is null");
                }

                if (!vehicle.HasSharedData(VEHICLE_ID_VAR))
                {
                    throw new Exception($"Vehicle doesn`t have {VEHICLE_ID_VAR} var");
                }

                return vehicle.GetSharedData<int>(VEHICLE_ID_VAR);
            }
            catch (Exception ex)
            {
                Log.MethodException(ex.TargetSite, ex.Message);
                return -1;
            }
        }


    }
}
