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
    
    [Inject]
    public void Construct(LocalPlayerSpawner localPlayerSpawner)
    {
        _localPlayerSpawner = localPlayerSpawner;
    }

    void Awake()
    {
      
            
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
