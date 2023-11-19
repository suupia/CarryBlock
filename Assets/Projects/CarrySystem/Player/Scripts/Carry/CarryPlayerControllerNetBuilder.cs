using Carry.CarrySystem.Cart.Scripts;
using Carry.CarrySystem.FloorTimer.Scripts;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Player.Interfaces;
using Carry.CarrySystem.VFX.Scripts;
using Fusion;
using Carry.Utility.Interfaces;
using UnityEngine;
using VContainer;
#nullable enable

namespace Carry.CarrySystem.Player.Scripts
{
    public enum PlayerColorType
    {
        Red,
        Blue,
        Green,
        Yellow
    }

    public class CarryPlayerControllerNetBuilder : IPlayerControllerNetBuilder
    {
        readonly NetworkRunner _runner;
        readonly IMapSwitcher _mapSwitcher;
        readonly IMapGetter _mapGetter;
        readonly IPrefabLoader<CarryPlayerControllerNet> _carryPlayerControllerLoader;
        readonly ICarryPlayerFactory _carryPlayerFactory;
        readonly PlayerCharacterTransporter _playerCharacterTransporter;
        readonly PlayerNearCartHandlerNet _playerNearCartHandler;
        readonly CarryPlayerContainer _carryPlayerContainer;
        readonly FloorTimerNet _floorTimerNet;

        [Inject]
        public CarryPlayerControllerNetBuilder(
            NetworkRunner runner,
            IMapSwitcher  mapSwitcher ,
            IMapGetter mapGetter,
            IPrefabLoader<CarryPlayerControllerNet> carryPlayerControllerLoader,
            ICarryPlayerFactory carryPlayerFactory,
            PlayerCharacterTransporter playerCharacterTransporter,
            PlayerNearCartHandlerNet playerNearCartHandler,
            CarryPlayerContainer carryPlayerContainer,
            FloorTimerNet floorTimerNet
            )
        {
            _runner = runner;
            _mapSwitcher = mapSwitcher;
            _mapGetter = mapGetter;
            _carryPlayerControllerLoader = carryPlayerControllerLoader;
            _carryPlayerFactory = carryPlayerFactory;
            _playerCharacterTransporter  = playerCharacterTransporter;
            _playerNearCartHandler = playerNearCartHandler;
            _carryPlayerContainer = carryPlayerContainer;
            _floorTimerNet = floorTimerNet;
        }

        public AbstractNetworkPlayerController Build(Vector3 position, Quaternion rotation, PlayerRef playerRef)
        {
            // プレハブをロード
            var playerController = _carryPlayerControllerLoader.Load();
            
            // ドメインスクリプトをnew
            var colorType = _playerCharacterTransporter.GetPlayerColorType(playerRef);
            var character = _carryPlayerFactory.CreateCharacter();
            
            // プレハブをスポーン
            var playerControllerObj = _runner.Spawn(playerController, position, rotation, playerRef,
                (runner, networkObj) =>
                {
                    Debug.Log($"OnBeforeSpawn: {networkObj}, carryPlayerControllerObj");
                    networkObj.GetComponent<CarryPlayerControllerNet>().Init(character.PlayerHoldingObjectContainer,
                        character, character, character, character, character, colorType, _mapSwitcher, _mapGetter,
                        _playerNearCartHandler, _playerCharacterTransporter, _floorTimerNet);
                    networkObj.GetComponent<PlayerHoldablePresenterNet>()?.Init(character, character);
                    networkObj.GetComponent<PlayerAidKitPresenterNet>()?.Init(character, character);
                    networkObj.GetComponent<PlayerAnimatorPresenterNet>()
                        ?.Init(character, character, character, character);
                    networkObj.GetComponentInChildren<DashEffectPresenterNet>()?.Init(character);
                    networkObj.GetComponentInChildren<ReviveEffectPresenter>()?.Init(character);
                    networkObj.GetComponentInChildren<PassBlockMoveExecutorNet>()?.Init(character);
                });
            var info = playerControllerObj.GetInfo;
            playerControllerObj.GetComponentInChildren<PassRangeNet>().Init(info,LayerMask.GetMask("Player"));
            playerControllerObj.GetComponentInChildren<AidKitRangeNet>().Init(info,LayerMask.GetMask("Player"));
            
            _carryPlayerContainer.AddPlayer(playerRef, playerControllerObj);

            // Factoryの差し替えが簡単にできるので、_resolver.InjectGameObjectを使う必要はない
            // BuilderとPlayerControllerが蜜結合なのは問題ないはず

            return playerControllerObj;
        }

        
    }
}