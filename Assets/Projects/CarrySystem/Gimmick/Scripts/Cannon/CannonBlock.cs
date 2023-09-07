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
    public class CannonBlock : IGimmickBlock
    {
        public Vector2Int GridPosition { get; set; }
        public int MaxPlacedBlockCount { get; } = 2;
        public CannonBlock.Kind KindValue { get; }

        readonly float _fireInterval = 3.0f;
        IDisposable? _gimmickDisposable = null;
        

        public enum Kind
        {
            None,
            Kind1,
        }

        public CannonBlock(CannonBlock.Kind kind, Vector2Int gridPosition)
        {
            KindValue = kind;
            GridPosition = gridPosition;
        }
        
        public void StartGimmick(NetworkRunner runner)
        {
            Debug.Log("StartGimmick GridPosition:" + GridPosition + " Kind:" + KindValue);

            var cannonBallBuilder = new CannonBallBuilder(runner);
            
            // 10秒ごとに実行（Intervalの場合、Subscribe後に待機時間を待機してから1回目の処理が実行される）
            _gimmickDisposable =  Observable.Interval(System.TimeSpan.FromSeconds(_fireInterval))
                .Subscribe(x =>
                    {
                        // CannonBallを生成
                        cannonBallBuilder.Build(GridConverter.GridPositionToWorldPosition(GridPosition), Quaternion.identity, PlayerRef.None);
                    }
                );
        }
        
        public void EndGimmick(NetworkRunner runner)
        {
            Debug.Log("EndGimmick GridPosition:" + GridPosition + " Kind:" + KindValue);
            _gimmickDisposable?.Dispose();
        }
    }
}