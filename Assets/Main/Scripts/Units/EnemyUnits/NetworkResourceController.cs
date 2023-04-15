using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

[RequireComponent(typeof(NetworkObject))]
public class NetworkResourceController :  PoolableObject
{
    public bool isOwned { get; private set; }
    bool _isInitialized = false;
    Transform _holder;
    
    public override void Render()
    {
        if (isOwned)
        {
            transform.position = _holder.position;
        }
    }

    public void OnHeld(Transform holder)
    {
        isOwned = true;
        _holder = holder;
    }


    public override void Spawned() // 必要であればInit()にして外部から呼び出せるようにする
    {
        _isInitialized = true;
    }
    protected override void OnInactive()
    {
        if(!_isInitialized)return;
        isOwned = false;
    }


}
