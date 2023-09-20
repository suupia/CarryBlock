using Cysharp.Threading.Tasks;
using Fusion;
using Projects.NetworkUtility.Inputs.Scripts;
using Projects.NetworkUtility.ObjectPool.Scripts;
using Projects.Utility.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = System.Random;

namespace Projects.NetworkUtility.NetworkRunnerManager.Scripts
{
    // 全てのシーンにこれを配置しておけば、NetworkRunnerを使える
// シーン上にNetworkRunnerがないならインスタンス化し、runner.StartGame()を実行
    public class NetworkRunnerManager : MonoBehaviour
    {
        [SerializeField] NetworkRunner networkRunner;
        [SerializeField] NetworkSceneManagerDefault networkSceneManagerDefault;
        [SerializeField] NetworkObjectPoolDefault networkObjectPoolDefault;
        public NetworkRunner Runner { get; private set; }

        public bool IsReady => Runner != null && Runner.SceneManager.IsReady(Runner);

        public async UniTask AttemptStartScene(string sessionName = default,
            GameMode gameMode = GameMode.AutoHostOrClient)
        {
            sessionName ??= RandomString(5);
            Runner = FindObjectOfType<NetworkRunner>();
            if (Runner == null)
            {
                // Set up NetworkRunner
                Runner = Instantiate(networkRunner);
                DontDestroyOnLoad(Runner);
                var inputActionMap = new InputActionMapLoader().InputActionMap;
                Runner.AddCallbacks(new LocalInputPoller(inputActionMap));

                // Set up SceneMangerDefault
                var sceneMangerDefault = Instantiate(networkSceneManagerDefault);
                DontDestroyOnLoad(sceneMangerDefault);

                await Runner.StartGame(new StartGameArgs
                {
                    GameMode = gameMode,
                    SessionName = sessionName,
                    Scene = SceneManager.GetActiveScene().buildIndex,
                    SceneManager = sceneMangerDefault,
                    ObjectPool = networkObjectPoolDefault
                });

                // Register to allow SceneLoadDone() of NetworkSceneManagerDefault to be called.
                Runner.AddSimulationBehaviour(networkObjectPoolDefault);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        // Create random char
        string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var result = new char[length];
            for (var i = 0; i < length; i++) result[i] = chars[random.Next(chars.Length)];

            return new string(result);
        }
    }
}