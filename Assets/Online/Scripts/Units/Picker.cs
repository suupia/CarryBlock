using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Picker : NetworkBehaviour
{
    [Networked] TickTimer Life { get; set; }

    Rigidbody rb;

    public override void Spawned()
    {
        rb = GetComponent<Rigidbody>();

        if (Object.HasStateAuthority)
        {
            //Tmp life time
            Life = TickTimer.CreateFromSeconds(Runner, 5.0f);
            rb.AddForce(Vector3.up * 1000f);

        }
    }

    public override void FixedUpdateNetwork()
    {
        if (Life.Expired(Runner))
            Runner.Despawn(Object);
    }
}
