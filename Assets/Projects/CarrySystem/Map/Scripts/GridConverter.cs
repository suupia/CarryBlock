using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Carry.CarrySystem.Map.Scripts
{
    public static class GridConverter
    {   
       static readonly float LengthBetweenHorizontal;
       static readonly float LengthBetweenVertical;
       
        public static  Vector3 GridPositionToWorldPosition(Vector2Int gridPos)
        {
            return new Vector3(gridPos.x * LengthBetweenHorizontal,0 , gridPos.y * LengthBetweenVertical);
        }
        
        // public Vector2Int WorldPositionToGridPosition(Vector3 worldPos)
    }

}