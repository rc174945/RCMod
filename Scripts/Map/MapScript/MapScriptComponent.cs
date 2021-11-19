using System.Collections.Generic;
using Utility;

namespace Map
{
    class MapScriptComponent: BaseCSVRowItem
    {
        public string ComponentName;
        public List<string> Parameters = new List<string>();
    }
}
