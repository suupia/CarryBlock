﻿using Carry.CarrySystem.Player.Interfaces;
using Fusion;
using UnityEngine;

#nullable enable

namespace Projects.CarrySystem.Gimmick.Scripts
{ 
    [RequireComponent(typeof(Collider))]
    public class SpikeBodyControllerNet : NetworkBehaviour
    {
        readonly float _lifeTime = 1f;

        float _timer;
        
        public void Init(SpikeGimmick.Kind kind)
        {
            Debug.Log($"SpikeBodyControllerNet.Init() called");
        }
        
        public override void FixedUpdateNetwork()
        {
            if(!HasStateAuthority) return;

            _timer += Time.deltaTime;
            if (_timer > _lifeTime)
            {
                Runner.Despawn(Object);  // you have to write this code at bottom of this method
            }
            
        }

        void OnTriggerEnter(Collider other)
        {
            if (!HasStateAuthority) return;

            Debug.Log($"SpikeBody OnTriggerEnter() called");
            if (other.CompareTag("Player"))
            {
                Debug.Log($"痛ったぁぁぁぁい");
                var playerController = other.GetComponent<AbstractNetworkPlayerController>();
                // var character = playerController.GetCharacter;
                // character.OnDamage();
                var onDamageExecutor = playerController.GetOnDamageExecutor;
                onDamageExecutor.OnDamage();
            }
        }
    }
}