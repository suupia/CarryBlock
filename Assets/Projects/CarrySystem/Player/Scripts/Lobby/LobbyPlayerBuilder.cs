using Carry.CarrySystem.Player.Interfaces;
using Fusion;
using Projects.Utility.Scripts;
using UnityEngine;
using VContainer;

namespace Carry.CarrySystem.Player.Scripts
{
    public class LobbyPlayerBuilder : IPlayerBuilder
    {
        readonly NetworkRunner _runner;
        readonly IPrefabLoader<LobbyPlayerControllerNet> _carryPlayerControllerLoader;
        readonly ICarryPlayerFactory _carryPlayerFactory;
        // ほかにも _carryPlayerModelLoader とか _carryPlayerViewLoader などが想定される

        [Inject]
        public LobbyPlayerBuilder(NetworkRunner runner ,IPrefabLoader<LobbyPlayerControllerNet> carryPlayerControllerLoader, ICarryPlayerFactory carryPlayerFactory)
        {
            _runner = runner;
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
                    networkObj.GetComponent<LobbyPlayerControllerNet>().Init(character,colorType);
                   //  networkObj.GetComponent<HoldPresenter_Net>().Init(character);
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