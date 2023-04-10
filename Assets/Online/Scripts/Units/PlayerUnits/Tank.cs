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

    NetworkCharacterControllerPrototype _cc;
    RangeDetector _rangeDetector;

    GameObjectPool _bulletPool;
    GameObjectPool _pickerPool;

    float _pickerHeight = 5.0f;
    
    public Tank(NetworkPlayerInfo info) : base(info)
    {
        this.info = info;
        _runner = info.runner;

        _cc = info.networkCharacterController; 
        _rangeDetector = info.rangeDetector;
        
        _bulletPool = new GameObjectPool(info.bulletParent,info.bulletPrefab, info.bulletPoolingCount);
        _pickerPool = new GameObjectPool(info.pickerParent,info.pickerPrefab, info.pickerPoolingCount);
    }

    public override void Move(Vector3 direction)
    {
        _cc.Move(direction);
    }

    public override void Action(NetworkButtons buttons, NetworkButtons preButtons)
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

        if (buttons.GetPressed(preButtons).IsSet(PlayerOperation.MainAction))
        {
            // var pickerObj = _runner.Spawn(info.pickerPrefab, info.unitObject.transform.position,  info.unitObject.transform.rotation, PlayerRef.None);
            
            var picker = _pickerPool.Get().GetComponent<NetworkPickerController>();
            var pickerPos = info.unitObject.transform.position + new Vector3(0, _pickerHeight, 0);
            picker.transform.position = pickerPos;;
            picker.Init(info.unitObject.gameObject, info.playerInfoForPicker, _pickerPool);
        }
    }
    

    void OnBeforeSpawnBullet(NetworkRunner runner, NetworkObject obj)
    {
        var bullet = obj.GetComponent<Bullet>();
        Debug.Log($"obj = {obj}");
        Debug.Log($"bullet = {bullet}");
        Debug.Log($"Target = {Target}");
        bullet.AddForce(Target.transform.position - info.unitObject.transform.position);
    }
}
