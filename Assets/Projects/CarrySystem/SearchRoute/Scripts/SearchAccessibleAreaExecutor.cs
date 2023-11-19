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
        readonly SearchedMapExpander _searchedMapExpander;
        public SearchAccessibleAreaExecutor(
            WaveletSearchExecutor waveletSearchExecutor,
            SearchedMapExpander searchedMapExpander)
        {
            _waveletSearchExecutor = waveletSearchExecutor;
            _searchedMapExpander = searchedMapExpander ;
        }

        public bool[] SearchAccessibleArea(Vector2Int startPos, Func<int, int, bool> isWall,
              SearcherSize searcherSize = SearcherSize.SizeOne)
        {
            var searchedMap = _waveletSearchExecutor.WaveletSearch(startPos, isWall, searcherSize);
            var accessibleAreaArray = _searchedMapExpander.ExpandAccessibleArea(searchedMap, searcherSize);
    
            return accessibleAreaArray;
        }
        


    }
}