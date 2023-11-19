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
        readonly IMapGetter _mapGetter;
        readonly SearchAccessibleAreaPresenterBuilder _searchAccessibleAreaPresenterBuilder;
        readonly IHoldingBlockNotifier _holdingBlockNotifier;
        readonly ReachRightEdgeChecker _reachRightEdgeChecker;
        
        SearchAccessibleAreaPresenter? _searchAccessibleAreaPresenter;
        IDisposable? _isHoldSubscription; // to hold the subscription to dispose it later if needed
        IDisposable? _mapSubscription; // to hold the subscription to dispose it later if needed
        
        public HoldingBlockObserver(
            IMapSwitcher mapSwitcher,
            IMapGetter mapGetter,
            SearchAccessibleAreaPresenterBuilder searchAccessibleAreaPresenterBuilder,
            IHoldingBlockNotifier holdingBlockNotifier,
            ReachRightEdgeChecker reachRightEdgeChecker
        )
        {
            _mapGetter = mapGetter;
            _searchAccessibleAreaPresenterBuilder = searchAccessibleAreaPresenterBuilder;
            _holdingBlockNotifier = holdingBlockNotifier;
            _reachRightEdgeChecker = reachRightEdgeChecker;

            mapSwitcher.RegisterResetAction(() =>
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
            _searchAccessibleAreaPresenter = _searchAccessibleAreaPresenterBuilder.BuildPresenter();
            
            ShowAccessibleArea();
        }


        void ShowAccessibleArea()
        {
            Debug.Log("ShowAccessibleArea");
            var accessibleArea = _searchAccessibleAreaPresenter?.SearchAccessibleAreaWithUpdatePresenter();

            ShowResultText(accessibleArea);
        }

        void ShowResultText(bool[] accessibleArea)
        {
            var map = _mapGetter.GetMap();
            var searcherSize = SearcherSize.SizeThree; 
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