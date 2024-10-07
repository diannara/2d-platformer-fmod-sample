using UnityEngine;

using TIGD.SceneManagement;
using TIGD.SceneManagement.Services;
using TIGD.Services;

namespace TIGD.Platformer.SceneManagement
{
    public abstract class SceneEventHandler : MonoBehaviour
    {
        [SerializeField] protected GameScene _currentScene;

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

        public abstract void SceneService_OnSceneLoaded(string sceneName);
    }
}
