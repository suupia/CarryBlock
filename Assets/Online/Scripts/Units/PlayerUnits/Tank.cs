using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows;

public class Tank : NetworkPlayerUnit
{
    [SerializeField] NetworkPrefabRef picker;
    [SerializeField] NetworkPrefabRef bullet;


    [Networked] TickTimer ReloadTimer { get; set; }
    [Networked] NetworkObject Target { get; set; }

    NetworkCharacterControllerPrototype cc;
    RangeDetector rangeDetector;

    public override void Spawned()
    {
        base.Spawned();
        cc = GetComponent<NetworkCharacterControllerPrototype>();
        rangeDetector = GetComponentInChildren<RangeDetector>();
    }

    public override void Move(Vector3 direction)
    {
        cc.Move(direction);

    }

    public override void Action(NetworkButtons buttons, NetworkButtons preButtons)
    {
        if (ReloadTimer.ExpiredOrNotRunning(Runner))
        {
            //Auto Aim
            //��U�^�O�Ŏ��ʁA�O���̃X�N���v�g�Ńg���K�[�Ǘ��B�ˑ��֌W�����炵�����Ȃ�Physics�n���g���Ă��ǂ�����
            //���̏ꍇ��Layer�������������肵�Ă�������
            //�����ꂱ�̏����͏���������ׂ�
            var enemies =  rangeDetector.GameObjects.Where(o => o != null && o.CompareTag("Enemy")).ToArray();
            if (enemies.Length > 0)
            {
                Target = enemies.First().GetComponent<NetworkObject>();

                //�ꎞ�I�ɒe�̔��ˈʒu�̂��߂ɃI�t�Z�b�g��K�p
                //�����I�ɂ́A��Ԃ��㕔�Ɖ����ŕʂɂ��āA�㕔�����ˈʒu���Ǘ�����悤�ɂ��悤����
                var offset = new Vector3(0, 1.2f, 0);
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
        bullet.AddForce(Target.transform.position - transform.position);
    }
}
