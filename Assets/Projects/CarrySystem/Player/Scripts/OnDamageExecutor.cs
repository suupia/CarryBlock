using Carry.CarrySystem.Player.Interfaces;
using UnityEngine;

#nullable enable

namespace Carry.CarrySystem.Player.Scripts
{
    public class OnDamageExecutor : IOnDamageExecutor
    {
        public void OnDamage()
        {
            Debug.Log($"ダメージを受けた！");
        }
    }
}