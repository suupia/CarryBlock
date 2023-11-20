using System.Collections;
using System.Collections.Generic;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using UnityEngine;
using VContainer;

#nullable enable

public class SpawnAreaPositionSetter : MonoBehaviour
{
    IMapGetter _mapGetter = null!;
    [SerializeField] RectTransform _spawnAreaRectTransform = null!;
    
    readonly Vector2Int _respawnAreaOrigin = new Vector2Int(0,4);
    readonly int _respawnSize = 3;
    
    //constructor
    [Inject]
    void Construct(IMapGetter mapGetter)
    {   
        _mapGetter = mapGetter;

        // _spawnAreaRectTransform の原点を _respawnAreaOrigin に設定する
    }
}
