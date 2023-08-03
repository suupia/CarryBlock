using System.Collections.Generic;
using System.Linq;
using Carry.CarrySystem.Player.Info;
using Fusion;
using UnityEngine;
#nullable enable

namespace Carry.CarrySystem.Player.Scripts
{
    public class PassAction
    {
        readonly float _radius;
        readonly int _layerMask;
        PlayerInfo _info = null!;
        
        public PassAction(Transform transform, float radius, int layerMask)
        {
            _radius = radius;
            _layerMask = layerMask; /* LayerMask.GetMask("Player");*/
        }
        public void SetUp(PlayerInfo info)
        {
            _info = info;
        }

        public void AttemptPass()
        {
            if (Search().Length > 0)
            {
                // バスの可能性原
            }
        }
        
        Transform[]? Search()
        {
            var center = _info.playerObj. transform.position;
            var colliders = Physics.OverlapSphere(center, _radius, _layerMask);
            return colliders.Map(c => c.transform);
        }

        public Transform? DetermineTarget(IEnumerable<Transform> targetUnits)
        {
            Transform? minTransform = null;
            float minDistance = float.PositiveInfinity;
            foreach (var targetTransform in targetUnits)
            {
                var distance = Vector3.Distance(_info.playerObj.transform.position, targetTransform.position);
                if (distance < minDistance)
                {
                    minTransform = targetTransform;
                    minDistance = distance;
                }
            }
            return minTransform;
        }
    }
}