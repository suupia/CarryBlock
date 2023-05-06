using UnityEngine;

namespace Main
{
    public interface ISearch
    {
        
        Collider[] Search();
    }
    
    public class RangeSearch: ISearch
    {
        private readonly Transform _transform;
        private readonly float _radius;
        private readonly int _layerMask;

        public RangeSearch(Transform transform, float radius, int layerMask)
        {
            _transform = transform;
            _radius = radius;
            _layerMask = layerMask;
        }
        // private Collider[] _colliders = new Collider[16];
        public Collider[] Search()
        {
            var center = _transform.position;
            // Physics.OverlapSphereNonAlloc(_transform.position, _radius, _colliders, _layerMask);
            return Physics.OverlapSphere(center, _radius, _layerMask);
        }
    }
}