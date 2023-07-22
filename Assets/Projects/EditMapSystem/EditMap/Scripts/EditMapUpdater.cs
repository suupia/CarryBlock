using System.Linq;
using System.Numerics;
using Carry.CarrySystem.Entity.Scripts;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using Projects.CarrySystem.Block.Scripts;
using TMPro;
using UnityEngine;
using VContainer;

#nullable enable

namespace Carry.EditMapSystem.EditMap.Scripts
{
    public class EditMapUpdater : IMapUpdater
    {
        // ToDo: クラス名を具体的に決める
        public MapKey MapKey => _mapKey;
        public int Index => _index;

        readonly EntityGridMapLoader _gridMapLoader;
        readonly TilePresenterBuilder _tilePresenterBuilder;
        EntityGridMap _map;
        MapKey _mapKey;
        int _index;

        [Inject]
        public EditMapUpdater(EntityGridMapLoader entityGridMapLoader, TilePresenterBuilder tilePresenterBuilder)
        {
            _gridMapLoader = entityGridMapLoader;
            _tilePresenterBuilder = tilePresenterBuilder;
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
            _tilePresenterBuilder.Build(_map);
        }

        public void UpdateMap(MapKey mapKey, int index)
        {
            _map = _gridMapLoader.LoadEntityGridMap(mapKey, index);
            _tilePresenterBuilder.Build(_map);
            _mapKey = mapKey;
            _index = index;
        }

    }
}