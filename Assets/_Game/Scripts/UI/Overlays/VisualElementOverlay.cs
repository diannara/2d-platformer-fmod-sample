using UnityEngine;
using UnityEngine.UIElements;

namespace TIGD.UI.Overlays
{
    [RequireComponent(typeof(UIDocument))]
    public class VisualElementOverlay : AbstractOverlay
    {
        private const string SHOW_CLASS = "show";
        private const string HIDE_CLASS = "hide";

        private UIDocument _document;

        private VisualElement _root => _document.rootVisualElement;

        private void Awake()
        {
            _document = GetComponent<UIDocument>();
        }

        public override void Hide()
        {
            _root.RemoveFromClassList(SHOW_CLASS);
            _root.AddToClassList(HIDE_CLASS);
        }

        public override void Show()
        {
            _root.RemoveFromClassList(HIDE_CLASS);
            _root.AddToClassList(SHOW_CLASS);
        }
    }
}
