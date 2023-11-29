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
        public Ground.Kind KindValue { get; }
        
        public enum Kind
        {
            None,
            Kind1,
        }

        public Ground(Ground.Kind kind)
        {
            KindValue = kind;
        }

    }
    
}