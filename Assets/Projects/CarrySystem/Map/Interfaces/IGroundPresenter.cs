#nullable enable
using UnityEngine;

namespace Carry.CarrySystem.Map.Interfaces
{
    public interface IGroundPresenter
    {
        public MonoBehaviour GetMonoBehaviour { get; }
        public void DestroyPresenter();
    }
}