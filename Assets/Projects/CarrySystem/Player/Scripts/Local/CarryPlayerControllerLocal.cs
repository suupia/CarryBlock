#nullable enable

using System;
using Carry.CarrySystem.Cart.Scripts;
using Carry.CarrySystem.FloorTimer.Scripts;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.Player.Info;
using Carry.CarrySystem.Player.Interfaces;
using Carry.NetworkUtility.Inputs.Scripts;
using UnityEngine;


namespace Carry.CarrySystem.Player.Scripts
{
        public class CarryPlayerControllerLocal : MonoBehaviour
    {
        [SerializeField] protected Transform unitObjectParent= null!; // The NetworkCharacterControllerPrototype interpolates this transform.

        [SerializeField] protected GameObject[] playerUnitPrefabs= null!;

        [SerializeField] protected PlayerInfo info= null!;
        
        protected PlayerColorType ColorType { get; set; } // ローカルに反映させるために必要

        protected GameObject CharacterObj= null!;

        public PlayerHoldingObjectContainer GetPlayerHoldingObjectContainer => BlockContainer;
        public IMoveExecutorSwitcher GetMoveExecutorSwitcher => MoveExecutorSwitcher;
        public IHoldActionExecutor GetHoldActionExecutor => HoldActionExecutor;
        public IOnDamageExecutor GetOnDamageExecutor => OnDamageExecutor;
        public IDashExecutor GetDashExecutor => DashExecutor;
        public IPassActionExecutor GetPassActionExecutor => PassActionExecutor;
        
        
        // ICharacterリファクタリング
         PlayerHoldingObjectContainer BlockContainer = null!;
         IMoveExecutorSwitcher MoveExecutorSwitcher = null!;
         IHoldActionExecutor HoldActionExecutor = null!;
         IOnDamageExecutor OnDamageExecutor = null!;
         IDashExecutor DashExecutor = null!;
         IPassActionExecutor PassActionExecutor = null!;

        public PlayerInfo Info => info;
        
        IMapUpdater? _mapUpdater;
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
            IMapUpdater mapUpdater,
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
            _mapUpdater = mapUpdater;
            _playerNearCartHandler = playerNearCartHandler;
            _playerCharacterTransporter = playerCharacterTransporter;
            _floorTimerNet = floorTimerNet;

            // _mapUpdater.RegisterResetAction(() => Reset(_mapUpdater.GetMap()));
        }
        //
        // public  void Spawned()
        // {
        //     // Debug.Log($"CarryPlayerController_Net.Spawned(), _character = {Character}");
        //     
        //     // init info
        //     info.Init(gameObject, this, Object.InputAuthority);
        //
        //     // Instantiate the character.
        //     InstantiateCharacter();
        //
        //
        //     if (HasStateAuthority)
        //     {
        //         if (_mapUpdater != null)
        //             ToSpawnPosition(_mapUpdater.GetMap());  // Init()がOnBeforeSpawned()よりも先に呼ばれるため、_mapUpdaterは受け取れているはず
        //         else
        //             Debug.LogError($"_mapUpdater is null");
        //     }
        //
        // }
        //
        // public void FixedUpdate()
        // {
        //     base.FixedUpdateNetwork();
        //     if (!HasStateAuthority) return;
        //
        //     if (GetInput(out NetworkInputData input))
        //     {
        //         if (input.Buttons.WasPressed(PreButtons, PlayerOperation.Ready))
        //         {
        //             IsReady = !IsReady;
        //             Debug.Log($"Toggled Ready -> {IsReady}");
        //         }
        //
        //         if (input.Buttons.WasPressed(PreButtons, PlayerOperation.MainAction))
        //         {
        //             HoldActionExecutor.HoldAction();
        //             // _decorationDetector.OnMainAction(ref DecorationDataRef);
        //         }
        //         
        //         if (input.Buttons.WasPressed(PreButtons, PlayerOperation.Dash))
        //         {
        //             DashExecutor.Dash();
        //         }
        //
        //
        //         var direction = new Vector3(input.Horizontal, 0, input.Vertical).normalized;
        //
        //         // Debug.Log($"_character = {_character}");
        //         MoveExecutorSwitcher.Move( direction);
        //          
        //         
        //         GetInputProcess(input); // 子クラスで処理を追加するためのメソッド
        //
        //         PreButtons = input.Buttons;
        //     }
        // }
        //
        //
        // protected  void GetInputProcess(NetworkInputData input)
        // {
        //     if (input.Buttons.WasPressed(PreButtons, PlayerOperation.MainAction))
        //     {
        //         // AidKit
        //         var isNear =  _playerNearCartHandler.IsNearCart(info.PlayerObj);
        //         // Debug.Log($"isNear = {isNear}");
        //     }
        //
        //     if (input.Buttons.WasPressed(PreButtons, PlayerOperation.Pass))
        //     {
        //         PassActionExecutor.PassAction();
        //     }
        //
        // }
        //
        //
        // public void Reset(EntityGridMap map)
        // {
        //     // フロア移動の際に呼ばれる
        //     // Character?.Reset();
        //     HoldActionExecutor.Reset();
        //     PassActionExecutor.Reset();
        //     ToSpawnPosition(map);
        // }
        //
        //
        //
        // void ToSpawnPosition(EntityGridMap map)
        // {
        //     var spawnGridPos = new Vector2Int(1, map.Height / 2 + _playerCharacterTransporter.GetPlayerIndex(info.PlayerRef) -1);
        //     var spawnWorldPos = GridConverter.GridPositionToWorldPosition(spawnGridPos);
        //     var height = 0.5f;  // 地面をすり抜けないようにするために、少し上に移動させておく（Spawnとの調整は後回し）
        //     info.PlayerObj.transform.position = new Vector3(spawnWorldPos.x, height, spawnWorldPos.z);
        //     info.PlayerRb.velocity = Vector3.zero;
        //     
        // }
        //
        // protected void InstantiateCharacter()
        // {
        //     // Instantiate the unit.
        //     var prefab = playerUnitPrefabs[(int)ColorType];
        //     CharacterObj = Instantiate(prefab, unitObjectParent);
        //     
        //     // Character?.Setup(info);
        //     Setup(info);
        //     CharacterObj.GetComponent<TsukinowaMaterialSetter>().SetClothMaterial(ColorType);
        //     var animatorPresenter = GetComponent<PlayerAnimatorPresenterNet>();
        //     animatorPresenter.SetAnimator(CharacterObj.GetComponentInChildren<Animator>());
        //     
        //     // Play spawn animation
        //     // _decorationDetector.OnSpawned();
        // }
        //
        // protected void Setup(PlayerInfo info)
        // {
        //     MoveExecutorSwitcher.Setup(info);
        //     HoldActionExecutor. Setup(info);
        //     PassActionExecutor.Setup(info);
        //     OnDamageExecutor.Setup(info);
        //     info.PlayerRb.useGravity = true;
        // }
        //



    }
}