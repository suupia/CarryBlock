using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

[RequireComponent(typeof(NetworkObject))]
public class NetworkResourceController : NetworkBehaviour, IPoolableObject
{
    public  bool isOwned = false;

    void OnDisable()
    {
        OnInactive();
    }
    public void OnInactive()
    {
        isOwned = false;
    }


}
