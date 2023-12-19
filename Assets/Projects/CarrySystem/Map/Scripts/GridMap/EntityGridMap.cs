#nullable enable
using System.Collections.Generic;
using Carry.CarrySystem.Entity.Interfaces;
using System.Linq;
using Carry.CarrySystem.Block.Interfaces;
using UnityEngine;
using Carry.CarrySystem.Map.Interfaces;


namespace Carry.CarrySystem.Map.Scripts
{
    public class EntityGridMap : IGridCoordinate, IGridMap
    {
        public int Width  => _coordinate.Width;
        public int Height => _coordinate.Height;
        public int Length => _coordinate.Length; 
        public int ToSubscript(int x, int y) => _coordinate.ToSubscript(x, y);
        public Vector2Int ToVector(int subscript) => _coordinate.ToVector(subscript);
        
        public bool IsInDataArea(int x, int y) => _coordinate.IsInDataArea(x, y);
        public bool IsInDataOrEdgeArea(int x, int y) => _coordinate.IsInDataOrEdgeArea(x, y);

        
        readonly List<IEntity>[] _entityMaps;
        readonly IGridCoordinate _coordinate;
        readonly IPlaceablePresenter?[] _blockPresenter;
        
        public EntityGridMap(IGridCoordinate coordinate)
        {
            _coordinate = coordinate;
            _entityMaps = new List<IEntity>[Length];
            _blockPresenter = new IPlaceablePresenter?[Length];
            for (int i = 0; i < Length; i++)
            {
                _entityMaps[i] = new List<IEntity>();
            }
        }
        
        public bool IsInDataArea(Vector2Int vector)
        {
            return _coordinate.IsInDataArea(vector);
        }

        public EntityGridMap ClearMap()
        {
            for (int i = 0; i < Length; i++)
            {
                _entityMaps[i] = new List<IEntity>();
            }
            // _blockPresenterは初期化していなことに注意
            return this;
        }
        
        public void RegisterTilePresenter(IPlaceablePresenter placeablePresenter, int index)
        {
            _blockPresenter[index] = placeablePresenter;
        }

        //Getter
        public T? GetSingleEntity<T>(Vector2Int vector) where T : IEntity
        {
            int x = vector.x;
            int y = vector.y;

            if (_coordinate.IsOutOfDataArea(x, y)) {return default(T);}

            return GetSingleEntity<T>(ToSubscript(x, y));
        }
        
        public T? GetSingleEntity<T>(int index) where T : IEntity
        {
            if (index < 0 || index > Length)
            {
                Debug.LogError("領域外の値を習得しようとしました");
                return default(T);
            }
            
            var entity = _entityMaps[index].OfType<T>().FirstOrDefault();

            if (entity == null)
            {
                // Debug.Log($"_entityMaps[{index}].OfType<TEntity>().FirstOrDefault()がnullです");
                return default(T);
            }
            else
            {
                //探しているEntityTypeの先頭のものを返す（存在しない場合はdefault）
                return entity;
            }
        }

        public List<T> GetSingleTypeList<T>(Vector2Int vector) where T : IEntity
        {
            var x = vector.x;
            var y = vector.y;

            if (_coordinate.IsOutOfDataArea(x, y)) return new List<T>();

            return GetSingleTypeList<T>(ToSubscript(x, y));
        }

        public List<T> GetSingleTypeList<T>(int index) 
        {
            
            if (index < 0 || index > Length)
            {
                Debug.LogError("領域外の値を習得しようとしました");
                return new List<T>(); // 空のリストを返す
            }

            var filteredEntities = _entityMaps[index].OfType<T>().ToList();
    
            if (!filteredEntities.Any())
            {
                //Debug.Log($"_entityMaps[{index}]の{typeof(EntityType)}のCountが0です");
                return  new List<T>(); // 空のリストを返す
            }

            return filteredEntities;
        }


        public IEnumerable<IEntity> GetAllTypeList(Vector2Int vector)
        {
            int x, y;
            x = vector.x;
            y = vector.y;

            if (_coordinate.IsOutOfDataArea(x, y)) return  new List<IEntity>();
            
            return GetSingleTypeList<IEntity>(ToSubscript(x, y));

        }
        
        public IEnumerable<IEntity> GetAllTypeList(int  index)
        {
            if (index < 0 || index > Length)
            {
                Debug.LogError("領域外の値を習得しようとしました");
                return new List<IEntity>(); // 空のリストを返す
            }

            return _entityMaps[index];
        }
        
        public void AddEntity<T>(Vector2Int vector, T entity) where T : IEntity
        {
            var x = vector.x;
            var y = vector.y;

            if (_coordinate.IsOutOfDataArea(x, y))
            {
                Debug.LogError($"IsOutOfDataRangeArea({x},{y})がtrueです");
                return;
            }

            AddEntity(ToSubscript(x, y), entity);
        }
        
        
        public void AddEntity<T>(int index, T entity) where T : IEntity
        {
            if (index < 0 || index > Length)
            {
                Debug.LogError("領域外に値を設定しようとしました");
                return;
            }

            var entities = _entityMaps[index].OfType<T>().ToList();

            if (entities.Any())
            {
                Debug.LogWarning(
                    $"[注意!!] 既に{typeof(T)}が入っています。現在の数: {entities.Count}");
            }

            // domain
            _entityMaps[index].Add(entity);

            // presenter
            CallSetEntityActiveData(entity, index);
        }

        
        public void RemoveEntity<T>(int x, int y, T entity) where T : IEntity
        {
            if (_coordinate. IsOutOfDataArea(x, y))
            {
                Debug.LogWarning($"IsOutOfDataRange({x},{y})がtrueです");
                return;
            }

            var index = ToSubscript(x, y);
            
            // domain
            _entityMaps[index].Remove(entity);

            // presenter
            CallSetEntityActiveData(entity, index);

        }

        public void RemoveEntity<T>(Vector2Int vector, T entity) where T : IEntity
        {
            RemoveEntity(vector.x, vector.y, entity);
        }
        
        void CallSetEntityActiveData(IEntity entity , int index)
        {
            if(entity is IPlaceable placeable)
            {
                var count = _entityMaps[index].OfType<IPlaceable>().Count();
                // Debug.Log($"AddEntity({index}) count:{count}");
                _blockPresenter[index]?.SetEntityActiveData(placeable, count);
            }
        }


    }

}