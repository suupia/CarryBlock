using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Network;
using UnityEngine;
using UnityEngine.Windows;

public class Tank : NetworkPlayerUnit
{
    readonly NetworkRunner　_runner;
    [Networked] TickTimer ReloadTimer { get; set; }
    [Networked] NetworkObject Target { get; set; }

    public override float DelayBetweenActions => 0.1f;

    NetworkCharacterControllerPrototype _cc;
    RangeDetector _rangeDetector;

    float _pickerHeight = 5.0f;
    
    public Tank(NetworkPlayerInfo info) : base(info)
    {
        this.info = info;
        _runner = info.runner;

        _cc = info.networkCharacterController; 
        _rangeDetector = info.rangeDetector;
        
    }

    public override void Move(Vector3 direction)
    {
        _cc.Move(direction);
    }

    public override void Action()
    {
        Debug.Log($"Actionを行います！");

        if (ReloadTimer.ExpiredOrNotRunning(_runner))
        {
            //Auto Aim
            //一旦タグで識別、外部のスクリプトでトリガー管理。依存関係を減らしたいならPhysics系を使っても良さそう
            //その場合はLayer分けもしっかりしていきたい
            //いずれこの処理は書き換えるべき
            var enemies =  _rangeDetector.GameObjects.Where(o => o != null && o.CompareTag("Enemy")).ToArray();
            if (enemies.Length > 0)
            {
                Target = enemies.First().GetComponent<NetworkObject>();

                //一時的に弾の発射位置のためにオフセットを適用
                //将来的には、戦車を上部と下部で別にして、上部が発射位置を管理するようにしようかな
                var offset = new Vector3(0, 1.2f, 0);
                Debug.Log($"info.bulletPrefab = {info.bulletPrefab}");
                Debug.Log($"info.unitObject = {info.unitObject}");
                Debug.Log($"_runner = {_runner}");
                _runner.Spawn(info.bulletPrefab, info.unitObject.transform.position + offset, info.unitObject.transform.rotation, PlayerRef.None, OnBeforeSpawnBullet);
                ReloadTimer = TickTimer.CreateFromSeconds(_runner, 2f);
            }
        }

        var pickerPos = info.unitObject.transform.position + new Vector3(0, _pickerHeight, 0);
        var picker = _runner.Spawn(info.pickerPrefab, pickerPos,  Quaternion.identity, PlayerRef.None).GetComponent<NetworkPickerController>();
        picker.Init(_runner,info.unitObject.gameObject, info.playerInfoForPicker);

    }
    

    void OnBeforeSpawnBullet(NetworkRunner runner, NetworkObject obj)
    {
        var bullet = obj.GetComponent<NetworkBulletController>();
        Debug.Log($"obj = {obj}");
        Debug.Log($"bullet = {bullet}");
        Debug.Log($"Target = {Target}");
        bullet.AddForce(Target.transform.position - info.unitObject.transform.position);
    }
}
