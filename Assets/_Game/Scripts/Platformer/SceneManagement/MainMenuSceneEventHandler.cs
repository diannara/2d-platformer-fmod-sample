using TIGD.Platformer.UI.Overlays;
using TIGD.Services;
using TIGD.UI.Services;

namespace TIGD.Platformer.SceneManagement
{
    public class MainMenuSceneEventHandler : SceneEventHandler
    {
        public override void SceneService_OnSceneLoaded(string sceneName)
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
