using System;
using System.Collections;
using System.Collections.Generic;
using Carry.CarrySystem.Block.Scripts;
using Carry.CarrySystem.Entity.Scripts;
using UnityEngine;

namespace Carry.CarrySystem.Map.Scripts
{
    [Serializable]
    public record EntityGridMapData
    {
        public int width;
        public int height;
        public GroundRecord[] groundRecords;
        
        // Block
        public BasicBlockRecord[] basicBlockRecords;
        public RockRecord[] rockRecords;
        public HeavyBlockRecord[] heavyBlockRecords;
    }

    public enum MapKey
    {
        Default,
        Morita,
        Osawa,
        Hasegawa,
        Suzuki,
        Kaburagi,
        Tsukamoto,
        Harano,
        Isikawa,
        Abe,
        // EditMapシーン開始時にこのenumの中から識別子を選ぶ
    }

}

