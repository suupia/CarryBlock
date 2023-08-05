using Carry.CarrySystem.Player.Interfaces;
using Fusion;
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
        readonly IObjectResolver _resolver;
        readonly IPrefabLoader<CarryPlayerControllerNet> _carryPlayerControllerLoader;
        readonly ICarryPlayerFactory _carryPlayerFactory;
        // ほかにも _carryPlayerModelLoader とか _carryPlayerViewLoader などが想定される

        [Inject]
        public CarryPlayerBuilder(NetworkRunner runner, IObjectResolver resolver ,IPrefabLoader<CarryPlayerControllerNet> carryPlayerControllerLoader, ICarryPlayerFactory carryPlayerFactory)
        {
            _runner = runner;
            _resolver = resolver;
            _carryPlayerControllerLoader = carryPlayerControllerLoader;
            _carryPlayerFactory = carryPlayerFactory;
        }

        public AbstractNetworkPlayerController Build(PlayerColorType colorType, Vector3 position, Quaternion rotation, PlayerRef playerRef)
        {
            // プレハブをロード
            var playerController = _carryPlayerControllerLoader.Load();
            
            // ドメインスクリプトをnew
            var character = _carryPlayerFactory.Create(colorType);
            
            // プレハブをスポーン
            var playerControllerObj = _runner.Spawn(playerController,position, rotation, playerRef,
                (runner, networkObj) =>
                {
                    Debug.Log($"OnBeforeSpawn: {networkObj}, carryPlayerControllerObj");
                    networkObj.GetComponent<CarryPlayerControllerNet>().Init(character,colorType);
                    networkObj.GetComponent<PlayerBlockPresenterNet>().Init(character);
                });
            
            // 各MonoBehaviourにドメインを設定
            // playerControllerObj.Init(character);
            // var holdPresenter = playerControllerObj.GetComponent<HoldPresenter_Net>();
            // holdPresenter.Init(character);
            
            // Factoryの差し替えが簡単にできるので、_resolver.InjectGameObjectを使う必要はない
            // BuilderとPlayerControllerが蜜結合なのは問題ないはず

            return playerControllerObj;
        }

        
    }
}