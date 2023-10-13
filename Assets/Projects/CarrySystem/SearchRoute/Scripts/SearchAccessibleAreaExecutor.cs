using System;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.RoutingAlgorithm.Interfaces;
using Cysharp.Threading.Tasks;
using UnityEngine;
#nullable enable

namespace Carry.CarrySystem.Map.Scripts
{
    public enum SearcherSize
    {
        SizeOne = 1,
        SizeThree = 3,
    }

    public class SearchAccessibleAreaExecutor
    {
        readonly WaveletSearchExecutor _waveletSearchExecutor;
        IRoutePresenter?[] _routePresenters;
        public SearchAccessibleAreaExecutor(IGridMap gridMap, WaveletSearchExecutor waveletSearchExecutor)
        {
            _waveletSearchExecutor = waveletSearchExecutor;
            _routePresenters = new IRoutePresenter[gridMap.Length];
        }
        
        public bool[] SearchAccessibleArea(Vector2Int startPos, Func<int, int, bool> isWall,
            SearcherSize searcherSize = SearcherSize.SizeOne)
        {
            var searchedMap = _waveletSearchExecutor.WaveletSearch(startPos, isWall, searcherSize);
            var accessibleAreaArray = CalcAccessibleArea(searchedMap, searcherSize);
    
            var extendedMap =  ExtendToAccessibleNumericMap(searchedMap, searcherSize);
            
            
            UpdatePresenter(extendedMap);
    
            return accessibleAreaArray;
        }
        
        
        /// <summary>
        /// 仮置きでおいた壁を戻すために数字のマスに接しているマスをtrueにする
        /// </summary>
        /// <param name="map"></param>
        /// <param name="searcherSize"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        bool[] CalcAccessibleArea(NumericGridMap map, SearcherSize searcherSize)
        {
            var routeArray = new bool[map.Length];
            var resultBoolArray = new bool[map.Length];
            var waveletResult = map;
            // 数字がある部分をtrueにする
            for (int i = 0; i < routeArray.Length; i++)
            {
                routeArray[i] = waveletResult.GetValue(i) != _waveletSearchExecutor.WallValue &&
                                  waveletResult.GetValue(i) != _waveletSearchExecutor.InitValue;
            }

            switch (searcherSize)
            {
                case SearcherSize.SizeOne:
                    resultBoolArray = routeArray;
                    break;
                case SearcherSize.SizeThree:
                    // set true to the squares around ture
                    for (int i = 0; i < routeArray.Length; i++)
                    {
                        if (routeArray[i])
                        {
                            for (int y = -1; y <= 1; y++)
                            {
                                for (int x = -1; x <= 1; x++)
                                {
                                    var pos = map.ToVector(i);
                                    var newX = pos.x + x;
                                    var newY = pos.y + y;
                                    if (!map.IsInDataRangeArea(newX, newY)) continue;
                                    resultBoolArray[map.ToSubscript(pos.x + x, pos.y + y)] = true;
                                }
                            }
                        }
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(searcherSize), searcherSize, null);
            }

            return resultBoolArray;
        }
        
        NumericGridMap ExtendToAccessibleNumericMap(NumericGridMap map, SearcherSize searcherSize)
        {
            var extendedMap = new NumericGridMap(map.Width, map.Height, _waveletSearchExecutor.InitValue, _waveletSearchExecutor.EdgeValue, _waveletSearchExecutor.OutOfRangeValue);
            var routeArray = new bool[map.Length];
            // 数字がある部分をtrueにする
            for (int i = 0; i < routeArray.Length; i++)
            {
                routeArray[i] = map.GetValue(i) != _waveletSearchExecutor.WallValue &&
                                map.GetValue(i) != _waveletSearchExecutor.InitValue;
            }

            switch (searcherSize)
            {
                case SearcherSize.SizeOne:
                    for (int i = 0; i < routeArray.Length; i++)
                    {
                        extendedMap.SetValue(i, map.GetValue(i));
                    }
                    break;
                case SearcherSize.SizeThree:
                    // set true to the squares around ture
                    for (int i = 0; i < routeArray.Length; i++)
                    {
                        if (routeArray[i])
                        {
                            for (int y = -1; y <= 1; y++)
                            {
                                for (int x = -1; x <= 1; x++)
                                {
                                    var pos = map.ToVector(i);
                                    var newX = pos.x + x;
                                    var newY = pos.y + y;
                                    if (!map.IsInDataRangeArea(newX, newY)) continue;
                                    extendedMap.SetValue(newX,newY, map.GetValue(i) + 1);
                                }
                            }
                        }
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(searcherSize), searcherSize, null);
            }

            return extendedMap;
        }
        
        // 時間差でpresenterをupdateする
        void UpdatePresenter(NumericGridMap numericGridMap)
        {
            for (int i = 0; i < numericGridMap.Length; i++)
            {
                DelayUpdate(_routePresenters[i], numericGridMap.GetValue(i)).Forget();
            }
        }

        async UniTaskVoid DelayUpdate(IRoutePresenter? routePresenter, long value)
        {
            if(routePresenter == null) return;
            if(value < 0)return;

            await UniTask.Delay((int)value * 250);
            routePresenter.SetPresenterActive(true);
        
        }

    }
}