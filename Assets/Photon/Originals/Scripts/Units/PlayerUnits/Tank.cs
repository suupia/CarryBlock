using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class Tank : NetworkPlayerUnit
{
    [SerializeField] NetworkPrefabRef picker;
    [SerializeField] NetworkPrefabRef bullet;


    [Networked] TickTimer ReloadTimer { get; set; }

    [Networked] float Speed { get; set; } = 10f;


    NetworkCharacterControllerPrototype cc;

    public override void Spawned()
    {
        base.Spawned();
        cc = GetComponent<NetworkCharacterControllerPrototype>();
    }

    public override void Move(Vector3 direction)
    {
        cc.Move(direction * Speed);

    }

    public override void Action(NetworkButtons buttons, NetworkButtons preButtons)
    {
        if (buttons.GetPressed(preButtons).IsSet(PlayerOperation.Fire))
        {
            if (ReloadTimer.ExpiredOrNotRunning(Runner))
            {
                //�ꎞ�I�ɃI�t�Z�b�g��K�p
                //�����I�ɂ́A��Ԃ��㕔�Ɖ����ŕʂɂ��āA�㕔�����ˈʒu���Ǘ�����悤�ɂ��悤����
                var offset = new Vector3(0, 1f, 0);
                Runner.Spawn(bullet, transform.position + offset, transform.rotation, PlayerRef.None, OnBeforeSpawnBullet);
                ReloadTimer = TickTimer.CreateFromSeconds(Runner, 2f);
            }
        }
        if (buttons.GetPressed(preButtons).IsSet(PlayerOperation.MainAction))
        {
            Runner.Spawn(picker, transform.position, transform.rotation, PlayerRef.None);
        }
    }

    void OnBeforeSpawnBullet(NetworkRunner runner, NetworkObject obj)
    {
        var bullet = obj.GetComponent<Bullet>();
        bullet.AddForce(transform.forward);
    }
}
