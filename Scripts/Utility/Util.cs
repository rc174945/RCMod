using Settings;
using System.Collections;
using UnityEngine;

public static class Util
{
    public static bool GetRandomBool()
    {
        return Random.Range(0f, 1f) > 0.5f;
    }

    public static float GetRandomSign()
    {
        return GetRandomBool() ? 1f : -1f;
    }

    public static Vector3 GetRandomDirection(bool flat = false)
    {
        Vector3 v = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        if (flat)
            v.y = 0f;
        return v.normalized;
    }

    public static void DebugTimeSince(float start, string prefix = "")
    {
        Debug.Log(prefix + ": " + (Time.realtimeSinceStartup - start).ToString());
    }

    public static IEnumerator WaitForFrames(int frames)
    {
        for (int i = 0; i < frames; i++)
            yield return new WaitForEndOfFrame();
    }
}
