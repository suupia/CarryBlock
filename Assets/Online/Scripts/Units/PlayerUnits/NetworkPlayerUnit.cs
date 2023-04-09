using System;
using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public interface IPlayerUnit
{
    void Move(Vector3 direction);
    void Action(NetworkButtons buttons, NetworkButtons preButtons);
}

public abstract class NetworkPlayerUnit : IPlayerUnit
{
    public  Transform transform => info.playerObjectParent.transform; // 後で消す。　コライダーで判定するようにするため、ピュアなスクリプトから位置情報を取る必要はない
    protected NetworkPlayerInfo info;
    public NetworkPlayerUnit(NetworkPlayerInfo info)
    {
        this.info = info;
    }

    public abstract void Move(Vector3 direction);
    public abstract void Action(NetworkButtons buttons, NetworkButtons preButtons);
}

[Serializable]
public  class NetworkPlayerInfo
{
    [NonSerialized]public NetworkRunner runner;
    
    [SerializeField] public Transform playerObjectParent; // The NetworkCharacterControllerPrototype interpolates this transform.
    [SerializeField] public Transform pickerParent;
    [SerializeField] public Transform bulletParent;
    [SerializeField] public NetworkCharacterControllerPrototype networkCharacterController;
    [SerializeField] public NetworkPrefabRef pickerPrefab;
    [SerializeField] public NetworkPrefabRef bulletPrefab;
    [NonSerialized] public GameObject unitObject; // This has 3D models, a RangeDetector, and more as children.
    [NonSerialized] public RangeDetector rangeDetector;

    public void Init( NetworkRunner runner, GameObject unitObj)
    {
        this.runner = runner;
        playerObjectParent = unitObj.transform.parent;
        Debug.Log($"playerObjectParent = {playerObjectParent}");
        unitObject = unitObj;
        rangeDetector = unitObj.GetComponentInChildren<RangeDetector>();
    }
}
