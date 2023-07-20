using System;
using System.Collections;
using System.Collections.Generic;
using Carry.CarrySystem.Entity.Interfaces;
using UnityEngine;
#nullable  enable

namespace Carry.CarrySystem.Entity.Scripts
{
    [Serializable]
    public record RockRecord
    {
        public Rock.Kind[] kinds =new Rock.Kind[5];
    }
    
    public class Rock : IEntity
    {
        public Vector2Int GridPosition { get; set; }
        public Rock.Kind KindValue { get; }

        public enum Kind
        {
            None,
            Kind1,
        }

        public Rock(Rock.Kind kind,Vector2Int gridPosition)
        {
            KindValue = kind;
            GridPosition = gridPosition;
        }

    }
    
}
