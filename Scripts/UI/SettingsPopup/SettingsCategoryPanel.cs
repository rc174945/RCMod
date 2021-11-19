using Settings;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    class SettingsCategoryPanel: BasePanel
    {
        protected override float Width => 1000f;
        protected override float Height => 490f;
        protected override bool DoublePanel => true;
        protected override bool DoublePanelDivider => true;
    }
}
