using UnityEngine;

#nullable enable

namespace Carry.CarrySystem.Entity.Interfaces
{
    public interface  IEntity
    {
        System.Type GetType();
         Vector2Int GridPosition { get; set; }
    }
}