using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Fusion;
using Nuts.NetworkUtility.Inputs.Scripts;
using Carry.CarrySystem.Player.Interfaces;
using Carry.CarrySystem.Player.Info;
using VContainer;


namespace Carry.CarrySystem.Player.Scripts
{
    [RequireComponent(typeof(HoldPresenter_Net))]
    public class CarryPlayerController_Net : AbstractNetworkPlayerController
    {
        [SerializeField]  Transform unitObjectParent; // The NetworkCharacterControllerPrototype interpolates this transform.
        public Transform InterpolationTransform => unitObjectParent;

        [SerializeField] GameObject[] playerUnitPrefabs;
        [SerializeField] CharacterType characterType;

        [SerializeField] PlayerInfo info;

        [Networked] NetworkButtons PreButtons { get; set; }
        [Networked] public NetworkBool IsReady { get; set; }

        // Detector
        // [Networked]
        // protected ref PlayerDecorationDetector.Data DecorationDataRef => ref MakeRef<PlayerDecorationDetector.Data>();
        // PlayerDecorationDetector _decorationDetector;
        
        
        ICharacter _character;
        GameObject _characterObj;
        
        bool _isInitialized;
        
        public void Init(ICharacter character)
        {
            _character = character;
            
            // init info
            info.Init(Runner, gameObject);

            // Instantiate the character.
            InstantiateCharacter(characterType);
            
            
            _isInitialized = true;
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
            if(!_isInitialized)return;
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
                    _character.Action();
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
        [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
        public void RPC_ChangeNextUnit()
        {
            characterType = (CharacterType)(((int)characterType + 1) % Enum.GetValues(typeof(CharacterType)).Length);
            Destroy(_characterObj);
            InstantiateCharacter(characterType);

            SetToOrigin();
        }

        [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
        public void RPC_SetToOrigin()
        {
            SetToOrigin();
        }

        void InstantiateCharacter(CharacterType characterType)
        {
            // Instantiate the unit.
            var prefab = playerUnitPrefabs[(int)characterType];
            _characterObj = Instantiate(prefab, unitObjectParent);

            _character.Setup(info);
            
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


        enum CharacterType
        {
            Red,
            Blue,
            Green,
            Yellow,
        }
    }
}

