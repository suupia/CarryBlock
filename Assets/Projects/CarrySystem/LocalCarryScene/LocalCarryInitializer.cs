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
    IMapUpdater _editMapUpdater;

    
    [Inject]
    public void Construct(
        IMapUpdater editMapUpdater,
        LocalPlayerSpawner localPlayerSpawner)
    {
        _editMapUpdater = editMapUpdater;
        _localPlayerSpawner = localPlayerSpawner;
    }

    void Awake()
    {
        _editMapUpdater.InitUpdateMap(MapKey.Morita,-1); // -1が初期マップ
            
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
