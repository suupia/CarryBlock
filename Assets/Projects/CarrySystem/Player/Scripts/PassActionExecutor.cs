using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Carry.CarrySystem.Cart.Scripts;
using Carry.CarrySystem.Player.Info;
using Carry.CarrySystem.Player.Interfaces;
using Fusion;
using UnityEngine;
#nullable enable

namespace Carry.CarrySystem.Player.Scripts
{
    public class PassActionExecutor : IPassActionExecutor
    {
        PlayerInfo _info = null!;
        readonly HoldActionExecutor _holdActionExecutor;
        readonly float _radius;
        readonly int _layerMask;
        
       readonly  Collider[] _targetBuffer = new Collider[10];
        
        public PassActionExecutor(HoldActionExecutor holdActionExecutor, float radius, int layerMask)
        {
            _holdActionExecutor = holdActionExecutor;
            _radius = radius;
            _layerMask = layerMask; /*LayerMask.GetMask("Player");*/
        }
        public void Setup(PlayerInfo info)
        {
            _info = info;
        }

        public void Reset()
        {
            
        }

        public void PassAction()
        {
            var targets = Search();
            Debug.Log($"targets {string.Join(",", targets.ToList())}");
            if (DetermineTarget(targets) is {} target)
            {
                var targetPlayerController  = target.GetComponent<CarryPlayerControllerNet>();
                if (targetPlayerController == null)
                {
                    Debug.LogError($"{target.name} には CarryPlayerControllerNet がアタッチされていません");
                    return;
                }
                
                Debug.Log($"targetPlayerController: {targetPlayerController.name}, {targetPlayerController.Runner.LocalPlayer}に対してPassを試みます");
                
                
                Debug.Log($"!_holdActionExecutorExecutor.IsHoldingBlock: {_holdActionExecutor.IsHoldingBlock}");
                Debug.Log($"!targetPlayerController.Character.CanReceivePass(): {targetPlayerController.Character.CanReceivePass()}");
                
                // Passする側がPassできる状況にある
                if(! _holdActionExecutor.IsHoldingBlock)return;
                // Passを受ける側がPassを受け取れる状況にある
                if(!targetPlayerController.Character.CanReceivePass())return;
                PassBlock();
                targetPlayerController.Character.ReceivePass();
            }
        }

        public bool CanReceivePass()
        {
            return !_holdActionExecutor.IsHoldingBlock;
        }

        public void PassBlock()
        {
            Debug.Log($"Pass Block");
        }
        
        public void ReceivePass()
        {
            Debug.Log("Receive Pass");
        }
        
        Transform[] Search()
        {
            var center = _info.playerObj. transform.position;
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