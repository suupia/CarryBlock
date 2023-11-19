using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.Spawners.Scripts;
using Projects.MapMakerSystem.Scripts;
using UnityEngine;
using UnityEngine.Assertions;
using VContainer;

#nullable enable

public class MapMakerInitializer : MonoBehaviour
{
    [SerializeField] GameObject playingCanvas;
    
    LocalPlayerSpawner _localPlayerSpawner;
    StageMapSwitcher _stageMapSwitcher;
    EditingMapTransporter _editingMapTransporter;
    FloorTimerLocal _floorTimerLocal;
        
    [Inject]
    public void Construct(
        LocalPlayerSpawner localPlayerSpawner,
        StageMapSwitcher stageMapSwitcher,
        FloorTimerLocal floorTimerLocal,
        EditingMapTransporter editingMapTransporter)
    {
        _stageMapSwitcher = stageMapSwitcher;
        _localPlayerSpawner = localPlayerSpawner;
        _editingMapTransporter = editingMapTransporter;
        _floorTimerLocal = floorTimerLocal;
    }

    void Start()
    {
        Load();
        
        // playingCanvas.SetActive(false);
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

    public void StartTestPlay()
    {
        Debug.Log($"CarryPlayerControllerLocalをスポーンします");
        _localPlayerSpawner.SpawnPlayer();
        playingCanvas.SetActive(true);
        _floorTimerLocal.StartTimer();
    }
}
