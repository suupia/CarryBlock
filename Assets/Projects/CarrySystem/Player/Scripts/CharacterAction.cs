using Carry.CarrySystem.Entity.Scripts;
using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.Player.Interfaces;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Carry.CarrySystem.Player.Info;
#nullable enable

namespace Carry.CarrySystem.Player.Scripts
{
    public class CharacterAction : ICharacterAction
    {
         EntityGridMap _map;
        
        bool _isCarrying = false;
        public void Setup()
        {
            var resolver = Object.FindObjectOfType<LifetimeScope>().Container; // このコンストラクタはNetworkBehaviour内で実行されるため、ここで取得してよい
            _map = resolver.Resolve<EntityGridMapSwitcher>().GetMap();
        }
        public void Action(PlayerInfo info)
        {
            var transform = info.playerObj.transform;
            
            Debug.Log($"ものを拾ったり、置いたりします");

            // 自身のGridPosを表示
            var gridPos = GridConverter.WorldPositionToGridPosition(transform.position);
            Debug.Log($"Player GridPos: {gridPos}");

            // 前方のGridPosを表示
            var forward = transform.forward;
            var direction = new Vector2(forward.x, forward.z);
            var gridDirection = GridConverter.WorldDirectionToGridDirection(direction);
            var forwardGridPos = gridPos + gridDirection;
            Debug.Log($"Player Forward GridPos: {forwardGridPos}");
            
            // そのGridPosにRockがあるかどうかを確認
            var index = _map.GetIndexFromVector(forwardGridPos);
            Debug.Log($"index : {index}のRockは{_map.GetSingleEntity<Rock>(index)}です");
            var rock = _map.GetSingleEntity<Rock>(forwardGridPos);
            if (rock != null)
            {
                Debug.Log($"Rockがあります！！！");
                if (_isCarrying)
                {
                    return;
                }
                else
                {
                    // ドメインのRockを削除（内部のプレゼンターを通して見た目も変わる）
                    _map.RemoveEntity(forwardGridPos, rock);
                    
                    // プレイヤーがRockを持つようにする
                }
            }
            else
            {
                Debug.Log($"Rockがありません");
            }

            // アイテムがある場合は、アイテムを拾う

            // アイテムがない、かつ、アイテムを持っているときは、アイテムを置く
        }
    }
}