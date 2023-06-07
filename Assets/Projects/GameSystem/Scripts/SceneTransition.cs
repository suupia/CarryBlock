using System.Collections.Generic;
using Fusion;
using UnityEngine;

namespace Nuts.Projects.GameSystem.Scripts
{
    public enum SceneName
    {
        TitleScene,
        GameScene,
        LobbyScene
    }

    public static class SceneTransition
    {
        static readonly Dictionary<SceneName, string> sceneNameTable = new()
        {
            { SceneName.TitleScene, "TitleScene" },
            { SceneName.GameScene, "GameScene" },
            { SceneName.LobbyScene, "LobbyScene" }
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