using Carry.CarrySystem.Map.Scripts;
using Fusion;
using UnityEngine;

namespace Projects.CarrySystem.Item.Scripts
{
    public class ItemControllerNet : NetworkBehaviour
    {
        [SerializeField] GameObject itemViewObj = null!;  // ランタイムで生成しないので、SerializeFieldで受け取れる
        [SerializeField] ItemInfo info = null!;
        public ItemInfo Info => info;

        public override void Spawned()
        {
            info.Init(itemViewObj, this);
            
        }
        
        public void OnGained()
        {
            var gridPos = GridConverter.WorldPositionToGridPosition(gameObject.transform.position);
        }
    }
}