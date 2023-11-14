using System.Collections.Generic;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.RoutingAlgorithm.Interfaces;
using Carry.CarrySystem.SearchRoute.Scripts;
using VContainer;

namespace Projects.CarrySystem.SearchRoute.Scripts
{
    public class SearchAccessibleAreaPresenterLocalBuilder : ISearchAccessibleAreaPresenterBuilder
    {
        IReadOnlyList<RoutePresenterNet> _routePresenters =  new List<RoutePresenterNet>();
         bool _isRoutePresentersInitialized;
         
        [Inject]
        public SearchAccessibleAreaPresenterLocalBuilder()
        {
            
        }
        
        public SearchAccessibleAreaPresenter BuildPresenter(IGridMap map)
        {
            var waveletSearchExecutor = new WaveletSearchExecutor(map);
            var searchedMapExpander = new SearchedMapExpander(waveletSearchExecutor);
            var searchAccessibleAreaExecutor = new SearchAccessibleAreaExecutor(waveletSearchExecutor,searchedMapExpander);
            
            var routePresenters = SetUpPresenter(map);
            var searchAccessibleAreaPresenter = new SearchAccessibleAreaPresenter(waveletSearchExecutor,searchAccessibleAreaExecutor,searchedMapExpander);
            searchAccessibleAreaPresenter.RegisterRoutePresenters(routePresenters);
            return searchAccessibleAreaPresenter;
            
        }

        IReadOnlyList<RoutePresenterNet> SetUpPresenter(IGridMap map)
        {
            // todo : 実装する
            
            // if (!_isRoutePresentersInitialized) //最初にすべてスポーン
            // {
            //     // Delete previous routePresenters
            //     foreach (var routePresenter in _routePresenters)
            //     {
            //         _runner.Despawn(routePresenter.Object);
            //     }
            //     _routePresenters = new List<RoutePresenterNet>();
            //
            //     // spawn new routePresenters
            //     var routePresenterSpawner = new RoutePresenterSpawner(_runner);
            //     var routePresenters = new List<RoutePresenterNet>();
            //     for (int i = 0; i < map.Length; i++)
            //     {
            //         var girdPos = map.ToVector(i);
            //         var worldPos = GridConverter.GridPositionToWorldPosition(girdPos);
            //         var routePresenter = routePresenterSpawner.SpawnPrefab(worldPos, Quaternion.identity);
            //         routePresenters.Add(routePresenter);
            //     }
            //
            //     _routePresenters = routePresenters;
            //     _isRoutePresentersInitialized = true;
            // }

            return _routePresenters;
        }
    }
}