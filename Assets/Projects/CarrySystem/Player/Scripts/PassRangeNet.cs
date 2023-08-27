using System.Collections.Generic;
using System.Linq;
using Carry.CarrySystem.Player.Info;
using Fusion;
using UnityEngine;
using UnityEngine.Serialization;
#nullable enable

namespace Carry.CarrySystem.Player.Scripts
{
    /// <summary>
    /// パスの範囲を可視化するオブジェクトにアタッチするクラス
    /// CarryPlayerControllerからGetComponentInChildrenで取得する
    /// </summary>
    public class PassRangeNet : NetworkBehaviour
    {
        [SerializeField] CapsuleCollider detectCollider = null!;
        PlayerInfo _info = null!;
         int _layerMask;

        public void Init(PlayerInfo info, int layerMask)
        {
            _info = info;
            _layerMask = layerMask;
        }


        public Transform? DetectedTarget()
        {
            var targetUnits = Search();
            var target = DetermineTarget(targetUnits);
            return target;
        }

        Transform? DetermineTarget(IEnumerable<Transform> targets)
        {
            Transform? minTransform = null;
            float minDistance = float.PositiveInfinity;
            foreach (var targetTransform in targets)
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
        
        Transform[] Search()
        {
            var center = _info.playerObj. transform.position;
            var radius = detectCollider.radius;
            var targetBuffer = new Collider[10];
            var numFound = Physics.OverlapSphereNonAlloc(center, radius,targetBuffer, _layerMask);
            var targets =  targetBuffer
                .Where(c => c != null) // Filter out any null colliders
                .Where(c => c.transform != _info.playerObj.transform) // 自分以外を選択する
                .Select(c => c.transform)
                .ToArray();
            Debug.Log($"targets {string.Join(",", targets.ToList())}");
            return targets;
        }
    }
}