﻿using Fusion;
using UnityEngine;

namespace Carry.CarrySystem.Gimmick.Scripts
{
    public class CannonBallControllerNet : NetworkBehaviour
    {
        // ToDo: とりあえず、まっすぐに進むようにしておく
        public void Init()
        {
            Debug.Log($"CannonBallControllerNet.Init() called");
        }
        
    }
}