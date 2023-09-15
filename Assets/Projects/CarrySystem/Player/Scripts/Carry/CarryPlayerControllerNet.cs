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
using Projects.NetworkUtility.Inputs.Scripts;
using Carry.CarrySystem.Player.Interfaces;
using Carry.CarrySystem.Player.Info;
using VContainer;
#nullable enable


namespace Carry.CarrySystem.Player.Scripts
{
    public class CarryPlayerControllerNet : AbstractNetworkPlayerController
    {
        public PlayerInfo Info => info;
        
        IMapUpdater? _mapUpdater;
        PlayerNearCartHandlerNet _playerNearCartHandler = null!;
        PlayerCharacterHolder _playerCharacterHolder = null!;
        FloorTimerNet _floorTimerNet = null!;
        
        public void Init(
            ICharacter character,
            PlayerColorType colorType,
            IMapUpdater mapUpdater,
            PlayerNearCartHandlerNet playerNearCartHandler,
            PlayerCharacterHolder playerCharacterHolder,
            FloorTimerNet floorTimerNet
            )
        {
            Debug.Log($"CarryPlayerController_Net.Init(), character = {character}");
            this.Character = character;
            ColorType = colorType;
            _mapUpdater = mapUpdater;
            _playerNearCartHandler = playerNearCartHandler;
            _playerCharacterHolder = playerCharacterHolder;
            _floorTimerNet = floorTimerNet;

            _mapUpdater.RegisterResetAction(() => Reset(_mapUpdater.GetMap()));
        }

        public override void Spawned()
        {
            Debug.Log($"CarryPlayerController_Net.Spawned(), _character = {Character}");
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
                var isNear =  _playerNearCartHandler.IsNearCart(info.PlayerObj);
                // Debug.Log($"isNear = {isNear}");
            }

            if (input.Buttons.WasPressed(PreButtons, PlayerOperation.Pass))
            {
                Character.PassAction();
            }

        }

        public override void Render() 
        {
            
        }

        public void Reset(EntityGridMap map)
        {
            // フロア移動の際に呼ばれる
            Character?.Reset();
            ToSpawnPosition(map);
        }



        void ToSpawnPosition(EntityGridMap map)
        {
            var spawnGridPos = new Vector2Int(1, map.Height / 2 + _playerCharacterHolder.GetPlayerIndex(info.PlayerRef) -1);
            var spawnWorldPos = GridConverter.GridPositionToWorldPosition(spawnGridPos);
            var height = 0.5f;  // 地面をすり抜けないようにするために、少し上に移動させておく（Spawnとの調整は後回し）
            info.PlayerObj.transform.position = new Vector3(spawnWorldPos.x, height, spawnWorldPos.z);
            info.PlayerRb.velocity = Vector3.zero;
            
        }




    }
}

