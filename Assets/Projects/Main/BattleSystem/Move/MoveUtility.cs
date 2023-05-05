using UnityEngine;
using UnityEngine.Assertions;

namespace Main
{
    public static class MoveUtility
    {
        /// <summary>
        /// timeの間ジャンプするために、yの速度を変更する
        /// </summary>
        /// <param name="rd"></param>
        /// <param name="time"></param>
        public static void Jump(Rigidbody rd, float time)
        {
            Assert.IsTrue(time > 0);
            var g = Physics.gravity.magnitude;
            var v0 = 0.5f * g * time;
            
            rd.velocity += Vector3.up * v0;
        }
    }
}