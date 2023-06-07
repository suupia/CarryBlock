using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Nuts.BattleSystem.Player.Scripts
{
    public class ReturnToMainBaseGauge
    {
        readonly int _fillTime = 3;

        CancellationTokenSource _cts;
        Action _onReturnToMainBase = () => { };
        float _startTime;
        CancellationToken _token;
        public bool IsReturnToMainBase { get; private set; }

        public float RemainingTime => Mathf.Max(0, _fillTime - (Time.time - _startTime));

        public void SetOnReturnToMainBase(Action onReturnToMainBase)
        {
            _onReturnToMainBase = onReturnToMainBase;
        }

        public async void FillGauge()
        {
            _cts = new CancellationTokenSource();
            _token = _cts.Token;

            _startTime = Time.time;
            try
            {
                IsReturnToMainBase = true;
                await UniTask.Delay(TimeSpan.FromSeconds(_fillTime), cancellationToken: _token);
                ReturnToMainBase();
            }
            catch (OperationCanceledException e)
            {
                Debug.Log("Canceled ReturnToMainBase");
                IsReturnToMainBase = false;
            }
        }

        public void ResetGauge()
        {
            _cts.Cancel();
            IsReturnToMainBase = false;
        }

        void ReturnToMainBase()
        {
            Debug.Log("MainBaseに帰還します");
            _onReturnToMainBase();
        }
    }
}