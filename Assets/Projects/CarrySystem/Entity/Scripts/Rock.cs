using System;
using System.Collections;
using System.Collections.Generic;
using Carry.CarrySystem.Entity.Interfaces;
using UnityEngine;
#nullable  enable

namespace Carry.CarrySystem.Entity.Scripts
{
    [Serializable]
    public class RockRecord
    {
        public Rock.Kind kind;
    }
    
    public class Rock : IEntity
    {
        public Vector2Int GridPosition { get; set; }
        public RockRecord Record { get; }

        public enum Kind
        {
            None,
            Kind1,
        }

        public Rock(RockRecord record,Vector2Int gridPosition)
        {
            Record = record;
            GridPosition = gridPosition;
        }

    }
    
}
