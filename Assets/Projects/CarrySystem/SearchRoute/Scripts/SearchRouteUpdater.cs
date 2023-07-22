using VContainer;

namespace Carry.CarrySystem.SearchRoute.Scripts
{
    public class SearchRouteUpdater
    {
        // マップ切り替え時にPresenterを再配置する用
        
        readonly SearchShortestRoute _searchShortestRoute;
        readonly RoutePresenterBuilder _routePresenterBuilder;
        
        [Inject]
        public SearchRouteUpdater(SearchShortestRoute searchShortestRoute, RoutePresenterBuilder routePresenterBuilder)
        {
            _searchShortestRoute = searchShortestRoute;
            _routePresenterBuilder = routePresenterBuilder;
        }
        public void InitUpdateMap()
        {
            // _map = _gridMapLoader.LoadEntityGridMap(mapKey, index);
            // _tilePresenterBuilder.Build(_map);
        }
        
        public void UpdateMap()
        {
            // _map = _gridMapLoader.LoadEntityGridMap(mapKey, index);
            // _tilePresenterBuilder.Build(_map);
            // _mapKey = mapKey;
            // _index = index;
        }
    }
}