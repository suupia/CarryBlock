using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using Main.VContainer;
using UnityEngine;
using VContainer;

namespace Main
{
    [RequireComponent(typeof(NetworkObject))]
    public class NetworkResourceController :  PoolableObject
    {
        ResourceAggregator _resourceAggregator;
        public enum State
        {
            Idle,Reserved,Held,
        }
    
        public State state { get; private set; } = State.Idle;
        public bool canAccess => state == State.Idle;
        bool _isInitialized = false;
        Transform _holder;
        
        
        [Inject]
        public void Construct(ResourceAggregator resourceAggregator)
        {
            _resourceAggregator = resourceAggregator;
        }
            
        public override void Spawned() // 必要であればInit()にして外部から呼び出せるようにする
        {
            _isInitialized = true;
          //   _resourceAggregator = FindObjectOfType<GameSceneLifetimeScope>().Container.Resolve<ResourceAggregator>();
        }
        
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
    

        protected override void OnInactive()
        {
            if(!_isInitialized)return;
            if (state == State.Held) _resourceAggregator.AddResource(1);
            state = State.Idle;
        }
    
    
    }
    
    public class ResourceAggregator 
    {
        int _amount;
        public int getAmount => _amount;
        
        public void AddResource(int amount)
        {
            _amount += amount;
        }
    }

}

