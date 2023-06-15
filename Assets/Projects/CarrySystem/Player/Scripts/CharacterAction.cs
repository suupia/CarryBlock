using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.Player.Interfaces;
using UnityEngine;
using VContainer;
using VContainer.Unity;
#nullable enable

namespace Carry.CarrySystem.Player.Scripts
{
    public class CharacterAction : ICharacterAction
    {
        readonly Transform _transform;
        readonly EntityGridMap _map;
        public CharacterAction(Transform transform)
        {
            _transform = transform;
            // var mapSwitcher = Object.FindObjectOfType<LifetimeScope>().Container.Resolve<EntityGridMapSwitcher>();
            // _map = mapSwitcher.GetMap();
        }
        public void Action()
        {
            Debug.Log($"ものを拾ったり、置いたりします");

            // 自身のGridPosを表示
            var gridPos = GridConverter.WorldPositionToGridPosition(_transform.position);
            Debug.Log($"Player GridPos: {gridPos}");

            var direction = new Vector2(_transform.forward.x, _transform.forward.z);
            var gridDirection = GridConverter.WorldDirectionToGridDirection(direction);
            // 前方のGridPosを表示
            var forwardGridPos = gridPos + gridDirection;
            Debug.Log($"Player Forward GridPos: {forwardGridPos}");
            // そのGridPosにアイテムがあるかどうかを確認


            // アイテムがある場合は、アイテムを拾う

            // アイテムがない、かつ、アイテムを持っているときは、アイテムを置く
        }
    }
}