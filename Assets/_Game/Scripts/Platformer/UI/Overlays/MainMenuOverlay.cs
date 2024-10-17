using UnityEngine;
using UnityEngine.UIElements;

using TIGD.Platformer.UI.Widgets;
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

        private ButtonEventHandler _buttonEventHandler;

        protected override void Awake()
        {
            base.Awake();

            _buttonEventHandler = GetComponent<ButtonEventHandler>();

            SetVisualElements();
            RegisterButtonCallbacks();
        }

        private void OnCreditsClicked(ClickEvent evt)
        {
            PlayButtonClick();

            if(ServiceLocator.TryGet(out OverlayService overlayService))
            {
                overlayService.TryShow<CreditsOverlay>();
                overlayService.TryHide<MainMenuOverlay>();
            }
        }

        private void OnHowToPlayClicked(ClickEvent evt)
        {
            PlayButtonClick();

            if(ServiceLocator.TryGet(out OverlayService overlayService))
            {
                overlayService.TryShow<HowToPlayOverlay>();
                overlayService.TryHide<MainMenuOverlay>();
            }
        }

        private async void OnPlayGameClicked(ClickEvent evt)
        {
            PlayButtonClick();

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
            PlayButtonClick();

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
            PlayButtonClick();

            if(ServiceLocator.TryGet(out OverlayService overlayService))
            {
                overlayService.TryShow<SettingsOverlay>();
                overlayService.TryHide<MainMenuOverlay>();
            }
        }

        private void PlayButtonClick()
        {
            if(_buttonEventHandler == null)
            {
                return;
            }

            _buttonEventHandler.OnButtonClicked();
        }
        
        private void RegisterButtonCallbacks()
        {
            _playGameButton.RegisterCallback<ClickEvent>(OnPlayGameClicked);
            _howToPlayButton.RegisterCallback<ClickEvent>(OnHowToPlayClicked);
            _settingsButton.RegisterCallback<ClickEvent>(OnSettingsClicked);
            _creditsButton.RegisterCallback<ClickEvent>(OnCreditsClicked);
            _quitButton.RegisterCallback<ClickEvent>(OnQuitGameClicked);

            if(_buttonEventHandler != null)
            {
                _playGameButton.RegisterCallback<MouseEnterEvent>(evt => _buttonEventHandler.OnButtonEnter());
                _playGameButton.RegisterCallback<MouseLeaveEvent>(evt => _buttonEventHandler.OnButtonExit());

                _howToPlayButton.RegisterCallback<MouseEnterEvent>(evt => _buttonEventHandler.OnButtonEnter());
                _howToPlayButton.RegisterCallback<MouseLeaveEvent>(evt => _buttonEventHandler.OnButtonExit());

                _settingsButton.RegisterCallback<MouseEnterEvent>(evt => _buttonEventHandler.OnButtonEnter());
                _settingsButton.RegisterCallback<MouseLeaveEvent>(evt => _buttonEventHandler.OnButtonExit());

                _creditsButton.RegisterCallback<MouseEnterEvent>(evt => _buttonEventHandler.OnButtonEnter());
                _creditsButton.RegisterCallback<MouseLeaveEvent>(evt => _buttonEventHandler.OnButtonExit());

                _quitButton.RegisterCallback<MouseEnterEvent>(evt => _buttonEventHandler.OnButtonEnter());
                _quitButton.RegisterCallback<MouseLeaveEvent>(evt => _buttonEventHandler.OnButtonExit());
            }
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
