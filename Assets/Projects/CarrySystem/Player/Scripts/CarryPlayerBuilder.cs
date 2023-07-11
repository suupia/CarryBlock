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
        // ほかにも _carryPlayerModelLoader とか _carryPlayerViewLoader などがありそう

        [Inject]
        public CarryPlayerBuilder(NetworkRunner runner, IObjectResolver resolver ,IPrefabLoader<CarryPlayerController_Net> carryPlayerControllerLoader)
        {
            _runner = runner;
            _resolver = resolver;
            _carryPlayerControllerLoader = carryPlayerControllerLoader;
            
        }

        public void Build(PlayerColorType colorType)
        {
            var playerController = _carryPlayerControllerLoader.Load();
            var holdPresenter = playerController.GetComponent<HoldPresenter_Net>();
            
            //ToDo 以下をcolorTypeによって切り替える
            var move = new QuickTurnMove();
            var action = new HoldAction(holdPresenter);
            var character = new Character(move, action);

            var playerControllerObj = _runner.Spawn(playerController).gameObject;
            
            _resolver.InjectGameObject(playerControllerObj);
        }
        
    }
}