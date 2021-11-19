using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    class EmoteEmojiPopup : EmoteTextPopup
    {
        protected RawImage _emojiImage;
        protected override Vector3 offset => Vector3.up * 3f;

        public override void Setup(BasePanel parent = null)
        {
            _emojiImage = transform.Find("Panel/Emoji").GetComponent<RawImage>();
            _transform = transform;
        }

        protected override void SetEmote(string text)
        {
            _emojiImage.texture = GameMenu.EmojiTextures[text];
        }
    }
}
