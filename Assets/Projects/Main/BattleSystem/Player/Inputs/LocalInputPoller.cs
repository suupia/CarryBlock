using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine;

namespace Main
{
    public enum PlayerOperation
    {
        MainAction = 0,
        Ready,
        ChangeUnit,
        ReturnToMainBase,
        Debug1,
        Debug2,
        Debug3
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
            localInput.Buttons.Set(PlayerOperation.MainAction, Input.GetKey(KeyCode.Space));
            localInput.Buttons.Set(PlayerOperation.Ready, Input.GetKey(KeyCode.R));
            localInput.Buttons.Set(PlayerOperation.ChangeUnit, Input.GetKey(KeyCode.C));
            localInput.Buttons.Set(PlayerOperation.ReturnToMainBase, Input.GetKey(KeyCode.LeftShift));
            localInput.Buttons.Set(PlayerOperation.Debug1, Input.GetKey(KeyCode.F1));
            localInput.Buttons.Set(PlayerOperation.Debug2, Input.GetKey(KeyCode.F2));
            localInput.Buttons.Set(PlayerOperation.Debug3, Input.GetKey(KeyCode.F3));
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

        public NetworkBool IsSpaceDown { get; set; }
        public NetworkBool IsShiftDown { get; set; }
        public NetworkBool IsShiftUp { get; set; }
    }
}