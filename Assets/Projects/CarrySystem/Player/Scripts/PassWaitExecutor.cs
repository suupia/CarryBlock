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
    public class PassWaitExecutor
    {
        //public bool IsPassing { get; private set; }
        CancellationTokenSource? _cts;
        
        public void WaitPassAction(ICharacter character, ICarriableBlock block)
        {
            _cts?.Cancel();
            _cts = new CancellationTokenSource();
            AsyncPassed(character, block , _cts.Token).Forget();
        }

        async UniTaskVoid AsyncPassed(ICharacter character, ICarriableBlock block, CancellationToken cts)
        {
            try
            {
                //Debug.Log($"PassBlockMoveExecutor.AsyncPassed()");
                await UniTask.Delay(1000, cancellationToken: cts);
                character.ReceivePass(block);
            }
            catch (OperationCanceledException)
            {
                // Do Nothing
            }
        }
    }
}