# nullable enable
using System.Collections.Generic;
using Fusion;
using Projects.BattleSystem.Boss.Scripts;
using UnityEngine;
using Projects.BattleSystem.Enemy.Monster.Interfaces;


namespace Projects.BattleSystem.Enemy.Scripts
{
    public class DoNothingSearch : IEnemySearch
    {
        public DoNothingSearch()
        {
        }

        public Transform[]? Search()
        {
            return null;
        }

        public Transform? DetermineTarget(IEnumerable<Transform> targetUnits)
        {
            return null;
        }
    }
    public class NearestSearch : IEnemySearch
    {
        readonly int _layerMask;
        readonly float _radius;
        readonly Transform _transform;

        public NearestSearch(Transform transform, float radius, int layerMask)
        {
            _transform = transform;
            _radius = radius;
            _layerMask = layerMask;
        }

        public Transform[]? Search()
        {
            var center = _transform.position;
            var colliders = Physics.OverlapSphere(center, _radius, _layerMask);
            return colliders.Map(c => c.transform);
        }

        public Transform? DetermineTarget(IEnumerable<Transform> targetUnits)
        {
            Transform? minTransform = null;
            float minDistance = float.PositiveInfinity;
            foreach (var targetTransform in targetUnits)
            {
                var distance = Vector3.Distance(_transform.position, targetTransform.position);
                if (distance < minDistance)
                {
                    minTransform = targetTransform;
                    minDistance = distance;
                }
            }
            return minTransform;
        }
    }
    
    public class FarthestSearch : IEnemySearch
    {
        readonly int _layerMask;
        readonly float _radius;
        readonly Transform _transform;

        public FarthestSearch(Transform transform, float radius, int layerMask)
        {
            _transform = transform;
            _radius = radius;
            _layerMask = layerMask;
        }

        public Transform[]? Search()
        {
            var center = _transform.position;
            var colliders = Physics.OverlapSphere(center, _radius, _layerMask);
            return colliders.Map(c => c.transform);
        }

        public Transform? DetermineTarget(IEnumerable<Transform> targetUnits)
        {
            Transform? maxTransform = null;
            float maxDistance = float.NegativeInfinity;
            foreach (var targetTransform in targetUnits)
            {
                var distance = Vector3.Distance(_transform.position, targetTransform.position);
                if (distance > maxDistance)
                {
                    maxTransform = targetTransform;
                    maxDistance = distance;
                }
            }
            return maxTransform;
        }
    }
}