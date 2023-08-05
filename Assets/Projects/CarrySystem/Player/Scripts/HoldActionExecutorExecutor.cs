using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Carry.CarrySystem.Entity.Scripts;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.Player.Interfaces;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Carry.CarrySystem.Player.Info;
using Projects.CarrySystem.Block.Interfaces;

#nullable enable

namespace Carry.CarrySystem.Player.Scripts
{
    public class HoldActionExecutorExecutor : IHoldActionExecutor
    {
        public bool IsHoldingBlock => _isHoldingBlock;
        readonly IMapUpdater _mapUpdater;
        PlayerInfo _info = null!;
        EntityGridMap _map = null!;
        IHoldActionPresenter? _presenter;
        bool _isHoldingBlock = false;
        IBlock? _holdingBlock = null;

        public HoldActionExecutorExecutor(IMapUpdater mapUpdater)
        {
            _mapUpdater = mapUpdater;
        }

        public void SetHoldPresenter(IHoldActionPresenter presenter)
        {
            _presenter = presenter;
        }

        public void Setup(PlayerInfo info)
        {
            _info = info;
            _map = _mapUpdater.GetMap();
        }

        public void Reset()
        {
            _isHoldingBlock = false;
            _presenter?.PutDownRock();
            _map = _mapUpdater.GetMap(); // Resetが呼ばれる時点でMapが切り替わっている可能性があるため、再取得
        }

        public void HoldAction()
        {
            var transform = _info.playerObj.transform;

            // 前方のGridPosを取得
            var forwardGridPos = GetForwardGridPos(transform);

            // そのGridPosにBlockがあるかどうかを確認
            var blocks = _map.GetSingleEntityList<IBlock>(forwardGridPos);
            Debug.Log($"forwardGridPos: {forwardGridPos}, blocks: {string.Join(",", blocks)}");

            if (_isHoldingBlock)
            {
                if (_holdingBlock == null)
                {
                    Debug.LogError($"_holdingBlockがnullです");
                    return;
                }

                if (_holdingBlock.CanPutDown(blocks))
                {
                    _holdingBlock.PutDown();
                    _map.AddEntity(forwardGridPos, _holdingBlock);
                    _presenter?.PutDownRock();
                    _isHoldingBlock = false;
                }
            }
            else
            {
                IBlock block = null!;
                if (blocks.Any()) block = blocks.First(); // 一つのマスにはIBlockは一種類しかないという前提
                else return;
                
                if (block.CanPickUp())
                {
                    block.PickUp();
                    _map.RemoveEntity(forwardGridPos, block);
                    _presenter?.PickUpRock();
                    _holdingBlock = block;
                    _isHoldingBlock = true;
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
        
    }
}