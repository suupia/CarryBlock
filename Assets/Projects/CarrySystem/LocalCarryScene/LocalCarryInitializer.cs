using System;
using System.Collections;
using System.Collections.Generic;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.Spawners.Scripts;
using UnityEngine;
using VContainer;

public class LocalCarryInitializer : MonoBehaviour
{
    LocalPlayerSpawner _localPlayerSpawner;
    IMapGetter _editMapGetter;

    
    [Inject]
    public void Construct(
        IMapGetter editMapGetter,
        LocalPlayerSpawner localPlayerSpawner)
    {
        _editMapGetter = editMapGetter;
        _localPlayerSpawner = localPlayerSpawner;
    }

    void Awake()
    {
        _editMapGetter.InitUpdateMap(MapKey.Default,-1); // -1が初期マップ
            
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log($"CarryPlayerControllerLocalをスポーンします");
            _localPlayerSpawner.SpawnPlayer();
            
        }
    }
}
