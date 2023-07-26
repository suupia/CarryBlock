using System;
using System.Collections.Generic;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.Player.Scripts;
using Carry.CarrySystem.SearchRoute.Scripts;
using Projects.CarrySystem.Block.Interfaces;
using UniRx;
using UnityEngine;

#nullable enable

namespace Carry.CarrySystem.Cart.Scripts
{
    public class HoldingBlockObserver
    {
        public bool IsMapClear { get; private set; }
        readonly List<HoldAction> _holdActions = new List<HoldAction>();
        readonly IMapUpdater _mapUpdater;
        readonly WaveletSearchBuilder _waveletSearchBuilder;
        readonly CartMovementNotifier _cartMovementNotifier = new CartMovementNotifier();
        IDisposable? _isHoldSubscription; // to hold the subscription to dispose it later if needed
        IDisposable? _mapSubscription; // to hold the subscription to dispose it later if needed

        public HoldingBlockObserver(
            IMapUpdater entityGridMapSwitcher,
            WaveletSearchBuilder waveletSearchBuilder,
            CartMovementNotifier cartMovementNotifier
        )
        {
            _mapUpdater = entityGridMapSwitcher;
            _waveletSearchBuilder = waveletSearchBuilder;
            _cartMovementNotifier = cartMovementNotifier;
        }

        public void StartObserveMap()
        {
            _mapSubscription = _mapUpdater.ObserveEveryValueChanged(x => x.GetMap())
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

            var startPos = new Vector2Int(1, map.Height % 2 == 1 ? (map.Height - 1) / 2 : map.Height / 2);
            var endPos = new Vector2Int(map.Width - 2, map.Height % 2 == 1 ? (map.Height - 1) / 2 : map.Height / 2);
            var searcherSize = SearcherSize.SizeThree;
            var accessibleArea = waveletSearchExecutor.SearchAccessibleArea(startPos, endPos, isWall, searcherSize);

            // Show the result
            if (CanCartReachRightEdge(accessibleArea, map, searcherSize))
            {
                if (AllPlayerIsNotHoldingBlock())
                {
                    _cartMovementNotifier.ShowReachableText();
                    _cartMovementNotifier.ShowMoveToCartText();
                    IsMapClear = true;
                }
                else
                {
                    _cartMovementNotifier.ShowReachableText();
                    _cartMovementNotifier.HideMoveToCartText();
                    IsMapClear = false;
                }
            }
            else
            {
                _cartMovementNotifier.HideReachableText();
                _cartMovementNotifier.HideMoveToCartText();
                IsMapClear = false;
            }
        }

        bool CanCartReachRightEdge(bool[] accessibleArea, SquareGridMap map, SearcherSize searcherSize)
        {
            bool[] rightEdgeArray = new bool[map.Height];
            for (int y = 0; y < map.Height; y++)
            {
                rightEdgeArray[y] = accessibleArea[map.ToSubscript(map.Width - 1, y)];
            }
            // Debug.Log($"rightEdgeArray:{string.Join("," , rightEdgeArray)}");

            // check if the rightEdgeArray has 3 consecutive true
            var consecutiveCount = (int)searcherSize;
            var counter = 0;
            for (int i = 0; i < rightEdgeArray.Length - 1; i++)
            {
                if (rightEdgeArray[i])
                {
                    counter++;
                    if (counter == consecutiveCount)
                    {
                        Debug.Log("Reachable");
                        return true;
                    }
                }
                else
                {
                    counter = 0;
                }
            }

            return false;
        }

        bool AllPlayerIsNotHoldingBlock()
        {
            foreach (var holdAction in _holdActions)
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