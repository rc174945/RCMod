using Utility;

namespace Map
{
    class MapScriptOptions: BaseCSVRow
    {
        protected override bool NamedParams => true;
        public string Version;
    }
}
