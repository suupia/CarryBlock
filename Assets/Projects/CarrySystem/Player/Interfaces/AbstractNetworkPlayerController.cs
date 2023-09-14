using Carry.CarrySystem.CG.Tsukinowa;
using Carry.CarrySystem.Player.Info;
using Carry.CarrySystem.Player.Scripts;
using Fusion;
using Projects.NetworkUtility.Inputs.Scripts;
using UnityEngine;

#nullable enable

namespace Carry.CarrySystem.Player.Interfaces
{
    public abstract class AbstractNetworkPlayerController : NetworkBehaviour
    {
        [SerializeField] protected Transform unitObjectParent= null!; // The NetworkCharacterControllerPrototype interpolates this transform.

        [SerializeField] protected GameObject[] playerUnitPrefabs= null!;

        [SerializeField] protected PlayerInfo info= null!;

        [Networked] protected NetworkButtons PreButtons { get; set; }
        [Networked] public NetworkBool IsReady { get; set; }

        [Networked] protected PlayerColorType ColorType { get; set; } // ローカルに反映させるために必要

        protected GameObject CharacterObj= null!;
        public ICharacter GetCharacter => Character;

        protected ICharacter Character = null!; // サブクラスのInitでcharacterを受け取るようになっているのでnullになることはない


        public override void Spawned()
        {
            Debug.Log($"AbstractNetworkPlayerController.Spawned(), _character = {Character}");

            // init info
            info.Init(gameObject, this, Object.InputAuthority);

            // Instantiate the character.
            InstantiateCharacter();
            

            
        }
        
        public override void FixedUpdateNetwork()
        {
            base.FixedUpdateNetwork();
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
                    Character.HoldAction();
                    // _decorationDetector.OnMainAction(ref DecorationDataRef);
                }
                
                if (input.Buttons.WasPressed(PreButtons, PlayerOperation.Dash))
                {
                    Character.Dash();
                }


                var direction = new Vector3(input.Horizontal, 0, input.Vertical).normalized;

                // Debug.Log($"_character = {_character}");
                Character.Move( direction);
                 
                
                GetInputProcess(input); // 子クラスで処理を追加するためのメソッド

                PreButtons = input.Buttons;
            }
            
        }

        protected abstract void GetInputProcess(NetworkInputData input);

        // StateAuthorityがない場合でも呼ぶため、Networkedプロパティを参照すると同期が遅れて思った通りの挙動をしないことがあるので注意
        protected void InstantiateCharacter()
        {
            // Instantiate the unit.
            var prefab = playerUnitPrefabs[(int)ColorType];
            CharacterObj = Instantiate(prefab, unitObjectParent);
            
            Character?.Setup(info);
            CharacterObj.GetComponent<TsukinowaMaterialSetter>().SetClothMaterial(ColorType);
            var animatorPresenter = GetComponent<PlayerAnimatorPresenterNet>();
            animatorPresenter.SetAnimator(CharacterObj.GetComponentInChildren<Animator>());
            
            // Play spawn animation
            // _decorationDetector.OnSpawned();
        }
    }
}