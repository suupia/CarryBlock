using System.Collections.Generic;
using System.Linq;
using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.Block.Scripts;
using Carry.CarrySystem.Gimmick.Interfaces;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Player.Interfaces;
using Fusion;
using Carry.CarrySystem.Block.Info;
using Carry.CarrySystem.Entity.Interfaces;
using Projects.CarrySystem.Gimmick.Scripts;
using Projects.CarrySystem.Item.Interfaces;
using Projects.CarrySystem.Item.Scripts;
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
    public class BlockMonoDelegate : IBlockMonoDelegate , IHighlightExecutor
    {
         public IBlock? Block => _blocks.FirstOrDefault(); 
         public IList<IBlock> Blocks => _blocks;
         public IList<IItem> Items => _items;
         public IList<IGimmick> Gimmicks => _gimmicks;

         readonly IList<IBlock> _blocks;
         readonly IList<BlockInfo> _blockInfos;
         readonly IList<IItem> _items;
         readonly IList<ItemInfo> _itemInfos;
         readonly IList<IGimmick> _gimmicks;
         readonly IList<GimmickInfo> _gimmickInfos;
         readonly IBlock? _block;
         readonly IEntityPresenter _entityPresenter;

         readonly IHighlightExecutor _highLightExecutor;

         Vector2Int _gridPosition;
        
         public BlockMonoDelegate(
             Vector2Int gridPos,
             IList<IBlock> blocks,
             IList<BlockInfo> blockInfos,
             IList<IItem> items,
             IList<ItemInfo> itemInfos,
             IList<IGimmick> gimmicks,
             IList<GimmickInfo> gimmickInfos,
             IEntityPresenter entityPresenter)
         {
             _gridPosition = gridPos;
             _blocks = blocks;
             _blockInfos = blockInfos;
             _items = items;
             _itemInfos = itemInfos;
             _gimmicks = gimmicks;
             _gimmickInfos = gimmickInfos;
             _entityPresenter = entityPresenter;
             
             _highLightExecutor = new HighlightExecutor(_blockInfos);
        
             // 最初のStartGimmickの処理
             foreach (var gimmick in gimmicks)
             {
                 gimmick.StartGimmick();
             }
             
             // 代表として最初のBlockControllerの親に対してOnDestroyAsObservableを登録
             var blockControllerParent = _blockInfos.First().BlockController.transform.parent;
             blockControllerParent.gameObject.OnDestroyAsObservable().Subscribe(_ =>
             {
                 foreach (var gimmick in _blocks.OfType<IGimmick>())
                 {
                     gimmick.EndGimmick();
                 }
             });
             

         }
         

         public void AddBlock(IBlock block)
         {
             if(block is IGimmick gimmickBlock) gimmickBlock.StartGimmick();
             _blocks.Add(block);
            _entityPresenter.SetEntityActiveData(block, _blocks.Count);

         }
         public void RemoveBlock(IBlock block)
         {
             if(block is IGimmick gimmickBlock) gimmickBlock.EndGimmick();
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