using System;
using System.Collections;
using System.Collections.Generic;
using Carry.CarrySystem.Entity.Scripts;
using UnityEngine;

namespace Carry.CarrySystem.Map.Scripts
{
    [Serializable]
    public class GridMapData
    {
        public int width;
        public int height;
        public GroundRecord[] groundRecords;
        public RockRecord[] rockRecords;
    }

}

