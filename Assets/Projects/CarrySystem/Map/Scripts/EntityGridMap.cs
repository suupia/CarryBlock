using System;
using System.Collections;
using System.Collections.Generic;
using Carry.CarrySystem.Entity.Interfaces;
using System.Linq;
using UnityEngine;
using  Carry.CarrySystem.Map.Interfaces;
using Fusion.Collections;

#nullable enable

namespace Carry.CarrySystem.Map.Scripts
{
    public class EntityGridMap : NumericGridMap
    {
        readonly List<IEntity>[] _entityMaps;
        ITilePresenter?[] _tilePresenter;
        
        public EntityGridMap(int width, int height) : base(width, height)
        {
            _entityMaps = new List<IEntity>[GetLength()];
            _tilePresenter = new ITilePresenter?[GetLength()];
            for (int i = 0; i < GetLength(); i++)
            {
                _entityMaps[i] = new List<IEntity>();
            }
        }

        public EntityGridMap CloneMap()
        {
            return (EntityGridMap)MemberwiseClone();
        }
        
        public void RegisterTilePresenter(ITilePresenter tilePresenter, int index)
        {
            _tilePresenter[index] = tilePresenter;
        }

        //Getter
        public TEntity? GetSingleEntity<TEntity>(Vector2Int vector) where TEntity : IEntity
        {
            int x = vector.x;
            int y = vector.y;

            if (IsOutOfDataRangeArea(x, y)) {return default(TEntity);}

            return GetSingleEntity<TEntity>(ToSubscript(x, y));
        }
        
        public TEntity? GetSingleEntity<TEntity>(int index) where TEntity : IEntity
        {
            if (index < 0 || index > GetLength())
            {
                Debug.LogError("領域外の値を習得しようとしました");
                return default(TEntity);
            }
            
            var entity = _entityMaps[index].OfType<TEntity>().FirstOrDefault();

            if (entity == null)
            {
                // Debug.Log($"_entityMaps[{index}].OfType<TEntity>().FirstOrDefault()がnullです");
                return default(TEntity);
            }
            else
            {
                //探しているEntityTypeの先頭のものを返す（存在しない場合はdefault）
                return entity;
            }
        }

        public List<TEntity> GetSingleEntityList<TEntity>(Vector2Int vector) where TEntity : IEntity
        {
            var x = vector.x;
            var y = vector.y;

            if (IsOutOfDataRangeArea(x, y)) return new List<TEntity>();

            return GetSingleEntityList<TEntity>(ToSubscript(x, y));
        }

        public List<TEntity> GetSingleEntityList<TEntity>(int index) where TEntity : IEntity
        {
            
            if (index < 0 || index > GetLength())
            {
                Debug.LogError("領域外の値を習得しようとしました");
                return new List<TEntity>(); // 空のリストを返す
            }

            var filteredEntities = _entityMaps[index].OfType<TEntity>().ToList();
    
            if (!filteredEntities.Any())
            {
                //Debug.Log($"_entityMaps[{index}]の{typeof(EntityType)}のCountが0です");
                return  new List<TEntity>(); // 空のリストを返す
            }

            return filteredEntities;
        }


        public List<IEntity> GetAllEntityList(Vector2Int vector)
        {
            int x, y;
            x = vector.x;
            y = vector.y;

            if (IsOutOfDataRangeArea(x, y)) return default(List<IEntity>);

            int index = ToSubscript(vector.x, vector.y);

            var resultEntityList = new List<IEntity>();

            if (index < 0 || index > GetLength())
            {
                Debug.LogError("領域外の値を習得しようとしました");
                return default(List<IEntity>);
            }

            return _entityMaps[index];
        }

        public void AddEntity<TEntity>(Vector2Int vector, TEntity entity) where TEntity : IEntity
        {
            var x = vector.x;
            var y = vector.y;

            if (IsOutOfDataRangeArea(x, y))
            {
                Debug.LogError($"IsOutOfDataRangeArea({x},{y})がtrueです");
                return;
            }

            AddEntity(ToSubscript(x, y), entity);
        }
        
        
        public void AddEntity<TEntity>(int index, TEntity entity) where TEntity : IEntity
        {
            if (index < 0 || index > GetLength())
            {
                Debug.LogError("領域外に値を設定しようとしました");
                return;
            }

            var entities = _entityMaps[index].OfType<TEntity>().ToList();

            if (entities.Any())
            {
                Debug.LogWarning(
                    $"[注意!!] 既に{typeof(TEntity)}が入っています。現在の数: {entities.Count}");
            }

            // domain
            _entityMaps[index].Add(entity);

            // presenter
            var count =_entityMaps[index].OfType<TEntity>().Count();
            // Debug.Log($"AddEntity({index}) count:{count}");
            _tilePresenter[index]?.SetEntityActiveData(entity, count);
        }

        
        public void RemoveEntity<TEntity>(int x, int y, TEntity entity) where TEntity : IEntity
        {
            if (IsOutOfDataRangeArea(x, y))
            {
                Debug.LogWarning($"IsOutOfDataRange({x},{y})がtrueです");
                return;
            }

            var index = ToSubscript(x, y);
            
            // domain
            _entityMaps[index].Remove(entity);
            

            // presenter
            var count = _entityMaps[index].OfType<TEntity>().Count();
            // Debug.Log($"RemoveEntity({x},{y}) count:{count}");
            _tilePresenter[index]?.SetEntityActiveData(entity, count);
        }

        public void RemoveEntity<TEntity>(Vector2Int vector, TEntity entity) where TEntity : IEntity
        {
            RemoveEntity(vector.x, vector.y, entity);
        }


    }

}