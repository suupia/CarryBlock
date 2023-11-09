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


    public class SearchAccessibleAreaExecutor
    {
        readonly WaveletSearchExecutor _waveletSearchExecutor;
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
        
        
        //　数字があるマスをtrueにし、WaveletSearchExecutorのExpandVirtualWall()で拡大した分を戻す
        bool[] CalcAccessibleArea(NumericGridMap map, SearcherSize searcherSize)
        {
            var routeArray = new bool[map.Length];
            var waveletResult = map;
            // 数字がある部分をtrueにする
            for (int i = 0; i < routeArray.Length; i++)
            {
                routeArray[i] = waveletResult.GetValue(i) != _waveletSearchExecutor.WallValue &&
                                  waveletResult.GetValue(i) != _waveletSearchExecutor.InitValue;
            }

            return ExpandAccessibleArea(map, searcherSize, routeArray);
        }
        
        
        // WaveletSearchExecutorのExpandVirtualWall()と対をなすようにする
        bool[] ExpandAccessibleArea(NumericGridMap map, SearcherSize searcherSize, bool[] routeArray)
        {
            var resultBoolArray = new bool[map.Length];
            var searcherSizeInt = (int) searcherSize;
            UnityEngine.Assertions.Assert.IsTrue(searcherSizeInt % 2 ==  1, "searcherSize must be odd number");
            
            var expandSize = (searcherSizeInt-1) / 2;
            for (int i = 0; i < routeArray.Length; i++)
            {
                if (routeArray[i])
                {
                    for (int y = -expandSize; y <= expandSize; y++)
                    {
                        for (int x = -expandSize; x <= expandSize; x++)
                        {
                            var pos = map.ToVector(i);
                            var newX = pos.x + x;
                            var newY = pos.y + y;
                            if (!map.IsInDataRangeArea(newX, newY)) continue;
                            resultBoolArray[map.ToSubscript(newX, newY)] = true;
                        }
                    }
                }
            }

            return resultBoolArray;
        }


    }
}