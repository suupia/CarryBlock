using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using  UnityEngine.SceneManagement;
using System;
using Fusion;

public enum SceneName 
{
    TitleScene,
    GameScene,
    LobbyScene,
}

public static class SceneTransition
{
    static Dictionary<SceneName, string> sceneNameTable = new ()
    {
        {SceneName.TitleScene, "TitleScene"},
        {SceneName.GameScene, "GameScene"},
        {SceneName.LobbyScene, "LobbyScene"},
    };

    public static void TransitioningScene(NetworkRunner runner,SceneName nextScene)
    {
        if (sceneNameTable.TryGetValue(nextScene, out string sceneName))
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
