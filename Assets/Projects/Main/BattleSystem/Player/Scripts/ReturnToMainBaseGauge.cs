using System;
using Fusion;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Main
{
    public class ReturnToMainBaseGauge
    {
        public float CurrentValue => _time / _fillTime;
        readonly NetworkRunner _runner;
        readonly float _fillTime;
        float _time = 0f;

        public ReturnToMainBaseGauge(NetworkRunner runner, float fillTime = 3.0f)
        {
            _runner = runner;
            _fillTime = fillTime;
        }

         public  void FillGauge()
        {
            _time += _runner.DeltaTime;
            Debug.Log($"_timer = {_time}");
            if (_time >= _fillTime)
            {
                ReturnToMainBase();
                ResetGauge();
            }
        }

        public void ResetGauge()
        {
            _time = 0f;
        }

        void ReturnToMainBase()
        {
            Debug.Log($"LocalPlayer:{_runner.LocalPlayer} MainBaseに帰還します");
        }
    }
}