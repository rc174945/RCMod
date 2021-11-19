using Settings;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

public static class RCextensions
{
    public static void Add<T>(ref T[] source, T value)
    {
        T[] localArray = new T[source.Length + 1];
        for (int i = 0; i < source.Length; i++)
        {
            localArray[i] = source[i];
        }
        localArray[localArray.Length - 1] = value;
        source = localArray;
    }

    public static bool IsNullOrEmpty(this string value)
    {
        return value == null || value.Length == 0;
    }

    public static string UpperFirstLetter(this string text)
    {
        if (text == string.Empty)
            return text;
        if (text.Length > 1)
            return char.ToUpper(text[0]) + text.Substring(1);
        return text.ToUpper();
    }

    public static string StripHex(this string text)
    {
        if (text.IsNullOrEmpty())
        {
            return text;
        }
        text = text.Replace("[-]", string.Empty);
        if (text.IsNullOrEmpty() || text.Length < 8)
        {
            return text;
        }
        if (text.Length > 20)
        {
            int num;
            if (text[7] == ']' && int.TryParse(text.Substring(1, 2), System.Globalization.NumberStyles.HexNumber, null, out num) && int.TryParse(text.Substring(3, 2), System.Globalization.NumberStyles.HexNumber, null, out num) && int.TryParse(text.Substring(5, 2), System.Globalization.NumberStyles.HexNumber, null, out num) && ((text = text.Remove(0, 8)).IsNullOrEmpty() || text.Length < 8))
            {
                return text;
            }
            for (int i = text.IndexOf("[", 0, text.Length, System.StringComparison.CurrentCulture); i >= 0; i = text.IndexOf("[", i, text.Length - i, System.StringComparison.CurrentCulture))
            {
                if (i >= text.Length)
                {
                    break;
                }
                if (text.Substring(i).Length > 7 && text[i + 7] == ']' && int.TryParse(text.Substring(i + 1, 2), System.Globalization.NumberStyles.HexNumber, null, out num) && int.TryParse(text.Substring(i + 3, 2), System.Globalization.NumberStyles.HexNumber, null, out num) && int.TryParse(text.Substring(i + 5, 2), System.Globalization.NumberStyles.HexNumber, null, out num))
                {
                    if ((text = text.Remove(i, 8)).IsNullOrEmpty() || text.Length < 8 || i >= text.Length)
                    {
                        return text;
                    }
                }
                else if (++i < 0 || i >= text.Length || text.Length < 8)
                {
                    return text;
                }
            }
        }
        else
        {
            int num2 = text.Length + 1;
            for (int i = 0; i < text.Length; i++)
            {
                num2--;
                int num;
                if (text[i] == '[' && num2 > 7 && text[i + 7] == ']' && int.TryParse(text.Substring(i + 1, 2), System.Globalization.NumberStyles.HexNumber, null, out num) && int.TryParse(text.Substring(i + 3, 2), System.Globalization.NumberStyles.HexNumber, null, out num) && int.TryParse(text.Substring(i + 5, 2), System.Globalization.NumberStyles.HexNumber, null, out num))
                {
                    text = text.Remove(i, 8);
                    num2 -= 7;
                    i--;
                }
            }
        }
        return text;
    }



    public static string hexColor(this string text)
    {
        if (text.Contains("]"))
        {
            text = text.Replace("]", ">");
        }
        bool flag2 = false;
        while (text.Contains("[") && !flag2)
        {
            int index = text.IndexOf("[");
            if (text.Length >= (index + 7))
            {
                string str = text.Substring(index + 1, 6);
                text = text.Remove(index, 7).Insert(index, "<color=#" + str);
                int length = text.Length;
                if (text.Contains("["))
                {
                    length = text.IndexOf("[");
                }
                text = text.Insert(length, "</color>");
            }
            else
            {
                flag2 = true;
            }
        }
        if (flag2)
        {
            return string.Empty;
        }
        return text;
    }

    public static bool isLowestID(this PhotonPlayer player)
    {
        foreach (PhotonPlayer player2 in PhotonNetwork.playerList)
        {
            if (player2.ID < player.ID)
            {
                return false;
            }
        }
        return true;
    }

    public static void RemoveAt<T>(ref T[] source, int index)
    {
        if (source.Length == 1)
        {
            source = new T[0];
        }
        else if (source.Length > 1)
        {
            T[] localArray = new T[source.Length - 1];
            int num = 0;
            int num2 = 0;
            while (num < source.Length)
            {
                if (num != index)
                {
                    localArray[num2] = source[num];
                    num2++;
                }
                num++;
            }
            source = localArray;
        }
    }

    public static bool returnBoolFromObject(object obj)
    {
        return (((obj != null) && (obj is bool)) && ((bool)obj));
    }

    public static float returnFloatFromObject(object obj)
    {
        if ((obj != null) && (obj is float))
        {
            return (float)obj;
        }
        return 0f;
    }

    public static int returnIntFromObject(object obj)
    {
        if ((obj != null) && (obj is int))
        {
            return (int)obj;
        }
        return 0;
    }

    public static string returnStringFromObject(object obj)
    {
        if (obj != null)
        {
            string str = obj as string;
            if (str != null)
            {
                return str;
            }
        }
        return string.Empty;
    }

    public static T ToEnum<T>(this string value, bool ignoreCase = true)
    {
        if (Enum.IsDefined(typeof(T), value))
            return (T)Enum.Parse(typeof(T), value, ignoreCase);
        return default(T);
    }

    public static string[] EnumToStringArray<T>()
    {
        return Enum.GetNames(typeof(T));
    }

    public static string[] EnumToStringArrayExceptNone<T>()
    {
        List<string> names = new List<string>();
        foreach (string str in EnumToStringArray<T>())
        {
            if (str != "None")
                names.Add(str);
        }
        return names.ToArray();
    }

    public static List<T> EnumToList<T>()
    {
        return Enum.GetValues(typeof(T)).Cast<T>().ToList();
    }

    public static Dictionary<string, T> EnumToDict<T>()
    {
        Dictionary<string, T> dict = new Dictionary<string, T>();
        foreach (T t in EnumToList<T>())
        {
            dict.Add(t.ToString(), t);
        }
        return dict;
    }

    public static float ParseFloat(string str)
    {
        return float.Parse(str, CultureInfo.InvariantCulture);
    }

    public static bool IsGray(this Color color)
    {
        return color.r == color.g && color.r == color.b && color.a == 1f;
    }

    public static HERO GetMyHero()
    {
        foreach (HERO hero in FengGameManagerMKII.instance.getPlayers())
        {
            if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || hero.photonView.isMine)
                return hero;
        }
        return null;
    }
}
