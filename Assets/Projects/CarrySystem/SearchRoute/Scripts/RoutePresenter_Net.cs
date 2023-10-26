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

        [SerializeField] GameObject routeHighlightObject = null!;
        
        private readonly Vector3 _vertex = new Vector3(0, 0.4f, 0);
        public override void Render()
        {
            routeHighlightObject.SetActive(IsActive);
            StartAnimation();
        }
        
        public void SetPresenterActive(bool isActive)
        {
            IsActive = isActive;
        }

        void StartAnimation()
        {
            if (!IsActive)
            {
                IsAnimated = false;
                return;
            }
            if (IsAnimated) return;

            var pos = routeHighlightObject.transform.position;
            routeHighlightObject.transform.localScale = Vector3.one * 0.001f;
            
            Sequence sequence = DOTween.Sequence().OnStart(() => {
                    routeHighlightObject.transform.DOScale(Vector3.one, 0.3f);
                })
                .Join(transform.DOMove(pos + _vertex, 0.1f))
                .Insert(0.1f,transform.DOMove(pos, 0.25f).SetEase(Ease.OutBounce))
                .SetLink(routeHighlightObject,LinkBehaviour.RewindOnDisable);

            sequence.Play();
            IsAnimated = true;
        }
    }
}