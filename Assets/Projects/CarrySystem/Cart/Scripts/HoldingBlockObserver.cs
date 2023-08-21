using System;
using System.Collections.Generic;
using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.Player.Scripts;
using Carry.CarrySystem.SearchRoute.Scripts;
using UniRx;
using UnityEngine;

#nullable enable

namespace Carry.CarrySystem.Cart.Scripts
{
    public class HoldingBlockObserver
    {
        public bool IsMapClear { get; private set; }
        readonly List<PlayerBlockContainer> _playerBlockContainers = new List<PlayerBlockContainer>();
        readonly IMapUpdater _mapUpdater;
        readonly WaveletSearchBuilder _waveletSearchBuilder;
        readonly CartMovementNotifierNet _cartMovementNotifierNet;
        readonly ReachRightEdgeChecker _reachRightEdgeChecker;
        IDisposable? _isHoldSubscription; // to hold the subscription to dispose it later if needed
        IDisposable? _mapSubscription; // to hold the subscription to dispose it later if needed

        public HoldingBlockObserver(
            IMapUpdater entityGridMapSwitcher,
            WaveletSearchBuilder waveletSearchBuilder,
            CartMovementNotifierNet cartMovementNotifierNet,
            ReachRightEdgeChecker reachRightEdgeChecker
        )
        {
            _mapUpdater = entityGridMapSwitcher;
            _waveletSearchBuilder = waveletSearchBuilder;
            _cartMovementNotifierNet = cartMovementNotifierNet;
            _reachRightEdgeChecker = reachRightEdgeChecker;
        }

        public void StartObserveMap()
        {
            _mapSubscription = _mapUpdater.ObserveEveryValueChanged(x => x.GetMap())
                .Subscribe(_ =>
                {
                    // リセット処理
                    ShowAccessibleArea();
                });
        }

        public void StopObserve()
        {
            _isHoldSubscription?.Dispose();
            _mapSubscription?.Dispose();
        }

        public void RegisterHoldAction(PlayerBlockContainer holdActionExecutor)
        {
            Debug.Log($"Register HoldAction {holdActionExecutor}");
            _playerBlockContainers.Add(holdActionExecutor);

            _isHoldSubscription?.Dispose();
            _isHoldSubscription = _playerBlockContainers.ToObservable()
                .SelectMany(holdAction => holdAction.ObserveEveryValueChanged(h => h.IsHoldingBlock))
                .Subscribe(_ => ShowAccessibleArea());
            
        }
        

        void ShowAccessibleArea()
        {
            Debug.Log("ShowAccessibleArea");
            var map = _mapUpdater.GetMap();
            Func<int, int, bool> isWall = (x, y) => map.GetSingleEntityList<IBlock>(new Vector2Int(x, y)).Count > 0;
            var waveletSearchExecutor = _waveletSearchBuilder.Build(_mapUpdater.GetMap());

            var startPos = new Vector2Int(1, map.Height / 2);
            var endPos = new Vector2Int(map.Width - 2,  map.Height / 2);
            var searcherSize = SearcherSize.SizeThree;
            var accessibleArea = waveletSearchExecutor.SearchAccessibleArea(startPos, isWall, searcherSize);

            // Show the result
            if (_reachRightEdgeChecker. CanCartReachRightEdge(accessibleArea, map, searcherSize))
            {
                if (AllPlayerIsNotHoldingBlock())
                {
                    _cartMovementNotifierNet.ShowReachableText();
                    _cartMovementNotifierNet.ShowMoveToCartText();
                    IsMapClear = true;
                }
                else
                {
                    _cartMovementNotifierNet.ShowReachableText();
                    _cartMovementNotifierNet.HideMoveToCartText();
                    IsMapClear = false;
                }
            }
            else
            {
                _cartMovementNotifierNet.HideReachableText();
                _cartMovementNotifierNet.HideMoveToCartText();
                IsMapClear = false;
            }
        }
        

        bool AllPlayerIsNotHoldingBlock()
        {
            foreach (var holdAction in _playerBlockContainers)
            {
                if (holdAction.IsHoldingBlock)
                {
                    return false;
                }
            }

            return true;
        }
    }
}