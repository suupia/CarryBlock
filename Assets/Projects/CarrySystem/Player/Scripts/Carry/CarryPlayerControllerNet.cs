using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Carry.CarrySystem.Cart.Scripts;
using Carry.CarrySystem.CG.Tsukinowa;
using Carry.CarrySystem.FloorTimer.Scripts;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using Fusion;
using Carry.NetworkUtility.Inputs.Scripts;
using Carry.CarrySystem.Player.Interfaces;
using Carry.CarrySystem.Player.Info;
using Projects.Utility.Scripts;
using VContainer;
#nullable enable


namespace Carry.CarrySystem.Player.Scripts
{
    public class CarryPlayerControllerNet : AbstractNetworkPlayerController
    {
        IMapGetter? _mapUpdater;
        PlayerNearCartHandlerNet _playerNearCartHandler = null!;
        PlayerCharacterTransporter _playerCharacterTransporter = null!;
        FloorTimerNet _floorTimerNet = null!;

        public void Init(
            PlayerHoldingObjectContainer blockContainer,
            IMoveExecutorSwitcher moveExecutorSwitcher,
            IHoldActionExecutor holdActionExecutor,
            IOnDamageExecutor onDamageExecutor,
            IDashExecutor dashExecutor,
            IPassActionExecutor passActionExecutor,
            PlayerColorType colorType,
            IMapGetter mapGetter,
            PlayerNearCartHandlerNet playerNearCartHandler,
            PlayerCharacterTransporter playerCharacterTransporter,
            FloorTimerNet floorTimerNet
        )
        {
            BlockContainer = blockContainer;
            MoveExecutorSwitcher = moveExecutorSwitcher;
            HoldActionExecutor = holdActionExecutor;
            PassActionExecutor = passActionExecutor;
            DashExecutor = dashExecutor;
            OnDamageExecutor = onDamageExecutor!;
            ColorType = colorType;
            _mapUpdater = mapGetter;
            _playerNearCartHandler = playerNearCartHandler;
            _playerCharacterTransporter = playerCharacterTransporter;
            _floorTimerNet = floorTimerNet;

            _mapUpdater.RegisterResetAction(() => Reset(_mapUpdater.GetMap()));
        }

        public override void Spawned()
        {
            // Debug.Log($"CarryPlayerController_Net.Spawned(), _character = {Character}");
            base.Spawned();

            if (HasStateAuthority)
            {
                if (_mapUpdater != null)
                    ToSpawnPosition(_mapUpdater.GetMap());  // Init()がOnBeforeSpawned()よりも先に呼ばれるため、_mapUpdaterは受け取れているはず
                else
                    Debug.LogError($"_mapUpdater is null");
            }

        }
        

        public override void FixedUpdateNetwork()
        {
            if (!HasStateAuthority) return;
            if(_floorTimerNet.IsExpired) return;
            base.FixedUpdateNetwork();
        }
        
        protected override void GetInputProcess(NetworkInputData input)
        {
            if (input.Buttons.WasPressed(PreButtons, PlayerOperation.MainAction))
            {
                // AidKit
                var isNear =  _playerNearCartHandler.IsNearCart(Info.PlayerObj);
                // Debug.Log($"isNear = {isNear}");
            }

            if (input.Buttons.WasPressed(PreButtons, PlayerOperation.Pass))
            {
                PassActionExecutor.PassAction();
            }

        }

        public override void Render() 
        {
            
        }

        public void Reset(EntityGridMap map)
        {
            // フロア移動の際に呼ばれる
            // Character?.Reset();
            HoldActionExecutor.Reset();
            PassActionExecutor.Reset();
            ToSpawnPosition(map);
        }



        void ToSpawnPosition(EntityGridMap map)
        {
            var spawnGridPos = new Vector2Int(1, map.Height / 2 + _playerCharacterTransporter.GetPlayerIndex(Info.PlayerRef) -1);
            var spawnWorldPos = GridConverter.GridPositionToWorldPosition(spawnGridPos);
            var height = 0.5f;  // 地面をすり抜けないようにするために、少し上に移動させておく（Spawnとの調整は後回し）
            Info.PlayerObj.transform.position = new Vector3(spawnWorldPos.x, height, spawnWorldPos.z);
            Info.PlayerRb.velocity = Vector3.zero;
            
        }




    }
}

