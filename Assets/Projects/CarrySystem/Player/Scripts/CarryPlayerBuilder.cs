using Fusion;
using Nuts.Utility.Scripts;
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

        public void Spawn(PlayerColorType colorType)
        {
            // 各MonoBehaviourを準備
            var playerController = _carryPlayerControllerLoader.Load();
            
            // ドメインスクリプトを準備
            var character = _carryPlayerFactory.Create(colorType);
            
            var playerControllerObj = _runner.Spawn(playerController);
            
            // 各MonoBehaviourにドメインを設定
            playerControllerObj.Init(character);
            var holdPresenter = playerControllerObj.GetComponent<HoldPresenter_Net>();
            holdPresenter.Init(character);

            // 今は特に注入するべきものがない
            // _resolver.InjectGameObject(playerControllerObj);
        }
        
        // Buildの流れ
        // ControllerをLoadする
        // ドメインスクリプトをnewする（ファクトリーから生成でもよい） <- これをInjectで受け取るようにする
        // Presenterにドメインを設定する
        
    }
}