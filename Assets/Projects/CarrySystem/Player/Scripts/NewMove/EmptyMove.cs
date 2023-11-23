#nullable enable
using Carry.CarrySystem.Player.Interfaces;
using UnityEngine;

namespace Carry.CarrySystem.Player.Scripts
{
    public class EmptyMove : IMoveParameter , IMoveFunction
    {
        public float Acceleration { get;  }
        public float MaxVelocity { get;  }
        public float StoppingForce { get;  }

        public void Move(Vector3 input)
        {
            
        }
    }
}