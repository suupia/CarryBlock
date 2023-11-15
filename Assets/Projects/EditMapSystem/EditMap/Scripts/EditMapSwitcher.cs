using System;
using System.Linq;
using System.Numerics;
using Carry.CarrySystem.Entity.Scripts;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using TMPro;
using UnityEngine;
using VContainer;

#nullable enable

namespace Carry.EditMapSystem.EditMap.Scripts
{
    public class EditMapSwitcher : IMapSwitcher , IMapGetter
    {
        public MapKey MapKey => _mapKey;
        public int Index => _index;

        readonly LoadedFilePresenter _loadedFilePresenter;
        readonly EntityGridMapLoader _gridMapLoader;
        readonly IPresenterPlacer _allPresenterPlacer;
        EntityGridMap _map;
        MapKey _mapKey;
        int _index;
        
        Action _resetAction = () => { };

        [Inject]
        public EditMapSwitcher(
            LoadedFilePresenter loadedFilePresenter,
            EntityGridMapLoader entityGridMapLoader,
            IPresenterPlacer allPresenterPlacer
            )
        {
            _loadedFilePresenter = loadedFilePresenter;
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
            _map = _gridMapLoader.LoadEntityGridMap(_mapKey, _index);
            _allPresenterPlacer.Place(_map);
            _loadedFilePresenter.FormatLoadedFileText(_mapKey,_index);
            
            _resetAction();
        }
        
        public void SwitchMap()
        {
            _map = _gridMapLoader.LoadEntityGridMap(_mapKey, _index);
            _allPresenterPlacer.Place(_map);
            _loadedFilePresenter.FormatLoadedFileText(_mapKey,_index);
            
            _resetAction();
        }
        

        public void RegisterResetAction(System.Action action)
        {
            _resetAction += action;
        }
        
    }
}