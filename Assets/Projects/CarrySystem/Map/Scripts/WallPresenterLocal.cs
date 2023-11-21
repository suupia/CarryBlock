#nullable enable
using Carry.CarrySystem.Map.Interfaces;
using UnityEngine;

namespace Carry.CarrySystem.Map.Scripts
{
    public class WallPresenterLocal : MonoBehaviour , IPresenterMono
    {
        public MonoBehaviour GetMonoBehaviour => this;

        public void DestroyPresenter()
        {
            Destroy(gameObject);
        }
    }
}