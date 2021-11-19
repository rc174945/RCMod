using System;
using System.Collections.Generic;
using System.Reflection;

namespace Utility
{
    class BaseCSVObject
    {
        protected virtual char Delimiter => ',';
        protected virtual char ParamDelimiter => ':';
        protected virtual bool NamedParams => false;
        private static Dictionary<Type, FieldInfo[]> _fields = new Dictionary<Type, FieldInfo[]>();

        public virtual string Serialize()
        {
            List<string> items = new List<string>();
            FieldInfo[] fields = GetFields();
            for (int i = 0; i < fields.Length; i++)
            {
                string item = SerializeField(fields[i], this);
                items.Add(item);
            }
            return string.Join(Delimiter.ToString(), items.ToArray());
        }

        public virtual void Deserialize(string csv)
        {
            string[] items = csv.Split(Delimiter);
            FieldInfo[] fields = GetFields();
            for (int i = 0; i < items.Length; i++)
                items[i] = items[i].Trim();
            if (NamedParams)
            {
                foreach (string item in items)
                {
                    string[] paramItems = item.Split(ParamDelimiter);
                    FieldInfo field = FindField(paramItems[0]);
                    if (field != null)
                        DeserializeField(field, this, paramItems[1]);
                }
            }
            else
            {
                for (int i = 0; i < fields.Length; i++)
                {
                    if (IsList(fields[i]))
                    {
                        Type t = fields[i].FieldType.GetGenericArguments()[0];
                        List<object> list = (List<object>)fields[i].GetValue(this);
                        list.Clear();
                        for (int j = i; j < items.Length; i++)
                            list.Add(DeserializeValue(t, items[j]));
                        break;
                    }
                    else
                        DeserializeField(fields[i], this, items[i]);
                }
            }
        }

        protected virtual FieldInfo[] GetFields()
        {
            Type t = GetType();
            if (!_fields.ContainsKey(t))
                _fields.Add(t, t.GetFields());
            return _fields[t];
        }

        protected virtual FieldInfo FindField(string name)
        {
            foreach (FieldInfo info in _fields[GetType()])
            {
                if (info.Name == name)
                    return info;
            }
            return null;
        }

        protected virtual bool IsList(FieldInfo field)
        {
            return field.FieldType.IsGenericType && field.FieldType.GetGenericTypeDefinition() == typeof(IList<>);
        }

        protected virtual string SerializeField(FieldInfo info, object instance)
        {
            string str = string.Empty;
            if (NamedParams)
                str = info.Name + ParamDelimiter;
            if (IsList(info))
            {
                List<string> list = new List<string>();
                Type t = info.FieldType.GetGenericArguments()[0];
                foreach (object obj in (List<object>)info.GetValue(instance))
                    list.Add(SerializeValue(t, obj));
                str += string.Join(Delimiter.ToString(), list.ToArray());
            }
            else
                str += SerializeValue(info.FieldType, info.GetValue(instance));
            return str;
        }

        protected virtual void DeserializeField(FieldInfo info, object instance, string value)
        {
            info.SetValue(instance, DeserializeValue(info.FieldType, value));
        }

        protected virtual string SerializeValue(Type t, object value)
        {
            if (t == typeof(string))
                return (string)value;
            else if (t == typeof(int) || t == typeof(float))
                return value.ToString();
            else if (t == typeof(bool))
                return Convert.ToInt32(value).ToString();
            else if (typeof(BaseCSVObject).IsAssignableFrom(t))
                return ((BaseCSVObject)value).Serialize();
            return string.Empty;
        }

        protected virtual object DeserializeValue(Type t, string value)
        {
            if (t == typeof(string))
                return value;
            else if (t == typeof(int))
                return int.Parse(value);
            else if (t == typeof(float))
                return float.Parse(value);
            else if (t == typeof(bool))
                return Convert.ToBoolean(int.Parse(value));
            else if (typeof(BaseCSVObject).IsAssignableFrom(t))
            {
                BaseCSVObject item = (BaseCSVObject)Activator.CreateInstance(t);
                item.Deserialize(value);
                return item;
            }
            return null;
        }
    }
}
