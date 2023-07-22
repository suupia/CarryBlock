using Fusion;
using Projects.BattleSystem.Scripts;
using Projects.NetworkUtility.NetworkRunnerManager.Scripts;
using UnityEngine;
using Assert = UnityEngine.Assertions.Assert;

namespace Projects.BattleSystem.TitleScene.Scripts
{
    public class TitleInitializer : MonoBehaviour
    {
        [SerializeField] SceneName TransitionToSceneName;
        bool _isStarted; // StarGameWithRoomName() is called only once.

        // Called by UI Button
        public async void StartGame(string roomName, GameMode gameMode)
        {
            if (_isStarted) return;
            var runnerManager = FindObjectOfType<NetworkRunnerManager>();
            Assert.IsNotNull(runnerManager, "NetworkRunnerManagerをシーンに配置してください");

            await runnerManager.AttemptStartScene(roomName, gameMode);

            Debug.Log("Transitioning to LobbySceneTestRoom");
            SceneTransition.TransitioningScene(runnerManager.Runner, TransitionToSceneName);
            _isStarted = true;
        }
    }
}