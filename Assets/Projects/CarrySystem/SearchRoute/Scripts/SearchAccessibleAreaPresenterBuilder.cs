using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Carry.CarrySystem.Entity.Scripts;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.RoutingAlgorithm.Interfaces;
using Fusion;
using UnityEngine;
using VContainer;
#nullable  enable

namespace Carry.CarrySystem.SearchRoute.Scripts
{
    /// <summary>
    /// ドメインスクリプトSearchAccessibleAreaExecutorにRoutePresenterを紐づけるクラス
    /// </summary>
    public class SearchAccessibleAreaPresenterBuilder
    {
        readonly IMapGetter _mapGetter;
        readonly IRoutePresenterSpawner _routePresenterSpawner;
        IReadOnlyList<IRoutePresenter>? _routePresenters;

        [Inject]
        public SearchAccessibleAreaPresenterBuilder(
            IMapGetter mapGetter,
            IRoutePresenterSpawner routePresenterSpawner
            )
        {
            _mapGetter = mapGetter;
            _routePresenterSpawner = routePresenterSpawner;
        }
        
        public SearchAccessibleAreaPresenter BuildPresenter()
        {
            var map = _mapGetter.GetMap();
            var waveletSearchExecutor = new WaveletSearchExecutor(map);
            var searchedMapExpander = new SearchedMapExpander(waveletSearchExecutor);
            var searchAccessibleAreaExecutor = new SearchAccessibleAreaExecutor(waveletSearchExecutor,searchedMapExpander);
            SetUpPresenter(map);
            var searchAccessibleAreaPresenter = new SearchAccessibleAreaPresenter(_mapGetter, waveletSearchExecutor,searchAccessibleAreaExecutor,searchedMapExpander);
            if(_routePresenters == null) throw new NullReferenceException("_routePresenters is null");
            searchAccessibleAreaPresenter.RegisterRoutePresenters(_routePresenters);
            return searchAccessibleAreaPresenter;
            
        }

        void SetUpPresenter(IGridMap map)
        {
            // foreachでの削除の処理が必要ないため、フィールドとして保持する必要がなく、さらに直接List<IRoutePresenter>に代入してよい

            if (_routePresenters == null)
            {
                // spawn new routePresenters
                var routePresenters = new List<IRoutePresenter>();
                for (int i = 0; i < map.Length; i++)
                {
                    var girdPos = map.ToVector(i);
                    var worldPos = GridConverter.GridPositionToWorldPosition(girdPos);
                    var routePresenter = _routePresenterSpawner.SpawnIRoutePresenter(worldPos, Quaternion.identity);
                    routePresenters.Add(routePresenter);
                }
                _routePresenters = routePresenters;
            }
        }
        
    }
}