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
    public class EditMapUpdater : IMapUpdater
    {
        public MapKey MapKey => _mapKey;
        public int Index => _index;

        readonly LoadedFilePresenter _loadedFilePresenter;
        readonly EntityGridMapLoader _gridMapLoader;
        readonly AllPresenterBuilder _allPresenterBuilder;
        EntityGridMap _map;
        MapKey _mapKey;
        int _index;
        
        Action _resetAction = () => { };

        [Inject]
        public EditMapUpdater(
            LoadedFilePresenter loadedFilePresenter,
            EntityGridMapLoader entityGridMapLoader,
            AllPresenterBuilder allPresenterBuilder
            )
        {
            _loadedFilePresenter = loadedFilePresenter;
            _gridMapLoader = entityGridMapLoader;
            _allPresenterBuilder = allPresenterBuilder;
            _mapKey = MapKey.Default;
            _index = -1; // LoadedFileを表示するために初期化が必要
            _map = _gridMapLoader.LoadDefaultEntityGridMap(); // Defaultのマップデータを読み込む
        }

        public EntityGridMap GetMap()
        {
            return _map;
        }

        public void InitUpdateMap(MapKey mapKey, int index)
        {
            _map = _gridMapLoader.LoadEntityGridMap(mapKey, index);
            _allPresenterBuilder.Build(_map);
            _loadedFilePresenter.FormatLoadedFileText(_mapKey,_index);
        }

        public void UpdateMap(MapKey mapKey, int index)
        {
            _map = _gridMapLoader.LoadEntityGridMap(mapKey, index);
            _allPresenterBuilder.Build(_map);
            _mapKey = mapKey;
            _index = index;
            _loadedFilePresenter.FormatLoadedFileText(_mapKey,_index);
            
            _resetAction();
        }

        public void RegisterResetAction(System.Action action)
        {
            _resetAction += action;
        }
        
    }
}