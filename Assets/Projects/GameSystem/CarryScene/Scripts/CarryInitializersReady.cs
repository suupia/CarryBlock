using System.Collections.Generic;
using Fusion;
using UnityEngine;

#nullable enable

namespace Carry.CarrySystem.CarryScene.Scripts
{
    public class CarryInitializersReady : NetworkBehaviour
    {
        [Networked]
        [Capacity(4)] // Sets the fixed capacity of the collection
        [UnitySerializeField] // Show this private property in the inspector.
        NetworkDictionary<int, NetworkBool> InitializersReady => default;

        public override void Spawned()
        {
            base.Spawned();
            DontDestroyOnLoad(this);
        }

        public bool IsAllInitializersReady()
        {
            Debug.Log($"InitializersReady.Count:{InitializersReady.Count}");
            int i = 0;
            foreach (var initializerReady in InitializersReady)
            {
                Debug.Log($"initializerReady.Value:{initializerReady.Value}, i:{i}");
                if (!initializerReady.Value)
                {
                    return false;
                }

                i++;
            }

            return true;
        }
        
        public void AddInitializerReady(PlayerRef playerRef)
        {
            InitializersReady.Add(playerRef, false);
        }
        
        public void SetInitializerReady(PlayerRef playerRef)
        {
            InitializersReady.Set(playerRef, true);
        }
    }
}