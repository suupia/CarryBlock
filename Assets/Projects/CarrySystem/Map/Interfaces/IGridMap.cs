using UnityEngine;

namespace Carry.CarrySystem.Map.Interfaces
{
    // This interface is used to treat EntityGridMap and NumericGridMap as the same thing for WaveletSearchExecutor.
    public interface IGridMap
    {
        public int Width { get; }
        public int Height { get; }
        public int Length { get; }
    }
}