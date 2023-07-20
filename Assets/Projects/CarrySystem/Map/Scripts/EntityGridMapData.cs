using System;
using System.Collections;
using System.Collections.Generic;
using Carry.CarrySystem.Entity.Scripts;
using UnityEngine;
using Projects.CarrySystem.Block.Scripts;

namespace Carry.CarrySystem.Map.Scripts
{
    [Serializable]
    public record EntityGridMapData
    {
        public int width;
        public int height;
        public GroundRecord[] groundRecords;
        public RockRecord[] rockRecords;
        
        // Block
        public BasicBlockRecord[] basicBlockRecords;
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
        Kurauchi,
        Harano,
        Isikawa,
        Abe,
        // EditMapシーン開始時にこのenumの中から識別子を選ぶ
    }

}

