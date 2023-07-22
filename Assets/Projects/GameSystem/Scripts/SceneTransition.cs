using System.Collections.Generic;
using Fusion;
using UnityEngine;

namespace Projects.BattleSystem.Scripts
{
    public enum SceneName
    {
        TitleScene,
        CarryScene,
        LobbyScene,
        EditMapScene,
        SearchRouteScene,
    }

    public static class SceneTransition
    {
        static readonly Dictionary<SceneName, string> sceneNameTable = new()
        {
            { SceneName.TitleScene, "TitleScene" },
            { SceneName.CarryScene, "CarryScene" },
            { SceneName.LobbyScene, "LobbyScene" },
            { SceneName.EditMapScene, "EditMapScene" },
            { SceneName.SearchRouteScene, "SearchRouteScene" },
        };

        public static void TransitioningScene(NetworkRunner runner, SceneName nextScene)
        {
            if (sceneNameTable.TryGetValue(nextScene, out var sceneName))
            {
                Debug.Log($"Transitioning to {sceneName}");
                runner.SetActiveScene(sceneName);
            }
            else
            {
                Debug.LogError($"{nextScene} is not registered in sceneNameTable.");
            }
        }
    }
}