using Fusion;
using Carry.CarrySystem.RoutingAlgorithm.Interfaces;
using UnityEngine;
#nullable enable


namespace Carry.CarrySystem.SearchRoute.Scripts
{
    public class RoutePresenter_Net : NetworkBehaviour, IRoutePresenter
    {
        [Networked] NetworkBool IsActive { get; set; }
        [Networked] NetworkBool IsAnimated { get; set; }
        
        [SerializeField] GameObject routeHighlightObject = null!;
        
        public override void Render()
        {
            routeHighlightObject.SetActive(IsActive);
            StartAnimation();
        }
        
        public void SetPresenterActive(bool isActive)
        {
            IsActive = isActive;
        }

        public void StartAnimation()
        {
            if (!IsActive)
            {
                IsAnimated = false;
                return;
            }
            if (IsAnimated) return;
            
            routeHighlightObject.SetActive(IsActive);
            IsAnimated = true;
        }
    }
}