using Carry.CarrySystem.Map.Scripts;
using UnityEngine;

#nullable enable

public class SpawnAreaPositionSetter : MonoBehaviour
{
    [SerializeField] RectTransform _spawnAreaRectTransform = null!;
    
    readonly Vector2Int _respawnAreaOrigin = new Vector2Int(0,4);

    void Start()
    {
        // Set SpawnArea Position
        var spawnAreaPosition = GridConverter.GridPositionToWorldPosition(_respawnAreaOrigin);
        
        var spawnArea = Camera.main.WorldToScreenPoint(spawnAreaPosition);
        
        // Set SpawnArea transform
        _spawnAreaRectTransform.position = spawnArea;
    }
}
