using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Carry.CarrySystem.Map.Scripts
{
    public static class  GridConverter
    {   
       static readonly float LengthBetweenHorizontal = 1.2f;
       static readonly float LengthBetweenVertical = 1.2f;
       
        public static  Vector3 GridPositionToWorldPosition(Vector2Int gridPos)
        {
            float x = gridPos.x * LengthBetweenHorizontal + LengthBetweenHorizontal / 2;
            float z = gridPos.y * LengthBetweenVertical + LengthBetweenVertical / 2;
            return new Vector3(x,0,z);
        }

        public static Vector2Int WorldPositionToGridPosition(Vector3 worldPos)
        {
            Vector2 postGridPos = new Vector2(worldPos.x / LengthBetweenHorizontal, worldPos.z / LengthBetweenVertical);
            int x = Mathf.FloorToInt(postGridPos.x);
            int y = Mathf.FloorToInt(postGridPos.y);
            return new Vector2Int(x, y);
        }
        
        /// <summary>
        /// Vector2を上下左右のVector2Intに変換する
        /// 0ベクトルの時は0ベクトルを返す
        /// </summary>
        /// <param name="directionVector"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static Vector2Int WorldDirectionToGridDirection(Vector2 directionVector)
        {
            if (directionVector == Vector2.zero)
            {
                //Debug.LogError($"unitDirectionVector:{unitDirectionVector}");
                return Vector2Int.zero;
            }

            float angle = Vector2.SignedAngle(Vector2.up, directionVector);
            //Debug.Log($"angle:{angle}");
            
            Vector2Int direction;

            if (-45.0f <= angle && angle < 45.0f)
            {
                direction = Vector2Int.up;
            }
            else if (45.0f <= angle && angle < 135.0f)
            {
                direction = Vector2Int.left;
            }
            else if (135.0f <= angle ||  angle < -135.0f)
            {
                direction = Vector2Int.down;
            }
            else if (-135.0f <= angle && angle < -45.0f)
            {
                direction = Vector2Int.right;
            }
            else
            {
                Debug.LogError($"angle:{angle}");
                throw new System.Exception();
            }

            //Debug.Log($"ToDirection({unitDirectionVector}):{direction}");
            return direction;
        }
        
    }

}