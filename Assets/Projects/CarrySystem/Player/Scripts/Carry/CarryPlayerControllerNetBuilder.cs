using Carry.CarrySystem.Cart.Scripts;
using Carry.CarrySystem.FloorTimer.Scripts;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Player.Interfaces;
using Fusion;
using Carry.Utility.Interfaces;
using Carry.Utility.Scripts;
using TMPro;
using UnityEngine;
using VContainer;
using VContainer.Unity;
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
        readonly IMapUpdater _mapUpdater;
        readonly IPrefabLoader<CarryPlayerControllerNet> _carryPlayerControllerLoader;
        readonly ICarryPlayerFactory _carryPlayerFactory;
        // ほかにも _carryPlayerModelLoader とか _carryPlayerViewLoader などが想定される
        readonly PlayerCharacterTransporter _playerCharacterTransporter;
        readonly PlayerNearCartHandlerNet _playerNearCartHandler;
        readonly CarryPlayerContainer _carryPlayerContainer;
        readonly FloorTimerNet _floorTimerNet;

        [Inject]
        public CarryPlayerControllerNetBuilder(
            NetworkRunner runner,
            IMapUpdater  mapUpdater ,
            IPrefabLoader<CarryPlayerControllerNet> carryPlayerControllerLoader,
            ICarryPlayerFactory carryPlayerFactory,
            PlayerCharacterTransporter playerCharacterTransporter,
            PlayerNearCartHandlerNet playerNearCartHandler,
            CarryPlayerContainer carryPlayerContainer,
            FloorTimerNet floorTimerNet
            )
        {
            _runner = runner;
            _mapUpdater = mapUpdater;
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
                        character, character, character, character, character, colorType, _mapUpdater,
                        _playerNearCartHandler, _playerCharacterTransporter, _floorTimerNet);
                    networkObj.GetComponent<PlayerBlockPresenterNet>()?.Init(character, character);
                    networkObj.GetComponent<PlayerAidKitPresenterNet>()?.Init(character);
                    networkObj.GetComponent<PlayerAnimatorPresenterNet>()
                        ?.Init(character, character, character, character);
                    networkObj.GetComponentInChildren<DashEffectPresenter>()?.Init(character);
                    networkObj.GetComponentInChildren<ReviveEffectPresenter>()?.Init(character);
                    networkObj.GetComponentInChildren<PassBlockMoveExecutorNet>()?.Init(character);
                });
            var info = playerControllerObj.Info;
            playerControllerObj.GetComponentInChildren<PassRangeNet>().Init(info,LayerMask.GetMask("Player"));
            playerControllerObj.GetComponentInChildren<AidKitRangeNet>().Init(info,LayerMask.GetMask("Player"));
            
            _carryPlayerContainer.AddPlayer(playerRef, playerControllerObj);

            // Factoryの差し替えが簡単にできるので、_resolver.InjectGameObjectを使う必要はない
            // BuilderとPlayerControllerが蜜結合なのは問題ないはず

            return playerControllerObj;
        }

        
    }
}