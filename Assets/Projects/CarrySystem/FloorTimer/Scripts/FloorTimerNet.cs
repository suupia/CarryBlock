using Fusion;
using VContainer;

namespace Carry.CarrySystem.FloorTimer.Scripts
{
    public class FloorTimerNet : NetworkBehaviour
    {
        GameContext _gameContext;
        [Networked] TickTimer FloorTickTimer { get; set; }

        [Inject]
        public void Construct(GameContext gameContext)
        {
            _gameContext = gameContext;
        }


        public void StartTimer()
        {
            FloorTickTimer = TickTimer.CreateFromSeconds(Runner, _gameContext.CurrentFloorLimitTime);
        }

        public override void FixedUpdateNetwork()
        {
            _gameContext.CurrentFloorTime = FloorTickTimer.RemainingTime(Runner).GetValueOrDefault();
        }
    }
}