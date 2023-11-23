#nullable enable
namespace Carry.CarrySystem.Player.Interfaces
{
    public interface IMoveParameter
    {
        public float Acceleration { get; set; }
        public float MaxVelocity { get; set; }
        public float StoppingForce { get; set; }
        
        public IMoveParameter Chain(IMoveParameter nextMoveParameter);
    }
}