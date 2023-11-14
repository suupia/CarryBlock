using System.Transactions;
using DG.Tweening;
using UnityEngine;

#nullable enable

namespace Projects.CarrySystem.SearchRoute.Scripts
{
    public static class RoutePresenterDoTweenSequence
    {
        static readonly Vector3 highestPoint = new Vector3(0, 0.4f, 0);

        public static Sequence GetSequence(Transform routePresenterTransform, GameObject routeHighlightObject)
        {
            routeHighlightObject.transform.localScale = Vector3.one * 0.001f;

            var pos = routeHighlightObject.transform.position;
            Sequence sequence = DOTween.Sequence();
            sequence.Append(routeHighlightObject.transform.DOScale(Vector3.one, 0.3f))
                .Join(routePresenterTransform.DOMove(pos + highestPoint, 0.1f))
                .Insert(0.1f,routePresenterTransform.DOMove(pos, 0.25f).SetEase(Ease.OutBounce))
                .SetLink(routeHighlightObject, LinkBehaviour.RewindOnDisable);
            return sequence;
        }
    }
}