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
    public  Transform transform => info.unitObjectParent.transform; // 後で消す。　コライダーで判定するようにするため、ピュアなスクリプトから位置情報を取る必要はない
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
    
    // Attach
    [SerializeField] public Transform unitObjectParent; // The NetworkCharacterControllerPrototype interpolates this transform.
    [SerializeField] public Transform pickerParent;
    [SerializeField] public Transform bulletParent;
    [SerializeField] public NetworkCharacterControllerPrototype networkCharacterController;
    [SerializeField] public GameObject pickerPrefab;
    [NonSerialized] public int pickerPoolingCount = 10;
    [SerializeField] public GameObject bulletPrefab;
    [NonSerialized] public int bulletPoolingCount = 10;
    [NonSerialized] public GameObject unitObject; // This has 3D models, a RangeDetector, and more as children.
    [NonSerialized] public RangeDetector rangeDetector;

    public PlayerInfoForPicker playerInfoForPicker;
    public void Init( NetworkRunner runner, GameObject unitObj)
    {
        this.runner = runner;
        unitObjectParent = unitObj.transform.parent;
        Debug.Log($"playerObjectParent = {unitObjectParent}");
        unitObject = unitObj;
        rangeDetector = unitObj.GetComponentInChildren<RangeDetector>();

        playerInfoForPicker = new PlayerInfoForPicker(this);
    }
}

[Serializable]
public class PlayerInfoForPicker
{
    public float RangeRadius => 12.0f; //ToDo : move to NetworkPlayerInfo
    NetworkPlayerInfo _info;

    public PlayerInfoForPicker(NetworkPlayerInfo info)
    {
        _info = info;
    }
}
