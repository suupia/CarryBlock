using System;
using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.Block.Scripts;
using Carry.CarrySystem.Gimmick.Interfaces;
using Carry.CarrySystem.Gimmick.Scripts;
using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.Player.Interfaces;
using Fusion;
using UniRx;
using UnityEngine;
#nullable enable

namespace Projects.CarrySystem.Gimmick.Scripts
{
    [Serializable]
    public record SpikeGimmickRecord
    {
        public SpikeGimmick.Kind[] kinds = new SpikeGimmick.Kind[10];
    }
    
    public class SpikeGimmick :  IGimmick
    {
        public int MaxPlacedBlockCount { get; } = 1;
        public SpikeGimmick.Kind KindValue { get; }
        
        public enum Kind
        {
            None,
            Kind1,
        }
        
        // SpikeBody
        readonly float _appearInterval = 4.0f;
        readonly float _spawnHeight = 0.8f;
        IDisposable? _gimmickDisposable;
        
        public SpikeGimmick(Kind kind)
        {
            KindValue = kind;
        }
        
        public void Dispose()
        {
            _gimmickDisposable?.Dispose();
        }
        
        
        public void StartGimmick(Vector2Int gridPosition)
        {
            Debug.Log("StartGimmick GridPosition:" + gridPosition + " Kind:" + KindValue);
            
            var spikeBodyBuilder = new SpikeBodyBuilder();
            
            // 10秒ごとに実行（Intervalの場合、Subscribe後に待機時間を待機してから1回目の処理が実行される）
            _gimmickDisposable =  Observable.Interval(System.TimeSpan.FromSeconds(_appearInterval))
                .Subscribe(_ =>
                    {
                        var worldPos = GridConverter.GridPositionToWorldPosition(gridPosition);
                        var spawnPos = new Vector3(worldPos.x, worldPos.y + _spawnHeight , worldPos.z);
                        // Spawn prefab
                        spikeBodyBuilder.Build(KindValue, spawnPos, Quaternion.identity, PlayerRef.None);
                    }
                );
        }
    }
}