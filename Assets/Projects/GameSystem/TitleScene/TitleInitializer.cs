using Carry.CarrySystem.CarryScene.Scripts;
using Carry.CarrySystem.Map.Scripts;
using Carry.GameSystem.Scripts;
using Carry.NetworkUtility.NetworkRunnerManager.Scripts;
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

        // Called by UI Button
        public async void StartGame(string roomName, GameMode gameMode)
        {
            if (_isStarted) return;
            
            DestroyBeforeNetworkComponent();
            
            var runnerManager = FindObjectOfType<NetworkRunnerManager>();
            Assert.IsNotNull(runnerManager, "NetworkRunnerManagerをシーンに配置してください");

            _isStarted = true;  // 連続で呼ばれるのを防ぐ
            
            await runnerManager.AttemptStartScene(roomName, gameMode);

            runnerManager.Runner.Spawn(carryInitializerPrefab);

            Debug.Log("Transitioning to LobbySceneTestRoom");
            SceneTransition.TransitioningScene(runnerManager.Runner, SceneName.LobbyScene);
        }
        
        void DestroyBeforeNetworkComponent()
        {
            var runnerGameObject = GameObject.Find("NetworkRunner(Clone)");
            Debug.Log($"runnerGameObject: {runnerGameObject}");
            var runner = runnerGameObject != null ? runnerGameObject.GetComponent<NetworkRunner>() : null;
            if (runner != null)
            {
                // 前の状態が残っているので、それを消す
                Debug.Log($"Destroying before NetworkComponents");
                var networkSceneManager = FindObjectOfType<NetworkSceneManagerDefault>();
                var mapKeyDataSelector = FindObjectOfType<MapKeyDataSelectorNet>();
                var carryInitializerReady = FindObjectOfType<CarryInitializersReady>();
                if(networkSceneManager != null) Destroy(networkSceneManager.gameObject);
                if(mapKeyDataSelector != null) runner.Despawn(mapKeyDataSelector.Object);
                if(carryInitializerReady != null) runner.Despawn(carryInitializerReady.Object);
                
                // ここでRunnerをさくじょ..?
                
            }

        }
    }
}