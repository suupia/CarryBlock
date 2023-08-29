﻿using Carry.CarrySystem.Block.Interfaces;
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
        IBlockMonoDelegate _blockMonoDelegate = null!;

        public void Init(IBlockMonoDelegate blockMonoDelegate)
        {
            _blockMonoDelegate = blockMonoDelegate;
        }
        
        public override void Spawned()
        {
            _info.Init(blockViewObj, this);
        }
    }
}