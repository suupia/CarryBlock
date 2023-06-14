using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine;

namespace Carry.CarrySystem.SetActiveTest
{
    public class SetActiveTestParent : NetworkBehaviour
    {
        [SerializeField] GameObject targetObject;
        [Networked] NetworkButtons PreButtons { get; set; }

        public override void FixedUpdateNetwork()
        {
            if (!HasStateAuthority) return;

            if (GetInput(out NetworkInputData input))
            {
                if (input.Buttons.WasPressed(PreButtons, PlayerOperation.ChangeSetActive))
                {
                    targetObject.SetActive(!targetObject.activeSelf);
                }
                
                PreButtons = input.Buttons;
            }
        }

        void Update()
        {
            if (Object.InputAuthority)
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    RPC_ChangeActive();
                }
            }
        }

        [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
        public void RPC_ChangeActive()
        {
            targetObject.SetActive(!targetObject.activeSelf);
        }
    }

    public enum PlayerOperation
    {
        ChangeSetActive,
    }


    public class LocalInputPoller : INetworkRunnerCallbacks
    {
        // Local variable to store the input polled.
        NetworkInputData localInput;

        public void OnInput(NetworkRunner runner, NetworkInput input)
        {
            localInput = new NetworkInputData();
            localInput.Horizontal = Input.GetAxisRaw("Horizontal");
            localInput.Vertical = Input.GetAxisRaw("Vertical");
            localInput.Buttons.Set(PlayerOperation.ChangeSetActive, Input.GetKey(KeyCode.N));

            input.Set(localInput);

            // Reset the input struct to start with a clean slate
            // when polling for the next tick
            localInput = default;
        }


        #region Ignore

        public void OnConnectedToServer(NetworkRunner runner)
        {
        }

        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
        {
        }

        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request,
            byte[] token)
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

        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
        {
        }

        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
        }

        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
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

        #endregion
    }


    public struct NetworkInputData : INetworkInput
    {
        [Networked] public float Horizontal { get; set; }
        [Networked] public float Vertical { get; set; }
        public NetworkButtons Buttons;

    }
}