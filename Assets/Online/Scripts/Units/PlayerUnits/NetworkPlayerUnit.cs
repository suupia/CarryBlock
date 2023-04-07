using System;
using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public  class NetworkPlayerInfo
{
    public readonly NetworkRunner runner;
    
    [NonSerialized] public NetworkObject PlayerObjectParent; // todo : できたらgetterを作る
    [SerializeField] public NetworkPrefabRef pickerPrefab; // todo : readonlyをつける
    [SerializeField] public NetworkPrefabRef bulletPrefab;
    [SerializeField] public NetworkCharacterControllerPrototype networkCharacterController;

    public void Init(NetworkObject playerObj)
    {
        this.PlayerObjectParent = playerObj;
    }
}

public interface IPlayerUnit
{
    void Move(Vector3 direction);
    void Action(NetworkButtons buttons, NetworkButtons preButtons);
}

public abstract class NetworkPlayerUnit : IPlayerUnit
{
    public  Transform transform => info.PlayerObjectParent.transform; // 後で消す。　コライダーで判定するようにするため、ピュアなスクリプトから位置情報を取る必要はない
    protected NetworkPlayerInfo info;
    public NetworkPlayerUnit(NetworkPlayerInfo info)
    {
        this.info = info;
    }

    public abstract void Move(Vector3 direction);
    public abstract void Action(NetworkButtons buttons, NetworkButtons preButtons);
}
