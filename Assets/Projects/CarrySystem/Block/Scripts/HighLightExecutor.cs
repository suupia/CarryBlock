using System.Collections.Generic;
using System.Linq;
using Carry.CarrySystem.Block.Interfaces;
using Cysharp.Threading.Tasks;
using Fusion;
using Carry.CarrySystem.Block;
using Carry.CarrySystem.Block.Info;
using UnityEngine;
#nullable enable

namespace Carry.CarrySystem.Block.Scripts
{
    public class HighlightExecutor :IHighlightExecutor
    {
        readonly IList<BlockInfo> _blockInfos;
        public HighlightExecutor(IList<BlockInfo> blockInfos)
        {
            _blockInfos = blockInfos;
        }
        public void Highlight(IBlock? block, PlayerRef playerRef)
        {
            if(block == null) return;
            // Debug.Log($"HighLight");
            DecideMaterialSetter(block).ChangeWhite(playerRef);
        }

        BlockMaterialSetter DecideMaterialSetter(IBlock block)
        {
            var type = block.GetType();
            var materialSetter =  _blockInfos.Where(info=> info.BlockType == type).Select(info => info.BlockMaterialSetter).First();
            if (materialSetter != null)
            {
                return materialSetter;
            }
            else
            {
                Debug.LogError($"materialSetter is null");
                return null!;
            }
        }
    }
}