using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.Spawners.Scripts;
using Projects.MapMakerSystem.Scripts;
using UnityEngine;
using UnityEngine.Assertions;
using VContainer;

#nullable enable

public class MapMakerInitializer : MonoBehaviour
{
    LocalPlayerSpawner _localPlayerSpawner;
    StageMapSwitcher _stageMapSwitcher;
    EditingMapTransporter _editingMapTransporter;
        
    [Inject]
    public void Construct(
        LocalPlayerSpawner localPlayerSpawner,
        StageMapSwitcher stageMapSwitcher,
        EditingMapTransporter editingMapTransporter)
    {
        _stageMapSwitcher = stageMapSwitcher;
        _localPlayerSpawner = localPlayerSpawner;
        _editingMapTransporter = editingMapTransporter;
    }

    void Awake()
    {
        var stage = StageFileUtility.Load(_editingMapTransporter.StageId);
        if (stage != null)
        {
            _stageMapSwitcher.Index = _editingMapTransporter.Index;
            _stageMapSwitcher.SetStage(stage);
        }
        else
        {
            Debug.LogError("該当するStageがLoadできませんでした。ファイルの存在を確認してください");
        }
        _stageMapSwitcher.InitSwitchMap(); // -1が初期マップ
        // Debug.Log($"stage id: {_editingMapTransporter.StageId}, map index: {_editingMapTransporter.Index}");
        
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
