using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �Ƃ肠����NetworkObject�Ƃ���B
/// �����d��������A�����g�𒴂���悤�Ȃ�ATickAligned��RPC�Ńg���K�[�����ʐM��������ɂ���
/// �Q�l�Fhttps://docs.google.com/presentation/d/1kGN7ZEleBgpXuXnUin8y67LmXrmQuAtbgu4rz3QSY6U/edit#slide=id.g1592fa1edef_0_25
/// </summary>
public class Bullet : NetworkBehaviour
{
    Rigidbody rb;

    [Networked] TickTimer Life { get; set; }

    Rigidbody Rb
    {
        get
        {
            if (rb == null) rb = GetComponent<Rigidbody>();
            return rb;
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
