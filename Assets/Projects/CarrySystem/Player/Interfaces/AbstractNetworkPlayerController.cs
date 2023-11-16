using Carry.CarrySystem.CG.Tsukinowa;
using Carry.CarrySystem.Player.Info;
using Carry.CarrySystem.Player.Scripts;
using Fusion;
using Carry.NetworkUtility.Inputs.Scripts;
using Projects.Utility.Scripts;
using UnityEngine;

#nullable enable

namespace Carry.CarrySystem.Player.Interfaces
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class AbstractNetworkPlayerController : NetworkBehaviour, IPlayerController
    {
        // IPlayerController implementation
        public GameObject GameObjectValue => gameObject;
        public Rigidbody RigidbodyValue
        {
            get
            {
                if (_rigidbody != null) return _rigidbody;
                _rigidbody = GetComponent<Rigidbody>();  // not null because of RequireComponent
                return _rigidbody;
            }
        }
        
        // PlayerInfo
        public PlayerInfo GetInfo => Info;
        
        // class of Character 
        public PlayerHoldingObjectContainer GetPlayerHoldingObjectContainer => BlockContainer;
        public IMoveExecutorSwitcher GetMoveExecutorSwitcher => MoveExecutorSwitcher;
        public IHoldActionExecutor GetHoldActionExecutor => HoldActionExecutor;
        public IOnDamageExecutor GetOnDamageExecutor => OnDamageExecutor;
        public IDashExecutor GetDashExecutor => DashExecutor;
        public IPassActionExecutor GetPassActionExecutor => PassActionExecutor;
        
        // Serialize Field
        [SerializeField] protected Transform unitObjectParent= null!; // The NetworkCharacterControllerPrototype interpolates this transform.
        [SerializeField] protected GameObject[] playerUnitPrefabs= null!;
        
        // protected fields
        [Networked] protected NetworkButtons PreButtons { get; set; }
        [Networked] protected PlayerColorType ColorType { get; set; } // ローカルに反映させるために必要
        protected GameObject CharacterObj= null!;
        protected PlayerInfo Info = null!;

        protected PlayerHoldingObjectContainer BlockContainer = null!;
        protected IMoveExecutorSwitcher MoveExecutorSwitcher = null!;
        protected IHoldActionExecutor HoldActionExecutor = null!;
        protected IOnDamageExecutor OnDamageExecutor = null!;
        protected IDashExecutor DashExecutor = null!;
        protected IPassActionExecutor PassActionExecutor = null!;
        
        // private fields
        Rigidbody? _rigidbody;
        [Networked] NetworkBool IsReady { get; set; }


        public override void Spawned()
        {
            // Debug.Log($"AbstractNetworkPlayerController.Spawned(), _character = {Character}");

            // init info
            Info = new PlayerInfo(this, Object.InputAuthority);

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
                    HoldActionExecutor.HoldAction();
                    // _decorationDetector.OnMainAction(ref DecorationDataRef);
                }
                
                if (input.Buttons.WasPressed(PreButtons, PlayerOperation.Dash))
                {
                    DashExecutor.Dash();
                }


                var direction = new Vector3(input.Horizontal, 0, input.Vertical).normalized;

                // Debug.Log($"_character = {_character}");
                MoveExecutorSwitcher.Move( direction);
                 
                
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
            
            // Character?.Setup(info);
            Setup(Info);
            CharacterObj.GetComponent<TsukinowaMaterialSetter>().SetClothMaterial(ColorType);
            var animatorPresenter = GetComponent<IPlayerAnimatorPresenter>();
            if(animatorPresenter != null) animatorPresenter.SetAnimator(CharacterObj.GetComponentInChildren<Animator>());
            
            // Play spawn animation
            // _decorationDetector.OnSpawned();
        }
        
        protected void Setup(PlayerInfo info)
        {
            MoveExecutorSwitcher.Setup(info);
            HoldActionExecutor. Setup(info);
            PassActionExecutor.Setup(info);
            OnDamageExecutor.Setup(info);
            info.PlayerRb.useGravity = true;
        }

    }
}