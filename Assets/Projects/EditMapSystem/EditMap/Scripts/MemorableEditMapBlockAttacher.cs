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
        readonly Stack<OperationData> _stack;
        readonly Stack<OperationData> _redoStack; 
        readonly IEditMapBlockAttacher _editMapBlockAttacher;
        
        public MemorableEditMapBlockAttacher(IEditMapBlockAttacher editMapBlockAttacher, int capacity = 100)
        {
            _editMapBlockAttacher = editMapBlockAttacher;
            _stack = new Stack<OperationData>(capacity);
            _redoStack = new Stack<OperationData>(capacity);
        }
        
        public void AddPlaceable(EntityGridMap map , Vector2Int gridPos, IPlaceable addBlock)
        {
            var op = new OperationData(Operation.Add, gridPos, addBlock);
            Do(map, op);
            _redoStack.Clear();
            _stack.Push(op);
        }
        
        public void RemovePlaceable(EntityGridMap map, Vector2Int gridPos)
        {
            var obj = map.GetSingleEntity<IPlaceable>(gridPos);
             
            if (obj == null) return;
            var op = new OperationData(Operation.Remove, gridPos, obj);
            Do(map, op);
            _redoStack.Clear();
            _stack.Push(op);
        }

        public bool Redo(EntityGridMap map)
        {
            var success =_redoStack.TryPop(out OperationData op);
            if (!success)
            {
                Debug.LogWarning("Cannot Redo.");
                return false;
            }

            Do(map, op);
            _stack.Push(op);
            return true;
        }

        public bool Undo(EntityGridMap map)
        {
            var success =_stack.TryPop(out OperationData op);
            if (!success)
            {
                Debug.LogWarning("Cannot Undo.");
                return false;
            }
            _redoStack.Push(op);
            
            Do(map, op.Reverse());
            return true;
        }

        public void Reset()
        {
            _stack.Clear();
            _redoStack.Clear();
        }

        void Do(EntityGridMap map, OperationData data)
        {
            switch (data.Operation)
            {
                case Operation.Add:
                    _editMapBlockAttacher.AddPlaceable(map, data.Coords, data.Placeable);
                    Debug.Log($"Add, {data.Coords}, {data.Placeable}");
                    break;
                case Operation.Remove:
                    _editMapBlockAttacher.RemovePlaceable(map, data.Coords);
                    Debug.Log($"Remove, {data.Coords}, {data.Placeable}");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override string ToString()
        {
            return $"stack: {_stack}, redoStack: {_redoStack}";
        }
    }
}
