using Carry.CarrySystem.Block.Interfaces;
using Fusion;
using Projects.CarrySystem.Block.Info;
using UnityEngine;

namespace Projects.CarrySystem.Block.Scripts
{
    public class BlockControllerNet : NetworkBehaviour
    {
        [SerializeField] GameObject blockObj;  // ランタイムで生成しないので、SerializeFieldで受け取れる

        [SerializeField] BlockInfo _info;
        IBlock _block;
        
        public override void Spawned()
        {
            _info.Init(blockObj, this);
        }
    }
}