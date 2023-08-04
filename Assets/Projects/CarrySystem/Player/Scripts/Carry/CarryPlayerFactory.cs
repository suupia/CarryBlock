using Carry.CarrySystem.Player.Interfaces;
using UnityEngine;
using VContainer;
#nullable enable

namespace Carry.CarrySystem.Player.Scripts
{
    // ToDo: このクラス使っていないならいらない？
    // 何かPlayerのドメインを差し替えたいときに使っていたのかも
    public class CarryPlayerFactory : ICarryPlayerFactory
    {
        readonly IObjectResolver _resolver;
        [Inject]
        public CarryPlayerFactory(IObjectResolver resolver)
        {
            _resolver = resolver;
        }
        public ICharacter Create(PlayerColorType colorType)
        {
            // ToDo: switch文で分ける
            var moveExe = new QuickTurnMoveExecutor();
            var holdExe = new HoldActionExecutorExecutor(_resolver);
            var passExe = new PassActionExecutor(holdExe,10, LayerMask.GetMask("Player"));
            var character = new Character(moveExe, holdExe, passExe);
            return character;
        }
    }
}