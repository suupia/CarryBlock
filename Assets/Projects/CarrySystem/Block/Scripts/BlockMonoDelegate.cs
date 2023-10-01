using System.Collections.Generic;
using System.Linq;
using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.Block.Scripts;
using Carry.CarrySystem.Gimmick.Interfaces;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Player.Interfaces;
using Fusion;
using Carry.CarrySystem.Block.Info;
using Projects.CarrySystem.Item.Interfaces;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
#nullable enable

namespace Carry.CarrySystem.Block.Scripts
{
    /// <summary>
    /// IBlock(ドメインの情報)とBlockInfo(NetworkBehaviourの情報)を持つクラス
    /// ドメインの処理はこのクラスにアクセスして行う
    /// </summary>
    public class BlockMonoDelegate : IBlockMonoDelegate
    {
         public IBlock? Block => _blocks.FirstOrDefault(); 
         public IList<IBlock> Blocks => _blocks;
         public IList<IItem> Items => _items;

         readonly NetworkRunner _runner;
         readonly IList<IBlock> _blocks;
         readonly IList<IItem> _items;
         readonly IList<BlockInfo> _blockInfos;
         readonly IBlock? _block;
         readonly IEntityPresenter _entityPresenter;

         readonly IHighlightExecutor _highLightExecutor;

         Vector2Int _gridPosition;
        
         public BlockMonoDelegate(NetworkRunner runner, Vector2Int gridPos, IList<IBlock> blocks, IList<BlockInfo> blockInfos, IList<IItem> items, IEntityPresenter entityPresenter)
         {
             _runner = runner;
             _gridPosition = gridPos;
             _blocks = blocks;
             _items = items;
             _blockInfos = blockInfos;
             _entityPresenter = entityPresenter;
             
             _highLightExecutor = new HighlightExecutor(_blockInfos);
        
             // 最初のStartGimmickの処理
             foreach (var gimmick in _blocks.OfType<IGimmickBlock>())
             {
                 gimmick.StartGimmick(runner);
             }
             
             // 代表として最初のBlockControllerの親に対してOnDestroyAsObservableを登録
             var blockControllerParent = _blockInfos.First().BlockController.transform.parent;
             blockControllerParent.gameObject.OnDestroyAsObservable().Subscribe(_ =>
             {
                 foreach (var gimmick in _blocks.OfType<IGimmickBlock>())
                 {
                     gimmick.EndGimmick(runner);
                 }
             });
             

         }
         

         public void AddBlock(IBlock block)
         {
             if(block is IGimmickBlock gimmickBlock) gimmickBlock.StartGimmick(_runner);
             _blocks.Add(block);
            _entityPresenter.SetEntityActiveData(block, _blocks.Count);

         }
         public void RemoveBlock(IBlock block)
         {
             if(block is IGimmickBlock gimmickBlock) gimmickBlock.EndGimmick(_runner);
             _blocks.Remove(block);
             _entityPresenter.SetEntityActiveData(block, _blocks.Count);

         }
         
         public void Highlight(IBlock? block , PlayerRef playerRef)
         {
             _highLightExecutor.Highlight(block, playerRef);
         }
         
         // IBlock implementation
         public Vector2Int GridPosition { get => _gridPosition; set => _gridPosition = value; }

         
    }
}