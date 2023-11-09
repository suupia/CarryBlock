using System;
using Carry.CarrySystem.CG.Tsukinowa;
using Carry.CarrySystem.Player.Interfaces;
using Fusion;
using Carry.GameSystem.Player.Scripts;
using Carry.NetworkUtility.Inputs.Scripts;
using UnityEngine;
using AbstractNetworkPlayerController = Carry.CarrySystem.Player.Interfaces.AbstractNetworkPlayerController;
using PlayerInfo = Carry.CarrySystem.Player.Info.PlayerInfo;
#nullable  enable

namespace Carry.CarrySystem.Player.Scripts
{
    public class LobbyPlayerControllerNet : AbstractNetworkPlayerController
    {
        public PlayerInfo Info => info;
        [SerializeField] GameObject cameraPrefab= null!;
        
        PlayerCharacterTransporter _playerCharacterTransporter = null!;  // PlayerColorTypeを次のシーンに保持するために必要
        
        
        public void Init(
            PlayerHoldingObjectContainer blockContainer,
            IMoveExecutorSwitcher moveExecutorSwitcher,
            IHoldActionExecutor holdActionExecutor,
            IOnDamageExecutor onDamageExecutor,
            IDashExecutor dashExecutor,
            IPassActionExecutor passActionExecutor,
            PlayerColorType colorType,
            PlayerCharacterTransporter playerCharacterTransporter)
        {
            BlockContainer = blockContainer;
            MoveExecutorSwitcher = moveExecutorSwitcher;
            HoldActionExecutor = holdActionExecutor;
            OnDamageExecutor = onDamageExecutor!;
            DashExecutor = dashExecutor;
            PassActionExecutor = passActionExecutor;
            ColorType = colorType;
            _playerCharacterTransporter = playerCharacterTransporter;
        }

        public override void Spawned()
        {
            // Debug.Log($"LobbyPlayerController_Net.Spawned(), _character = {Character}");    

            // Set camera
            if (Object.HasInputAuthority)
            {
                var cameraObj = Instantiate(cameraPrefab);
                var followTarget = cameraObj.GetComponent<CameraFollowTarget>();
                var playerCamera = cameraObj.GetComponent<Camera>();
                followTarget.SetTarget(unitObjectParent.transform);
            }
            
            base.Spawned();
            
        }
        

        public override void FixedUpdateNetwork()
        {
            base.FixedUpdateNetwork();
            if (!HasStateAuthority) return;

        }

        protected override void GetInputProcess(NetworkInputData input)
        {
            if (input.Buttons.WasPressed(PreButtons, PlayerOperation.ChangeUnit))
            {
                Debug.Log($"ChangeUnit");
                var afterColor  = (PlayerColorType)(((int)ColorType + 1) % Enum.GetValues(typeof(PlayerColorType)).Length);

                ColorType = afterColor;
                _playerCharacterTransporter.SetColor(Object.InputAuthority, afterColor); // プレイヤーの色を設定して覚えておく

                RPC_ChangeNextUnit();
            }

        }

        public override void Render()
        {
            
        }
        
        //Deal as RPC for changing unit
        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        public void RPC_ChangeNextUnit()
        {
            Destroy(CharacterObj);
            InstantiateCharacter();

            SetToOrigin();
        }
        

        void SetToOrigin()
        {
            // ToDo: 地面をすり抜けないようにするために、少し上に移動させておく（Spawnとの調整は後回し）
            info.PlayerObj.transform.position = new Vector3(0, 5, 0);
            info.PlayerRb.velocity = Vector3.zero;
        }

      
    }
}