using Carry.CarrySystem.Map.Scripts;
using UnityEditorInternal;
using VContainer;

namespace Carry.CarrySystem.SearchRoute.Scripts
{
    public class SearchRouteUpdater
    {
        // マップ切り替え時にPresenterを再配置する用

        readonly EntityGridMapLoader _gridMapLoader;
        readonly SearchShortestRoute _searchShortestRoute;
        readonly RoutePresenterBuilder _routePresenterBuilder;
        EntityGridMap _map;
        
        [Inject]
        public SearchRouteUpdater(
            EntityGridMapLoader gridMapLoader,
            SearchShortestRoute searchShortestRoute,
            RoutePresenterBuilder routePresenterBuilder)
        {
            _gridMapLoader = gridMapLoader;
            _searchShortestRoute = searchShortestRoute;
            _routePresenterBuilder = routePresenterBuilder;
        }
        
        public EntityGridMap GetMap()
        {
            return _map;
        }

        public void InitUpdateMap(MapKey mapKey, int index)
        {
            // _map = _gridMapLoader.LoadEntityGridMap(mapKey, index);
            // _tilePresenterBuilder.Build(_map);
            _map = _gridMapLoader.LoadEntityGridMap(mapKey, index);
            _routePresenterBuilder.Build(_map);  // ToDo: ここで渡すのはmapでないとだめかも。。
        }
        
        public void UpdateMap(MapKey mapKey, int index)
        {
            // _map = _gridMapLoader.LoadEntityGridMap(mapKey, index);
            // _tilePresenterBuilder.Build(_map);
            // _mapKey = mapKey;
            // _index = index;
        }
    }
}