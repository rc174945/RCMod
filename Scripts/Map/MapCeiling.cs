using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Settings;

public class MapCeiling : MonoBehaviour
{
    GameObject _barrierRef;
    Color _color;
    float _minAlpha = 0f;
    float _maxAlpha = 0.6f;
    float _minimumHeight = 3;

    static float _forestHeight = 280f;
    static float _cityHeight = 210f;
    static float _forestWidth = 1320f;
    static float _cityWidth = 1400f;
    static float _depth = 20f;

    public static void CreateMapCeiling()
    {
        if (FengGameManagerMKII.level.StartsWith("The Forest"))
            CreateMapCeilingWithDimensions(_forestHeight, _forestWidth, _depth);
        else if (FengGameManagerMKII.level.StartsWith("The City"))
            CreateMapCeilingWithDimensions(_cityHeight, _cityWidth, _depth);
    }

    static void CreateMapCeilingWithDimensions(float height, float width, float depth)
    {
        GameObject ceiling = new GameObject();
        ceiling.AddComponent<MapCeiling>();
        ceiling.transform.position = new Vector3(0f, height, 0f);
        ceiling.transform.rotation = Quaternion.identity;
        ceiling.transform.localScale = new Vector3(width, depth, width);
    }

    void Start()
    {
        CreateCeilingPart("barrier");
        _barrierRef = CreateCeilingPart("killcuboid");
        _color = new Color(1, 0, 0, _maxAlpha);
        UpdateTransparency();
    }

    GameObject CreateCeilingPart(string asset)
    {
        GameObject obj = (GameObject)Instantiate(FengGameManagerMKII.RCassets.Load(asset), Vector3.zero, Quaternion.identity);
        obj.transform.position = transform.position;
        obj.transform.rotation = transform.rotation;
        obj.transform.localScale = transform.localScale;
        return obj;
    }

    void Update()
    {
        UpdateTransparency();
    }

    float getMinAlpha()
    {
        return _minAlpha;
    }

    void setMinAlpha(float newMinAlpha)
    {
        if (newMinAlpha > 1 || newMinAlpha < 0)
        {
            throw new Exception("Error: _minAlpha must in range (0 <= _minAlpha <= 1)");
        }
        _minAlpha = newMinAlpha;
    }

    public float getMaxAlpha()
    {
        return _maxAlpha;
    }

    public void setMaxAlpha(float newMaxAlpha)
    {
        if (newMaxAlpha > 1 || newMaxAlpha < 0)
        {
            throw new Exception("Error: _minAlpha must in range (0 <= _minAlpha <= 1)");
        }
        _maxAlpha = newMaxAlpha;
    }

    public void UpdateTransparency()
    {
        if (Camera.main != null && _barrierRef != null)
        {
            if (_barrierRef.renderer != null)
            {
                float newAlpha = _maxAlpha;
                try
                {
                    float startHeight = _barrierRef.transform.position.y / _minimumHeight;
                    // convert player position between floor and ceiling to a value x, (0 <= x <= 1)
                    // given that they are above a specific height.
                    if (Camera.main.transform.position.y < startHeight)
                    {
                        newAlpha = _minAlpha;
                    }
                    else
                    {
                        newAlpha = Map(Camera.main.transform.position.y, startHeight, _barrierRef.transform.position.y, _minAlpha, _maxAlpha);
                    }

                    // use mapped player position with a function that adds exponential growth but is still bounded, (0 <= x' <= 1)
                    newAlpha = fadeByGradient(newAlpha);
                }
                catch
                {

                }
                _color.a = newAlpha;
                _barrierRef.renderer.material.color = _color;
            }
        }

    }

    public float fadeByGradient(float x)
    {
        float gradient = 10f;
        float result = gradient * x * x;
        return Mathf.Clamp(result, _minAlpha, _maxAlpha);
    }

    public float Map(float x, float inMin, float inMax, float outMin, float outMax)
    {
        if (x > inMax || x < inMin)
        {
            throw new Exception("Error,\npublic float map(float x, float inMin, float inMax, float outMin, float outMax)\nis not defined for values (x > inMax || x < inMin)");
        }
        return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }
}
