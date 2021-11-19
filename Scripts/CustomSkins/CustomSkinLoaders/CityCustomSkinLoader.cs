using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CustomSkins
{
    class CityCustomSkinLoader : LevelCustomSkinLoader
    {
        protected override string RendererIdPrefix { get { return "city"; } }
        private List<GameObject> _houseObjects = new List<GameObject>();
        private List<GameObject> _groundObjects = new List<GameObject>();
        private List<GameObject> _wallObjects = new List<GameObject>();
        private List<GameObject> _gateObjects = new List<GameObject>();

        public override IEnumerator LoadSkinsFromRPC(object[] data)
        {
            FindAndIndexLevelObjects();
            char[] randomIndices = ((string)data[0]).ToCharArray();
            string[] houseUrls = ((string)data[1]).Split(',');
            string[] miscUrls = ((string)data[2]).Split(',');
            for (int i = 0; i < _houseObjects.Count; i++)
            {
                int randomIndex = int.Parse(randomIndices[i].ToString());
                BaseCustomSkinPart part = GetCustomSkinPart((int)CityCustomSkinPartId.House, _houseObjects[i]);
                if (!part.LoadCache(houseUrls[randomIndex]))
                    yield return StartCoroutine(part.LoadSkin(houseUrls[randomIndex]));
            }
            foreach (GameObject groundObject in _groundObjects)
            {
                BaseCustomSkinPart part = GetCustomSkinPart((int)CityCustomSkinPartId.Ground, groundObject);
                if (!part.LoadCache(miscUrls[0]))
                    yield return StartCoroutine(part.LoadSkin(miscUrls[0]));
            }
            foreach (GameObject wallObject in _wallObjects)
            {
                BaseCustomSkinPart part = GetCustomSkinPart((int)CityCustomSkinPartId.Wall, wallObject);
                if (!part.LoadCache(miscUrls[1]))
                    yield return StartCoroutine(part.LoadSkin(miscUrls[1]));

            }
            foreach (GameObject gateObject in _gateObjects)
            {
                BaseCustomSkinPart part = GetCustomSkinPart((int)CityCustomSkinPartId.Gate, gateObject);
                if (!part.LoadCache(miscUrls[2]))
                    yield return StartCoroutine(part.LoadSkin(miscUrls[2]));
            }
            FengGameManagerMKII.instance.unloadAssets();
        }

        protected BaseCustomSkinPart GetCustomSkinPart(int partId, GameObject levelObject)
        {
            List<Renderer> renderers = new List<Renderer>();
            switch ((CityCustomSkinPartId)partId)
            {
                case CityCustomSkinPartId.House:
                    AddAllRenderers(renderers, levelObject);
                    return new BaseCustomSkinPart(this, renderers, GetRendererId(partId), MaxSizeLarge);
                case CityCustomSkinPartId.Ground:
                    AddAllRenderers(renderers, levelObject);
                    return new BaseCustomSkinPart(this, renderers, GetRendererId(partId), MaxSizeSmall);
                case CityCustomSkinPartId.Wall:
                    AddAllRenderers(renderers, levelObject);
                    return new BaseCustomSkinPart(this, renderers, GetRendererId(partId), MaxSizeSmall);
                case CityCustomSkinPartId.Gate:
                    AddAllRenderers(renderers, levelObject);
                    return new BaseCustomSkinPart(this, renderers, GetRendererId(partId), MaxSizeLarge);
                default:
                    return null;
            }
        }

        protected override void FindAndIndexLevelObjects()
        {
            _houseObjects.Clear();
            _groundObjects.Clear();
            _wallObjects.Clear();
            _gateObjects.Clear();
            foreach (GameObject levelObject in FindObjectsOfType(typeof(GameObject)))
            {
                string name = levelObject.name;
                if (levelObject != null && name.Contains("Cube_") && levelObject.transform.parent.gameObject.tag != "Player")
                {
                    if (name.EndsWith("001"))
                        _groundObjects.Add(levelObject);
                    else if (name.EndsWith("006") || name.EndsWith("007") || name.EndsWith("015") || name.EndsWith("000"))
                        _wallObjects.Add(levelObject);
                    else if (name.EndsWith("002") && levelObject.transform.position == Vector3.zero)
                        _wallObjects.Add(levelObject);
                    else if (name.EndsWith("005") || name.EndsWith("003"))
                        _houseObjects.Add(levelObject);
                    else if (name.EndsWith("002") && levelObject.transform.position != Vector3.zero)
                        _houseObjects.Add(levelObject);
                    else if (name.EndsWith("019") || name.EndsWith("020"))
                        _gateObjects.Add(levelObject);
                }
            }
        }
    }

    public enum CityCustomSkinPartId
    {
        House,
        Ground,
        Wall,
        Gate
    }
}
