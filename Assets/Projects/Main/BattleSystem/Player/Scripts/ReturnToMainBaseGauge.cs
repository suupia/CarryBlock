using System;
using Fusion;
using UnityEngine;
using Cysharp.Threading.Tasks;
using VContainer;
using System.Threading;

namespace Main
{
    public class ReturnToMainBaseGauge
    {
        public float RemainingTime => _fillTime - (Time.time - _startTime);
        readonly int _fillTime = 3;
        float _startTime;
         CancellationTokenSource _cts;
         CancellationToken _token;

         public async void FillGauge()
         {
             _cts = new ();
             _token = _cts.Token;
             
             _startTime = Time.time;
             await UniTask.Delay(TimeSpan.FromSeconds(_fillTime),  cancellationToken:_token);
             ReturnToMainBase();
         }

        public void ResetGauge()
        {
            _cts.Cancel();
        }

        void ReturnToMainBase()
        {
            Debug.Log($"MainBaseに帰還します");
        }
    }
}