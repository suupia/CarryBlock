using Fusion;
using UnityEngine;

#nullable enable

namespace Projects.CarrySystem.Enemy
{
    public class EnemyControllerNet : NetworkBehaviour
    {
        public override void Spawned()
        {
            // スポーンされた時に呼ばれる関数です。
            
            Debug.Log($"EnemyController.Spawned()");
            
        }
        
        public override void FixedUpdateNetwork()
        {
            // Tickごとに呼ばれる関数です。
            // まあ、ざっくりUpdate関数みたいなものです。
        }
    }
}