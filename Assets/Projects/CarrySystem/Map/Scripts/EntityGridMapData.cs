using System;
using System.Collections;
using System.Collections.Generic;
using Carry.CarrySystem.Entity.Scripts;
using UnityEngine;

namespace Carry.CarrySystem.Map.Scripts
{
    [Serializable]
    public class EntityGridMapData
    {
        public int width;
        public int height;
        public GroundRecord[] groundRecords;
        public RockRecord[] rockRecords;
    }

    public enum MapKey
    {
        Default,
        Koki,
        // EditMapシーン開始時にこのenumの中から識別子を選ぶ
    }

}

