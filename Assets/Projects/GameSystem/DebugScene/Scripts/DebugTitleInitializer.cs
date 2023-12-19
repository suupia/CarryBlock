using Carry.GameSystem.Scripts;
using Carry.NetworkUtility.NetworkRunnerManager.Scripts;
using Fusion;
using UnityEngine;
using Assert = UnityEngine.Assertions.Assert;

namespace Carry.GameSystem.DebugScene.Scripts
{
    public class DebugTitleInitializer : MonoBehaviour
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

            Debug.Log($"Transitioning to {TransitionToSceneName.ToString()}");
            SceneTransition.TransitionSceneWithNetworkRunner(runnerManager.Runner, TransitionToSceneName);
            _isStarted = true;
        }
    }
}