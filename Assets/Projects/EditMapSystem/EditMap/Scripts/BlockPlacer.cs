using System.Linq;
using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.Entity.Scripts;
using Carry.CarrySystem.Map.Scripts;
using UnityEngine;

namespace Carry.EditMapSystem.EditMap.Scripts
{
    public class BlockPlacer
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
            var addBlockCount = allEntityList.OfType<T>().Count();
            var groundCount = allEntityList.OfType<Ground>().Count();
            var othersCount = allEntityList.Count() - addBlockCount - groundCount;

            // Debug.Log($"addBlockCount:{addBlockCount} groundCount:{groundCount} othersCount:{othersCount}");
            
            if(addBlock is ICarriableBlock carriableBlock && addBlockCount >= carriableBlock.MaxPlacedBlockCount) return ;
            if (othersCount > 0) return ;

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