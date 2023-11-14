using Fusion;

namespace Carry.CarrySystem.RoutingAlgorithm.Interfaces
{
    public interface IRoutePresenter
    {
         public bool IsActive { get; }
         void SetPresenterActive(bool isActive);
    }
}