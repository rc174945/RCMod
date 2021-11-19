using System.Collections.Generic;
using Utility;

namespace Map
{
    class MapScript: BaseCSVContainer
    {
        public MapScriptOptions Options;
        public List<MapScriptGameObject> Objects = new List<MapScriptGameObject>();
    }
}
