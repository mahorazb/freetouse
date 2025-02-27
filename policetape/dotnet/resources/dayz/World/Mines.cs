using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;

namespace dayz.World
{
    internal class Mines : Script
    {
        public static Dictionary<int, Mine> MineSpawned = new Dictionary<int, Mine>();

        public static List<Vector3> MinePositions = new List<Vector3>()
        {
            new Vector3(-1564.2853, 2771.2112, 16.41528),
            new Vector3(-1567.3147, 2767.887, 16.40017),
        };

       

        [ServerEvent(Event.ResourceStart)]
        public static void ResourceStart()
        {
            foreach(Vector3 pos in MinePositions)
            {
                new Mine(1, pos);
            }
        }

        public class Mine 
        {
            public int Id { get; }
            public int Type { get; }
            public Vector3 Position { get; }

            private GTANetworkAPI.Object Object;
            private ColShape Shape;

            public Mine(int type, Vector3 pos)
            {
                Id = MineSpawned.Count;
                Type = type;
                Position = pos;

                Object = NAPI.Object.CreateObject(NAPI.Util.GetHashKey("w_ex_vehiclemine"), pos, new Vector3());

                Shape = NAPI.ColShape.CreateCylinderColShape(pos, 1.4f, 1);
                Shape.OnEntityEnterColShape += (shape, entity) =>
                {
                    Explode();

                };

                MineSpawned.Add(Id, this);
              
            }

            public void Explode()
            {
                NAPI.ClientEvent.TriggerClientEventInRange(Position, 250f, "client:world:explosion", Position.X, Position.Y, Position.Z);

                NAPI.Task.Run(() => {
                    Object.Delete();
                    Shape.Delete();

                    MineSpawned.Remove(Id);
                });
            }
        }
    }
}
