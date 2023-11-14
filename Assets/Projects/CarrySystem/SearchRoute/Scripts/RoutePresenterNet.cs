using System;
using Fusion;
using Carry.CarrySystem.RoutingAlgorithm.Interfaces;
using UnityEngine;
using DG.Tweening;
#nullable enable


namespace Carry.CarrySystem.SearchRoute.Scripts
{
    public class RoutePresenterNet : NetworkBehaviour, IRoutePresenter
    {
        public bool IsActive => IsActiveNet;
        [Networked]  NetworkBool IsActiveNet { get; set; }
        [Networked]  NetworkBool IsAnimated { get; set; }

        [SerializeField] GameObject routeHighlightObject = null!;
        
        readonly Vector3 _vertex = new Vector3(0, 0.4f, 0);
        
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

            var pos = routeHighlightObject.transform.position;
            routeHighlightObject.transform.localScale = Vector3.one * 0.001f;

            Sequence sequence = DOTween.Sequence();
            sequence.Append(routeHighlightObject.transform.DOScale(Vector3.one, 0.3f))
                .Join(transform.DOMove(pos + _vertex, 0.1f))
                .Append(transform.DOMove(pos, 0.25f).SetEase(Ease.OutBounce))
                .SetLink(routeHighlightObject, LinkBehaviour.RewindOnDisable);
            sequence.Play();
            
            IsAnimated = true;
        }
    }
}