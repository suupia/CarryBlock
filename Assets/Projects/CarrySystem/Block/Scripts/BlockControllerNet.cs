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
        [FormerlySerializedAs("blockObj")] [SerializeField] GameObject blockViewObj;  // ランタイムで生成しないので、SerializeFieldで受け取れる

        [SerializeField] BlockInfo _info = null!;
        public BlockInfo Info => _info;
        
        public override void Spawned()
        {
            _info.Init(blockViewObj, this);
        }
    }
    

}