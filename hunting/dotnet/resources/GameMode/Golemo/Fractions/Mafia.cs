﻿using System.Collections.Generic;
using GTANetworkAPI;

namespace Golemo.Fractions
{
    class Mafia : Script
    {
        public static Dictionary<int, Vector3> EnterPoints = new Dictionary<int, Vector3>()
        {
            { 10, new Vector3(1392.066, 1153.032, 113.3233) },
            { 11, new Vector3(-113.4213, 985.761, 234.6341) },
            { 12, new Vector3(-1549.331, -90.05454, 53.80917) },
            { 13, new Vector3(-1805.049, 438.1696, 127.5874) },
        };
        public static Dictionary<int, Vector3> ExitPoints = new Dictionary<int, Vector3>()
        {
            { 10, new Vector3(1396.62, 1142.823, 83.24014) },
            { 11, new Vector3(-123.8163, 975.3881, 58.63158) },
            { 12, new Vector3(-1550.298, -94.81767, -193.2058) },
            { 13, new Vector3(-1812.82, 466.4906, -185.7867) },
        };

        [ServerEvent(Event.ResourceStart)]
        public void Event_ResourceStart()
        {
            foreach (var point in EnterPoints)
            {
                NAPI.Marker.CreateMarker(1, point.Value - new Vector3(0, 0, 0.7), new Vector3(), new Vector3(), 1, new Color(0, 255, 255), false, NAPI.GlobalDimension);

                var col = NAPI.ColShape.CreateCylinderColShape(point.Value, 1.2f, 2, NAPI.GlobalDimension);
                col.SetData("FRAC", point.Key);

                col.OnEntityEnterColShape += (s, e) =>
                {
                    if (!Main.Players.ContainsKey(e)) return;
                    e.SetData("FRACTIONCHECK", s.GetData<object>("FRAC"));
                    e.SetData("INTERACTIONCHECK", 64);
                };
                col.OnEntityExitColShape += (s, e) =>
                {
                    if (!Main.Players.ContainsKey(e)) return;
                    e.SetData("INTERACTIONCHECK", -1);
                };
            }

            foreach (var point in ExitPoints)
            {
                NAPI.Marker.CreateMarker(1, point.Value - new Vector3(0, 0, 0.7), new Vector3(), new Vector3(), 1, new Color(0, 255, 255), false, NAPI.GlobalDimension);

                var col = NAPI.ColShape.CreateCylinderColShape(point.Value, 1.2f, 2, NAPI.GlobalDimension);
                col.SetData("FRAC", point.Key);

                col.OnEntityEnterColShape += (s, e) =>
                {
                    if (!Main.Players.ContainsKey(e)) return;
                    e.SetData("FRACTIONCHECK", s.GetData<object>("FRAC"));
                    e.SetData("INTERACTIONCHECK", 65);
                };
                col.OnEntityExitColShape += (s, e) =>
                {
                    if (!Main.Players.ContainsKey(e)) return;
                    e.SetData("INTERACTIONCHECK", -1);
                };
            }
        }
    }
}
