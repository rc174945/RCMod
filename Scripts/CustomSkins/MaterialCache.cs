using System.Collections.Generic;
using UnityEngine;

namespace CustomSkins
{
    class MaterialCache
    {
        private static Dictionary<string, Material> _IdToMaterial = new Dictionary<string, Material>();

        public static void Clear()
        {
            _IdToMaterial.Clear();
        }

        public static bool ContainsKey(string rendererId, string url)
        {
            return _IdToMaterial.ContainsKey(GetId(rendererId, url));
        }

        public static Material GetMaterial(string rendererId, string url)
        {
            return _IdToMaterial[GetId(rendererId, url)];
        }

        public static void SetMaterial(string rendererId, string url, Material material)
        {
            string id = GetId(rendererId, url);
            if (_IdToMaterial.ContainsKey(id))
                _IdToMaterial[id] = material;
            else
                _IdToMaterial.Add(id, material);
        }

        private static string GetId(string rendererId, string url)
        {
            return rendererId + "," + url;
        }
    }
}
