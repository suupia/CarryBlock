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
using Projects.CarrySystem.Block;
using Projects.CarrySystem.Block.Scripts;
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
        IList<IBlockMonoDelegate> _searchedBlocks = new List<IBlockMonoDelegate>();

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
            
            Debug.Log($"IsHoldingBlock : {_blockContainer.IsHoldingBlock}");

            if (_blockContainer.IsHoldingBlock)
            {
                // // 前方にGroundがあるかどうかを確認
                // var grounds = _map.GetSingleEntityList<Ground>(forwardGridPos);
                // if(!grounds.Any()) return;
                
                // マップの内部かどうかを判定
                if(!_map.IsInDataRangeArea(forwardGridPos))return;
                
                Debug.Log($"CanPutDown : {_blockContainer.CanPutDown(_searchedBlocks)}");
                if (_blockContainer.CanPutDown(_searchedBlocks))
                {
                    var block = _blockContainer.PopBlock();
                    if (block == null)
                    {
                        Debug.LogError($" _blockContainer.PopBlock() : null"); // IsHoldingBlockがtrueのときはnullにならないから呼ばれない
                        return;
                    }
                    block.Block.PutDown(_info.playerController.GetCharacter);
                    _map.AddEntity(forwardGridPos, block);
                    _playerPresenterContainer.PutDownBlock();
                }
            }
            else
            {
                IBlockMonoDelegate blockMonoDelegate = null!;
                if (_searchedBlocks.Any()) blockMonoDelegate = _searchedBlocks.First(); // 一つのマスにはIBlockは一種類しかないという前提
                else return;

                var block = blockMonoDelegate.Block;
                if (block.CanPickUp())
                {
                    block.PickUp(_info.playerController.GetCharacter);
                    _map.RemoveEntity(forwardGridPos,block);
                    _playerPresenterContainer.PickUpBlock(blockMonoDelegate);
                    _blockContainer.SetBlock(blockMonoDelegate);
                }
            }
        }
        
        void SearchBlocks()
        {
            var transform = _info.playerObj.transform;
            var forwardGridPos = GetForwardGridPos(transform);

            // 前方にBlockがあるかどうかを確認
            var blocks = _map.GetSingleEntityList<IBlockMonoDelegate>(forwardGridPos);
            // Debug.Log($"forwardGridPos: {forwardGridPos}, blocks: {string.Join(",", blocks)}");
            
            _searchedBlocks = blocks;

            // ここにブロックの見た目を変える処理を入れる
            blocks.ForEach(block =>
            {
                // ToDo: IBlockMonoDelegateを取ってきてハイライトの処理をする
                // var blockObj = blockMonoDelegate.info.blockObj;
                // var materialSetter = blockObj.GetComponetInChildren<BlockMaterialSetter>();
                // materialSetter.Highlight();
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