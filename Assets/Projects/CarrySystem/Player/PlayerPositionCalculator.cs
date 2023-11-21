#nullable enable
using Carry.CarrySystem.Player.Scripts;
using UnityEngine;
using VContainer;

namespace Carry.CarrySystem.Player
{
    public static class PlayerPositionCalculator
    {
        static readonly float Radius = 1f;  // プレイヤーはこの円周上に配置していく
        
        public static Vector3 CalcPlayerPosition(Vector3 centerPosition, int playerNumber, int maxPlayerCount)
        {
            var offset = maxPlayerCount switch
            {
                1 => Vector3.zero,
                2 => playerNumber switch
                {
                    1 => new Vector3(Radius, 0 , 0),
                    2 => new Vector3(-Radius, 0, 0),
                    _ => Vector3.zero,
                },
                3 => playerNumber switch
                {
                    1 => new Vector3(Radius, 0, 0),
                    2 => new Vector3(-Radius / 2, 0,  Radius*Mathf.Sqrt(3) / 2),
                    3 => new Vector3(- Radius/ 2, 0, -Radius*Mathf.Sqrt(3) / 2),
                    _ => Vector3.zero,
                },
                4 => playerNumber switch
                {
                    1 => new Vector3(Radius * 1/Mathf.Sqrt(2), 0, Radius * 1/Mathf.Sqrt(2)),
                    2 => new Vector3(Radius * 1/Mathf.Sqrt(2), 0, -Radius * 1/Mathf.Sqrt(2)),
                    3 => new Vector3(-Radius * 1/Mathf.Sqrt(2), 0, Radius * 1/Mathf.Sqrt(2)),
                    4 => new Vector3(-Radius * 1/Mathf.Sqrt(2), 0, -Radius * 1/Mathf.Sqrt(2)),
                    _ => Vector3.zero,
                },
                _ => Vector3.zero,
            };
            return centerPosition + offset;

        }
        

    }
}