
#nullable enable
namespace Carry.CarrySystem.Player.Interfaces
{
    public interface IMoveExecutorLeaf : IMoveExecutor
    {
        public float Acceleration { get; set; }
        public float MaxVelocity { get; set; }
        public float StoppingForce { get; set; }

        public IMoveExecutorLeaf CreateNewLeaf();
    }
}