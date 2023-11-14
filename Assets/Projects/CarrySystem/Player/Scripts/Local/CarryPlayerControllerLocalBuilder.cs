#nullable enable

using Carry.CarrySystem.Cart.Scripts;
using Carry.CarrySystem.FloorTimer.Scripts;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Player.Interfaces;
using Carry.Utility.Interfaces;
using UnityEngine;
using VContainer;

namespace Carry.CarrySystem.Player.Scripts.Local
{
    public class CarryPlayerControllerLocalBuilder
    {
        readonly IMapUpdater _mapUpdater;
        readonly IPrefabLoader<CarryPlayerControllerLocal> _carryPlayerControllerLoader;
        readonly ICarryPlayerFactory _carryPlayerFactory;
        // ほかにも _carryPlayerModelLoader とか _carryPlayerViewLoader などが想定される
        readonly PlayerCharacterTransporter _playerCharacterTransporter;
        readonly PlayerNearCartHandlerNet _playerNearCartHandler;
        readonly CarryPlayerContainer _carryPlayerContainer;
        readonly FloorTimerNet _floorTimerNet;

        [Inject]
        public CarryPlayerControllerLocalBuilder(
            IMapUpdater  mapUpdater ,
            IPrefabLoader<CarryPlayerControllerLocal> carryPlayerControllerLoader,
            ICarryPlayerFactory carryPlayerFactory,
            PlayerCharacterTransporter playerCharacterTransporter,
            PlayerNearCartHandlerNet playerNearCartHandler,
            CarryPlayerContainer carryPlayerContainer,
            FloorTimerNet floorTimerNet
            )
        {
            _mapUpdater = mapUpdater;
            _carryPlayerControllerLoader = carryPlayerControllerLoader;
            _carryPlayerFactory = carryPlayerFactory;
            _playerCharacterTransporter  = playerCharacterTransporter;
            _playerNearCartHandler = playerNearCartHandler;
            _carryPlayerContainer = carryPlayerContainer;
            _floorTimerNet = floorTimerNet;
        }

        public CarryPlayerControllerLocal Build(Vector3 position, Quaternion rotation)
        {
            // プレハブをロード
            var playerController = _carryPlayerControllerLoader.Load();
            
            // ドメインスクリプトをnew
            var colorType = PlayerColorType.Red;  // いったんこれで固定
            var character = _carryPlayerFactory.CreateCharacter();
            
            // プレハブをスポーン
            var playerControllerObj = UnityEngine.Object.Instantiate(playerController,position, rotation);
            playerControllerObj.GetComponent<CarryPlayerControllerLocal>().Init(character.PlayerHoldingObjectContainer, character,character,character,character,character, colorType,_mapUpdater, _playerNearCartHandler, _playerCharacterTransporter,_floorTimerNet);
            playerControllerObj.GetComponent<PlayerBlockPresenterNet>()?.Init(character,character);
            playerControllerObj.GetComponent<PlayerAidKitPresenterNet>()?.Init(character);
            playerControllerObj.GetComponent<PlayerAnimatorPresenterNet>()?.Init(character,character,character,character);
            playerControllerObj.GetComponentInChildren<DashEffectPresenter>()?.Init(character);
            playerControllerObj.GetComponentInChildren<ReviveEffectPresenter>()?.Init(character);
            playerControllerObj.GetComponentInChildren<PassBlockMoveExecutorNet>()?.Init(character);
      
            var info = playerControllerObj.Info;
            playerControllerObj.GetComponentInChildren<PassRangeNet>().Init(info,LayerMask.GetMask("Player"));
            playerControllerObj.GetComponentInChildren<AidKitRangeNet>().Init(info,LayerMask.GetMask("Player"));

            // Factoryの差し替えが簡単にできるので、_resolver.InjectGameObjectを使う必要はない
            // BuilderとPlayerControllerが蜜結合なのは問題ないはず

            return playerControllerObj;
        }

    }
}