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
         readonly IPlaceablePresenter _placeablePresenter;
         readonly IHighlightExecutor _highLightExecutor;

         Vector2Int _gridPosition;
        
         public BlockMonoDelegate(
             EntityGridMap map,
             Vector2Int gridPos,
             IPlaceablePresenter placeablePresenter)
         {
             _map = map;
             _gridPosition = gridPos;
             _placeablePresenter = placeablePresenter;
             
             // get blockInfos from blockController
             var blockControllerComponents = placeablePresenter.GetMonoBehaviour.GetComponentsInChildren<IBlockController>();
             var blockInfos = blockControllerComponents.Select(c => c.Info).ToList();
             
             // // get itemInfos from itemController
             // var itemControllerComponents = entityPresenter.GetComponentsInChildren<ItemControllerNet>();
             // var itemInfos = itemControllerComponents.Select(c => c.Info).ToList();
             
             // // get gimmickInfos from gimmickController
             // var gimmickControllerComponents = entityPresenter.GetComponentsInChildren<GimmickControllerNet>();
             // var gimmickInfos = gimmickControllerComponents.Select(c => c.Info).ToList(); 
             
             _highLightExecutor = new HighlightExecutor(blockInfos);
             

         }
        
         // _entityPresenterに依存している
         // BlockMonoDelegateそのものをIEntityとしてEntityGridMapに配置してしまうことで、
         // 別のところから、positionを指定して、Addしたり、Highlightしたりできるようになる。
         
         // 本来はクライアントコードでAddEntityできないようにするために、PlaceableGridMapクラスを作るべきだが、
         // 横着して、MonoDelegateをマップに入れる構造になっている

         public void AddBlock(IBlock block , Vector2Int gridPosition)
         {
             if(block is IGimmick gimmickBlock) gimmickBlock.StartGimmick(gridPosition);
             _map.AddEntity(_gridPosition, block);
            _placeablePresenter.SetEntityActiveData(block, GetBlocks().Count);

         }
         
         public void RemoveBlock(IBlock block)
         {
             if(block is IGimmick gimmickBlock) gimmickBlock.Dispose();
             _map.RemoveEntity(_gridPosition, block);
             _placeablePresenter.SetEntityActiveData(block, GetBlocks().Count);

         }
         
         IList<IBlock> GetBlocks()
         {
             var blocks = _map.GetSingleTypeList<IBlock>(_gridPosition);
             return blocks;
         }

         
         public void Highlight(IBlock? block , PlayerRef playerRef)
         {
             _highLightExecutor.Highlight(block, playerRef);
         }
         
         
         
    }
}