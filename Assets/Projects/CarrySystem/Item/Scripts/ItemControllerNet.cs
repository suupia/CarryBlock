using System.Collections.Generic;
using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using Fusion;
using Projects.CarrySystem.Item.Interfaces;
using UnityEngine;
#nullable enable

namespace Projects.CarrySystem.Item.Scripts
{
    public class ItemControllerNet : NetworkBehaviour
    {
        [SerializeField] GameObject itemViewObj = null!;  // ランタイムで生成しないので、SerializeFieldで受け取れる
        [SerializeField] ItemInfo info = null!;
        public ItemInfo Info => info;
        IList<IItem>  _items;

        public void Init(IList<IItem> items)
        {
            _items = items;
            Debug.Log($"_items = {_items}");
        }

        public override void Spawned()
        {
            info.Init(itemViewObj, this);
            
        }
        
        public void OnGained()
        {
            var gridPos = GridConverter.WorldPositionToGridPosition(gameObject.transform.position);
            foreach (var item in _items)
            {
                Debug.Log($"item.OnGained()");
                item.OnGained();
            }
        }
    }
}