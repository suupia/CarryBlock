using System.Collections;
using System.Collections.Generic;
using Fusion;
using Main;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;

namespace Boss
{
    // public interface IEnemyMoveExecutor : IEnemyMove
    // {
    //     void Move(IUnitOnTargeted? target = default);
    // }

    public class RandomMove : IEnemyMoveExecutor
    {
        readonly float _simulationInterval = 2f;
        readonly IEnemyMoveExecutor _move;
        Vector3 _simulatedInput;
        bool _isSimulating;
        readonly CancellationTokenSource _cancellationTokenSource;

        public RandomMove(float simulationInterval, IEnemyMoveExecutor move)
        {
            _simulationInterval = simulationInterval;
            _move = move;
            _cancellationTokenSource = new CancellationTokenSource();
        }
        
        public void Move(Vector3 input = default)
        {
            InputSimulation();
            _move.Move(_simulatedInput);
        }
        
        async UniTaskVoid InputSimulation()
        {
            if(_isSimulating)return;
            _isSimulating = true;
            _simulatedInput = MoveUtility.SimulateRandomInput();
            await UniTask.Delay(TimeSpan.FromSeconds(_simulationInterval),
                cancellationToken: _cancellationTokenSource.Token);
            _isSimulating = false;
        }
    }
}