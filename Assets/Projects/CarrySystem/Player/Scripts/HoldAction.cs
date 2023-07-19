﻿using System.Linq;
using Carry.CarrySystem.Entity.Scripts;
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
    public class HoldAction : ICharacterHoldAction
    {
        readonly int _maxPlacedBlockCount = 1;

        IObjectResolver _resolver;
        PlayerInfo _info;
        EntityGridMap _map;
        EntityGridMapSwitcher _mapSwitcher;
        IHoldActionPresenter? _presenter;
        bool _isHoldingBlock = false;
        IBlock? _holdingBlock = null;
        
        public HoldAction(IObjectResolver resolver)
        {
            _resolver = resolver;
        }
        
        public void SetHoldPresenter(IHoldActionPresenter presenter)
        {
            _presenter = presenter;
        }

        public void Setup(PlayerInfo info)
        {
            _info = info;
            _mapSwitcher = _resolver.Resolve<EntityGridMapSwitcher>();
            _map =_mapSwitcher.GetMap();
        }

        public void Reset()
        {
            _isHoldingBlock = false;
            _presenter?.PutDownRock();
            _map =_mapSwitcher.GetMap(); // Resetが呼ばれる時点でMapが切り替わっている可能性があるため、再取得

        }

        public void Action()
        {
            var transform = _info.playerObj.transform;

            // 前方のGridPosを取得
            var forwardGridPos = GetForwardGridPos(transform);

            // そのGridPosにBlockがあるかどうかを確認
            var blocks = _map.GetSingleEntityList<IBlock>(forwardGridPos);
            var blocksCount = blocks.Count;
            Debug.Log($"forwardGridPos: {forwardGridPos}, blocks: {string.Join(",",blocks)}");
            
            if (_isHoldingBlock)
            {
                if (blocksCount < _maxPlacedBlockCount)
                {
                    PutDownBlock(forwardGridPos,blocksCount);
                }
            }
            else
            {
                if (blocksCount != 0)
                {
                    PickUpBlock(forwardGridPos, blocks.First());
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

        void PickUpBlock(Vector2Int forwardGridPos, IBlock block)
        {
            // ドメインのBlockを削除（内部のプレゼンターを通して見た目も変わる）
            _map.RemoveEntity(forwardGridPos, block);

            _isHoldingBlock = true;
            _holdingBlock = block;

            _presenter?.PickUpRock();
        }

        void PutDownBlock(Vector2Int forwardGridPos, int blocksCount)
        {
            if (_holdingBlock == null)
            {
                Debug.LogError($"_holdingBlockがnullです");
                return;
            }
            _map.AddEntity(forwardGridPos, _holdingBlock);

            _isHoldingBlock = false;
            
            _presenter?.PutDownRock();
            
        }
    }
}