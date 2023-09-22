using System.Linq;
using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.Block.Scripts;
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
        public void AddBlock<T>(EntityGridMap map , Vector2Int gridPos, T addBlock) where T : IBlock
        {
            if (!map.IsInDataRangeArea(gridPos)) return ;

            var allEntityList = map.GetAllEntityList(gridPos).ToList();
            var addedBlockCount = allEntityList.Count(e => e.GetType() == addBlock.GetType());
            var groundCount = allEntityList.OfType<Ground>().Count();
            var othersCount = allEntityList.Count() - addedBlockCount - groundCount;

            Debug.Log($"addedBlockCount:{addedBlockCount} groundCount:{groundCount} othersCount:{othersCount}");
            
            // Judge MaxPlacedBlockCount by type.
            if(addBlock is ICarriableBlock carriableBlock && addedBlockCount >= carriableBlock.MaxPlacedBlockCount) return;
            if(addBlock is IGimmickBlock gimmickBlock && addedBlockCount >= 1) return;
            
            // If there already exits an another type of block, then return.
            if (othersCount > 0) return;

            map.AddEntity(gridPos, addBlock);
        }

        /// <summary>
        /// Removeできない場合はnullを返す
        /// </summary>
        /// <param name="map"></param>
        /// <param name="gridPos"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public void RemoveBlock<T>(EntityGridMap map, Vector2Int gridPos) where T : IBlock
        {
            var entities = map.GetSingleEntityList<T>(gridPos);
            if (!entities.Any()) return ;
            var entity = entities.First();
            if (map.IsInDataRangeArea(gridPos)) map.RemoveEntity(gridPos, entity);
        }
        
    }
}