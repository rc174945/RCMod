using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    class EmoteTextPopup : BasePopup
    {
        const float ShowTime = 3f;
        protected override float AnimationTime => 0.25f;
        protected override PopupAnimation PopupAnimationType => PopupAnimation.Fade;
        private Text _text;
        protected Transform _parent;
        protected float _currentShowTime = 0f;
        protected bool _isHiding = false;
        protected Transform _transform;
        protected Camera _camera;
        protected virtual Vector3 offset => Vector3.up * 2.5f;

        public override void Setup(BasePanel parent = null)
        {
            _text = transform.Find("Panel/Text/Label").GetComponent<Text>();
            _transform = transform;
        }

        public void Show(string text, Transform parent)
        {
            _parent = parent;
            _currentShowTime = ShowTime;
            _isHiding = false;
            _camera = Camera.main;
            SetEmote(text);
            SetPosition();
            base.Show();
        }

        protected virtual void SetEmote(string text)
        {
            _text.text = text;
        }

        protected void SetPosition()
        {
            if (_parent != null)
            {
                Vector3 worldPosition = _parent.position + offset;
                Vector3 screenPosition = _camera.WorldToScreenPoint(worldPosition);
                _transform.position = screenPosition;
            }
        }

        protected void LateUpdate()
        {
            SetPosition();
            _currentShowTime -= Time.deltaTime;
            if (_currentShowTime <= 0f && !_isHiding)
            {
                _isHiding = true;
                Hide();
            }
        }
    }
}
