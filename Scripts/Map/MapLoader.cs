using System;
using System.Collections.Generic;
using UnityEngine;

namespace Map
{
    class MapLoader: MonoBehaviour
    {
        public static void LoadObjects(List<MapScriptGameObject> objects)
        {
            foreach (MapScriptGameObject obj in objects)
            {
                // load obj into game
            }
        }
    }
}
