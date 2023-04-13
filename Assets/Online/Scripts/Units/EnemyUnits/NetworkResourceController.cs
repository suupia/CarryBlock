using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

[RequireComponent(typeof(NetworkObject))]
public class NetworkResourceController :  PoolableObject
{
    public  bool isOwned = false;
    bool isInitialized = false;
    
    public override void Spawned() // 必要であればInit()にして外部から呼び出せるようにする
    {
        isInitialized = true;
    }
    protected override void OnInactive()
    {
        if(!isInitialized)return;
        isOwned = false;
    }


}
