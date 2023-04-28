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
        public bool IsReturnToMainBase => _isReturnToMainBase;
        public float RemainingTime => Mathf.Max( 0,_fillTime - (Time.time - _startTime));
        readonly int _fillTime = 3;
        float _startTime;
        bool _isReturnToMainBase;
        CancellationTokenSource _cts;
        CancellationToken _token;

        public async void FillGauge()
        {
            _cts = new();
            _token = _cts.Token;

            _startTime = Time.time;
            try {  
                _isReturnToMainBase = true;
                await UniTask.Delay(TimeSpan.FromSeconds(_fillTime), cancellationToken: _token);
                ReturnToMainBase(); 
            }
            catch (OperationCanceledException e){  
                Debug.Log("Canceled ReturnToMainBase");
                _isReturnToMainBase = false;
            }
        }

        public void ResetGauge()
        {
            _cts.Cancel();
            _isReturnToMainBase = false;
        }

        void ReturnToMainBase()
        {
            Debug.Log($"MainBaseに帰還します");
        }
    }
}