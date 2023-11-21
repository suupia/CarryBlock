#nullable enable
using UnityEngine;

namespace Carry.CarrySystem.Map.Interfaces
{
    public interface IWallPresenter
    {
        public MonoBehaviour GetMonoBehaviour { get; }
        public void DestroyPresenter();
    }
}