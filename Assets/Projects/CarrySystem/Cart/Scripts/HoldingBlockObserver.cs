﻿using System;
using System.Collections.Generic;
using System.Threading;
using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.Player.Scripts;
using Carry.CarrySystem.SearchRoute.Scripts;
using Projects.CarrySystem.SearchRoute.Scripts;
using UniRx;
using UnityEngine;

#nullable enable

namespace Carry.CarrySystem.Cart.Scripts
{
    public class HoldingBlockObserver
    {
        public bool IsMapClear { get; private set; }
        readonly List<PlayerHoldingObjectContainer> _playerBlockContainers = new List<PlayerHoldingObjectContainer>();
        readonly IMapUpdater _mapUpdater;
        readonly SearchAccessibleAreaBuilder _searchAccessibleAreaBuilder;
        readonly CartMovementNotifierNet _cartMovementNotifierNet;
        readonly ReachRightEdgeChecker _reachRightEdgeChecker;
        IDisposable? _isHoldSubscription; // to hold the subscription to dispose it later if needed
        IDisposable? _mapSubscription; // to hold the subscription to dispose it later if needed
        CancellationTokenSource[]? _ctss;
        
        public HoldingBlockObserver(
            IMapUpdater entityGridMapSwitcher,
            SearchAccessibleAreaBuilder searchAccessibleAreaBuilder,
            CartMovementNotifierNet cartMovementNotifierNet,
            ReachRightEdgeChecker reachRightEdgeChecker
        )
        {
            _mapUpdater = entityGridMapSwitcher;
            _searchAccessibleAreaBuilder = searchAccessibleAreaBuilder;
            _cartMovementNotifierNet = cartMovementNotifierNet;
            _reachRightEdgeChecker = reachRightEdgeChecker;

            entityGridMapSwitcher.RegisterResetAction(() =>
            {
                IsMapClear = false;
                ShowAccessibleArea();
            });
        }

        public void StopObserve()
        {
            _isHoldSubscription?.Dispose();
            _mapSubscription?.Dispose();
        }

        public void RegisterHoldAction(PlayerHoldingObjectContainer holdActionExecutor)
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
            Func<int, int, bool> isWall = (x, y) =>
                map.GetSingleEntity<IBlockMonoDelegate>(new Vector2Int(x, y))?.Blocks.Count > 0;
            
            if (_ctss == null || _ctss.Length != map.Length)
            {
                _ctss = new CancellationTokenSource[map.Length];
            }

            var startPos = new Vector2Int(1, map.Height / 2);
            var endPos = new Vector2Int(map.Width - 2, map.Height / 2);
            var searcherSize = SearcherSize.SizeThree;
            var searchAccessibleAreaExecutor = _searchAccessibleAreaBuilder.BuildPresenter(_mapUpdater.GetMap());
            var accessibleArea = searchAccessibleAreaExecutor.SearchAccessibleAreaWithUpdate(startPos, isWall, _ctss,searcherSize);

            // Show the result  
            if (_reachRightEdgeChecker.CanCartReachRightEdge(accessibleArea, map, searcherSize))
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