using System.Collections.Generic;
using Carry.CarrySystem.Player.Info;
using Carry.CarrySystem.Player.Interfaces;

namespace Carry.CarrySystem.Player.Scripts
{
    public class MoveExecutorContainer
    { 
        public IMoveExecutor RegularMoveExecutor => _regularMoveExecutor;
        public IMoveExecutor SlowMoveExecutor => _slowMoveExecutor;
        readonly IMoveExecutor _regularMoveExecutor;
        readonly IMoveExecutor _slowMoveExecutor;
        
        public MoveExecutorContainer(
            )
        {
            _regularMoveExecutor = new CorrectlyStopMoveExecutor();
            _slowMoveExecutor = new SlowMoveExecutor();
        }

        public void Setup(PlayerInfo info)
        {
            _regularMoveExecutor.Setup(info);
            _slowMoveExecutor.Setup(info);
        }
    }
}