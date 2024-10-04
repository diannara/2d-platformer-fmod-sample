using UnityEngine;

using TIGD.Platformer.UI.Overlays;
using TIGD.SceneManagement;
using TIGD.SceneManagement.Services;
using TIGD.Services;
using TIGD.UI.Services;

namespace TIGD.Platformer.SceneManagement
{
    public class SceneEventHandler : MonoBehaviour
    {
        [SerializeField] private GameScene _currentScene;

        private void OnDisable()
        {
            if(ServiceLocator.TryGet(out SceneService sceneService))
            {
                sceneService.OnSceneLoaded -= SceneService_OnSceneLoaded;
            }
        }

        private void OnEnable()
        {
            if(ServiceLocator.TryGet(out SceneService sceneService))
            {
                sceneService.OnSceneLoaded += SceneService_OnSceneLoaded;
            }
        }

        private void SceneService_OnSceneLoaded(string sceneName)
        {
            if(!sceneName.Equals(_currentScene.DisplayName))
            {
                return;
            }

            if(ServiceLocator.TryGet(out OverlayService overlayService))
            {
                overlayService.TryHide<LoadingOverlay>();
                overlayService.TryShow<MainMenuOverlay>();
            }
        }
    }
}
