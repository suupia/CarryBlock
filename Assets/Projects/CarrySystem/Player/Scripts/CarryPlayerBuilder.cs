using Fusion;
using Nuts.Utility.Scripts;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Carry.CarrySystem.Player.Scripts
{
    public enum PlayerColorType
    {
        Red,
        Blue,
        Green,
        Yellow
    }

    public class CarryPlayerBuilder
    {
        readonly NetworkRunner _runner;
        readonly IObjectResolver _resolver;
        readonly IPrefabLoader<CarryPlayerController_Net> _carryPlayerControllerLoader;
        readonly CarryPlayerFactory _carryPlayerFactory;
        // ほかにも _carryPlayerModelLoader とか _carryPlayerViewLoader などがありそう

        [Inject]
        public CarryPlayerBuilder(NetworkRunner runner, IObjectResolver resolver ,IPrefabLoader<CarryPlayerController_Net> carryPlayerControllerLoader, CarryPlayerFactory carryPlayerFactory)
        {
            _runner = runner;
            _resolver = resolver;
            _carryPlayerControllerLoader = carryPlayerControllerLoader;
            _carryPlayerFactory = carryPlayerFactory;
        }

        public CarryPlayerController_Net Build(PlayerColorType colorType, Vector3 position, Quaternion rotation, PlayerRef playerRef)
        {
            // 各MonoBehaviourを準備
            var playerController = _carryPlayerControllerLoader.Load();
            
            // ドメインスクリプトを準備
            var character = _carryPlayerFactory.Create(colorType);
            
            // Spawn
            var playerControllerObj = _runner.Spawn(playerController,position, rotation, playerRef);
            
            // 各MonoBehaviourにドメインを設定
            playerControllerObj.Init(character);
            var holdPresenter = playerControllerObj.GetComponent<HoldPresenter_Net>();
            holdPresenter.Init(character);

            // 今は特に注入するべきものがない
            // _resolver.InjectGameObject(playerControllerObj);

            return playerControllerObj;
        }
        
        // Buildの流れ
        // ControllerをLoadする
        // ドメインスクリプトをnewする（ファクトリーから生成でもよい） <- これをInjectで受け取るようにする
        // Presenterにドメインを設定する
        
    }
}