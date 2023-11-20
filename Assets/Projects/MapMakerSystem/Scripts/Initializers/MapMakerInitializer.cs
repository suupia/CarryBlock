using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.Spawners.Scripts;
using Projects.MapMakerSystem.Scripts;
using UnityEngine;
using UnityEngine.Assertions;
using VContainer;

#nullable enable

public class MapMakerInitializer : MonoBehaviour
{
    StageMapSwitcher _stageMapSwitcher = null!;
        
    [Inject]
    public void Construct(
        StageMapSwitcher stageMapSwitcher
        )
    {
        _stageMapSwitcher = stageMapSwitcher;
    }

    void Start()
    {
        Load();
    }

    void Load()
    {

        _stageMapSwitcher.InitSwitchMap(); // -1が初期マップ
    }
}
