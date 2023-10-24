using System;
using Fusion;
using Carry.CarrySystem.RoutingAlgorithm.Interfaces;
using UnityEngine;
using DG.Tweening;
#nullable enable


namespace Carry.CarrySystem.SearchRoute.Scripts
{
    public class RoutePresenter_Net : NetworkBehaviour, IRoutePresenter
    {
        [Networked] public NetworkBool IsActive { get; set; }
        [Networked] private NetworkBool IsAnimated { get; set; }
        private bool _isInit = false;

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

            routeHighlightObject.transform.localScale = Vector3.one * 0.001f;
            routeHighlightObject.transform.DOScale(Vector3.one, 0.2f);
            IsAnimated = true;
        }
    }
}