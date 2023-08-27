using System.Collections.Generic;
using System.Linq;
using Carry.CarrySystem.Player.Info;
using Fusion;
using UnityEngine;

namespace Carry.CarrySystem.Player.Scripts
{
    /// <summary>
    /// パスの範囲を可視化するオブジェクトにアタッチするクラス
    /// </summary>
    public class PassRangeNet : NetworkBehaviour
    {
        [SerializeField] CapsuleCollider collider = null!;
        PlayerInfo _info = null!;
         int _layerMask;

        public void Init(PlayerInfo info, int layerMask)
        {
            _info = info;
            _layerMask = layerMask;
        }


        public (bool, GameObject) IsPassable()
        {
            var targetUnits = Search();
            var targetTransform = DetermineTarget(targetUnits);
            if (targetTransform == null)
            {
                return (false, null!);
            }
            var targetObj = targetTransform.gameObject;
            return (true, targetObj);
        }
        public Transform[] Search()
        {
            var center = _info.playerObj. transform.position;
            var _radius = collider.radius;
            var _targetBuffer = new Collider[10];
            var numFound = Physics.OverlapSphereNonAlloc(center, _radius,_targetBuffer, _layerMask);
            return  _targetBuffer
                .Where(c => c != null) // Filter out any null colliders
                .Where(c => c.transform != _info.playerObj.transform) // 自分以外を選択する
                .Select(c => c.transform)
                .ToArray();
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