using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.RoutingAlgorithm.Interfaces;
using Carry.CarrySystem.SearchRoute.Scripts;
using Cysharp.Threading.Tasks;
using UnityEngine;
#nullable enable

namespace Projects.CarrySystem.SearchRoute.Scripts
{
        public class SearchAccessibleAreaPresenter
    {
        readonly WaveletSearchExecutor _waveletSearchExecutor;
        readonly SearchAccessibleAreaExecutor _searchAccessibleAreaExecutor;
        readonly IRoutePresenter?[] _routePresenters;
        readonly int _delayMilliSec = 7;
        CancellationTokenSource?[]? _cancellationTokenSources;
        
        public SearchAccessibleAreaPresenter(WaveletSearchExecutor waveletSearchExecutor, SearchAccessibleAreaExecutor searchAccessibleAreaExecutor)
        {
            _waveletSearchExecutor = waveletSearchExecutor;
            _searchAccessibleAreaExecutor = searchAccessibleAreaExecutor;
            var mapLength = waveletSearchExecutor.Map.Length;
            _routePresenters = new IRoutePresenter[mapLength];
        }

        public void RegisterRoutePresenters(IReadOnlyList<RoutePresenterNet> routePresenters)
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
        }

        public bool[] SearchAccessibleAreaWithUpdate(Vector2Int startPos, Func<int, int, bool> isWall,CancellationTokenSource[]? cancellationTokenSources,SearcherSize searcherSize = SearcherSize.SizeOne)
        {
            _cancellationTokenSources = cancellationTokenSources;
            var searchedMap = _waveletSearchExecutor.WaveletSearch(startPos, isWall, searcherSize);
            var accessibleAreaArray =_searchAccessibleAreaExecutor.SearchAccessibleAreaWithNotUpdate(startPos, isWall, searcherSize);
    
            var extendedMap =  ExpandSearchedMap(searchedMap, searcherSize, CalcRouteArray(searchedMap));
            UpdatePresenter(extendedMap);
    
            return accessibleAreaArray;
        }

        bool[] CalcRouteArray(NumericGridMap map)
        {
            var routeArray = new bool[map.Length];
            // 数字がある部分をtrueにする
            for (int i = 0; i < routeArray.Length; i++)
            {
                routeArray[i] = map.GetValue(i) != _waveletSearchExecutor.WallValue &&
                                map.GetValue(i) != _waveletSearchExecutor.InitValue;
            }

            return routeArray;
        }

        NumericGridMap ExpandSearchedMap(NumericGridMap map, SearcherSize searcherSize, bool[] routeArray)
        {
            var extendedMap = new NumericGridMap(map.Width, map.Height, _waveletSearchExecutor.InitValue, _waveletSearchExecutor.EdgeValue, _waveletSearchExecutor.OutOfRangeValue);
            var searcherSizeInt = (int) searcherSize;
            UnityEngine.Assertions.Assert.IsTrue(searcherSizeInt % 2 ==  1, "searcherSize must be odd number");

            // 一般化するには、map.GetValue(i) + 1 の部分を変える必要があり、少し面倒
            // 少なくともテストコードが必要そう
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
            if (_cancellationTokenSources == null)
            {
                Debug.LogError($"_cancellationTokenSources is null");
                return;
            }
            for (int i = 0; i < numericGridMap.Length; i++)
            {
                _cancellationTokenSources[i]?.Cancel();
                _cancellationTokenSources[i] = new CancellationTokenSource();
                if(_cancellationTokenSources[i] == null) return;
                DelayUpdate(_cancellationTokenSources[i].Token, _routePresenters[i], numericGridMap.GetValue(i)).Forget();  // _cancellationTokenSources[i]でDereference of a possibly null referenceがでる　なぜ？
            }
        }

        async UniTaskVoid DelayUpdate(CancellationToken cts, IRoutePresenter? routePresenter, long value)
        {
            if(routePresenter == null) return;
            if (value < 0)
            {
                routePresenter.SetPresenterActive(false);
                return;
            }

            if (routePresenter.IsActive) return;

            try
            {
                await UniTask.Delay((int)value * _delayMilliSec, cancellationToken: cts);
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