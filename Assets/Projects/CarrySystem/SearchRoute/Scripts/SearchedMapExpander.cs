using System;
using Carry.CarrySystem.Map.Scripts;

#nullable enable

namespace Carry.CarrySystem.SearchRoute.Scripts
{
    public enum SearcherSize
    {
        SizeOne = 1,
        SizeThree = 3,
    }
    public class SearchedMapExpander
    {
        readonly WaveletSearchExecutor _waveletSearchExecutor;

        public SearchedMapExpander(WaveletSearchExecutor waveletSearchExecutor)
        {
            _waveletSearchExecutor = waveletSearchExecutor;
        }
        
        // WaveletSearchExecutorのExpandVirtualWall()と対をなすようにする
        public  bool[] ExpandAccessibleArea(NumericGridMap map, SearcherSize searcherSize)
        {
            var resultBoolArray = new bool[map.Length];
            var searcherSizeInt = (int) searcherSize;
            UnityEngine.Assertions.Assert.IsTrue(searcherSizeInt % 2 ==  1, "searcherSize must be odd number");
            
            var expandSize = (searcherSizeInt-1) / 2;
            var routeArray = CalcRouteArray(map);
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
        
        
        // 一般化するには、map.GetValue(i) + 1 の部分を変える必要があり、少し面倒
        // 少なくともテストコードが必要そう
        public NumericGridMap ExpandSearchedMap(NumericGridMap map, SearcherSize searcherSize)
        {
            var extendedMap = new NumericGridMap(map.Width, map.Height, _waveletSearchExecutor.InitValue, _waveletSearchExecutor.EdgeValue, _waveletSearchExecutor.OutOfRangeValue);
            var searcherSizeInt = (int) searcherSize;
            UnityEngine.Assertions.Assert.IsTrue(searcherSizeInt % 2 ==  1, "searcherSize must be odd number");

            var routeArray = CalcRouteArray(map);
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

    }
}