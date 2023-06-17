using System;
using System.Collections;
using System.Collections.Generic;
using Carry.CarrySystem.Entity.Interfaces;
using UnityEngine;
#nullable  enable

namespace Carry.CarrySystem.Entity.Scripts
{
    [Serializable]
    public class GroundRecord
    {
        public Ground.Kind kind;
    }
    
    public class Ground : IEntity
    {
        public Vector2Int GridPosition { get; set; }
        public GroundRecord Record { get; }
        
        public enum Kind
        {
            None,
            Kind1,
        }

        public Ground(GroundRecord record,Vector2Int gridPosition)
        {
            Record = record;
            GridPosition = gridPosition;
        }

    }
    
}