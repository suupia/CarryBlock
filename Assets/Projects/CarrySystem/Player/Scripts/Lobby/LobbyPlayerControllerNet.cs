using System;
using Carry.CarrySystem.CG.Tsukinowa;
using Carry.CarrySystem.Player.Interfaces;
using Fusion;
using Projects.BattleSystem.Player.Scripts;
using Projects.NetworkUtility.Inputs.Scripts;
using UnityEngine;
using AbstractNetworkPlayerController = Carry.CarrySystem.Player.Interfaces.AbstractNetworkPlayerController;
using PlayerInfo = Carry.CarrySystem.Player.Info.PlayerInfo;

#nullable  enable

namespace Carry.CarrySystem.Player.Scripts
{
    public class LobbyPlayerControllerNet : AbstractNetworkPlayerController
    { 
        [SerializeField]  Transform unitObjectParent= null!; // The NetworkCharacterControllerPrototype interpolates this transform.
        public Transform InterpolationTransform => unitObjectParent;

        [SerializeField] GameObject cameraPrefab= null!;
        [SerializeField] GameObject[] playerUnitPrefabs= null!;

        [SerializeField] PlayerInfo info= null!;

        [Networked] NetworkButtons PreButtons { get; set; }
        [Networked] public NetworkBool IsReady { get; set; }

        [Networked] PlayerColorType ColorType { get; set; } // ローカルに反映させるために必要
        
        PlayerCharacterHolder _playerCharacterHolder = null!;

        // Detector
        // [Networked]
        // protected ref PlayerDecorationDetector.Data DecorationDataRef => ref MakeRef<PlayerDecorationDetector.Data>();
        // PlayerDecorationDetector _decorationDetector;
        
        
        ICharacter _character = null!;
        GameObject _characterObj= null!;
        
        bool _isSpawned; // FixedUpdateNetwork()が呼ばれる前にSpawned()が呼ばれるため必要ないと言えば必要ない
        
        public void Init(ICharacter character, PlayerColorType colorType, PlayerCharacterHolder playerCharacterHolder)
        {
            Debug.Log($"LobbyPlayerControllerNet.Init(), character = {character}");
            _character = character;
            ColorType = colorType;
            _playerCharacterHolder = playerCharacterHolder;
        }

        public override void Spawned()
        {
            Debug.Log($"LobbyPlayerControllerNet.Spawned(), _character = {_character}");

            // init info
            info.Init(Runner, gameObject,this);
            
            // Set camera
            if (Object.HasInputAuthority)
            {
                var cameraObj = Instantiate(cameraPrefab);
                var followTarget = cameraObj.GetComponent<CameraFollowTarget>();
                var playerCamera = cameraObj.GetComponent<Camera>();
                followTarget.SetTarget(unitObjectParent.transform);
            }


            // Instantiate the character.
            InstantiateCharacter();
            
            _isSpawned = true;
        }

        protected virtual void Update()
        {
            if (Object.HasInputAuthority)
            {
                if (Input.GetKeyDown(KeyCode.C))
                {
                    RPC_ChangeNextUnit();
                }
            }
        }


        public override void FixedUpdateNetwork()
        {
            if(!_isSpawned)return;
            if (!HasStateAuthority) return;

            if (GetInput(out NetworkInputData input))
            {
                if (input.Buttons.WasPressed(PreButtons, PlayerOperation.Ready))
                {
                    IsReady = !IsReady;
                    Debug.Log($"Toggled Ready -> {IsReady}");
                }

                if (input.Buttons.WasPressed(PreButtons, PlayerOperation.MainAction))
                {
                    _character.HoldAction();
                    // _decorationDetector.OnMainAction(ref DecorationDataRef);
                }



                var direction = new Vector3(input.Horizontal, 0, input.Vertical).normalized;

                // Debug.Log($"_character = {_character}");
                _character.Move( direction);

                PreButtons = input.Buttons;
            }
        }

        public override void Render()
        {
            // _decorationDetector.OnRendered(DecorationDataRef, PlayerStruct.Hp);
            
        }
        
        //Deal as RPC for changing unit
        [Rpc(RpcSources.InputAuthority, RpcTargets.InputAuthority)]
        public void RPC_ChangeNextUnit()
        {
            ColorType = (PlayerColorType)(((int)ColorType + 1) % Enum.GetValues(typeof(PlayerColorType)).Length);
            if (HasStateAuthority)
            {
                _playerCharacterHolder.SetColor(Runner.LocalPlayer, ColorType);  // プレイヤーの色を設定して覚えておく
            }
            Destroy(_characterObj);
            InstantiateCharacter();

            SetToOrigin();
        }

        [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
        public void RPC_SetToOrigin()
        {
            SetToOrigin();
        }

        void InstantiateCharacter()
        {
            // Instantiate the unit.
            var prefab = playerUnitPrefabs[(int)ColorType];
            _characterObj = Instantiate(prefab, unitObjectParent);
            _characterObj.GetComponent<TsukinowaMaterialSetter>().SetClothMaterial(ColorType);


            _character?.Setup(info);
            
            // Play spawn animation
            // _decorationDetector.OnSpawned();
        }

        public void Reset()
        {
            // フロア移動の際に呼ばれる
            _character?.Reset();
            SetToOrigin();
        }



        void SetToOrigin()
        {
            // ToDo: 地面をすり抜けないようにするために、少し上に移動させておく（Spawnとの調整は後回し）
            info.playerObj.transform.position = new Vector3(0, 5, 0);
            info.playerRb.velocity = Vector3.zero;
        }

      
    }
}