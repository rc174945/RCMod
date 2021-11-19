using System.Collections.Generic;
using Utility;

namespace Map
{
    class MapScriptGameObject: BaseCSVRow
    {
        public int Id;
        public string Name;
        public int Parent;
        public string Category;
        public string AssetName;
        public float PositionX;
        public float PositionY;
        public float PositionZ;
        public float RotationX;
        public float RotationY;
        public float RotationZ;
        public float RotationW;
        public float ScaleX;
        public float ScaleY;
        public float ScaleZ;
        public string Texture;
        public float TilingX;
        public float TilingY;
        public float ColorR;
        public float ColorG;
        public float ColorB;
        public float ColorA;
        public bool Networked;
        public List<MapScriptComponent> Components = new List<MapScriptComponent>();
    }
}
