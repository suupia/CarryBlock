using Fusion;
using UnityEngine;

namespace Nuts.BattleSystem.Enemy.Scripts.Player.Scripts
{
    public class Picker : NetworkBehaviour
    {
        Rigidbody _rb;
        [Networked] TickTimer Life { get; set; }

        public override void Spawned()
        {
            _rb = GetComponent<Rigidbody>();

            if (Object.HasStateAuthority)
            {
                //Tmp life time
                Life = TickTimer.CreateFromSeconds(Runner, 5.0f);
                _rb.AddForce(Vector3.up * 1000f);
            }
        }

        public override void FixedUpdateNetwork()
        {
            if (Life.Expired(Runner))
                Runner.Despawn(Object);
        }
    }
}