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
        CancellationTokenSource? _cancellationTokenSource;
        
        public void WaitPassAction()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = new CancellationTokenSource();
            var _ = AsyncPassed(_cancellationTokenSource.Token);
        }
        
        async UniTaskVoid AsyncPassed(CancellationToken cancellationToken)
        {
            try
            {
                await UniTask.Delay(1000, cancellationToken: cancellationToken);
            }
            catch (OperationCanceledException)
            {
                // Do Nothing
            }
        }
    }
}