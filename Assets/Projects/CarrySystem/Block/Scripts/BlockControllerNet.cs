using Carry.CarrySystem.Block.Interfaces;
using Fusion;
using Projects.CarrySystem.Block.Info;
using UnityEngine;
using UnityEngine.Serialization;
#nullable enable

namespace Projects.CarrySystem.Block.Scripts
{
    public class BlockControllerNet : NetworkBehaviour
    {
        [FormerlySerializedAs("blockObj")] [SerializeField] GameObject blockViewObj = null!;  // ランタイムで生成しないので、SerializeFieldで受け取れる

        [FormerlySerializedAs("_info")] [SerializeField] BlockInfo info = null!;
        public BlockInfo Info => info;
        
        public override void Spawned()
        {
            info.Init(blockViewObj, this);
        }
    }
    

}