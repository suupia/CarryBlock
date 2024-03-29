using Carry.GameSystem.Scripts;
using Carry.NetworkUtility.NetworkRunnerManager.Scripts;
using DG.Tweening.Core;
using Fusion;
using UnityEngine;
using Assert = UnityEngine.Assertions.Assert;

#nullable enable

namespace Carry.GameSystem.TitleScene.Scripts
{
    public class TitleInitializer : MonoBehaviour
    {
        [SerializeField] NetworkPrefabRef carryInitializerPrefab;
        bool _isStarted; // StarGameWithRoomName() is called only once.

        void Awake()
        {
            DestroyAllDontDestroyOnLoadObjects();
        }

        // Called by UI Button
        public async void StartGame(string roomName, GameMode gameMode)
        {
            if (_isStarted) return;

            var runnerManager = FindObjectOfType<NetworkRunnerManager>();
            Assert.IsNotNull(runnerManager, "NetworkRunnerManagerをシーンに配置してください");

            _isStarted = true; // 連続で呼ばれるのを防ぐ

            await runnerManager.AttemptStartScene(roomName, gameMode);

            runnerManager.Runner.Spawn(carryInitializerPrefab);

            Debug.Log("Transitioning to LobbySceneTestRoom");
            SceneTransition.TransitionSceneWithNetworkRunner(runnerManager.Runner, SceneName.LobbyScene);
        }


        // 前回のNetwork関連のオブジェクトを全て削除する
        void DestroyAllDontDestroyOnLoadObjects()
        {
            var sacrifice = new GameObject("Sacrificial Object");
            DontDestroyOnLoad(sacrifice);

            foreach (var dontDestroy in sacrifice.scene.GetRootGameObjects())
            {
                if (dontDestroy.GetComponent<DOTweenComponent>()) continue;
                Destroy(dontDestroy);
            }
        }
    }
}