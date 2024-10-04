using UnityEngine;

namespace TIGD.UI.Overlays
{
    [RequireComponent(typeof(CanvasGroup))]
    public class CanvasOverlay : AbstractOverlay
    {
        private CanvasGroup _canvasGroup;

        private bool _isInteractable;
        private bool _blocksRaycasts;

        protected virtual void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        
            _isInteractable = _canvasGroup.interactable;
            _blocksRaycasts = _canvasGroup.blocksRaycasts;
        }

        public override void Hide()
        {
            _canvasGroup.alpha = 0.0f;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }

        public override void Show()
        {
            _canvasGroup.alpha = 1.0f;
            _canvasGroup.interactable = _isInteractable;
            _canvasGroup.blocksRaycasts = _blocksRaycasts;
        }
    }
}