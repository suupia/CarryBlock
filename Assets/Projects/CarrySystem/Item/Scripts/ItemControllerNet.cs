using Fusion;
using UnityEngine;

namespace Projects.CarrySystem.Item.Scripts
{
    public class ItemControllerNet : NetworkBehaviour
    {
        public void OnGain()
        {
            Debug.Log($"Gain Item {gameObject.name} pos:{transform.position}");
        }
    }
}