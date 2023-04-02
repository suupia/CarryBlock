using Fusion;
using System;
using System.Linq;
using UnityEngine;

public class NetworkEnemy : NetworkBehaviour
{

    NetworkCharacterControllerPrototype cc;

    [Networked] protected Vector2 Direction { get; set; }
    [Networked] protected float Speed { get; set; } = 5f;

    [Networked] protected int Hp { get; set; } = 1;

    public Action OnDespawn = () => { };

    public override void Spawned()
    {
        cc = GetComponent<NetworkCharacterControllerPrototype>();

    }

    public override void FixedUpdateNetwork()
    {
        if (Hp <= 0)
        {
            Runner.Despawn(Object);
            return;
        }
        cc.Move(new Vector3(Direction.x, 0, Direction.y) * Speed);
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
            if (_min < min)
            {
                min = _min;
                minIndex = i;
            }
        }

        var target = playerUnits[minIndex].transform.position;
        var direction = target - transform.position;
        Direction = new Vector2(direction.x, direction.z);
    }

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        OnDespawn.Invoke();
    }

    
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Harmful"))
        {
            Hp -= 1;
        }
    }
}
