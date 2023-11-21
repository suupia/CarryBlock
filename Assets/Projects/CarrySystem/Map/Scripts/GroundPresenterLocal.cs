#nullable enable
using Carry.CarrySystem.Map.Interfaces;
using UnityEngine;

namespace Carry.CarrySystem.Map.Scripts
{
    public class GroundPresenterLocal : MonoBehaviour, IPresenterMono
    {
        public MonoBehaviour GetMonoBehaviour => this;

        public void DestroyPresenter()
        {
            Destroy(gameObject);
        }
    }
}