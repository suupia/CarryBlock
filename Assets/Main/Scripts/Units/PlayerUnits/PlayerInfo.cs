using System;
using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public interface IPlayerUnit
{
    void Move(Vector3 direction);
    float ActionCooldown();
    void Action();
}

[Serializable]
public  class PlayerInfo
{
    [NonSerialized]public NetworkRunner runner;
    
    // constant fields 
    public readonly float bulletOffset = 1;
    
    public readonly float rangeRadius = 12.0f;
    
    // Attach
    [SerializeField] public Transform unitObjectParent; // The NetworkCharacterControllerPrototype interpolates this transform.
    [SerializeField] public NetworkCharacterControllerPrototype networkCharacterController;
    [SerializeField] public NetworkPrefabRef pickerPrefab;
    [SerializeField] public NetworkPrefabRef bulletPrefab;
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


public class PlayerInfoForPicker
{
    public float RangeRadius => 12.0f; //ToDo : move to NetworkPlayerInfo
    PlayerInfo _info;

    public PlayerInfoForPicker(PlayerInfo info)
    {
        _info = info;
    }
}
