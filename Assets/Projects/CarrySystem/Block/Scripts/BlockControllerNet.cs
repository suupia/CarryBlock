using System.Collections.Generic;
using System.Linq;
using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.Gimmick.Interfaces;
using Fusion;
using Projects.CarrySystem.Block.Info;
using UnityEngine;
using UnityEngine.Serialization;
#nullable enable

namespace Carry.CarrySystem.Block.Scripts
{
    public class BlockControllerNet : NetworkBehaviour
    {
        [FormerlySerializedAs("blockObj")] [SerializeField] GameObject blockViewObj = null!;  // ランタイムで生成しないので、SerializeFieldで受け取れる

        [FormerlySerializedAs("_info")] [SerializeField] BlockInfo info = null!;
        public BlockInfo Info => info;
        
        IList<IBlock> _blocks = new List<IBlock>();

        public void Init(IList<IBlock> blocks)
        {
            _blocks = blocks;
            
            foreach (var gimmick in _blocks.OfType<IGimmickBlock>())
            {
                gimmick.StartGimmick();
            }
        }
        public override void Spawned()
        {
            info.Init(blockViewObj, this);
        }

        public void OnDestroy()
        {
            foreach (var gimmick in _blocks.OfType<IGimmickBlock>())
            {
                gimmick.EndGimmick();
            }
        }
    }
    

}