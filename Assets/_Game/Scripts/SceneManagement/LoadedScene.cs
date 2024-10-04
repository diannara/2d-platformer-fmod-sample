using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace TIGD.SceneManagement
{
    public struct LoadedScene
    {
        public AsyncOperationHandle<SceneInstance> Handle;

        public GameScene GameScene;
    }
}
