using System;
using System.Collections;
using System.Collections.Generic;
using Carry.CarrySystem.Entity.Interfaces;
using UnityEngine;
#nullable  enable

namespace Carry.CarrySystem.Entity.Scripts
{
    [Serializable]
    public record GroundRecord
    {
        public Ground.Kind[] kinds = new Ground.Kind[1];
    }
    
    public class Ground : IEntity
    {
        public Vector2Int GridPosition { get; set; }
        public Ground.Kind KindValue { get; }
        
        public enum Kind
        {
            None,
            Kind1,
        }

        public Ground(Ground.Kind kind,Vector2Int gridPosition)
        {
            KindValue = kind;
            GridPosition = gridPosition;
        }

    }
    
}