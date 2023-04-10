using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// とりあえずNetworkObjectとする。
/// もし重かったり、無料枠を超えるようなら、TickAlignedのRPCでトリガーだけ通信する方式にする
/// 参考：https://docs.google.com/presentation/d/1kGN7ZEleBgpXuXnUin8y67LmXrmQuAtbgu4rz3QSY6U/edit#slide=id.g1592fa1edef_0_25
/// </summary>
public class Bullet : NetworkBehaviour
{
    Rigidbody _rb;

    [Networked] TickTimer Life { get; set; }

    Rigidbody Rb
    {
        get
        {
            if (_rb == null) _rb = GetComponent<Rigidbody>();
            return _rb;
        }
    }

    public override void Spawned()
    {
        if (Object.HasStateAuthority)
        {
            Life = TickTimer.CreateFromSeconds(Runner, 3f);
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (Object.HasStateAuthority)
        {
            if (Life.Expired(Runner)) Runner.Despawn(Object);
        }
    }

    public void AddForce(Vector3 direction, float power = 300f)
    {
        Rb.AddForce(direction * power);
    }
}
