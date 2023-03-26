using Fusion;
using System;
using System.Linq;
using UnityEngine;

public class NetworkEnemy : NetworkBehaviour
{

    NetworkCharacterControllerPrototype cc;

    [Networked] protected Vector3 Direction { get; set; }
    [Networked] protected float Speed { get; set; } = 5f;

    public Action OnDespawn = () => { };

    public override void Spawned()
    {
        cc = GetComponent<NetworkCharacterControllerPrototype>();

    }

    public override void FixedUpdateNetwork()
    {
        cc.Move(Direction * Speed);
    }

    //��U�ȒP�ȃ��f���Ŏ�������
    //�����NetworkManager�ɂ���ČĂ΂�A���g�̐i�ނׂ����������߂�
    //�ς���\��������
    public void SetDirection(NetworkPlayerUnit[] playerUnits)
    {
        //�Ƃ肠�����A��ԋ߂��v���C���[�Ɍ������B���d���������ɂȂ邩
        int minIndex = 0;
        float min = float.MaxValue;
        for (int i = 0; i < playerUnits.Length; i++)
        {
            var _min = Vector3.Distance(playerUnits[i].transform.position, transform.position);
            if (_min < min )
            {
                min = _min;
                minIndex = i;
            }
        }
        
        var target = playerUnits[minIndex].transform.position;
        Direction = target - transform.position;
    }

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        OnDespawn.Invoke();
    }
}
