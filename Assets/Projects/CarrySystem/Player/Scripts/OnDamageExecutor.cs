using Carry.CarrySystem.CG.Tsukinowa;
using Carry.CarrySystem.Player.Info;
using Carry.CarrySystem.Player.Interfaces;
using Cysharp.Threading.Tasks;
using UnityEngine;

#nullable enable

namespace Carry.CarrySystem.Player.Scripts
{
    public class OnDamageExecutor : IOnDamageExecutor
    {
        PlayerInfo _info = null!;
        readonly float _faintDuration = 1.0f;
        readonly IMoveExecutorSwitcher _moveExecutorSwitcher;

        public OnDamageExecutor(IMoveExecutorSwitcher moveExecutorSwitcher)
        {
            _moveExecutorSwitcher = moveExecutorSwitcher;
        }

        public void Setup(PlayerInfo info)
        {
            _info = info;
        }
        public void OnDamage()
        {
            Debug.Log($"ダメージを受けた！");
            var _ = Faint();
        }
        
        async UniTaskVoid Faint()
        {
            Debug.Log($"気絶する");
            _moveExecutorSwitcher.SwitchToFaintedMove();
            Debug.Log($"_info.PlayerObj.GetComponentInChildren<TsukinowaMaterialSetter>()) : {_info.PlayerObj.GetComponentInChildren<TsukinowaMaterialSetter>()}");
            _info.PlayerObj.GetComponentInChildren<TsukinowaMaterialSetter>().Blinking();
            await UniTask.Delay((int)(_faintDuration * 1000));
            Debug.Log($"気絶から復帰する");
            _moveExecutorSwitcher.SwitchToRegularMove();
        }
    }
}