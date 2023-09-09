using Carry.CarrySystem.Player.Interfaces;
using Cysharp.Threading.Tasks;
using UnityEngine;

#nullable enable

namespace Carry.CarrySystem.Player.Scripts
{
    public class OnDamageExecutor : IOnDamageExecutor
    {
        readonly float _faintDuration = 1.0f;
        readonly IMoveExecutorSwitcher _moveExecutorSwitcher;

        public OnDamageExecutor(IMoveExecutorSwitcher moveExecutorSwitcher)
        {
            _moveExecutorSwitcher = moveExecutorSwitcher;
        }
        public void OnDamage()
        {
            Debug.Log($"ダメージを受けた！");
            var _ = Faint();
        }
        
        async UniTaskVoid Faint()
        {
            _moveExecutorSwitcher.SwitchToFaintedMove();
            Debug.Log($"気絶する");
            await UniTask.Delay((int)(_faintDuration * 1000));
            Debug.Log($"気絶から復帰する");
            _moveExecutorSwitcher.SwitchToRegularMove();
        }
    }
}