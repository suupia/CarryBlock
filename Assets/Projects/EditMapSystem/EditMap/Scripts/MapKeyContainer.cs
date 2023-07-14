using System;
using Carry.CarrySystem.Map.Scripts;
using UnityEngine;

namespace Carry.EditMapSystem.EditMap.Scripts
{
    public class MapKeyContainer : MonoBehaviour
    {
        public MapKey MapKey { get; private set; }
        void Start()
        {
            DontDestroyOnLoad(gameObject);
        }
        
        public void SetMapKey(MapKey mapKey)
        {
            MapKey = mapKey;
        }
    }
}