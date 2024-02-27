using System.Collections.Generic;
using Fusion;
using UnityEngine;

namespace Carry.GameSystem.Scripts
{
    public enum SceneName
    {
        // PhotonFusionを使用する用
        TitleScene,
        CarryScene,
        LobbyScene,
        EditMapScene,
        SearchRouteScene,
        
        // 完全ローカル用
        TitleSceneLocal,
        CarrySceneLocal,
        LobbySceneLocal,
        EditMapSceneLocal,
        SearchRouteSceneLocal,
    }

    public static class SceneTransition
    {
        static readonly Dictionary<SceneName, string> SeneNameTable = new()
        {
            // PhotonFusionを使用する用
            { SceneName.TitleScene, "TitleScene" },
            { SceneName.CarryScene, "CarryScene" },
            { SceneName.LobbyScene, "LobbyScene" },
            { SceneName.EditMapScene, "EditMapScene" },
            { SceneName.SearchRouteScene, "SearchRouteScene" },
            
            // 完全ローカル用
            { SceneName.TitleSceneLocal, "TitleSceneLocal" },
            { SceneName.CarrySceneLocal, "CarrySceneLocal" },
            { SceneName.LobbySceneLocal, "LobbySceneLocal" },
            { SceneName.EditMapSceneLocal, "EditMapSceneLocal" },
            { SceneName.SearchRouteSceneLocal, "SearchRouteSceneLocal" },
        };

        public static void TransitionSceneWithNetworkRunner(NetworkRunner runner, SceneName nextScene)
        {
            if (SeneNameTable.TryGetValue(nextScene, out var sceneName))
            {
                Debug.Log($"Transitioning to {sceneName}");
                runner.SetActiveScene(sceneName);
            }
            else
            {
                Debug.LogError($"{nextScene} is not registered in sceneNameTable.");
            }
        }
        
        public static void TransitioningScene(SceneName nextScene)
        {
            if (SeneNameTable.TryGetValue(nextScene, out var sceneName))
            {
                Debug.Log($"Transitioning to {sceneName}");
                UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
            }
            else
            {
                Debug.LogError($"{nextScene} is not registered in sceneNameTable.");
            }
        }
    }
}