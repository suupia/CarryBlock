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
    public class BlockMonoDelegate : IHighlightExecutor , IEntity
    {
         readonly EntityGridMap _map;
         readonly IEntityPresenter _entityPresenter;
         readonly IHighlightExecutor _highLightExecutor;

         Vector2Int _gridPosition;
        
         public BlockMonoDelegate(
             EntityGridMap map,
             Vector2Int gridPos,
             IEntityPresenter entityPresenter)
         {
             _map = map;
             _gridPosition = gridPos;
             _entityPresenter = entityPresenter;
             
             // get blockInfos from blockController
             var blockControllerComponents = entityPresenter.GetMonoBehaviour.GetComponentsInChildren<IBlockController>();
             var blockInfos = blockControllerComponents.Select(c => c.Info).ToList();
             
             // // get itemInfos from itemController
             // var itemControllerComponents = entityPresenter.GetComponentsInChildren<ItemControllerNet>();
             // var itemInfos = itemControllerComponents.Select(c => c.Info).ToList();
             
             // // get gimmickInfos from gimmickController
             // var gimmickControllerComponents = entityPresenter.GetComponentsInChildren<GimmickControllerNet>();
             // var gimmickInfos = gimmickControllerComponents.Select(c => c.Info).ToList(); 
             
             _highLightExecutor = new HighlightExecutor(blockInfos);
             

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
         
         IList<IBlock> GetBlocks()
         {
             var blocks = _map.GetSingleEntityList<IBlock>(_gridPosition);
             return blocks;
         }

         
         public void Highlight(IBlock? block , PlayerRef playerRef)
         {
             _highLightExecutor.Highlight(block, playerRef);
         }
         
         // IBlock implementation
         public Vector2Int GridPosition { get => _gridPosition; set => _gridPosition = value; }
         
         
    }
}