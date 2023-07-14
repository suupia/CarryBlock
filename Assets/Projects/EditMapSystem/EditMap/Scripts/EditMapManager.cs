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
        
        readonly EntityGridMapLoader _gridMapLoader;
        EntityGridMap _map;

        [Inject]
        public EditMapManager(EntityGridMapLoader entityGridMapLoader)
        {
            _gridMapLoader = entityGridMapLoader;
            var key = MapKey.Koki;
            var index = 11;
            _map = _gridMapLoader.LoadEntityGridMap(key,index); // indexはとりあえず0にしておく
        }
        
        public void RegisterTilePresenterContainer( TilePresenterAttacher tilePresenterAttacher)
        {
            tilePresenterAttacher.AttachTilePresenter(_map);
        }
        
        public EntityGridMap GetMap()
        {
            return _map;
        }

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

        public void LoadMap()
        {
            var key = MapKey.Koki;
            var index = 11;
            // _map = _gridMapLoader.LoadEntityGridMap(key, index);
        }
        
    }
}