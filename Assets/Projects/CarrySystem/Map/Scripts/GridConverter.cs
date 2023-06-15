using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Carry.CarrySystem.Map.Scripts
{
    public class GridConverter
    {   
       readonly float _lengthBetweenHorizontal;
       readonly float _lengthBetweenVertical;
        
        public GridConverter(float lengthBetweenHorizontal, float lengthBetweenVertical)
        {
            _lengthBetweenHorizontal = lengthBetweenHorizontal;
            _lengthBetweenVertical = lengthBetweenVertical;
        }
        
        public  Vector3 GridPositionToWorldPosition(Vector2Int gridPos)
        {
            return new Vector3(gridPos.x * _lengthBetweenHorizontal,0 , gridPos.y * _lengthBetweenVertical);
        }
        
        // public Vector2Int WorldPositionToGridPosition(Vector3 worldPos)
    }

}