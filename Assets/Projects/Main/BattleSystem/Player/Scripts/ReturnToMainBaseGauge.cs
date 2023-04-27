using System;
using Fusion;
using UnityEngine;
using Cysharp.Threading.Tasks;
using VContainer;

namespace Main
{
    public class ReturnToMainBaseGauge
    {
        public float CurrentValue => _time / _fillTime;
        readonly float _fillTime = 3.0f;
        float _time = 0f;
        
         public  void FillGauge()
        {
            _time += Time.deltaTime;
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
            Debug.Log($"MainBaseに帰還します");
        }
    }
}