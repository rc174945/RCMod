using Settings;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utility;
using Weather;

namespace UI
{
    class EditWeatherSchedulePopup: PromptPopup
    {
        /*
        protected override string Title => "Edit schedule";
        protected override float Width => 1000f;
        protected override float Height => 800f;
        protected override int VerticalPadding => 20;
        protected override float VerticalSpacing => 10f;
        protected override bool ScrollBar => true;
        const int ElementsPerPage = 100;
        private StringSetting _setting;
        private WeatherSchedule _schedule = new WeatherSchedule();
        private List<WeatherEventUIElement> _elements = new List<WeatherEventUIElement>();
        private Text _pageLabel;
        private int _page = 0;

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            ElementFactory.CreateDefaultButton(BottomBar, "Add Event", fontSize: ButtonFontSize, onClick: () => OnButtonClick("AddEvent"));
            ElementFactory.CreateDefaultButton(BottomBar, "Add Event At", fontSize: ButtonFontSize, onClick: () => OnButtonClick("AddEventAt"));
            ElementFactory.CreateDefaultButton(BottomBar, UIManager.GetLocaleCommon("Save"), fontSize: ButtonFontSize, onClick: () => OnButtonClick("Save"));
            ElementFactory.CreateDefaultButton(BottomBar, UIManager.GetLocaleCommon("Cancel"), fontSize: ButtonFontSize, onClick: () => OnButtonClick("Cancel"));
            GameObject group = ElementFactory.CreateHorizontalGroup(TopBar, 10f, TextAnchor.UpperRight);
            ElementFactory.CreateDefaultButton(group.transform, "<", fontSize: ButtonFontSize, onClick: () => OnButtonClick("PrevPage"));
            _pageLabel = ElementFactory.CreateDefaultLabel(group.transform, _page.ToString(), fontSize: ButtonFontSize).GetComponent<Text>();
            ElementFactory.CreateDefaultButton(group.transform, ">", fontSize: ButtonFontSize, onClick: () => OnButtonClick("NextPage"));
            SinglePanel.GetComponent<LayoutElement>().preferredWidth = Width;
        }

        public void Show(StringSetting setting)
        {
            if (gameObject.activeSelf)
                return;
            base.Show();
            _setting = setting;
            _schedule.DeserializeFromCSV(setting.Value);
            _page = 0;
            LoadCurrentPage();
        }

        private void LoadCurrentPage()
        {
            int totalPages = GetTotalPages();
            if (_page < 0)
                _page = 0;
            if (_page >= totalPages)
                _page = totalPages - 1;
            _pageLabel.text = string.Format("{0}/{1}", _page + 1, totalPages);
            foreach (List<GameObject> group in _elements)
            {
                foreach (GameObject obj in group)
                    Destroy(obj);
            }
            _elements.Clear();
            _values.Clear();
            int start = _page * ElementsPerPage;
            int end = Math.Min(start + ElementsPerPage, _schedule.Events.Count);
            for(int i = 0; i < end; i++)
            {
                WeatherEvent ev = _schedule.Events[i];
                AddElement(ev, i);
            }
        }

        private int GetTotalPages()
        {
            return 1 + Mathf.FloorToInt((_schedule.Events.Count - 1) / ElementsPerPage);
        }
        
        private int GetLastPage()
        {
            return GetTotalPages() - 1;
        }

        private void AddEvent()
        {
            WeatherEvent ev = new WeatherEvent();
            _schedule.Events.Add(ev);
            if (_page == GetLastPage() && _schedule.Events.Count % ElementsPerPage != 0)
                AddElement(ev, _schedule.Events.Count - 1);
            else
            {
                _page = GetLastPage();
                LoadCurrentPage();
            }
        }

        private void AddEventAt(int index)
        {
            WeatherEvent ev = new WeatherEvent();
            index = Math.Min(index, _schedule.Events.Count);
            index = Math.Max(index, 0);
            _schedule.Events.Insert(index, ev);
            _page = Mathf.FloorToInt((index - 1) / ElementsPerPage);
            LoadCurrentPage();
        }

        private void RemoveLastEvent()
        {
            int index = _schedule.Events.Count - 1;
            _schedule.Events.RemoveAt(index);
            foreach (GameObject obj in _elements[index])
                Destroy(obj);
            _elements.RemoveAt(index);
            _values.RemoveAt(index);
            if (_page > GetLastPage())
            {
                _page = GetLastPage();
                LoadCurrentPage();
            }
        }

        public void RemoveEventAt(int index)
        {
            index = Math.Min(index, _schedule.Events.Count - 1);
            index = Math.Max(index, 0);
            if (index == _schedule.Events.Count - 1)
                RemoveLastEvent();
            else
            {
                _schedule.Events.RemoveAt(index);
                LoadCurrentPage();
            }
        }

        

        private void OnButtonClick(string name)
        {
            if (name == "Cancel")
            {
                Hide();
            }
            else if (name == "AddEvent")
            {
                AddEvent();
            }
            else if (name == "AddEventAt")
            {

            }
            else if (name == "Save")
            {
                _setting.Value = _schedule.SerializeToCSV();
                Hide();
            }
            else if (name == "PrevPage" && _page > 0)
            {
                _page -= 1;
                LoadCurrentPage();
            }
            else if (name == "NextPage" && _page < GetTotalPages() - 1)
            {
                _page += 1;
                LoadCurrentPage();
            }
        }
        */
    }
    /*

    class WeatherEventUIElement: MonoBehaviour
    {
        const int FontSize = 22;
        private IntSetting _action = new IntSetting(0);
        private IntSetting _effect = new IntSetting(0);
        private IntSetting _valueSelectType = new IntSetting(0);
        private List<BaseSetting> _values = new List<BaseSetting>();
        private List<GameObject> _elements = new List<GameObject>();
        private StringSetting _randomFromListValue = new StringSetting(string.Empty);
        private int _line;
        private EditWeatherSchedulePopup _popup;
        private WeatherEvent _event;

        public WeatherEventUIElement(EditWeatherSchedulePopup popup, Transform panel, WeatherEvent ev, int line)
        {
            _popup = popup;
            _event = ev;
            _line = line;
            _elements.Add(ElementFactory.CreateHorizontalGroup(panel, 10f, TextAnchor.UpperLeft));
            LoadEvent(ev);
        }

        public void LoadEvent(WeatherEvent ev)
        {
            _action.Value = (int)ev.Action;
            _effect.Value = (int)ev.Effect;
            _valueSelectType.Value = (int)ev.ValueSelectType;
            _values.Clear();
            foreach (object value in ev.Values)
            {
                BaseSetting setting = SettingsUtil.CreateBaseSetting(ev.GetSettingType());
                SettingsUtil.SetSettingValue(setting, ev.GetSettingType(), value);
                _values.Add(setting);
            }
            CreateElements();
        }

        private void CreateElements()
        {
            for (int i = 1; i < _elements.Count; i++)
                Destroy(_elements[i]);
            _elements.RemoveRange(1, _elements.Count - 1);
            CreateDropdownElements();
            CreateValueElements();
        }

        private void CreateDropdownElements()
        {
            _elements.Add(ElementFactory.CreateDefaultButton(_elements[0].transform, "X", fontSize: FontSize, onClick: () => _popup.RemoveEventAt(_line)));
            _elements.Add(ElementFactory.CreateDefaultLabel(_elements[0].transform, _line.ToString()));
            _elements.Add(ElementFactory.CreateDropdownSetting(_elements[0].transform, _action, string.Empty, RCextensions.EnumToStringArrayExceptNone<WeatherAction>(),
                onDropdownOptionSelect: () => OnDropdownSelect()));
            if (_event.SupportsWeatherEffects())
            {
                _elements.Add(ElementFactory.CreateDropdownSetting(_elements[0].transform, _effect, string.Empty, _event.SupportedWeatherEffects(),
                    onDropdownOptionSelect: () => OnDropdownSelect()));
            }
            if (_event.SupportsWeatherValueSelectTypes())
            {
                _elements.Add(ElementFactory.CreateDropdownSetting(_elements[0].transform, _valueSelectType, string.Empty, _event.SupportedWeatherValueSelectTypes(),
                    onDropdownOptionSelect: () => OnDropdownSelect()));
            }
        }

        private void CreateValueElements()
        {
            if (_event.SupportsWeatherValueSelectTypes())
            {
                if (_valueSelectType.Value == (int)WeatherValueSelectType.Constant)
                {
                    CreateValueElement();
                }
                else  if (_valueSelectType.Value == (int)WeatherValueSelectType.RandomBetween)
                {
                    CreateValueElement();
                    CreateValueElement();

                }
                else if (_valueSelectType.Value == (int)WeatherValueSelectType.RandomFromList)
                {
                    _randomFromListValue.Value = 
                    valueList.Add(new StringSetting());
                    List<string> strs = new List<string>();
                    foreach (StringSetting setting in ev.Values.Value)
                        strs.Add(setting.Value);
                    ((StringSetting)valueList[0]).Value = string.Join(",", strs.ToArray());
                    element.Add(ElementFactory.CreateInputSetting(element[0].transform, valueList[0], string.Empty));
                }
            }
        }

        private void CreateValueElement()
        {
            switch (ev.GetSettingType())
            {
                case SettingType.Float:
                case SettingType.Int:
                case SettingType.String:
                    element.Add(ElementFactory.CreateInputSetting(element[0].transform, valueList[valueIndex], string.Empty));
                    break;
                case SettingType.Color:
                    element.Add(ElementFactory.CreateColorSetting(element[0].transform, valueList[valueIndex], string.Empty, ((SettingsPopup)Parent).ColorPickPopup));
                    break;
            }
        }

        private void OnDropdownSelect()
        {
            
        }
    }
    */
}
