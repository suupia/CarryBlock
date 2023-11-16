using System;
using System.Collections;
using System.Collections.Generic;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.Spawners.Scripts;
using UnityEngine;
using VContainer;

public class MapMakerInitializer : MonoBehaviour
{
    LocalPlayerSpawner _localPlayerSpawner;
    IMapSwitcher _editMapSwitcher;

    
    [Inject]
    public void Construct(
        IMapSwitcher editMapSwitcher,
        LocalPlayerSpawner localPlayerSpawner)
    {
        _editMapSwitcher = editMapSwitcher;
        _localPlayerSpawner = localPlayerSpawner;
    }

    void Awake()
    {
        _editMapSwitcher.InitSwitchMap(); // -1が初期マップ
            
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1) )
        {
            Debug.Log($"CarryPlayerControllerLocalをスポーンします");
            _localPlayerSpawner.SpawnPlayer();
            
        }
    }
}
