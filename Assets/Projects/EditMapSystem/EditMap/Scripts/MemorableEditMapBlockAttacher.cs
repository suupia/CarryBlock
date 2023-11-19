using System;
using System.Collections.Generic;
using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using Carry.EditMapSystem.EditMap.Scripts;
using UnityEngine;

#nullable enable

namespace Carry.EditMapSystem.EditMapForPlayer.Scripts
{
    internal enum Operation
    {
        Add,
        Remove
    }
    
    //TODO structかrecordか
    internal readonly struct OperationData
    {
        public OperationData(Operation operation, Vector2Int coords, IPlaceable? placeable = null)
        {
            Operation = operation;
            Coords = coords;
            Placeable = placeable;
        }

        public Operation Operation { get; }
        public Vector2Int Coords { get; }
        public IPlaceable? Placeable { get; }

        public OperationData Reverse() =>
             Operation switch
            {
                Operation.Add => new OperationData(Operation.Remove, Coords, Placeable),
                Operation.Remove => new OperationData(Operation.Add, Coords, Placeable),
                _ => throw new ArgumentOutOfRangeException()
            };
        
    }
    
    public class MemorableEditMapBlockAttacher: IEditMapBlockAttacher
    {
        //1度の入力で複数個のブロックを置くことがあるので、Listで管理する
        readonly Stack<List<OperationData> > _undoStack;
        readonly Stack<List<OperationData> > _redoStack; 
        readonly IEditMapBlockAttacher _editMapBlockAttacher;
        
        public int GetUndoStackCount() => _undoStack.Count;

        public int GetRedoStackCount() => _redoStack.Count;
        
        public MemorableEditMapBlockAttacher(IEditMapBlockAttacher editMapBlockAttacher, int capacity = 100)
        {
            _editMapBlockAttacher = editMapBlockAttacher;
            
            _undoStack = new Stack<List<OperationData> >(capacity);
            _redoStack = new Stack<List<OperationData> >(capacity);
        }

        public bool AddPlaceable(EntityGridMap map, Vector2Int gridPos, IPlaceable addBlock)
        {
            //空のオペレーションを追加しないため，範囲外ならば処理を行わない
            if (!map.IsInDataRangeArea(gridPos)) { return false; }

            var op = new OperationData(Operation.Add, gridPos, addBlock);
            
            if (Do(map, op))
            {
                _redoStack.Clear();
                _undoStack.Push(new List<OperationData>() { op });    
            }
            
            return true;
        }

        public bool RemovePlaceable(EntityGridMap map, Vector2Int gridPos)
        {
            //空のオペレーションを追加しないため，範囲外ならば処理を行わない
            if (!map.IsInDataRangeArea(gridPos)) { return false; }

            var obj = map.GetSingleEntity<IPlaceable>(gridPos);

            if (obj == null){ return false;}
            var op = new OperationData(Operation.Remove, gridPos, obj);

            if (Do(map, op))
            {
                _redoStack.Clear();
                _undoStack.Push(new List<OperationData>() { op });   
            }

            return true;
        }

        public void Clear(EntityGridMap map)
        {
            var opList = new List<OperationData>();
            
            for (int i = 0; i < map.Length; i++)
            {
                var entities = map.GetSingleEntityList<IPlaceable>(i);
                
                int count = entities.Count;
                
                for (int j = 0; j < count; j++)
                {
                    map.RemoveEntity(map.ToVector(i), entities[j]);
                    
                    opList.Add(new OperationData(Operation.Remove, map.ToVector(i), entities[j]));
                }
            }

            //空のオペレーションを追加しないため，opListの要素数が0でないときだけ追加する
            if (opList.Count > 0)
            {
                _undoStack.Push(opList);
            }
        }
        
        public void Redo(EntityGridMap map)
        {
            var success =_redoStack.TryPop(out List<OperationData> opList);
            if (!success)
            {
                Debug.LogWarning("Cannot Redo.");
                return;
            }
            
            foreach (var data in opList)
            {
                Do(map, data);
            }
            
            _undoStack.Push(opList);
        }

        public void Undo(EntityGridMap map)
        {
            var success =_undoStack.TryPop(out List<OperationData> opList);
            if (!success)
            {
                Debug.LogWarning("Cannot Undo.");
                return;
            }
            _redoStack.Push(opList);

            foreach (var data in opList)
            {
                Do(map, data.Reverse());
            }
        }

        bool Do(EntityGridMap map, OperationData data)
        {
            switch (data.Operation)
            {
                case Operation.Add:
                    Debug.Log($"Add, {data.Coords}, {data.Placeable}");
                    return _editMapBlockAttacher.AddPlaceable(map, data.Coords, data.Placeable);
                
                case Operation.Remove:
                    Debug.Log($"Remove, {data.Coords}, {data.Placeable}");
                    return _editMapBlockAttacher.RemovePlaceable(map, data.Coords);
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override string ToString()
        {
            return $"undoStack: {_undoStack}, redoStack: {_redoStack}";
        }
    }
}
