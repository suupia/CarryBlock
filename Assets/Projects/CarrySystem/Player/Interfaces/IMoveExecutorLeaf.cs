
#nullable enable
namespace Carry.CarrySystem.Player.Interfaces
{
    public interface IMoveExecutorLeaf : IMoveExecutor
    {
        public float Acceleration { get; }
        public float MaxVelocity { get; }
        public float StoppingForce { get; }
    }
}