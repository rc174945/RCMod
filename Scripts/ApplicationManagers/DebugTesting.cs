using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Utility;
using System.IO;

namespace ApplicationManagers
{
    class DebugTesting : MonoBehaviour
    {
        static DebugTesting _instance;

        public static void Init()
        {
            _instance = SingletonFactory.CreateSingleton(_instance);
        }

        public static void RunTests()
        {
            if (!ApplicationConfig.DevelopmentMode)
                return;
        }

        public static void Log(object message)
        {
            Debug.Log(message);
        }

        void Update()
        {
        }

        public static void RunDebugCommand(string command)
        {
            if (!ApplicationConfig.DevelopmentMode)
            {
                Debug.Log("Debug commands are not available in release mode.");
                return;
            }
            string[] args = command.Split(' ');
            switch (args[0])
            {
                case "spawnasset":
                    string name = args[1];
                    string[] vectors = args[2].Split(',');
                    Vector3 position = new Vector3(float.Parse(vectors[0]), float.Parse(vectors[1]), float.Parse(vectors[2]));
                    string[] quats = args[3].Split(',');
                    Quaternion rot = new Quaternion(float.Parse(quats[0]), float.Parse(quats[1]), float.Parse(quats[2]), float.Parse(quats[3]));
                    Instantiate(FengGameManagerMKII.RCassets.Load(name), position, rot);
                    break;
                default:
                    Debug.Log("Invalid debug command.");
                    break;
            }
        }
    }
}
