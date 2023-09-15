using Carry.CarrySystem.Cart.Scripts;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Player.Interfaces;
using UnityEngine;
using VContainer;
#nullable enable

namespace Carry.CarrySystem.Player.Scripts
{
    // ToDo: このクラス使っていないならいらない？
    // 何かPlayerのドメインを差し替えたいときに使っていたのかも
    public class MockPlayerFactory : ICarryPlayerFactory
    {
        readonly IMapUpdater _mapUpdater;
        readonly PlayerNearCartHandlerNet _playerNearCartHandler;
        [Inject]
        public MockPlayerFactory(
            IMapUpdater mapUpdater,
            PlayerNearCartHandlerNet playerNearCartHandler
            )
        {
            _mapUpdater = mapUpdater;
            _playerNearCartHandler = playerNearCartHandler;
        }
        public ICharacter Create(PlayerColorType colorType)
        {
            // ToDo: switch文で分ける
            var moveExeSwitcher = new MoveExecutorSwitcher();
            var blockContainer = new PlayerHoldingObjectContainer();
            var holdExe = new HoldActionExecutor(blockContainer,_playerNearCartHandler,_mapUpdater);
            var dashExe = new DashExecutor(moveExeSwitcher);
            var passExe = new PassActionExecutor(blockContainer, holdExe,10, LayerMask.GetMask("Player"));
            var onDamageExe = new OnDamageExecutor(moveExeSwitcher);
            var character = new Character( moveExeSwitcher, holdExe,dashExe, passExe,onDamageExe, blockContainer);
            return character;
        }
    }
}