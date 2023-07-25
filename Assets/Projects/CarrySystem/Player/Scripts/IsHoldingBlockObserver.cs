using System;
using System.Collections.Generic;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.SearchRoute.Scripts;
using Projects.CarrySystem.Block.Interfaces;
using UniRx;
using UnityEngine;

namespace Carry.CarrySystem.Player.Scripts
{
    public class IsHoldingBlockObserver
    {
        readonly List<HoldAction> _holdActions = new List<HoldAction>();
        readonly IMapUpdater _mapUpdater;
        readonly WaveletSearchBuilder _waveletSearchBuilder;
        IDisposable _subscription;  // to hold the subscription to dispose it later if needed
        
        public IsHoldingBlockObserver(IMapUpdater entityGridMapSwitcher,
            WaveletSearchBuilder waveletSearchBuilder)
        {
            _mapUpdater = entityGridMapSwitcher;
            _waveletSearchBuilder = waveletSearchBuilder;
        }
        
        public void StartObserve()
        {
            _subscription = _holdActions.ToObservable()
                .SelectMany(holdAction => holdAction.ObserveEveryValueChanged(h => h.IsHoldingBlock))
                .Subscribe(_ => ShowAccessibleArea());
        }
        public void StopObserve()
        {
            _subscription.Dispose();
        }

        public void RegisterHoldAction(HoldAction holdAction)
        {
            _holdActions.Add(holdAction);
        }

        void ShowAccessibleArea()
        {
            var map = _mapUpdater.GetMap();
            Func<int, int, bool> isWall = (x, y) => map.GetSingleEntityList<IBlock>(new Vector2Int(x, y)).Count > 0;
            var waveletSearchExecutor = _waveletSearchBuilder.Build(_mapUpdater.GetMap());

            var startPos = new Vector2Int(1, 4); // ToDo: Tmp
            var endPos = new Vector2Int(10, 4); // ToDo: Tmp
            waveletSearchExecutor.SearchAccessibleArea(startPos, endPos, isWall);
        }
    }
}