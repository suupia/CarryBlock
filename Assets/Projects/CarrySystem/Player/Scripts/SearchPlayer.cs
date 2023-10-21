using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#nullable enable

namespace Carry.CarrySystem.Player.Scripts
{
    public class SearchPlayer
    {
        readonly Collider _detectCollider;
        readonly float _detectSize;
        readonly Transform _playerTransform;
        readonly int _layerMask;
        
        public SearchPlayer(Collider detectCollider, float detectSize, Transform playerTransform,int layerMask )
        {
            _detectCollider = detectCollider;
            _detectSize = detectSize;
            _playerTransform = playerTransform;
            _layerMask = layerMask;
        }
        
        public CarryPlayerControllerNet? DetectedTarget()
        {
            var targetUnits = Search();
            var target = DetermineTarget(targetUnits);
            return target;
        }

        CarryPlayerControllerNet? DetermineTarget(IEnumerable<CarryPlayerControllerNet?> targets)
        {
            CarryPlayerControllerNet? minPlayerController = null;
            float minDistance = float.PositiveInfinity;
            foreach (var targetController in targets)
            {
                if(targetController == null) continue;
                var distance = Vector3.Distance(_playerTransform.position, targetController.gameObject.transform.position);
                if (distance < minDistance)
                {
                    minPlayerController = targetController;
                    minDistance = distance;
                }
            }
            return minPlayerController;
        }
        
        CarryPlayerControllerNet?[] Search()
        {
            var center = _playerTransform.position;
            var radius = _detectCollider.transform.localScale.x * _detectSize; // 外部で決める （radiusを倍率とするとうまく計算できる（0.5倍））
            var targetBuffer = new Collider[10];
            var numFound = Physics.OverlapSphereNonAlloc(center, radius,targetBuffer, _layerMask);
            Debug.Log($"targetBuffer {string.Join(",", targetBuffer.ToList())}");
            var targets =  targetBuffer
                .Where(c => c != null) // Filter out any null colliders
                .Where(c => c.transform != _playerTransform) // 自分以外を選択する
                .Select(c => c.gameObject.GetComponent<CarryPlayerControllerNet>())
                .ToArray();
            Debug.Log($"targets {string.Join(",", targets.ToList())}");
            return targets;
        }
    }
}