using System;
using Carry.CarrySystem.Map.Interfaces;
using VContainer;

namespace Carry.CarrySystem.Map.Scripts
{
    public class LobbyMapSwitcher : IMapSwitcher , IMapGetter
    {
        public MapKey MapKey => _mapKey;
        public int Index => _index;

        readonly EntityGridMapLoader _gridMapLoader;
        readonly IPresenterPlacer _allPresenterPlacer;
        EntityGridMap _map;
        MapKey _mapKey;
        int _index;
        
        Action _resetAction = () => { };

        [Inject]
        public LobbyMapSwitcher(
            EntityGridMapLoader entityGridMapLoader,
            IPresenterPlacer allPresenterPlacer
        )
        {
            _gridMapLoader = entityGridMapLoader;
            _allPresenterPlacer = allPresenterPlacer;
            _mapKey = MapKey.Default;
            _index = -1; // LoadedFileを表示するために初期化が必要
            _map = _gridMapLoader.LoadDefaultEntityGridMap(); // Defaultのマップデータを読み込む
        }

        public EntityGridMap GetMap()
        {
            return _map;
        }

        public void SetMapKey(MapKey mapKey)
        {
            _mapKey = mapKey;
        }
        
        public void SetIndex(int index)
        {
            _index = index;
        }
        public void InitSwitchMap()
        {
            var mapKey = MapKey.Default;
            var index = -1;
            _map = _gridMapLoader.LoadEntityGridMap(mapKey, index);
            _allPresenterPlacer.Place(_map);
        }

        public void SwitchMap()
        {
            _map = _gridMapLoader.LoadEntityGridMap(_mapKey, _index);
            _allPresenterPlacer.Place(_map);

            _resetAction();
        }

        public void RegisterResetAction(System.Action action)
        {
            _resetAction += action;
        }
    }
}