﻿using System.Collections.Generic;
using System.Linq;
using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.GimmickBlock.Interfaces;
using Fusion;
using Carry.CarrySystem.Block.Info;
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
        
        public override void Spawned()
        {
            info.Init(blockViewObj, this);
        }
    }
    

}