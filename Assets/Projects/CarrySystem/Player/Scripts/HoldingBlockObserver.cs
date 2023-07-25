using System;
using System.Collections.Generic;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.SearchRoute.Scripts;
using Projects.CarrySystem.Block.Interfaces;
using UniRx;
using UnityEngine;
#nullable enable

namespace Carry.CarrySystem.Player.Scripts
{
    public class HoldingBlockObserver
    {
        readonly List<HoldAction> _holdActions = new List<HoldAction>();
        readonly IMapUpdater _mapUpdater;
        readonly WaveletSearchBuilder _waveletSearchBuilder;
        IDisposable? _isHoldSubscription;  // to hold the subscription to dispose it later if needed
        IDisposable? _mapSubscription;  // to hold the subscription to dispose it later if needed

        public HoldingBlockObserver(IMapUpdater entityGridMapSwitcher,
            WaveletSearchBuilder waveletSearchBuilder)
        {
            _mapUpdater = entityGridMapSwitcher;
            _waveletSearchBuilder = waveletSearchBuilder;
        }
        
        public void StartObserveMap()
        {
            _mapSubscription = _mapUpdater.ObserveEveryValueChanged(x=> x.GetMap())
                .Subscribe(_ => ShowAccessibleArea());
        }
        public void StopObserve()
        {
            _isHoldSubscription?.Dispose();
            _mapSubscription?.Dispose();
        }

        public void RegisterHoldAction(HoldAction holdAction)
        {
            _holdActions.Add(holdAction);
            
            _isHoldSubscription?.Dispose();
            _isHoldSubscription = _holdActions.ToObservable()
                .SelectMany(holdAction => holdAction.ObserveEveryValueChanged(h => h.IsHoldingBlock))
                .Subscribe(_ => ShowAccessibleArea());
        }

        void ShowAccessibleArea()
        {
            var map = _mapUpdater.GetMap();
            Func<int, int, bool> isWall = (x, y) => map.GetSingleEntityList<IBlock>(new Vector2Int(x, y)).Count > 0;
            var waveletSearchExecutor = _waveletSearchBuilder.Build(_mapUpdater.GetMap());

            var startPos = new Vector2Int(1, 4); // ToDo: Tmp
            var endPos = new Vector2Int(10, 4); // ToDo: Tmp
            waveletSearchExecutor.SearchAccessibleArea(startPos, endPos, isWall,SearcherSize.SizeThree);
        }
    }
}