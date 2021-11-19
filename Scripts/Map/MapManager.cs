using System;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace Map
{
    class MapManager: MonoBehaviour
    {
        private static MapManager _instance;

        public static void Init()
        {
            _instance = SingletonFactory.CreateSingleton(_instance);
        }

        public static void LoadObjects(List<MapScriptGameObject> objects)
        {
            foreach (MapScriptGameObject obj in objects)
            {
                // load obj into game
            }
        }
    }
}
