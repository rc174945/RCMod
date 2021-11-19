using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CustomSkins
{
    class CustomLevelCustomSkinLoader : LevelCustomSkinLoader
    {
        protected override string RendererIdPrefix { get { return "customlevel"; } }
        private List<GameObject> _groundObjects = new List<GameObject>();

        public override IEnumerator LoadSkinsFromRPC(object[] data)
        {
            FindAndIndexLevelObjects();
            string groundUrl = (string)data[6];
            foreach (GameObject groundObject in _groundObjects)
            {
                BaseCustomSkinPart part = GetCustomSkinPart((int)CustomLevelCustomSkinPartId.Ground, groundObject);
                if (!part.LoadCache(groundUrl))
                    yield return StartCoroutine(part.LoadSkin(groundUrl));
            }
            FengGameManagerMKII.instance.unloadAssets();
        }

        protected BaseCustomSkinPart GetCustomSkinPart(int partId, GameObject levelObject)
        {
            List<Renderer> renderers = new List<Renderer>();
            switch ((CustomLevelCustomSkinPartId)partId)
            {
                case CustomLevelCustomSkinPartId.Ground:
                    AddAllRenderers(renderers, levelObject);
                    return new BaseCustomSkinPart(this, renderers, GetRendererId(partId), MaxSizeSmall);
                default:
                    return null;
            }
        }

        protected override void FindAndIndexLevelObjects()
        {
            _groundObjects.Clear();
            foreach (GameObject levelObject in FindObjectsOfType(typeof(GameObject)))
            {
                if (levelObject != null)
                {
                    if (levelObject.name.Contains("Cube_001") && levelObject.transform.parent.gameObject.tag != "Player" && levelObject.renderer != null)
                        _groundObjects.Add(levelObject);
                }
            }
        }
    }

    public enum CustomLevelCustomSkinPartId
    {
        Ground
    }
}
