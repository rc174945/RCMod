using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CustomSkins
{
    class ForestCustomSkinLoader : LevelCustomSkinLoader
    {
        protected override string RendererIdPrefix { get { return "forest"; } }
        private List<GameObject> _treeObjects = new List<GameObject>();
        private List<GameObject> _groundObjects = new List<GameObject>();

        public override IEnumerator LoadSkinsFromRPC(object[] data)
        {
            FindAndIndexLevelObjects();
            char[] randomIndices = ((string)data[0]).ToCharArray();
            int[] trunkRandomIndices = SplitRandomIndices(randomIndices, 0);
            int[] leafRandomIndices = SplitRandomIndices(randomIndices, 1);
            string[] trunkUrls = ((string)data[1]).Split(',');
            string[] leafUrls = ((string)data[2]).Split(',');
            string groundUrl = leafUrls[8];
            for (int i = 0; i < _treeObjects.Count; i++)
            {
                int trunkRandomIndex = trunkRandomIndices[i];
                int leafRandomIndex = leafRandomIndices[i];
                string trunkUrl = trunkUrls[trunkRandomIndex];
                string leafUrl = leafUrls[leafRandomIndex];
                BaseCustomSkinPart trunkPart = GetCustomSkinPart((int)ForestCustomSkinPartId.TreeTrunk, _treeObjects[i]);
                BaseCustomSkinPart leafPart = GetCustomSkinPart((int)ForestCustomSkinPartId.TreeLeaf, _treeObjects[i]);
                if (!trunkPart.LoadCache(trunkUrl))
                    yield return StartCoroutine(trunkPart.LoadSkin(trunkUrl));
                if (!leafPart.LoadCache(leafUrl))
                    yield return StartCoroutine(leafPart.LoadSkin(leafUrl));
            }
            foreach (GameObject groundObject in _groundObjects)
            {
                BaseCustomSkinPart part = GetCustomSkinPart((int)ForestCustomSkinPartId.Ground, groundObject);
                if (!part.LoadCache(groundUrl))
                    yield return StartCoroutine(part.LoadSkin(groundUrl));
            }
            FengGameManagerMKII.instance.unloadAssets();
        }

        protected BaseCustomSkinPart GetCustomSkinPart(int partId, GameObject levelObject)
        {
            List<Renderer> renderers = new List<Renderer>();
            switch ((ForestCustomSkinPartId)partId)
            {
                case ForestCustomSkinPartId.TreeTrunk:
                    AddRenderersContainingName(renderers, levelObject, "Cube");
                    return new BaseCustomSkinPart(this, renderers, GetRendererId(partId), MaxSizeLarge);
                case ForestCustomSkinPartId.TreeLeaf:
                    AddRenderersContainingName(renderers, levelObject, "Plane_031");
                    return new BaseCustomSkinPart(this, renderers, GetRendererId(partId), MaxSizeSmall);
                case ForestCustomSkinPartId.Ground:
                    AddAllRenderers(renderers, levelObject);
                    return new BaseCustomSkinPart(this, renderers, GetRendererId(partId), MaxSizeSmall);
                default:
                    return null;
            }
        }

        protected override void FindAndIndexLevelObjects()
        {
            _treeObjects.Clear();
            _groundObjects.Clear();
            foreach (GameObject levelObject in FindObjectsOfType(typeof(GameObject)))
            {
                if (levelObject != null)
                {
                    if (levelObject.name.Contains("TREE"))
                        _treeObjects.Add(levelObject);
                    else if (levelObject.name.Contains("Cube_001") && levelObject.transform.parent.gameObject.tag != "Player")
                        _groundObjects.Add(levelObject);
                }
            }
        }

        private int[] SplitRandomIndices(char[] randomIndices, int offset)
        {
            List<int> result = new List<int>();
            for (int i = offset; i < randomIndices.Length; i += 2)
            {
                if (i < randomIndices.Length)
                    result.Add(int.Parse(randomIndices[i].ToString()));
            }
            return result.ToArray<int>();
        }
    }

    public enum ForestCustomSkinPartId
    {
        TreeTrunk,
        TreeLeaf,
        Ground
    }
}
