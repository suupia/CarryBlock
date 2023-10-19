using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.CarriableBlock.Interfaces;
using Carry.CarrySystem.Cart.Scripts;
using Carry.CarrySystem.Player.Info;
using Carry.CarrySystem.Player.Interfaces;
using Fusion;
using UnityEngine;
#nullable enable

namespace Carry.CarrySystem.Player.Scripts
{
    using System.Threading;
    using Cysharp.Threading.Tasks;
    public class PassBlockMoveExecutor
    {
        public bool IsPassing { get; private set; }
        CancellationTokenSource? _cts;
        
        public void WaitPassAction()
        {
            IsPassing = true;
            _cts?.Cancel();
            _cts = new CancellationTokenSource();
            AsyncPassed(_cts.Token).Forget();
        }
        
        async UniTaskVoid AsyncPassed(CancellationToken cts)
        {
            try
            {
                //Debug.Log($"PassBlockMoveExecutor.AsyncPassed()");
                await UniTask.Delay(5000, cancellationToken: cts);
                IsPassing = false;
            }
            catch (OperationCanceledException)
            {
                // Do Nothing
            }
        }
    }
}