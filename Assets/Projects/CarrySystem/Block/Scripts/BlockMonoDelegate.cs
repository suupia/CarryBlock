using System.Collections.Generic;
using System.Linq;
using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.Block.Scripts;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Player.Interfaces;
using Fusion;
using Projects.CarrySystem.Block.Info;
using UnityEngine;
#nullable enable

namespace Projects.CarrySystem.Block.Scripts
{
    /// <summary>
    /// IBlock(ドメインの情報)とBlockInfo(NetworkBehaviourの情報)を持つクラス
    /// ドメインの処理はこのクラスにアクセスして行う
    /// </summary>
    public class BlockMonoDelegate : IBlockMonoDelegate
    {
         public IBlock? Block => _blocks.FirstOrDefault(); 
         public IList<IBlock> Blocks => _blocks;
         

         readonly IList<IBlock> _blocks;
         readonly IList<BlockInfo> _blockInfos;
         readonly IBlock? _block;
         readonly IBlockPresenter _blockPresenter;

         readonly IHighlightExecutor _highLightExecutor;

         public BlockMonoDelegate(IList<IBlock> blocks, IList<BlockInfo> blockInfos, IBlockPresenter blockPresenter)
         {
             _blocks = blocks;
             _blockInfos = blockInfos;
             _blockPresenter = blockPresenter;
             
             _highLightExecutor = new HighlightExecutor(_blockInfos);

         }
         //
         // public void SetInfo(BlockInfo info)
         // {
         //     Info = info;
         //     var materialSetter = Info.blockController.GetComponent<BlockMaterialSetter>();
         //     if (materialSetter == null) Debug.LogError($"materialSetter is null");
         //     materialSetter.Init(Info);
         //     _highLightExecutor = new HighlightExecutor(Info.blockController.gameObject.GetComponent<BlockMaterialSetter>());
         //
         // }
         
         public void AddBlock(IBlock block)
         {
             _blocks.Add(block);
            _blockPresenter.SetBlockActiveData(block, _blocks.Count);

         }
         public void RemoveBlock(IBlock block)
         {
             _blocks.Remove(block);
             _blockPresenter.SetBlockActiveData(block, _blocks.Count);

         }
         
         public void Highlight(IBlock? block , PlayerRef playerRef)
         {
             _highLightExecutor.Highlight(block, playerRef);
         }
         
         // IBlock implementation
         public Vector2Int GridPosition { get => _block.GridPosition; set => _block.GridPosition = value; }
         public int MaxPlacedBlockCount => _block.MaxPlacedBlockCount;
         public bool CanPickUp() => _block.CanPickUp();
         public void PickUp(ICharacter character) => _block.PickUp(character);
         public bool CanPutDown(IList<IBlock> blocks) => _block.CanPutDown(blocks);
         public void PutDown(ICharacter character) => _block.PutDown(character);
    }
}