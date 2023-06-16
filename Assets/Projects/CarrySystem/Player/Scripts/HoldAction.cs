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
        IHoldActionPresenter? _presenter;
        bool _isHoldingRock = false;

        public HoldAction(IHoldActionPresenter? presenter)
        {
            _presenter = presenter;
        }

        public void Setup(PlayerInfo info)
        {
            _info = info;
            var resolver = Object.FindObjectOfType<LifetimeScope>().Container; // このコンストラクタはNetworkBehaviour内で実行されるため、ここで取得してよい
            _map = resolver.Resolve<EntityGridMapSwitcher>().GetMap();
        }
        public void Action()
        {
            var transform = _info.playerObj.transform;
            
            // 前方のGridPosを取得
            var forwardGridPos = GetForwardGridPos(transform);
            // Debug.Log($"Player Forward GridPos: {forwardGridPos}");
            
            // そのGridPosにRockがあるかどうかを確認
            var index = _map.GetIndexFromVector(forwardGridPos);
            Debug.Log($"index : {index}のRockは{_map.GetSingleEntity<Rock>(index)}です");
            var rock = _map.GetSingleEntity<Rock>(forwardGridPos);
            if (rock == null)
            {
                Debug.Log($"forwardGridPos: {forwardGridPos}にRockはありません");
                if (_isHoldingRock)
                {
                    PutDownRock(forwardGridPos);
                }
                else
                {
                    // 何もしない
                }
            }
            else
            {
                Debug.Log($"forwardGridPos: {forwardGridPos}にRockがあります");
                if (_isHoldingRock)
                {
                    // 何もしない
                }
                else
                {
                    PickUpRock(forwardGridPos, rock);
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

        void PutDownRock(Vector2Int forwardGridPos)
        {
            // 新しくRockを生成して置く
            _map.AddEntity<Rock>(forwardGridPos, new Rock(Rock.Kind.Kind1, forwardGridPos));

            _isHoldingRock = false;
                    
            _presenter?.PutDownRock();
        }
    }
}