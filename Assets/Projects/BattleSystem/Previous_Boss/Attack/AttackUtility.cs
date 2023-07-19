using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Assertions;

namespace Projects.BattleSystem.Enemy.Scripts.Boss_Previous.Attack
{
    public static class AttackUtility
    {
        public static void Throw([NotNull] Rigidbody rb, Vector3 from, Vector3 to, float time)
        {
            //y軸方向の速度を算出
            Assert.IsTrue(time > 0);
            var g = Physics.gravity.magnitude;
            var vy = 0.5f * g * time;

            //指定された時間で from から to に到達するためのxz平面の速度
            var u = (to - from).normalized;
            var d = (to - from).magnitude;
            var velocity = u * d / time;

            velocity.y = vy;

            rb.velocity = velocity;
        }
    }
}