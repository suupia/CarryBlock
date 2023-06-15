using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.Player.Interfaces;
using UnityEngine;

namespace Carry.CarrySystem.Player.Scripts
{
    public class CharacterAction : ICharacterAction
    {
        readonly Transform _transform;
        public CharacterAction(Transform transform)
        {
            _transform = transform;
        }
        public void Action()
        {
            Debug.Log($"ものを拾ったり、置いたりします");

            // 自身のGridPosを表示
            var gridPos = GridConverter.WorldPositionToGridPosition(_transform.position);
            Debug.Log($"Player GridPos: {gridPos}");

            // そのGridPosにアイテムがあるかどうかを確認

            // アイテムがある場合は、アイテムを拾う

            // アイテムがない、かつ、アイテムを持っているときは、アイテムを置く
        }
    }
}