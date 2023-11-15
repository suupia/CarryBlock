using Fusion;
using UnityEngine;

namespace Carry.CarrySystem.RoutingAlgorithm.Interfaces
{
    public interface IRoutePresenter
    {
         public GameObject GameObject { get; }
         public bool IsActive { get; }
         void SetPresenterActive(bool isActive);
    }
}