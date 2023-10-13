using System;
using UnityEngine;
#nullable enable

namespace Carry.CarrySystem.Map.Scripts
{
    public enum SearcherSize
    {
        SizeOne = 1,
        SizeThree = 3,
    }

    // public class SearchAccessibleAreaExecutor
    // {
    //     public bool[] SearchAccessibleArea(Vector2Int startPos, Func<int, int, bool> isWall,
    //         SearcherSize searcherSize = SearcherSize.SizeOne)
    //     {
    //         var searchedMap = WaveletSearch(startPos, isWall, searcherSize);
    //         var accessibleAreaArray = CalcAccessibleArea(searchedMap, searcherSize);
    //
    //         var extendedMap =  ExtendToAccessibleNumericMap(searchedMap, searcherSize);
    //
    //         // UpdatePresenter(accessibleAreaArray);
    //         UpdatePresenter(extendedMap);
    //
    //         return accessibleAreaArray;
    //     }
    // }
}