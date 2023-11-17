using System;
using System.Collections.Generic;
using System.Linq;
using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.Block.Scripts;
using Carry.CarrySystem.Gimmick.Interfaces;
using Carry.CarrySystem.Gimmick.Scripts;
using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.Player.Interfaces;
using Fusion;
using UniRx;
using UnityEngine;
using UnityEngine.Video;

#nullable enable

namespace Carry.CarrySystem.Block.Scripts
{
    // JSONファイルに書き出すためにSerializableをつける
    [Serializable]
    public record CannonBlockRecord
    {
        public CannonBlock.Kind[] kinds = new CannonBlock.Kind[10];
    }
    public class CannonBlock : IBlock, IGimmick, IHoldable
    {
        public Vector2Int GridPosition { get; set; }
        public int MaxPlacedBlockCount { get; } = 1;
        public CannonBlock.Kind KindValue { get; }

        // CannonBall関連
        readonly float _fireInterval = 4.0f;
        readonly float _spawnHeight = 0.8f;
        IDisposable? _gimmickDisposable = null;
        NetworkRunner? _runner;
        

        public enum Kind
        {
            None,
            Up,
            Left,
            Down,
            Right,
        }

        public CannonBlock(CannonBlock.Kind kind, Vector2Int gridPosition)
        {
            KindValue = kind;
            GridPosition = gridPosition;
        }
        
        public void StartGimmick()
        {
            Debug.Log("StartGimmick GridPosition:" + GridPosition + " Kind:" + KindValue);

            var cannonBallBuilder = new CannonBallBuilder();
            
            // 10秒ごとに実行（Intervalの場合、Subscribe後に待機時間を待機してから1回目の処理が実行される）
            _gimmickDisposable =  Observable.Interval(System.TimeSpan.FromSeconds(_fireInterval))
                .Subscribe(x =>
                    {
                        var worldPos = GridConverter.GridPositionToWorldPosition(GridPosition);
                        var spawnPos = new Vector3(worldPos.x, worldPos.y + _spawnHeight , worldPos.z);
                        // CannonBallを生成
                        var _ = cannonBallBuilder.Build(KindValue, spawnPos, Quaternion.identity, PlayerRef.None);
                    }
                );
        }
        
        public void EndGimmick()
        {
            Debug.Log("EndGimmick GridPosition:" + GridPosition + " Kind:" + KindValue);
            _gimmickDisposable?.Dispose();
        }
    }
}