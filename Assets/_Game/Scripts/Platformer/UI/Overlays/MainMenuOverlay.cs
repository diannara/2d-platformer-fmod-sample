using UnityEngine;
using UnityEngine.UIElements;

using TIGD.SceneManagement;
using TIGD.SceneManagement.Services;
using TIGD.Services;
using TIGD.UI.Overlays;
using TIGD.UI.Services;

namespace TIGD.Platformer.UI.Overlays
{
    public class MainMenuOverlay : VisualElementOverlay
    {
        [SerializeField] private GameScene[] _gameplayScenes;

        private const string PlayGameButtonTemplateID = "play-game-button";
        private const string HowToPlayButtonTemplateID = "how-to-play-button";
        private const string SettingsButtonTemplateID = "settings-button";
        private const string CreditsButtonTemplateID = "credits-button";
        private const string QuitButtonTemplateID = "quit-button";

        private Button _playGameButton;
        private Button _howToPlayButton;
        private Button _settingsButton;
        private Button _creditsButton;
        private Button _quitButton;

        protected override void Awake()
        {
            base.Awake();
            SetVisualElements();
            RegisterButtonCallbacks();
        }

        private void OnCreditsClicked(ClickEvent evt)
        {
            Debug.Log("Credits clicked");
        }

        private void OnHowToPlayClicked(ClickEvent evt)
        {
            Debug.Log("How to play clicked");
        }

        private async void OnPlayGameClicked(ClickEvent evt)
        {
            if(ServiceLocator.TryGet(out OverlayService overlayService))
            {
                overlayService.TryShow<LoadingOverlay>();
                overlayService.TryHide<MainMenuOverlay>();
            }

            if(ServiceLocator.TryGet(out SceneService sceneService))
            {
                LoadingProgress progress = new LoadingProgress();
                await sceneService.LoadScenes(_gameplayScenes, progress);
            }
        }

        private void OnQuitGameClicked(ClickEvent evt)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBGL
            Application.OpenURL("about:blank");
#else
            Application.Quit();
#endif
        }
        
        private void OnSettingsClicked(ClickEvent evt)
        {
            Debug.Log("Settings clicked");
        }
        
        private void RegisterButtonCallbacks()
        {
            _playGameButton.RegisterCallback<ClickEvent>(OnPlayGameClicked);
            _howToPlayButton.RegisterCallback<ClickEvent>(OnHowToPlayClicked);
            _settingsButton.RegisterCallback<ClickEvent>(OnSettingsClicked);
            _creditsButton.RegisterCallback<ClickEvent>(OnCreditsClicked);
            _quitButton.RegisterCallback<ClickEvent>(OnQuitGameClicked);
        }

        private Button GetButtonElement(string templateId, string buttonId = "button")
        {
            VisualElement template = GetVisualElement<VisualElement>(templateId);
            return template.Q<Button>(buttonId);
        }

        private void SetVisualElements()
        {
            _playGameButton = GetButtonElement(PlayGameButtonTemplateID);
            _howToPlayButton = GetButtonElement(HowToPlayButtonTemplateID);
            _settingsButton = GetButtonElement(SettingsButtonTemplateID);
            _creditsButton = GetButtonElement(CreditsButtonTemplateID);
            _quitButton = GetButtonElement(QuitButtonTemplateID);
        }
    }
}
