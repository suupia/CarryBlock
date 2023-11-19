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
    EditingMapTransporter _editingMapTransporter = null!;
        
    [Inject]
    public void Construct(
        StageMapSwitcher stageMapSwitcher,
        EditingMapTransporter editingMapTransporter)
    {
        _stageMapSwitcher = stageMapSwitcher;
        _editingMapTransporter = editingMapTransporter;
    }

    void Start()
    {
        Load();
    }

    void Load()
    {
        var stage = StageFileUtility.Load(_editingMapTransporter.StageId);
        if (stage != null)
        {
            _stageMapSwitcher.Index = _editingMapTransporter.Index;
            _stageMapSwitcher.SetStage(stage);
        }
        else
        {
            Debug.LogError("該当するStageがLoadできませんでした。TestStageを読み込みます");
        }
        _stageMapSwitcher.InitSwitchMap(); // -1が初期マップ
    }
}
