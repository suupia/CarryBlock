using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

[RequireComponent(typeof(NetworkObject))]
public class NetworkResourceController : NetworkBehaviour, IPoolableObject
{
    public  bool isOwned = false;
    bool isInitialized = false;
    
    public void Init()
    {
        isInitialized = true;
    }
    void OnDisable()
    {
        OnInactive();
    }
    public void OnInactive()
    {
        if(!isInitialized)return;
        isOwned = false;
    }


}
