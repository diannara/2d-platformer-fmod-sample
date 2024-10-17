using UnityEngine;
using UnityEngine.SceneManagement;

using TIGD.Platformer.UI.Overlays;
using TIGD.SceneManagement;
using TIGD.SceneManagement.Services;
using TIGD.Services;
using TIGD.UI.Services;

namespace TIGD.Platformer.SceneManagement
{
    public class Bootstrapper : MonoBehaviour
    {
        private const string BOOTSTRAP_SCENE_NAME = "Bootstrap";

        [SerializeField] private GameScene[] _commonAssetsScenes;
        [SerializeField] private GameScene _mainMenuScene;

        private async void Start()
        {
            // Disable all input

            // Get the SceneService
            SceneService sceneService = ServiceLocator.Get<SceneService>();
            if(sceneService == null)
            {
                Debug.LogError("Bootstrapper :: Start() :: Failed to get SceneService!", this);
                return;
            }

            LoadingProgress progress = new LoadingProgress();

            // Load the common assets scenes
            await sceneService.LoadScenes(_commonAssetsScenes, progress);

            // display loading screen
            if(ServiceLocator.TryGet(out OverlayService overlayService))
            {
                overlayService.TryShow<LoadingOverlay>();
            }

            // Load main menu scene
            await sceneService.LoadScene(_mainMenuScene, progress);

            // Bootstrap scene is no longer needed, unload it
            await SceneManager.UnloadSceneAsync(BOOTSTRAP_SCENE_NAME);
        }


        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        public static async void Initialize()
        {
            await SceneManager.LoadSceneAsync(BOOTSTRAP_SCENE_NAME, LoadSceneMode.Single);
        }
    }
}
