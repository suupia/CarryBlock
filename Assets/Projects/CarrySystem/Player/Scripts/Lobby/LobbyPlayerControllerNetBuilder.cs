using Carry.CarrySystem.Player.Interfaces;
using Fusion;
using Carry.Utility.Interfaces;
using Carry.Utility.Scripts;
using UnityEngine;
using VContainer;

namespace Carry.CarrySystem.Player.Scripts
{
    public class LobbyPlayerControllerNetBuilder : IPlayerControllerNetBuilder
    {
        readonly NetworkRunner _runner;
        readonly IPrefabLoader<LobbyPlayerControllerNet> _carryPlayerControllerLoader;
        readonly ICarryPlayerFactory _carryPlayerFactory;
        // ほかにも _carryPlayerModelLoader とか _carryPlayerViewLoader などが想定される
        readonly PlayerCharacterTransporter _playerCharacterTransporter;
        readonly LobbyPlayerContainer _lobbyPlayerContainer;

        [Inject]
        public LobbyPlayerControllerNetBuilder(
            NetworkRunner runner ,
            IPrefabLoader<LobbyPlayerControllerNet> carryPlayerControllerLoader, 
            ICarryPlayerFactory carryPlayerFactory,
            PlayerCharacterTransporter playerCharacterTransporter,
            LobbyPlayerContainer lobbyPlayerContainer
            )
        {
            _runner = runner;
            _carryPlayerControllerLoader = carryPlayerControllerLoader;
            _carryPlayerFactory = carryPlayerFactory;
            _playerCharacterTransporter  = playerCharacterTransporter;
            _lobbyPlayerContainer = lobbyPlayerContainer;
        }

        public AbstractNetworkPlayerController Build( Vector3 position, Quaternion rotation, PlayerRef playerRef)
        {
            // プレハブをロード
            var playerController = _carryPlayerControllerLoader.Load();
            
            // ドメインスクリプトをnew
            var colorType = _playerCharacterTransporter.GetPlayerColorType(playerRef);
            var character = _carryPlayerFactory.CreateCharacter();
            
            // プレハブをスポーン
            var playerControllerObj = _runner.Spawn(playerController,position, rotation, playerRef,
                (runner, networkObj) =>
                {
                    Debug.Log($"OnBeforeSpawn: {networkObj}, carryPlayerControllerObj");
                    networkObj.GetComponent<LobbyPlayerControllerNet>().Init(character.PlayerHoldingObjectContainer, character,character, character,character,character,colorType,_playerCharacterTransporter);
                    networkObj.GetComponent<PlayerAnimatorPresenterNet>()?.Init(character, character, character,character);
                    networkObj.GetComponentInChildren<DashEffectPresenter>()?.Init(character);

                });
            
            // 各MonoBehaviourにドメインを設定
            // playerControllerObj.Init(character);
            // var holdPresenter = playerControllerObj.GetComponent<HoldPresenter_Net>();
            // holdPresenter.Init(character);
            
            // Factoryの差し替えが簡単にできるので、_resolver.InjectGameObjectを使う必要はない
            // BuilderとPlayerControllerが蜜結合なのは問題ないはず
            _lobbyPlayerContainer.AddPlayer(playerRef, playerControllerObj);
            return playerControllerObj;
        }


    }
}