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
    [SerializeField] public NetworkCharacterControllerPrototype networkCharacterController;
    [SerializeField] public NetworkPrefabRef pickerPrefab;
    [SerializeField] public NetworkPrefabRef bulletPrefab;
    
    // Property
    public GameObject playerObj => networkCharacterController.gameObject;

    public PlayerInfoForPicker playerInfoForPicker;
    public void Init( NetworkRunner runner)
    {
        this.runner = runner;
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
