using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.Entity.Scripts;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.Player.Interfaces;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Carry.CarrySystem.Player.Info;
using UniRx;

#nullable enable

namespace Carry.CarrySystem.Player.Scripts
{
    public class HoldActionExecutor : IHoldActionExecutor
    {
        readonly IMapUpdater _mapUpdater;
        PlayerInfo _info = null!;
        EntityGridMap _map = null!;
        readonly PlayerBlockContainer _blockContainer;
        readonly IPlayerBlockPresenter _playerPresenterContainer;

        IDisposable? _searchBlockDisposable;
        IList<IBlock> _searchedBlocks = new List<IBlock>();

        public HoldActionExecutor(
            PlayerBlockContainer blockContainer, 
            IPlayerBlockPresenter playerPresenterContainer,
            IMapUpdater mapUpdater)
        {
            _blockContainer = blockContainer;
            _playerPresenterContainer = playerPresenterContainer;    
            _mapUpdater = mapUpdater;
        }

        public void Setup(PlayerInfo info)
        {
            _info = info;
            _map = _mapUpdater.GetMap();

            _searchBlockDisposable?.Dispose();
            _searchBlockDisposable = Observable.EveryUpdate().Subscribe(_ =>
            {
                SearchBlocks();
            });
        }

        public void Reset()
        {
            var _ =  _blockContainer.PopBlock(); // Hold中のBlockがあれば取り出して削除
            _playerPresenterContainer.PutDownBlock();
            _map = _mapUpdater.GetMap(); // Resetが呼ばれる時点でMapが切り替わっている可能性があるため、再取得
        }
        public void HoldAction()
        {
            var transform = _info.playerObj.transform;
            var forwardGridPos = GetForwardGridPos(transform);

            if (_blockContainer.IsHoldingBlock)
            {
                // 前方にGroundがあるかどうかを確認
                var grounds = _map.GetSingleEntityList<Ground>(forwardGridPos);
                if(!grounds.Any()) return;
                
                if (_blockContainer.CanPutDown(_searchedBlocks))
                {
                    var block = _blockContainer.PopBlock();
                    if (block == null)
                    {
                        Debug.LogError($" _blockContainer.PopBlock() : null"); // IsHoldingBlockがtrueのときはnullにならないから呼ばれない
                        return;
                    }
                    block.PutDown(_info.playerController.GetCharacter);
                    _map.AddEntity(forwardGridPos, block);
                    _playerPresenterContainer.PutDownBlock();
                }
            }
            else
            {
                IBlock block = null!;
                if (_searchedBlocks.Any()) block = _searchedBlocks.First(); // 一つのマスにはIBlockは一種類しかないという前提
                else return;
                
                if (block.CanPickUp())
                {
                    block.PickUp(_info.playerController.GetCharacter);
                    _map.RemoveEntity(forwardGridPos, block);
                    _playerPresenterContainer.PickUpBlock(block);
                    _blockContainer.SetBlock(block);
                }
            }
        }
        
        void SearchBlocks()
        {
            var transform = _info.playerObj.transform;
            var forwardGridPos = GetForwardGridPos(transform);

            // 前方にBlockがあるかどうかを確認
            var blocks = _map.GetSingleEntityList<IBlock>(forwardGridPos);
            // Debug.Log($"forwardGridPos: {forwardGridPos}, blocks: {string.Join(",", blocks)}");
            
            _searchedBlocks = blocks;

            // ここにブロックの見た目を変える処理を入れる
            blocks.ForEach(block =>
            {
                block.Highlight();
            });
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