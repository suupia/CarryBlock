using Fusion;
using Nuts.BattleSystem.Scripts;
using Nuts.NetworkUtility.NetworkRunnerManager.Scripts;
using UnityEngine;
using Assert = UnityEngine.Assertions.Assert;

namespace Nuts.BattleSystem.TitleScene.Scripts
{
    public class TitleInitializer : MonoBehaviour
    {
        bool _isStarted; // StarGameWithRoomName() is called only once.

        // Called by UI Button
        public async void StartGame(string roomName, GameMode gameMode)
        {
            if (_isStarted) return;
            var runnerManager = FindObjectOfType<NetworkRunnerManager>();
            Assert.IsNotNull(runnerManager, "NetworkRunnerManagerをシーンに配置してください");

            await runnerManager.AttemptStartScene(roomName, gameMode);

            Debug.Log("Transitioning to LobbySceneTestRoom");
            SceneTransition.TransitioningScene(runnerManager.Runner, SceneName.LobbyScene);
            _isStarted = true;
        }
    }
}