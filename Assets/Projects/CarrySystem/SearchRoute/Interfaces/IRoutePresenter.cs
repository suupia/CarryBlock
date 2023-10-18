using Fusion;

namespace Carry.CarrySystem.RoutingAlgorithm.Interfaces
{
    public interface IRoutePresenter
    {
         [Networked] public NetworkBool IsActive { get; set; }
         void SetPresenterActive(bool isActive);

         void StartAnimation();
    }
}