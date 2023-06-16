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
    public class HoldAction : ICharacterAction
    {
        PlayerInfo _info;
        EntityGridMap _map;
        IHoldActionPresenter? _presenter;
        public bool IsHoldingRock { get; set; } = false;

        public HoldAction(IHoldActionPresenter? presenter)
        {
            _presenter = presenter;
        }

        public void Setup(PlayerInfo info)
        {
            _info = info;
            var resolver = Object.FindObjectOfType<LifetimeScope>().Container; // このコンストラクタはNetworkBehaviour内で実行されるため、ここで取得してよい
            _map = resolver.Resolve<EntityGridMapSwitcher>().GetMap();
        }
        public void Action()
        {
            var transform = _info.playerObj.transform;
            
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
            if (rock == null)
            {
                Debug.Log($"Rockがありません");
                
                // 持っていたら置く処理をする
                if (IsHoldingRock)
                {
                    // 持っていいるRockを置く
                    _map.AddEntity<Rock>(forwardGridPos, new Rock(Rock.Kind.Kind1, forwardGridPos));

                    IsHoldingRock = false;
                    
                    _presenter?.PutDownRock();
                    
                    return;
                }
                else
                {
                    // 何もしない
                }

            }
            else
            {
                Debug.Log($"Rockがあります！！！");
                if (IsHoldingRock)
                {
                    // 何もしない
                    return;
                }
                else
                {
                    // ドメインのRockを削除（内部のプレゼンターを通して見た目も変わる）
                    _map.RemoveEntity(forwardGridPos, rock);
                    
                    // プレイヤーがRockを持つようにする
                    IsHoldingRock = true;
                    
                    _presenter?.PickUpRock();

                }
            }

            // アイテムがある場合は、アイテムを拾う

            // アイテムがない、かつ、アイテムを持っているときは、アイテムを置く
        }
    }
}