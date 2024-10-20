﻿using UnityEngine;
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

        protected virtual void Awake()
        {
            _document = GetComponent<UIDocument>();
        }

        protected T GetVisualElement<T>(string elementName) where T : VisualElement
        {
            if(string.IsNullOrEmpty(elementName) || _root == null)
            {
                return null;
            }
            return _root.Q(elementName) as T;
        }

        protected Button GetButtonElement(string templateId, string buttonId = "button")
        {
            VisualElement template = GetVisualElement<VisualElement>(templateId);
            return template.Q<Button>(buttonId);
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
