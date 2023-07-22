using Fusion;
using Projects.CarrySystem.RoutingAlgorithm.Interfaces;
using UnityEngine;

namespace Carry.CarrySystem.SearchRoute.Scripts
{
    public class RoutePresenter_Net : NetworkBehaviour, IRoutePresenter
    {
        [Networked] NetworkBool IsActive { get; set; }
        
        [SerializeField] GameObject routeHighlightObject = null!;
        
        public override void Render()
        {
            routeHighlightObject.SetActive(IsActive);
        }
        
        public void SetActive(bool isActive)
        {
            IsActive = isActive;
        }
    }
}