#nullable enable
namespace Carry.CarrySystem.Player.Interfaces
{
    public interface IMoveParameter
    {
        public float Acceleration { get;  }
        public float MaxVelocity { get;  }
        public float StoppingForce { get;  }
        
    }
}