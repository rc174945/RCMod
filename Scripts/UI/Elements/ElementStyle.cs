using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    class ElementStyle
    {
        public int FontSize;
        public float TitleWidth;
        public string ThemePanel;

        public static ElementStyle Default = new ElementStyle();

        public ElementStyle(int fontSize = 24, float titleWidth = 120f, string themePanel = "DefaultPanel")
        {
            FontSize = fontSize;
            TitleWidth = titleWidth;
            ThemePanel = themePanel;
        }
    }
}
