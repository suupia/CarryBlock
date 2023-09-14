using Carry.CarrySystem.Player.Info;
using Carry.CarrySystem.Player.Interfaces;
using UnityEngine;

namespace Carry.CarrySystem.Player.Scripts
{
    public class DashExecutor : IDashExecutor
    {
        DashEffectPresenter _dashEffectPresenter;
        bool _isDashingTest;  // テスト用のフラグ
        
        public void Setup(PlayerInfo info)
        {
            
        }

        public void Dash()
        {
            Debug.Log($"Executing Dash");
            if (_isDashingTest)
            {
                _isDashingTest = false;
                _dashEffectPresenter.StopDash();
            }
            else
            {
                _isDashingTest = true;
                _dashEffectPresenter.StartDash();
            }
        }

        public void SetDashEffectPresenter(DashEffectPresenter presenter)
        {
            _dashEffectPresenter = presenter;
        }

        
    }
} 