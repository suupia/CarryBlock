﻿using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
        
       readonly  Collider[] _targetBuffer = new Collider[10];
        
        public PassAction(PlayerInfo info, float radius, int layerMask)
        {
            _info = info;
            _radius = radius;
            _layerMask = layerMask; /* LayerMask.GetMask("Player");*/
        }
        public void SetUp(PlayerInfo info)
        {
            _info = info;
        }

        public void AttemptPass()
        {
            var targets = Search();
            if (DetermineTarget(targets) is {} target)
            {
                var targetPlayerController  = target.GetComponent<CarryPlayerControllerNet>();
                if (targetPlayerController == null)
                {
                    Debug.LogError($"{target.name} には CarryPlayerControllerNet がアタッチされていません");
                    return;
                }
                
                Debug.Log($"targetPlayerController: {targetPlayerController.name}に対してPassを試みます");
                
                // Passする側がPassできる状況にある
                // Passを受ける側がPassを受け取れる状況にある
                // this.playerController.Pass(targetPlayerController); 
                // 的な処理を書く
            }
        }
        
        Transform[] Search()
        {
            var center = _info.playerObj. transform.position;
            var numFound = Physics.OverlapSphereNonAlloc(center, _radius,_targetBuffer, _layerMask);
            Debug.Log($"_targetBuffer: {_targetBuffer}");
            return _targetBuffer.Map(c => c.transform);
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