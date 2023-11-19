using Carry.CarrySystem.RoutingAlgorithm.Interfaces;
using UnityEngine;
using DG.Tweening;
#nullable enable

namespace Projects.CarrySystem.SearchRoute.Scripts
{
    public class RoutePresenterLocal : MonoBehaviour, IRoutePresenter
    {
        public GameObject GameObject => gameObject;
        public bool IsActive { get; private set; }
        bool IsAnimated { get; set; }
        
        [SerializeField] GameObject routeHighlightObject = null!;
        
        readonly Vector3 _vertex = new Vector3(0, 0.4f, 0);
        
        public void Update()
        {
            routeHighlightObject.SetActive(IsActive);
            RouteAnimation();
        }
        
        public void SetPresenterActive(bool isActive)
        {
            IsActive = isActive;
        }

        void RouteAnimation()
        {
            if (!IsActive)
            {
                IsAnimated = false;
                return;
            }
            if (IsAnimated) return;

            var pos = routeHighlightObject.transform.position;
            routeHighlightObject.transform.localScale = Vector3.one * 0.001f;

            Sequence sequence = DOTween.Sequence().OnStart(() =>
            {
                routeHighlightObject.transform.DOScale(Vector3.one, 0.3f);
            });
            
            sequence.Join(transform.DOMove(pos + _vertex, 0.1f))
                .Insert(0.1f,transform.DOMove(pos, 0.25f).SetEase(Ease.OutBounce))
                .SetLink(routeHighlightObject, LinkBehaviour.RewindOnDisable);

            sequence.Play();
            IsAnimated = true;
        }
    }
}