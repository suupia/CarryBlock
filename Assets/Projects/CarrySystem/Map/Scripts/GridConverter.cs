using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Carry.CarrySystem.Map.Scripts
{
    public static class  GridConverter
    {   
       static readonly float LengthBetweenHorizontal = 1.0f;
       static readonly float LengthBetweenVertical = 1.0f;
       
        public static  Vector3 GridPositionToWorldPosition(Vector2Int gridPos)
        {
            return new Vector3(gridPos.x * LengthBetweenHorizontal,0 , gridPos.y * LengthBetweenVertical);
        }

        public static Vector2Int WorldPositionToGridPosition(Vector3 worldPos)
        {
            Vector2 postGridPos = new Vector2(worldPos.x / LengthBetweenHorizontal, worldPos.z / LengthBetweenVertical);
            int x = Mathf.FloorToInt(postGridPos.x);
            int y = Mathf.FloorToInt(postGridPos.y);
            return new Vector2Int(x, y);
        }
        
    }

}