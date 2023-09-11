using Carry.CarrySystem.Cart.Scripts;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Player.Interfaces;
using Fusion;
using Projects.Utility.Interfaces;
using Projects.Utility.Scripts;
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

    public class CarryPlayerBuilder : IPlayerBuilder
    {
        readonly NetworkRunner _runner;
        readonly IMapUpdater _mapUpdater;
        readonly IPrefabLoader<CarryPlayerControllerNet> _carryPlayerControllerLoader;
        readonly ICarryPlayerFactory _carryPlayerFactory;
        // ほかにも _carryPlayerModelLoader とか _carryPlayerViewLoader などが想定される
        readonly PlayerCharacterHolder _playerCharacterHolder;
        readonly CarryPlayerContainer _carryPlayerContainer;

        [Inject]
        public CarryPlayerBuilder(
            NetworkRunner runner,
            IMapUpdater  mapUpdater ,
            IPrefabLoader<CarryPlayerControllerNet> carryPlayerControllerLoader,
            ICarryPlayerFactory carryPlayerFactory,
            PlayerCharacterHolder playerCharacterHolder,
            CarryPlayerContainer carryPlayerContainer
            )
        {
            _runner = runner;
            _mapUpdater = mapUpdater;
            _carryPlayerControllerLoader = carryPlayerControllerLoader;
            _carryPlayerFactory = carryPlayerFactory;
            _playerCharacterHolder  = playerCharacterHolder;
            _carryPlayerContainer = carryPlayerContainer;
        }

        public AbstractNetworkPlayerController Build(Vector3 position, Quaternion rotation, PlayerRef playerRef)
        {
            // プレハブをロード
            var playerController = _carryPlayerControllerLoader.Load();
            
            // ドメインスクリプトをnew
            var colorType = _playerCharacterHolder.GetPlayerColorType(playerRef);
            var character = _carryPlayerFactory.Create(colorType);
            
            // プレハブをスポーン
            var playerControllerObj = _runner.Spawn(playerController,position, rotation, playerRef,
                (runner, networkObj) =>
                {
                    Debug.Log($"OnBeforeSpawn: {networkObj}, carryPlayerControllerObj");
                    networkObj.GetComponent<CarryPlayerControllerNet>().Init(character,colorType,_mapUpdater);
                    networkObj.GetComponent<PlayerBlockPresenterNet>()?.Init(character);
                    networkObj.GetComponent<PlayerAidKitPresenterNet>()?.Init(character);
                    networkObj.GetComponent<PlayerAnimatorPresenterNet>()?.Init(character);
                });
            var info = playerControllerObj.Info;
            playerControllerObj.GetComponentInChildren<PassRangeNet>().Init(info,LayerMask.GetMask("Player"));

            
            _carryPlayerContainer.AddPlayer(playerRef, playerControllerObj);

            // Factoryの差し替えが簡単にできるので、_resolver.InjectGameObjectを使う必要はない
            // BuilderとPlayerControllerが蜜結合なのは問題ないはず

            return playerControllerObj;
        }

        
    }
}