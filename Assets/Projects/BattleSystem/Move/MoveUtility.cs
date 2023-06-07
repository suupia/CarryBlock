using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Assertions;

namespace Nuts.BattleSystem.Move.Scripts
{
    public static class MoveUtility
    {
        /// <summary>
        /// timeの間ジャンプするために、yの速度を変更する
        /// 
        /// y = v0 * t - 1/2 * g * t^2
        ///
        /// をv0について解いただけ
        /// </summary>
        /// <param name="rb"></param>
        /// <param name="time"></param>
        public static void Jump([NotNull] Rigidbody rb, float time)
        {
            Assert.IsTrue(time > 0);
            var g = Physics.gravity.magnitude;
            var v0 = 0.5f * g * time;
            
            rb.velocity += Vector3.up * v0;
        }
        
        /// <summary>
        /// プレイヤーの入力をシミュレーションする関数。正規化して返す
        /// </summary>
        /// <returns></returns>
        public static Vector3 SimulateRandomInput() => new Vector3(
            Random.Range(-1, 2),
            0,
            Random.Range(-1, 2)
        ).normalized;
    }
}