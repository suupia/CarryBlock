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
    public class EditMapBlockAttacher
    {
        /// <summary>
        /// Addできない場合はnullを返す
        /// </summary>
        /// <param name="map"></param>
        /// <param name="gridPos"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public void AddPlaceable<T>(EntityGridMap map , Vector2Int gridPos, T addBlock) where T : IPlaceable
        {
            if (!map.IsInDataRangeArea(gridPos)) return ;

            var allEntityList = map.GetAllEntityList(gridPos).ToList();
            var allPlaceableList = allEntityList.OfType<IPlaceable>().ToList();
            var addPlaceableList = allEntityList.Where(e => e.GetType() == addBlock.GetType());

            // If there already exits an another type of block, then return.
            if(allPlaceableList.Count() != addPlaceableList.Count()) return;
            
            // Judge MaxPlacedBlockCount by type.
            if(addBlock is ICarriableBlock carriableBlock && allPlaceableList.OfType<IBlock>().Count() >= carriableBlock.MaxPlacedBlockCount) return;
            if(addBlock is IGimmick gimmickBlock && allPlaceableList.OfType<IGimmick>().Any()) return;
            

            map.AddEntity(gridPos, addBlock);
        }

        /// <summary>
        /// Removeできない場合はnullを返す
        /// </summary>
        /// <param name="map"></param>
        /// <param name="gridPos"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public void RemovePlaceable<T>(EntityGridMap map, Vector2Int gridPos) where T : IPlaceable
        {
            var entities = map.GetSingleEntityList<T>(gridPos);
            if (!entities.Any()) return ;
            var entity = entities.First();
            if (map.IsInDataRangeArea(gridPos)) map.RemoveEntity(gridPos, entity);
        }
        
    }
}