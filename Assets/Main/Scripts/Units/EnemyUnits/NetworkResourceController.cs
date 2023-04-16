using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

namespace Main
{
    [RequireComponent(typeof(NetworkObject))]
    public class NetworkResourceController :  PoolableObject
    {
        public enum State
        {
            Idle,Reserved,Held,
        }
    
        public State state { get; private set; } = State.Idle;
        public bool canAccess => state == State.Idle;
        bool _isInitialized = false;
        Transform _holder;
        
        public override void Render()
        {
            switch (state)
            {
                case State.Idle:
                    break;
                case State.Reserved:
                    break;
                case State.Held:
                    transform.position = _holder.position;
                    break;
            }
        }
        
        public void OnReserved()
        {
            state = State.Reserved;
        }
    
        public void OnHeld(Transform holder)
        {
            state = State.Held;
            _holder = holder;
        }
    
    
        public override void Spawned() // 必要であればInit()にして外部から呼び出せるようにする
        {
            _isInitialized = true;
        }
        protected override void OnInactive()
        {
            if(!_isInitialized)return;
            state = State.Idle;
        }
    
    
    }

}

