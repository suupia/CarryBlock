#nullable enable
using Carry.CarrySystem.Block.Info;
using UnityEngine;

namespace Carry.CarrySystem.Block.Interfaces
{
    public interface IBlockController
    {
        public MonoBehaviour GetMonoBehaviour { get; }
        public BlockInfo Info { get; }

    }
}