using System.Linq;
using System.Numerics;
using Carry.CarrySystem.Entity.Scripts;
using Carry.CarrySystem.Map.Scripts;
using UnityEngine;
using VContainer;
#nullable  enable

namespace Carry.EditMapSystem.EditMap.Scripts
{
    public class EditMapManager
    {
        // ToDo: クラス名を具体的に決める
        
        public MapKey MapKey => _mapKey;
        
        readonly EntityGridMapLoader _gridMapLoader;
        readonly TilePresenterBuilder _tilePresenterBuilder;
        EntityGridMap _map;
        MapKey _mapKey;

        [Inject]
        public EditMapManager(EntityGridMapLoader entityGridMapLoader, TilePresenterBuilder tilePresenterBuilder)
        {
            _gridMapLoader = entityGridMapLoader;
            _tilePresenterBuilder = tilePresenterBuilder;
            var key = MapKey.Morita;
            var index = 11;
            _map = _gridMapLoader.LoadEntityGridMap(key,index); // indexはとりあえず0にしておく
        }

        public EntityGridMap GetMap()
        {
            return _map;
        }
        
        public void SetMapKey(MapKey mapKey)
        {
            _mapKey = mapKey;
        }
        
        // ToDo: 以下の関数をMapEditorクラスに移して、このクラスをコンテナの役割に特化させる

        public void AddRock(Vector2Int gridPos)
        {
            var record = new RockRecord() { kind = Rock.Kind.Kind1 };
            if(_map.IsInDataRangeArea(gridPos)) _map.AddEntity<Rock>(gridPos, new Rock(record, gridPos));
        }
        
        public void RemoveRock(Vector2Int gridPos)
        {
            var rocks = _map.GetSingleEntityList<Rock>(gridPos);
            var rock = rocks.First();
            if(_map.IsInDataRangeArea(gridPos)) _map.RemoveEntity(gridPos,rock);
        }


        
    }
}