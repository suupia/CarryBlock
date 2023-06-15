using System;
using System.Collections;
using System.Collections.Generic;
using Carry.CarrySystem.Entity.Interfaces;
using System.Linq;
using UnityEngine;

#nullable enable

namespace Carry.CarrySystem.Map.Scripts
{
    public class EntityGridMap : NumericGridMap
    {
        readonly List<IEntity>[] _entityMaps;
        
        public EntityGridMap(int width, int height) : base(width, height)
        {
            _entityMaps = new List<IEntity>[GetLength()];
            for (int i = 0; i < GetLength(); i++)
            {
                _entityMaps[i] = new List<IEntity>();
            }
        }

        public EntityGridMap CloneMap()
        {
            return (EntityGridMap)MemberwiseClone();
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
            var resultEntityList = new List<TEntity>();

            if (index < 0 || index > GetLength())
            {
                Debug.LogError("領域外の値を習得しようとしました");
                return default(TEntity);
            }

            if (_entityMaps[index].Count(s => s.GetType() == typeof(TEntity)) == 0)
            {
                //Debug.Log($"_entityMaps[{index}]の{typeof(EntityType)}のCountが0です");
                return default(TEntity);
            }

            foreach (var entity in _entityMaps[index])
            {
                if (entity.GetType() == typeof(TEntity)) resultEntityList.Add((TEntity)entity);
            }


            ////今はリストであることは使わない
            //return resultEntityList[0];

            foreach (var entity in resultEntityList)
            {
                //探しているEntityTypeの先頭のものを返す
                if (entity.GetType() == typeof(TEntity)) return entity;
            }

            //なかった場合
            return default(TEntity);
        }

        public List<TEntity> GetSingleEntityList<TEntity>(Vector2Int vector) where TEntity : IEntity
        {
            var x = vector.x;
            var y = vector.y;

            if (IsOutOfDataRangeArea(x, y)) return default(List<TEntity>);

            return GetSingleEntityList<TEntity>(ToSubscript(x, y));
        }

        public List<TEntity> GetSingleEntityList<TEntity>(int index) where TEntity : IEntity
        {
            var resultEntityList = new List<TEntity>();

            if (index < 0 || index > GetLength())
            {
                Debug.LogError("領域外の値を習得しようとしました");
                return default(List<TEntity>);
            }

            if (_entityMaps[index].Count(s => s.GetType() == typeof(TEntity)) == 0)
            {
                //Debug.Log($"_entityMaps[{index}]の{typeof(EntityType)}のCountが0です");
                return default(List<TEntity>);
            }

            foreach (var entity in _entityMaps[index])
            {
                if (entity.GetType() == typeof(TEntity)) resultEntityList.Add((TEntity)entity);
            }

            return resultEntityList;
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

        public void AddEntity<TEntity>(Vector2Int vector, IEntity entity) where TEntity : IEntity
        {
            var x = vector.x;
            var y = vector.y;

            if (IsOutOfDataRangeArea(x, y))
            {
                Debug.LogError($"IsOutOfDataRangeArea({x},{y})がtrueです");
                return;
            }

            AddEntity<TEntity>(ToSubscript(x, y), entity);
        }

        public void AddEntity<TEntity>(int index, IEntity entity) where TEntity : IEntity
        {
            if (index < 0 || index > GetLength())
            {
                Debug.LogError("領域外に値を設定しようとしました");
                return;
            }

            if (_entityMaps[index].Count(s => s.GetType() == typeof(TEntity)) > 0)
            {
                Debug.LogWarning(
                    $"[注意!!] GetEntityList<EntityType>(index).Countが0より大きいため、既に{typeof(TEntity)}が入っています");
            }

            _entityMaps[index].Add(entity);
        }

        public void RemoveEntity(int x, int y, IEntity entity)
        {
            if (IsOutOfDataRangeArea(x, y))
            {
                Debug.LogWarning($"IsOutOfDataRange({x},{y})がtrueです");
                return;
            }

            _entityMaps[ToSubscript(x, y)].Remove(entity);
        }

        public void RemoveEntity(Vector2Int vector, IEntity entity)
        {
            RemoveEntity(vector.x, vector.y, entity);
        }


    }

}