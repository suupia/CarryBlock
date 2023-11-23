using Carry.CarrySystem.CG.Tsukinowa;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.Player.Info;
using Carry.CarrySystem.Player.Interfaces;
using Projects.Utility.Scripts;
using UnityEngine;
#nullable enable


namespace Carry.CarrySystem.Player.Scripts
{
    [RequireComponent(typeof(Rigidbody))]
    public class CarryPlayerControllerLocal : MonoBehaviour , IPlayerController
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
        public PlayerInfo Info => _info;

        // class of Character 
        public PlayerHoldingObjectContainer GetPlayerHoldingObjectContainer => _blockContainer;
        public IMoveExecutorSwitcher GetMoveExecutorSwitcher => _moveExecutorSwitcher;
        public IHoldActionExecutor GetHoldActionExecutor => _holdActionExecutor;
        public IOnDamageExecutor GetOnDamageExecutor => _onDamageExecutor;
        public IDashExecutor GetDashExecutor => _dashExecutor;
        public IPassActionExecutor GetPassActionExecutor => _passActionExecutor;

        // Serialize Field 
        [SerializeField]
        Transform unitObjectParent = null!; // The NetworkCharacterControllerPrototype interpolates this transform.
        [SerializeField] GameObject[] playerUnitPrefabs = null!;

        // private fields 
        PlayerColorType ColorType { get; set; } // ローカルに反映させるために必要
        GameObject _characterObj = null!;
        
        Rigidbody? _rigidbody;
        
        PlayerInfo _info = null!;
        
        PlayerHoldingObjectContainer _blockContainer = null!;
        IMoveExecutorSwitcher _moveExecutorSwitcher = null!;
        IHoldActionExecutor _holdActionExecutor = null!;
        IOnDamageExecutor _onDamageExecutor = null!;
        IDashExecutor _dashExecutor = null!;
        IPassActionExecutor _passActionExecutor = null!;
        
        LocalLocalInputPoller _localLocalInputPoller;
        IMapGetter _mapGetter;
        LocalButtons _preButtons;

        public void Init(
            PlayerHoldingObjectContainer blockContainer,
            IMoveExecutorSwitcher moveExecutorSwitcher,
            IHoldActionExecutor holdActionExecutor,
            IOnDamageExecutor onDamageExecutor,
            IDashExecutor dashExecutor,
            IPassActionExecutor passActionExecutor,
            PlayerColorType colorType,
            IMapGetter mapGetter
        )
        {
            _blockContainer = blockContainer;
            _moveExecutorSwitcher = moveExecutorSwitcher;
            _holdActionExecutor = holdActionExecutor;
            _passActionExecutor = passActionExecutor;
            _dashExecutor = dashExecutor;
            _onDamageExecutor = onDamageExecutor!;
            ColorType = colorType;
            _mapGetter = mapGetter;

            // _mapUpdater.RegisterResetAction(() => Reset(_mapUpdater.GetMap()));
            SetUp();
            
            Spawned();
        }


        void Spawned()
        {
            // Debug.Log($"CarryPlayerController_Net.Spawned(), _character = {Character}");

            // init info
            _info = new PlayerInfo(this);

            // Instantiate the character.
            InstantiateCharacter();


            if (_mapGetter != null)
                ToSpawnPosition(_mapGetter.GetMap()); // Init()がOnBeforeSpawned()よりも先に呼ばれるため、_mapUpdaterは受け取れているはず
            else
                Debug.LogError($"_mapUpdater is null");
            
            _localLocalInputPoller = new LocalLocalInputPoller();
        }

        public void FixedUpdate()
        {
        
            if (_localLocalInputPoller.GetInput(out LocalInputData input))
            {
                Debug.Log($"GEtInput!");

                if (input.Buttons.WasPressed(_preButtons,PlayerOperation.MainAction))
                {
                    Debug.Log($"Start HoldAction!");
                    _holdActionExecutor.HoldAction();
                    // _decorationDetector.OnMainAction(ref DecorationDataRef);
                }
                
                if (input.Buttons.WasPressed(_preButtons,PlayerOperation.Dash))
                {
                    _dashExecutor.Dash();
                }
            
            
                var direction = new Vector3(input.Horizontal, 0, input.Vertical).normalized;
            
                // Debug.Log($"_character = {_character}");
                _moveExecutorSwitcher.Move( direction);
                
                if (input.Buttons.WasPressed(_preButtons,PlayerOperation.MainAction))
                {
                    // AidKit
                    // var isNear =  _playerNearCartHandler.IsNearCart(info.PlayerObj);  todo : _playerNearCartHandlerを追加すべきかどうか考える
                    // Debug.Log($"isNear = {isNear}");
                }

                if (input.Buttons.WasPressed(_preButtons,PlayerOperation.Pass))
                {
                    _passActionExecutor.PassAction();
                }

                _preButtons = input.Buttons;
            }
        }
        
        
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
        
        public void Reset(EntityGridMap map)
        {
            // フロア移動の際に呼ばれる
            // Character?.Reset();
            _holdActionExecutor.Reset();
            _passActionExecutor.Reset();
            ToSpawnPosition(map);
        }
        
        
        
        void ToSpawnPosition(EntityGridMap map)
        {
            // var spawnGridPos = new Vector2Int(1, map.Height / 2 + _playerCharacterTransporter.GetPlayerIndex(info.PlayerRef) -1);
            var spawnGridPos = new Vector2Int(1, map.Height / 2);
            var spawnWorldPos = GridConverter.GridPositionToWorldPosition(spawnGridPos);
            var height = 0.5f;  // 地面をすり抜けないようにするために、少し上に移動させておく（Spawnとの調整は後回し）
            _info.PlayerObj.transform.position = new Vector3(spawnWorldPos.x, height, spawnWorldPos.z);
            _info.PlayerRb.velocity = Vector3.zero;
            
        }
        
        protected void InstantiateCharacter()
        {
            // Instantiate the unit.
            var prefab = playerUnitPrefabs[(int)ColorType];
            _characterObj = Instantiate(prefab, unitObjectParent);
            
            // Character?.Setup(info);
            _characterObj.GetComponent<TsukinowaMaterialSetter>().SetClothMaterial(ColorType);
            var animatorPresenter = GetComponent<IPlayerAnimatorPresenter>();
            if(animatorPresenter != null) animatorPresenter.SetAnimator(_characterObj.GetComponentInChildren<Animator>());
            
            // Play spawn animation
            // _decorationDetector.OnSpawned();
        }
        
        void SetUp()
        {
            // init info
            var info = new PlayerInfo(this);
            _moveExecutorSwitcher.Setup(info);
            _holdActionExecutor. Setup(info);
            _passActionExecutor.Setup(info);
            _onDamageExecutor.Setup(info);
            info.PlayerRb.useGravity = true;
            _info = info;
        }
        



    }
}