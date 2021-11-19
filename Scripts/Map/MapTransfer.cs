using System;
using System.Collections.Generic;
using UnityEngine;

namespace Map
{
    class MapTransfer: MonoBehaviour
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
