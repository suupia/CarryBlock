using Fusion;
using Fusion.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BasicSpawner : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] NetworkPrefabRef playerPrefab;

    TestNetworkInputData data = default;
    NetworkRunner runner;
    readonly Dictionary<PlayerRef, NetworkObject> spawnedCharacters = new();

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
            data.buttons.Set(TestPlayerOperations.Attack, true);

        data.buttons.Set(TestPlayerOperations.Forward, Input.GetKey(KeyCode.W));
        data.buttons.Set(TestPlayerOperations.Backward, Input.GetKey(KeyCode.S));
        data.buttons.Set(TestPlayerOperations.Left, Input.GetKey(KeyCode.A));
        data.buttons.Set(TestPlayerOperations.Right, Input.GetKey(KeyCode.D));
    }

    void OnGUI()
    {
        if (runner == null)
        {
            if (GUI.Button(new Rect(0, 0, 200, 40), "Host"))
            {
                StartGame(GameMode.Host);
            }
            if (GUI.Button(new Rect(0, 40, 200, 40), "Join"))
            {
                StartGame(GameMode.Client);
            }
        }
    }

    async void StartGame(GameMode mode)
    {
        runner = gameObject.AddComponent<NetworkRunner>();
        //���͂�񋟂���
        runner.ProvideInput = true;

        //SceneManager �́A�V�[���ɒ��ڔz�u����� NetworkObjects �̃C���X�^���X������������
        await runner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = "TestRoom",
            Scene = SceneManager.GetActiveScene().buildIndex,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
    }

    public void OnDisconnectedFromServer(NetworkRunner runner)
    {
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
    }

    public void OnInput(NetworkRunner runner, Fusion.NetworkInput input)
    {
        input.Set(data);

        data = default;
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, Fusion.NetworkInput input)
    {
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (runner.IsServer)
        {
            var spawnPosition = new Vector3(player.RawEncoded % runner.Config.Simulation.DefaultPlayers * 3, 1, 0);
            //Host���I�u�W�F�N�g���Ǘ�����BUnity��Instantiate��Spawn�ɒu�������
            //��S������player�͂��̃I�u�W�F�N�g�𑀍�ł���v���C���[���w��
            var networkPlayerObject = runner.Spawn(playerPrefab, spawnPosition, Quaternion.identity, player);
            spawnedCharacters.Add(player, networkPlayerObject);
        }
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if (spawnedCharacters.TryGetValue(player, out var networkObject))
        {
            runner.Despawn(networkObject);
            spawnedCharacters.Remove(player);
        }
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
    {
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
    }

}
