using System;
using System.Collections;
using System.Collections.Generic;
using Carry.CarrySystem.Block.Scripts;
using Carry.CarrySystem.CarriableBlock.Scripts;
using Carry.CarrySystem.Entity.Scripts;
using Projects.CarrySystem.Gimmick.Scripts;
using Projects.CarrySystem.Item.Scripts;
using UnityEngine;
#nullable enable

namespace Carry.CarrySystem.Map.Scripts
{
    [Serializable]
    public record EntityGridMapData
    {
        public int width;
        public int height;
        public GroundRecord[]? groundRecords;
        
        // Block
        public BasicBlockRecord[]? basicBlockRecords;
        public RockRecord[]? rockRecords;
        public HeavyBlockRecord[]? heavyBlockRecords;
        public FragileBlockRecord[]? fragileBlockRecords;
        public ConfusionBlockRecord[]? confusionBlockRecords;
        public CannonBlockRecord[]? cannonBlockRecords;
        public TreasureCoinRecord[]? treasureCoinRecords;
        public SpikeGimmickRecord[]? spikeGimmickRecords;
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
        Sakai,
        // EditMapシーン開始時にこのenumの中から識別子を選ぶ
    }

}

