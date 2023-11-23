using Carry.CarrySystem.Map.Scripts;
using UnityEngine;

#nullable enable

public class SpawnAreaPositionSetter : MonoBehaviour
{
    [SerializeField] RectTransform spawnAreaRectTransform = null!;
    
    readonly Vector2Int _respawnAreaOrigin = new Vector2Int(1,5);   //中心の座標, ～Input.csとは違うとり方をしているので注意
    
    readonly int _respawnAreaLength = 3;

    void Start()
    {
        var spawnAreaPosWorld = GridConverter.GridPositionToWorldPosition(_respawnAreaOrigin);

        var spawnAreaPosScreen = Camera.main.WorldToScreenPoint(spawnAreaPosWorld);

        Debug.Log($"SpawnAreaPositionWorld: {spawnAreaPosWorld}");
        Debug.Log($"SpawnAreaPositionScreen: {spawnAreaPosScreen}");

        spawnAreaRectTransform.position = spawnAreaPosScreen;
        
        //スクリーン座標でのサイズを計算
        var respawnAreaScreen = Camera.main.WorldToScreenPoint(
                                    GridConverter.GridPositionToWorldPosition(_respawnAreaOrigin + new Vector2Int(1, 1))
                                ) - 
                                spawnAreaPosScreen;
        
        spawnAreaRectTransform.sizeDelta = respawnAreaScreen * (_respawnAreaLength / 2.0f);
    }
}
