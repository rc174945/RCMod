using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Settings;
using System.Collections;

namespace UI
{
    abstract class BaseMenu: MonoBehaviour
    {
        protected List<BasePopup> _popups = new List<BasePopup>();
        public TooltipPopup TooltipPopup;
        public MessagePopup MessagePopup;
        public ConfirmPopup ConfirmPopup;

        public virtual void Setup()
        {
            SetupPopups();
        }

        public void ApplyScale()
        {
            StartCoroutine(WaitAndApplyScale());
        }

        protected IEnumerator WaitAndApplyScale()
        {
            float scaleFactor = 1f / SettingsManager.UISettings.UIMasterScale.Value;
            GetComponent<CanvasScaler>().referenceResolution = new Vector2(1920 * scaleFactor, 1080 * scaleFactor);
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            UIManager.CurrentCanvasScale = GetComponent<RectTransform>().localScale.x;
            foreach (BaseScaler scaler in GetComponentsInChildren<BaseScaler>(includeInactive: true))
            {
                scaler.ApplyScale();
            }
        }

        protected virtual void SetupPopups()
        {
            TooltipPopup = ElementFactory.CreateTooltipPopup<TooltipPopup>(transform).GetComponent<TooltipPopup>();
            MessagePopup = ElementFactory.CreateHeadedPanel<MessagePopup>(transform).GetComponent<MessagePopup>();
            ConfirmPopup = ElementFactory.CreateHeadedPanel<ConfirmPopup>(transform).GetComponent<ConfirmPopup>();
            _popups.Add(TooltipPopup);
            _popups.Add(MessagePopup);
            _popups.Add(ConfirmPopup);
        }

        protected virtual void HideAllPopups()
        {
            foreach (BasePopup popup in _popups)
            {
                popup.Hide();
            }
        }
    }
}
