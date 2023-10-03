using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using Carry.Utility.Scripts;
using UnityEngine;
using UnityEngine.InputSystem;
using Assert = UnityEngine.Assertions.Assert;

namespace Carry.NetworkUtility.Inputs.Scripts
{
    public enum PlayerOperation
    {
        MainAction = 0,
        Ready,
        ToggleSelectStageCanvas,
        ChangeUnit,
        Dash,
        Pass,
        Debug1,
        Debug2,
        Debug3
    }


    public class LocalInputPoller : INetworkRunnerCallbacks
    {
        NetworkInputData _localInput;

        readonly InputAction _toggleSelectStageCanvas;
        readonly InputAction _move;
        readonly InputAction _mainAction;
        readonly InputAction _dash;
        readonly InputAction _pass;
        readonly InputAction _changeUnit;

        public LocalInputPoller(InputActionMap inputActionMap)
        {
            inputActionMap.Enable();
            
            //本来は以下を適切なタイミングで呼ぶべき
            // _inputActionMap.Disable();

            _toggleSelectStageCanvas = inputActionMap.FindAction("ToggleSelectStageCanvas");
            _move = inputActionMap.FindAction("Move");
            _mainAction = inputActionMap.FindAction("MainAction");
            _dash = inputActionMap.FindAction("Dash");
            _pass = inputActionMap.FindAction("Pass");
            _changeUnit = inputActionMap.FindAction("ChangeUnit");
            
        }


        public void OnInput(NetworkRunner runner, NetworkInput input)
        {
            _localInput = default;

            var isDownOpenSelectStageCanvas = _toggleSelectStageCanvas.ReadValue<float>();
            var moveVector = _move.ReadValue<Vector2>().normalized;
            var mainActionValue = _mainAction.ReadValue<float>(); 
            var dashValue = _dash.ReadValue<float>();
            var passValue = _pass.ReadValue<float>();
            var changeUnitValue = _changeUnit.ReadValue<float>();
            
            // Debug.Log(moveVector);
            // Debug.Log(isDownMainAction);
            
            _localInput.Horizontal = moveVector.x;
            _localInput.Vertical = moveVector.y;
            _localInput.Buttons.Set(PlayerOperation.ToggleSelectStageCanvas, isDownOpenSelectStageCanvas != 0);
            _localInput.Buttons.Set(PlayerOperation.MainAction, mainActionValue != 0);
            _localInput.Buttons.Set(PlayerOperation.Dash, dashValue != 0);
            _localInput.Buttons.Set(PlayerOperation.Pass, passValue != 0);
            _localInput.Buttons.Set(PlayerOperation.ChangeUnit, changeUnitValue != 0);
            // _localInput.Buttons.Set(PlayerOperation.Ready, Input.GetKey(KeyCode.R));
            // _localInput.Buttons.Set(PlayerOperation.ChangeUnit, Input.GetKey(KeyCode.C));
            // _localInput.Buttons.Set(PlayerOperation.ReturnToMainBase, Input.GetKey(KeyCode.LeftShift));
            // _localInput.Buttons.Set(PlayerOperation.Debug1, Input.GetKey(KeyCode.F1));
            // _localInput.Buttons.Set(PlayerOperation.Debug2, Input.GetKey(KeyCode.F2));
            // _localInput.Buttons.Set(PlayerOperation.Debug3, Input.GetKey(KeyCode.F3));
            input.Set(_localInput);

            // Reset the input struct to start with a clean slate
            // when polling for the next tick
            _localInput = default;
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