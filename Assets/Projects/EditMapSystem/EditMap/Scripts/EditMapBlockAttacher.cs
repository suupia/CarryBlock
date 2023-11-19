using System.Linq;
using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.Block.Scripts;
using Carry.CarrySystem.CarriableBlock.Interfaces;
using Carry.CarrySystem.Entity.Interfaces;
using Carry.CarrySystem.Entity.Scripts;
using Carry.CarrySystem.Gimmick.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using UnityEngine;

namespace Carry.EditMapSystem.EditMap.Scripts
{
    public interface IEditMapBlockAttacher
    {
        bool AddPlaceable(EntityGridMap map , Vector2Int gridPos, IPlaceable addBlock);
        bool RemovePlaceable(EntityGridMap map, Vector2Int gridPos);
    }

    public class EditMapBlockAttacher : IEditMapBlockAttacher
    {
        public bool AddPlaceable(EntityGridMap map , Vector2Int gridPos, IPlaceable addBlock)
        {
            if (!map.IsInDataRangeArea(gridPos)) return false;

            var allEntityList = map.GetAllEntityList(gridPos).ToList();
            var allPlaceableList = allEntityList.OfType<IPlaceable>().ToList();
            var addPlaceableList = allEntityList.Where(e => e.GetType() == addBlock.GetType());

            // If there already exits an another type of block, then return false.
            if(allPlaceableList.Count() != addPlaceableList.Count()) return false;
            
            // Judge MaxPlacedBlockCount by type.
            if(addBlock.MaxPlacedBlockCount <= allPlaceableList.Count() )return false;
            
            map.AddEntity(gridPos, addBlock);

            return true;
        }

        public bool RemovePlaceable(EntityGridMap map, Vector2Int gridPos)
        {
            var entities = map.GetSingleEntityList<IPlaceable>(gridPos);
            if (!entities.Any()) return false;
            
            var entity = entities.First();
            if (map.IsInDataRangeArea(gridPos)) map.RemoveEntity(gridPos, entity);
            
            return true;
        }
        
    }
}