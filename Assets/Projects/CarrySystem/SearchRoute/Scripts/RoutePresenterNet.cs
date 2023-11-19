using System;
using Fusion;
using Carry.CarrySystem.RoutingAlgorithm.Interfaces;
using UnityEngine;
using DG.Tweening;
using Projects.CarrySystem.SearchRoute.Scripts;

#nullable enable


namespace Carry.CarrySystem.SearchRoute.Scripts
{
    public class RoutePresenterNet : NetworkBehaviour, IRoutePresenter
    {
        public GameObject GameObject => gameObject;
        public bool IsActive => IsActiveNet;
        [Networked]  NetworkBool IsActiveNet { get; set; }
        [Networked]  NetworkBool IsAnimated { get; set; }

        [SerializeField] GameObject routeHighlightObject = null!;
        
        public override void Render()
        {
            routeHighlightObject.SetActive(IsActiveNet);
            RouteAnimation();
        }
        
        public void SetPresenterActive(bool isActive)
        {
            IsActiveNet = isActive;
        }

        void RouteAnimation()
        {
            if (!IsActiveNet)
            {
                IsAnimated = false;
                return;
            }
            if (IsAnimated) return;

            var sequence = RoutePresenterDoTweenSequence.GetSequence(transform, routeHighlightObject);
            sequence.Play();

            IsAnimated = true;
        }
    }
}