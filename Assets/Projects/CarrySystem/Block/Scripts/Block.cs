#nullable enable

using Projects.CarrySystem.Block.Interfaces;
using UnityEngine;

namespace Projects.CarrySystem.Block.Scripts
{
    public abstract class Block : IBlock
    {
        public abstract Vector2Int GridPosition { get; set; }
        bool beingCarried; // ToDo: プロパティにする？
        bool beingNotCarried;
    }
}