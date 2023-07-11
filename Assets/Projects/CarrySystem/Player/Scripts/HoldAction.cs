using System.Linq;
using Carry.CarrySystem.Entity.Scripts;
using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.Player.Interfaces;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Carry.CarrySystem.Player.Info;

#nullable enable

namespace Carry.CarrySystem.Player.Scripts
{
    public class HoldAction : ICharacterAction
    {
        PlayerInfo _info;
        EntityGridMap _map;
        EntityGridMapSwitcher _mapSwitcher;
        IHoldActionPresenter? _presenter;
        bool _isHoldingRock = false;
        int _maxHoldRockCount = 2;
        
        public void SetHoldPresenter(IHoldActionPresenter presenter)
        {
            _presenter = presenter;
        }

        public void Setup(PlayerInfo info)
        {
            _info = info;
            var resolver =
                Object.FindObjectOfType<LifetimeScope>().Container; // このコンストラクタはNetworkBehaviour内で実行されるため、ここで取得してよい
            _mapSwitcher = resolver.Resolve<EntityGridMapSwitcher>();
            _map =_mapSwitcher.GetMap();
        }

        public void Reset()
        {
            _isHoldingRock = false;
            _presenter?.PutDownRock();
            _map =_mapSwitcher.GetMap(); // Resetが呼ばれる時点でMapが切り替わっている可能性があるため、再取得

        }

        public void Action()
        {
            var transform = _info.playerObj.transform;

            // 前方のGridPosを取得
            var forwardGridPos = GetForwardGridPos(transform);
             Debug.Log($"Player Forward GridPos: {forwardGridPos}");

            // そのGridPosにRockがあるかどうかを確認
            var index = _map.GetIndexFromVector(forwardGridPos);
            Debug.Log($"index : {index}のRockは{_map.GetSingleEntity<Rock>(index)}です");
            var rocks = _map.GetSingleEntityList<Rock>(forwardGridPos);
            var rockCount = rocks.Count;
            
            if (_isHoldingRock)
            {
                if (rockCount < _maxHoldRockCount)
                {
                    PutDownRock(forwardGridPos,rockCount);
                }
            }
            else
            {
                if (rockCount != 0)
                {
                    PickUpRock(forwardGridPos, rocks.First());
                }
            }
            
        }

        Vector2Int GetForwardGridPos(Transform transform)
        {
            var gridPos = GridConverter.WorldPositionToGridPosition(transform.position);
            var forward = transform.forward;
            var direction = new Vector2(forward.x, forward.z);
            var gridDirection = GridConverter.WorldDirectionToGridDirection(direction);
            return gridPos + gridDirection;
        }

        void PickUpRock(Vector2Int forwardGridPos, Rock rock)
        {
            // ドメインのRockを削除（内部のプレゼンターを通して見た目も変わる）
            _map.RemoveEntity(forwardGridPos, rock);

            _isHoldingRock = true;

            _presenter?.PickUpRock();
        }

        void PutDownRock(Vector2Int forwardGridPos, int rockCount)
        {
            // 新しくRockを生成して置く
            var record = new RockRecord() { kind = Rock.Kind.Kind1 };
            _map.AddEntity<Rock>(forwardGridPos, new Rock(record, forwardGridPos));

            _isHoldingRock = false;
            
            _presenter?.PutDownRock();
            
        }
    }
}