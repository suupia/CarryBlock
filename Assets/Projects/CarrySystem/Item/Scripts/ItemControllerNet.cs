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
        
        public void OnGain()
        {
            Debug.Log($"Gain Item {gameObject.name} pos:{transform.position}");
            Debug.Log($"Gain Item info.ItemType :{info.ItemType}");
        }
    }
}