using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.Block.Scripts;
using Carry.CarrySystem.Gimmick.Interfaces;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Player.Interfaces;
using Fusion;
using Carry.CarrySystem.Block.Info;
using Carry.CarrySystem.Entity.Interfaces;
using Carry.CarrySystem.Map.Scripts;
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
         public IBlock? Block =>  GetBlocks().FirstOrDefault(); 
         public IList<IBlock> Blocks => GetBlocks();
         public IList<IItem> Items => GetItems();
         public IList<IGimmick> Gimmicks => GetGimmicks();
         
         readonly EntityGridMap _map;
         readonly IEntityPresenter _entityPresenter;
         readonly IHighlightExecutor _highLightExecutor;

         Vector2Int _gridPosition;
        
         public BlockMonoDelegate(
             EntityGridMap map,
             Vector2Int gridPos,
             IList<IBlock> blocks,
             IList<IItem> items,
             IList<IGimmick> gimmicks,
             IEntityPresenter entityPresenter)
         {
             _map = map;
             _gridPosition = gridPos;
             _entityPresenter = entityPresenter;
             
             // get blockInfos from blockController
             var blockControllerComponents = entityPresenter.GetMonoBehaviour.GetComponentsInChildren<IBlockController>();
             var blockInfos = blockControllerComponents.Select(c => c.Info).ToList();
             
             _highLightExecutor = new HighlightExecutor(blockInfos);
        
             // 最初のStartGimmickの処理
             foreach (var gimmick in gimmicks)
             {
                 gimmick.StartGimmick();
             }
             
             // 代表として最初のBlockControllerの親に対してOnDestroyAsObservableを登録
             var blockControllerParent = blockInfos.First().BlockController.GetMonoBehaviour.transform.parent;
             blockControllerParent.gameObject.OnDestroyAsObservable().Subscribe(_ =>
             {
                 foreach (var gimmick in blocks.OfType<IGimmick>())
                 {
                     gimmick.EndGimmick();
                 }
             });
             

         }

         IList<IBlock> GetBlocks()
         {
             var blocks = _map.GetSingleEntityList<IBlock>(_gridPosition);
             CheckAllBlockTypesAreSame(blocks);
             return blocks;
         }

         IList<IItem> GetItems()
         {
             return _map.GetSingleEntityList<IItem>(_gridPosition);
         }
         
         IList<IGimmick> GetGimmicks()
         {
             return _map.GetSingleEntityList<IGimmick>(_gridPosition);
         }


         public void AddBlock(IBlock block)
         {
             if(block is IGimmick gimmickBlock) gimmickBlock.StartGimmick();
             // _blocks.Add(block);
             _map.AddEntity(_gridPosition, block);
            _entityPresenter.SetEntityActiveData(block, GetBlocks().Count);

         }
         public void RemoveBlock(IBlock block)
         {
             if(block is IGimmick gimmickBlock) gimmickBlock.EndGimmick();
             // _blocks.Remove(block);
             _map.RemoveEntity(_gridPosition, block);
             _entityPresenter.SetEntityActiveData(block, GetBlocks().Count);

         }
         
         public void Highlight(IBlock? block , PlayerRef playerRef)
         {
             _highLightExecutor.Highlight(block, playerRef);
         }
         
         // IBlock implementation
         public Vector2Int GridPosition { get => _gridPosition; set => _gridPosition = value; }

         
         // Check if the blocks are the same type.
         void CheckAllBlockTypesAreSame(List<IBlock> blocks)
         {
             if (!blocks.Any())
             {
                 // Debug.Log($"IBlockが存在しません。{string.Join(",", blocks)}");
                 return;
             }

             var firstBlock = blocks.First();

             if (blocks.Any(block => block.GetType() != firstBlock.GetType()))
             {
                 Debug.LogError(
                     $"異なる種類のブロックが含まれています。　firstBlock.GetType() : {firstBlock.GetType()} {string.Join(",", blocks)}");
             }
             
         }
         
    }
}