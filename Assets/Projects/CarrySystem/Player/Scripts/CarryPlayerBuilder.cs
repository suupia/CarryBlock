using Carry.CarrySystem.Player.Interfaces;
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
        readonly ICarryPlayerFactory _carryPlayerFactory;
        // ほかにも _carryPlayerModelLoader とか _carryPlayerViewLoader などがありそう

        [Inject]
        public CarryPlayerBuilder(NetworkRunner runner, IObjectResolver resolver ,IPrefabLoader<CarryPlayerController_Net> carryPlayerControllerLoader, ICarryPlayerFactory carryPlayerFactory)
        {
            _runner = runner;
            _resolver = resolver;
            _carryPlayerControllerLoader = carryPlayerControllerLoader;
            _carryPlayerFactory = carryPlayerFactory;
        }

        public CarryPlayerController_Net Build(PlayerColorType colorType, Vector3 position, Quaternion rotation, PlayerRef playerRef)
        {
            // プレハブをロード
            var playerController = _carryPlayerControllerLoader.Load();
            
            // ドメインスクリプトをnew
            var character = _carryPlayerFactory.Create(colorType);
            
            // プレハブをスポーン
            var playerControllerObj = _runner.Spawn(playerController,position, rotation, playerRef);
            
            // 各MonoBehaviourにドメインを設定
            playerControllerObj.Init(character);
            var holdPresenter = playerControllerObj.GetComponent<HoldPresenter_Net>();
            holdPresenter.Init(character);

            // _resolver.InjectGameObject(playerControllerObj);
            // とできるならこの方がよいが、Factoryで生成したものをInjectするのは難しい
            // Factoryの差し替えが簡単にできるので、Injectを使わなくても良い
            // BuilderとPlayerControllerが蜜結合なのは問題ないはず

            return playerControllerObj;
        }

        
    }
}