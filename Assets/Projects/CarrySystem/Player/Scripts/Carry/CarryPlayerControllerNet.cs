using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Carry.CarrySystem.CG.Tsukinowa;
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
        public void Init(ICharacter character, PlayerColorType colorType)
        {
            Debug.Log($"CarryPlayerController_Net.Init(), character = {character}");
            this.character = character;
            ColorType = colorType;
        }

        public override void Spawned()
        {
            Debug.Log($"CarryPlayerController_Net.Spawned(), _character = {character}");

            // init info
            info.Init(Runner, gameObject, this);

            // Instantiate the character.
            InstantiateCharacter();
            
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
                    character.HoldAction();
                    // _decorationDetector.OnMainAction(ref DecorationDataRef);
                }

                if (input.Buttons.WasPressed(PreButtons, PlayerOperation.Pass))
                {
                    character.PassAction();
                }

                var direction = new Vector3(input.Horizontal, 0, input.Vertical).normalized;

                // Debug.Log($"_character = {_character}");
                character.Move( direction);

                if (direction == Vector3.zero)
                {
                    character.PresenterContainer.Idle();
                }
                else
                {
                    character.PresenterContainer.Walk();
                }

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
            if (HasStateAuthority)
            {
                ColorType = (PlayerColorType)(((int)ColorType + 1) % Enum.GetValues(typeof(PlayerColorType)).Length);
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

            character?.Setup(info);
            _characterObj.GetComponent<TsukinowaMaterialSetter>().SetClothMaterial(ColorType);
            var animatorPresenter = GetComponent<PlayerAnimatorPresenterNet>();
            animatorPresenter.SetAnimator(_characterObj.GetComponentInChildren<Animator>());

            
            // Play spawn animation
            // _decorationDetector.OnSpawned();
        }

        public void Reset()
        {
            // フロア移動の際に呼ばれる
            character?.Reset();
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

