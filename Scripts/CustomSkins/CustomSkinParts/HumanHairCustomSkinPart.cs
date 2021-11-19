using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Utility;

namespace CustomSkins
{
    class HumanHairCustomSkinPart: BaseCustomSkinPart
    {
        private CostumeHair HairInfo;

        public HumanHairCustomSkinPart(BaseCustomSkinLoader loader, List<Renderer> renderers, string rendererId, int maxSize, CostumeHair hairInfo, Vector2? textureScale = null): 
            base(loader, renderers, rendererId, maxSize, textureScale)
        {
            HairInfo = hairInfo;
        }

        protected override Material SetNewTexture(Texture2D texture)
        {
            if (HairInfo.id >= 0)
            {
                _renderers[0].material = CharacterMaterials.materials[HairInfo.texture];
            }
            return base.SetNewTexture(texture);
        }
    }
}
