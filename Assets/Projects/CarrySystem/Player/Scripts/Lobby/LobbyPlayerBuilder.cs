using Carry.CarrySystem.Player.Interfaces;
using Fusion;
using Carry.Utility.Interfaces;
using Carry.Utility.Scripts;
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
        readonly PlayerCharacterTransporter _playerCharacterTransporter;

        [Inject]
        public LobbyPlayerBuilder(
            NetworkRunner runner ,
            IPrefabLoader<LobbyPlayerControllerNet> carryPlayerControllerLoader, 
            ICarryPlayerFactory carryPlayerFactory,
            PlayerCharacterTransporter playerCharacterTransporter
            )
        {
            _runner = runner;
            _carryPlayerControllerLoader = carryPlayerControllerLoader;
            _carryPlayerFactory = carryPlayerFactory;
            _playerCharacterTransporter  = playerCharacterTransporter;
        }

        public AbstractNetworkPlayerController Build( Vector3 position, Quaternion rotation, PlayerRef playerRef)
        {
            // プレハブをロード
            var playerController = _carryPlayerControllerLoader.Load();
            
            // ドメインスクリプトをnew
            var colorType = _playerCharacterTransporter.GetPlayerColorType(playerRef);
            var character = _carryPlayerFactory.Create(colorType);
            
            // プレハブをスポーン
            var playerControllerObj = _runner.Spawn(playerController,position, rotation, playerRef,
                (runner, networkObj) =>
                {
                    Debug.Log($"OnBeforeSpawn: {networkObj}, carryPlayerControllerObj");
                    networkObj.GetComponent<LobbyPlayerControllerNet>().Init(character,colorType,_playerCharacterTransporter);
                    networkObj.GetComponent<PlayerAnimatorPresenterNet>()?.Init(character);
                    networkObj.GetComponentInChildren<DashEffectPresenter>()?.Init(character);

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