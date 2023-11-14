using System.Collections;
using System.Collections.Generic;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using UnityEngine;
using VContainer;

public class LocalCarryInitializer : MonoBehaviour
{
    IMapUpdater _editMapUpdater;
        
    [Inject]
    public void Construct(
        IMapUpdater editMapUpdater)
    {
        _editMapUpdater = editMapUpdater;

    }

    void Awake()
    {
        _editMapUpdater.InitUpdateMap(MapKey.Morita,-1); // -1が初期マップ
            
    }
}
