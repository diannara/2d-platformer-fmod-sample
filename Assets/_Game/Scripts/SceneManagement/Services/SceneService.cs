using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

using TIGD.Services;

namespace TIGD.SceneManagement.Services
{
    public class SceneService : AbstractService
    {
        private const string GAME_SCENES_LABEL = "GameScenes";

        public event Action<string> OnSceneLoaded = delegate { };
        public event Action<string> OnSceneUnloaded = delegate { };

        private Dictionary<string, GameScene> _gameScenes = new Dictionary<string, GameScene>();
        private Dictionary<string, LoadedScene> _loadedScenes = new Dictionary<string, LoadedScene>();

        private List<AsyncOperationHandle> _gameSceneHandles = new List<AsyncOperationHandle>();

        public bool AsyncIsDone()
        {
            return _loadedScenes.Count == 0 || _loadedScenes.All(x => x.Value.Handle.IsDone);
        }

        public override void Initialize()
        {
            base.Initialize();
            InitializeGameScenes();
        }

        private void InitializeGameScenes()
        {
            AsyncOperationHandle<IList<GameScene>> handle = Addressables.LoadAssetsAsync<GameScene>(GAME_SCENES_LABEL, null);
            _gameSceneHandles.Add(handle);

            handle.Completed += (op) =>
            {
                foreach (GameScene gameScene in op.Result)
                {
                    if(_gameScenes.ContainsKey(gameScene.SceneReference.AssetGUID))
                    { 
                        continue;
                    }
                    _gameScenes.Add(gameScene.SceneReference.AssetGUID, gameScene);
                }
            };
        }

        public GameScene GetSceneByGUID(string guid)
        {
            if(_gameScenes.ContainsKey(guid))
            {
                return _gameScenes[guid];
            }
            return null;
        }

        public float GetProgress()
        {
            if(_loadedScenes.Count == 0)
            {
                return 0.0f;
            }
            return _loadedScenes.Values.Average(x => x.Handle.PercentComplete);
        }

        public async Awaitable LoadScene(GameScene scene, IProgress<float> progress, LoadSceneMode sceneMode = LoadSceneMode.Additive)
        {
            await UnloadScenes(progress);

            if(_loadedScenes.ContainsKey(scene.SceneReference.AssetGUID))
            {
                Debug.LogWarning($"SceneService :: LoadScene() :: Scene is already loaded: {scene.SceneReference.AssetGUID}.");
                return;
            }

            AsyncOperationHandle<SceneInstance> handle = scene.SceneReference.LoadSceneAsync(sceneMode);
            handle.Completed += (op) =>
            {
                LoadedScene loadedScene = new LoadedScene
                {
                    Handle = handle,
                    GameScene = scene
                };

                _loadedScenes.Add(scene.SceneReference.AssetGUID, loadedScene);

                if(loadedScene.GameScene.IsActiveScene)
                {
                    SceneManager.SetActiveScene(op.Result.Scene);
                }

                OnSceneLoaded.Invoke(op.Result.Scene.name);
            };

            while(!handle.IsDone)
            {
                progress?.Report(handle.PercentComplete);
                await Awaitable.WaitForSecondsAsync(0.1f);
            }
        }

        public async Awaitable LoadScenes(GameScene[] scenes, IProgress<float> progress)
        {
            await UnloadScenes(progress);

            foreach(GameScene scene in scenes)
            {
                if(_loadedScenes.ContainsKey(scene.SceneReference.AssetGUID))
                {
                    Debug.LogWarning($"SceneService :: LoadScenes() :: Scene is already loaded: {scene.SceneReference.AssetGUID}.");
                    continue;
                }

                AsyncOperationHandle<SceneInstance> handle = scene.SceneReference.LoadSceneAsync(LoadSceneMode.Additive);
                handle.Completed += (op) =>
                {
                    LoadedScene loadedScene = new LoadedScene
                    {
                        Handle = handle,
                        GameScene = scene
                    };

                    _loadedScenes.Add(scene.SceneReference.AssetGUID, loadedScene);

                    if(loadedScene.GameScene.IsActiveScene)
                    {
                        SceneManager.SetActiveScene(op.Result.Scene);
                    }

                    OnSceneLoaded.Invoke(op.Result.Scene.name);
                };
            }

            while(!AsyncIsDone())
            {
                progress?.Report(GetProgress());
                await Awaitable.WaitForSecondsAsync(0.1f);
            }
        }

