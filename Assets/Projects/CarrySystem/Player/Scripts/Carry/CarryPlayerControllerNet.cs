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
            base.Spawned();

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

