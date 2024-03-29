﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.RoutingAlgorithm.Interfaces;
using Carry.CarrySystem.SearchRoute.Scripts;
using Cysharp.Threading.Tasks;
using UnityEngine;
#nullable enable

namespace Carry.CarrySystem.SearchRoute.Scripts
{
        public class SearchAccessibleAreaPresenter
    {
        readonly IMapGetter _mapGetter;
        readonly WaveletSearchExecutor _waveletSearchExecutor;
        readonly SearchAccessibleAreaExecutor _searchAccessibleAreaExecutor;
        readonly SearchedMapExpander _searchedMapExpander;
        readonly IRoutePresenter?[] _routePresenters;
        readonly int _delayMilliSec = 30;
        CancellationTokenSource?[]? _ctss;
        long _beforeMaxValue = 0;


        public SearchAccessibleAreaPresenter(
            IMapGetter mapGetter,
            WaveletSearchExecutor waveletSearchExecutor, 
            SearchAccessibleAreaExecutor searchAccessibleAreaExecutor,
            SearchedMapExpander searchedMapExpander)
        {
            _mapGetter = mapGetter;
            _waveletSearchExecutor = waveletSearchExecutor;
            _searchAccessibleAreaExecutor = searchAccessibleAreaExecutor;
            _searchedMapExpander = searchedMapExpander;
            var mapLength = waveletSearchExecutor.Map.Length;
            _routePresenters = new IRoutePresenter[mapLength];
        }

        public void RegisterRoutePresenters(IReadOnlyList<IRoutePresenter> routePresenters)
        {
            var mapLength = _waveletSearchExecutor.Map.Length;
            if (routePresenters.Count() != mapLength)
            {
                Debug.LogError($"routePresentersの数がmapのマスの数と一致しません。" +
                               $"routePresentersの数: {routePresenters.Count()} " +
                               $"mapのマスの数: {mapLength}");
            }

            for (int i = 0; i < routePresenters.Count(); i++)
            {
                _routePresenters[i] = routePresenters[i];
            }
            
            if (_ctss == null || _ctss.Length != mapLength)
            {
                _ctss = new CancellationTokenSource[mapLength];
            }
        }

        public bool[] SearchAccessibleAreaWithUpdatePresenter()
        {
            var map = _mapGetter.GetMap();
            var startPos = new Vector2Int(1, map.Height / 2);
            Func<int, int, bool> isWall = (x, y) =>
                map.GetSingleTypeList<IBlock>(new Vector2Int(x, y))?.Count > 0;
            var searcherSize = SearcherSize.SizeThree;

            var searchedMap = _waveletSearchExecutor.WaveletSearch(startPos, isWall, searcherSize);
            var accessibleAreaArray =_searchAccessibleAreaExecutor.SearchAccessibleArea(startPos, isWall, searcherSize);
    
            var expandedMap = _searchedMapExpander.ExpandSearchedMap(searchedMap, searcherSize);

            UpdatePresenter(expandedMap);
    
            return accessibleAreaArray; 
        }

        // 時間差でpresenterをupdateする
        void UpdatePresenter(NumericGridMap numericGridMap)
        {
            if (_ctss == null)
            {
                Debug.LogError($"_cancellationTokenSources is null");
                return;
            }
            
            long nextMaxValue = 0;
            for (int i = 0; i < numericGridMap.Length; i++)
            {
                _ctss[i]?.Cancel();
                _ctss[i] = new CancellationTokenSource();
                if(_ctss[i] == null) return;
                DelayUpdate(_ctss[i].Token, _routePresenters[i], numericGridMap.GetValue(i),i).Forget();  // _cancellationTokenSources[i]でDereference of a possibly null referenceがでる　なぜ？
                if(numericGridMap.GetValue(i) != _waveletSearchExecutor.InitValue
                   && numericGridMap.GetValue(i) != _waveletSearchExecutor.WallValue
                   && numericGridMap.GetValue(i) != _waveletSearchExecutor.EdgeValue
                   && numericGridMap.GetValue(i) != _waveletSearchExecutor.OutOfRangeValue)
                    nextMaxValue = Math.Max(nextMaxValue, numericGridMap.GetValue(i));
            }

            _beforeMaxValue = nextMaxValue;
        }

        async UniTaskVoid DelayUpdate(CancellationToken cts, IRoutePresenter? routePresenter, long value, int i)
        {
            if(routePresenter == null) return;
            
            if(value == _waveletSearchExecutor.InitValue
               || value == _waveletSearchExecutor.WallValue
               || value == _waveletSearchExecutor.EdgeValue
               || value == _waveletSearchExecutor.OutOfRangeValue)
            {
                routePresenter.SetPresenterActive(false);
                return;
            }

            if (routePresenter.IsActive) return;

            try
            {
                var delayValue = Math.Max((int)(value - _beforeMaxValue),0);
                // Debug.Log($"value:{value} _beforeMaxValue:{_beforeMaxValue} delayValue:{delayValue}");
                await UniTask.Delay(delayValue * _delayMilliSec, cancellationToken: cts);
                routePresenter.SetPresenterActive(true);
            }
            catch (OperationCanceledException)
            {
                //Debug.Log("canceled");
            }
        }
        
        public void DebugAccessibleArea(int height, int width,bool[] accessibleAreaArray )
        {
            //デバッグ用
            StringBuilder debugCell = new StringBuilder();
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    bool value = accessibleAreaArray[x + (height - y - 1) * width];
                    debugCell.AppendFormat("{0,4},", value.ToString()); // 桁数をそろえるために0を追加していると思う
                }
            
                debugCell.AppendLine();
            }
            Debug.Log($"すべてのresultBoolArrayの結果は\n{debugCell}");
        }

    }
}