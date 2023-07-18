using System.Linq;
using System.Numerics;
using Carry.CarrySystem.Entity.Scripts;
using Carry.CarrySystem.Map.Scripts;
using TMPro;
using UnityEngine;
using VContainer;
#nullable  enable

namespace Carry.EditMapSystem.EditMap.Scripts
{
    public class EditMapManager
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
        public EditMapManager(EntityGridMapLoader entityGridMapLoader, TilePresenterBuilder tilePresenterBuilder)
        {
            _gridMapLoader = entityGridMapLoader;
            _tilePresenterBuilder = tilePresenterBuilder;
            _mapKey = MapKey.Default;
            _index = 0;
            _map = _gridMapLoader.LoadEntityGridMap(_mapKey,_index); // Defaultのマップの0番目のデータを読み込む
        }

        public EntityGridMap GetMap()
        {
            return _map;
        }
        
        public void UpdateMap(MapKey mapKey, int index)
        {
            _map = _gridMapLoader.LoadEntityGridMap(mapKey,index);
            _tilePresenterBuilder.Build(_map);
            _mapKey = mapKey;
            _index = index;
        }
        
        // ToDo: 以下の関数をMapEditorクラスに移して、このクラスをコンテナの役割に特化させる

        public void AddRock(Vector2Int gridPos)
        {
            var record = new RockRecord() { kind = Rock.Kind.Kind1 };
            if(_map.IsInDataRangeArea(gridPos)) _map.AddEntity(gridPos, new Rock(record, gridPos));
        }
        
        public void RemoveRock(Vector2Int gridPos)
        {
            var rocks = _map.GetSingleEntityList<Rock>(gridPos);
            var rock = rocks.First();
            if(_map.IsInDataRangeArea(gridPos)) _map.RemoveEntity(gridPos,rock);
        }


        
    }
}