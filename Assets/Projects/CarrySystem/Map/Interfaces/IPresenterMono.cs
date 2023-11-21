#nullable enable
using UnityEngine;

namespace Carry.CarrySystem.Map.Interfaces
{
    public interface IPresenterMono
    {
        public MonoBehaviour GetMonoBehaviour { get; }
        public void DestroyPresenter();
    }
}