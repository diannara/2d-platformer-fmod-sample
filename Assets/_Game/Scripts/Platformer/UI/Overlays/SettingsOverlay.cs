using UnityEngine.UIElements;

using TIGD.Services;
using TIGD.UI.Overlays;
using TIGD.UI.Services;

namespace TIGD.Platformer.UI.Overlays
{
    public class SettingsOverlay : VisualElementOverlay
    {
        private const string BackButtonTemplateID = "back-button";

        private Button _backButton;

        protected override void Awake()
        {
            base.Awake();
            SetVisualElements();
            RegisterButtonCallbacks();
        }

        private void OnBackButtonClicked(ClickEvent evt)
        {
            if(ServiceLocator.TryGet(out OverlayService overlayService))
            {
                overlayService.TryShow<MainMenuOverlay>();
                overlayService.TryHide<SettingsOverlay>();
            }
        }

        private void RegisterButtonCallbacks()
        {
            _backButton.RegisterCallback<ClickEvent>(OnBackButtonClicked);
        }

        private void SetVisualElements()
        {
            _backButton = GetButtonElement(BackButtonTemplateID);
        }
    }
}
