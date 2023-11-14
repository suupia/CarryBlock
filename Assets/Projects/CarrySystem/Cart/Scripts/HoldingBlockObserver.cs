using System;
using System.Collections.Generic;
using System.Threading;
using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.Player.Scripts;
using Carry.CarrySystem.RoutingAlgorithm.Interfaces;
using Carry.CarrySystem.SearchRoute.Scripts;
using Projects.CarrySystem.Cart.Interfaces;
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
        readonly ISearchAccessibleAreaPresenterBuilder _searchAccessibleAreaPresenterBuilder;
        readonly IHoldingBlockNotifier _holdingBlockNotifier;
        readonly ReachRightEdgeChecker _reachRightEdgeChecker;
        
        SearchAccessibleAreaPresenter? _searchAccessibleAreaPresenter;
        IDisposable? _isHoldSubscription; // to hold the subscription to dispose it later if needed
        IDisposable? _mapSubscription; // to hold the subscription to dispose it later if needed
        CancellationTokenSource[]? _ctss;
        
        public HoldingBlockObserver(
            IMapUpdater entityGridMapSwitcher,
            ISearchAccessibleAreaPresenterBuilder searchAccessibleAreaPresenterBuilder,
            IHoldingBlockNotifier holdingBlockNotifier,
            ReachRightEdgeChecker reachRightEdgeChecker
        )
        {
            _mapUpdater = entityGridMapSwitcher;
            _searchAccessibleAreaPresenterBuilder = searchAccessibleAreaPresenterBuilder;
            _holdingBlockNotifier = holdingBlockNotifier;
            _reachRightEdgeChecker = reachRightEdgeChecker;

            entityGridMapSwitcher.RegisterResetAction(() =>
            {
                IsMapClear = false;
                ResetAccessibleArea();
            });
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

        void ResetAccessibleArea()
        {
            var map = _mapUpdater.GetMap();
            
            if (_ctss == null || _ctss.Length != map.Length)
            {
                _ctss = new CancellationTokenSource[map.Length];
            }

            _searchAccessibleAreaPresenter = _searchAccessibleAreaPresenterBuilder.BuildPresenter(_mapUpdater.GetMap());
            
            ShowAccessibleArea();
        }


        void ShowAccessibleArea()
        {
            Debug.Log("ShowAccessibleArea");
            var map = _mapUpdater.GetMap();
            var startPos = new Vector2Int(1, map.Height / 2);
            Func<int, int, bool> isWall = (x, y) =>
                map.GetSingleEntity<IBlockMonoDelegate>(new Vector2Int(x, y))?.Blocks.Count > 0;
            var searcherSize = SearcherSize.SizeThree;

            var accessibleArea = _searchAccessibleAreaPresenter?.SearchAccessibleAreaWithUpdate(startPos, isWall, _ctss,searcherSize);

            ShowResultText(accessibleArea, map, searcherSize);
        }

        void ShowResultText(bool[] accessibleArea,EntityGridMap map,SearcherSize searcherSize  )
        {
            if (_reachRightEdgeChecker.CanCartReachRightEdge(accessibleArea, map, searcherSize))
            {
                if (AllPlayerIsNotHoldingBlock())
                {
                    _holdingBlockNotifier.ShowReachableText();
                    _holdingBlockNotifier.ShowMoveToCartText();
                    IsMapClear = true;
                }
                else
                {
                    _holdingBlockNotifier.ShowReachableText();
                    _holdingBlockNotifier.HideMoveToCartText();
                    IsMapClear = false;
                }
            }
            else
            {
                _holdingBlockNotifier.HideReachableText();
                _holdingBlockNotifier.HideMoveToCartText();
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