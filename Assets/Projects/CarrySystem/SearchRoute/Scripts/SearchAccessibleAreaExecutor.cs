using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.RoutingAlgorithm.Interfaces;
using Carry.CarrySystem.SearchRoute.Scripts;
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
        CancellationTokenSource?[]? _cancellationTokenSources;
        public SearchAccessibleAreaExecutor(WaveletSearchExecutor waveletSearchExecutor)
        {
            _waveletSearchExecutor = waveletSearchExecutor;
        }

        public bool[] SearchAccessibleAreaWithNotUpdate(Vector2Int startPos, Func<int, int, bool> isWall,
              SearcherSize searcherSize = SearcherSize.SizeOne)
        {
            var searchedMap = _waveletSearchExecutor.WaveletSearch(startPos, isWall, searcherSize);
            var accessibleAreaArray = CalcAccessibleArea(searchedMap, searcherSize);
    
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


    }
}