        public override void Shutdown()
        {
            base.Shutdown();

            foreach(AsyncOperationHandle handle in _gameSceneHandles)
            {
                Addressables.Release(handle);
            }

            _gameSceneHandles.Clear();
            _gameScenes.Clear();
        }

        public bool TryGetSceneByName(string sceneName, out GameScene gameScene)
        {
            if(_gameScenes.TryGetValue(sceneName, out gameScene))
            {
                return true;
            }
            return false;
        }

        public async Awaitable UnloadScene(GameScene scene, bool force = false)
        {
            if(_loadedScenes.TryGetValue(scene.SceneReference.AssetGUID, out LoadedScene loadedScene))
            {
                if(!force && loadedScene.GameScene.IsPersistentScene)
                {
                    Debug.LogWarning($"SceneService :: UnloadScene() :: Scene is persistent and cannot be unloaded: {loadedScene.GameScene.SceneReference.AssetGUID}.");
                    return;
                }

                AsyncOperationHandle<SceneInstance> unloadHandle = Addressables.UnloadSceneAsync(loadedScene.Handle, false);
                unloadHandle.Completed += (op) =>
                {
                    OnSceneUnloaded.Invoke(op.Result.Scene.name);
                };

                while(!unloadHandle.IsDone)
                {
                    await Awaitable.WaitForSecondsAsync(0.1f);
                }

                _loadedScenes.Remove(scene.SceneReference.AssetGUID);

                Addressables.Release(loadedScene.Handle);
            }
        }

        public async Awaitable UnloadScenes(IProgress<float> progress, bool force = false, bool unloadUnusedResources = false)
        {
            List<AsyncOperationHandle> unloadHandles = new List<AsyncOperationHandle>();

            foreach(LoadedScene loadedScene in _loadedScenes.Values)
            {
                if(!loadedScene.Handle.IsValid())
                {
                    Debug.LogWarning($"SceneService :: UnloadScenes() :: Scene handle is not valid for: {loadedScene.GameScene.name}.");
                    continue;
                }

                string sceneName = loadedScene.Handle.Result.Scene.name;
                if(!force && loadedScene.GameScene.IsPersistentScene)
                {
                    Debug.LogWarning($"SceneService :: UnloadScene() :: Scene is persistent and cannot be unloaded: {loadedScene.GameScene.DisplayName} :: {sceneName}.");
                    continue;
                }
                
                AsyncOperationHandle<SceneInstance> unloadHandle = Addressables.UnloadSceneAsync(loadedScene.Handle, false);
                unloadHandles.Add(unloadHandle);

                unloadHandle.Completed += (op) =>
                {
                    _loadedScenes.Remove(loadedScene.GameScene.SceneReference.AssetGUID);
                    OnSceneUnloaded.Invoke(op.Result.Scene.name);
                };
            }

            bool isDone = unloadHandles.Count == 0 || unloadHandles.All(x => x.IsDone);
            while(!isDone)
            {
                progress?.Report(unloadHandles.Average(x => x.PercentComplete));
                await Awaitable.WaitForSecondsAsync(0.1f);

                isDone = unloadHandles.Count == 0 || unloadHandles.All(x => x.IsDone);
            }

            foreach(AsyncOperationHandle handle in unloadHandles)
            {
                Addressables.Release(handle);
            }
            unloadHandles.Clear();

            //Optional: Unloaded Unused Assets
            if(unloadUnusedResources)
            {
                await Resources.UnloadUnusedAssets();
            }
        }
    }
}
