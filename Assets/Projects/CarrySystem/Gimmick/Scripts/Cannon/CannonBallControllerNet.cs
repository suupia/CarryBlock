using System;
using Fusion;
using UnityEngine;

namespace Carry.CarrySystem.Gimmick.Scripts
{
    public class CannonBallControllerNet : NetworkBehaviour
    {
        readonly float _speed = 5f;
        public void Init()
        {
            Debug.Log($"CannonBallControllerNet.Init() called");
        }

        void Update()
        {
            transform.position += transform.forward * Time.deltaTime * _speed;
        }
    }
}