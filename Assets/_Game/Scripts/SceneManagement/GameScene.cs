using UnityEngine;
using UnityEngine.AddressableAssets;

namespace TIGD.SceneManagement
{
    [CreateAssetMenu(fileName = "GameScene", menuName = "TIGD/Scene Management/Game Scene")]
    public class GameScene : ScriptableObject
    {
        public string DisplayName;
        [TextArea] public string Description;
        public AssetReference SceneReference;
        public bool IsPersistentScene;
        public bool IsActiveScene;
    }
}
