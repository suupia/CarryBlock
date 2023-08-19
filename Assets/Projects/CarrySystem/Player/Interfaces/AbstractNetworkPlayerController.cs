using Carry.CarrySystem.CG.Tsukinowa;
using Carry.CarrySystem.Player.Info;
using Carry.CarrySystem.Player.Scripts;
using Fusion;
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

        protected GameObject _characterObj= null!;
        public ICharacter Character => character;

        protected ICharacter? character;


        public override void Spawned()
        {
            Debug.Log($"AbstractNetworkPlayerController.Spawned(), _character = {character}");

            // init info
            info.Init(Runner, gameObject, this);

            // Instantiate the character.
            InstantiateCharacter();
            
        }
        
        // StateAuthorityがない場合でも呼ぶため、Networkedプロパティを参照すると同期が遅れて思った通りの挙動をしないことがあるので注意
        protected void InstantiateCharacter()
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
    }
}