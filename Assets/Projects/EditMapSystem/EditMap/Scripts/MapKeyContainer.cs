using System;
using Carry.CarrySystem.Map.Scripts;
using UnityEngine;
#nullable enable

namespace Carry.EditMapSystem.EditMap.Scripts
{
    /// <summary>
    /// DontDestroyOnLoadでシーンを跨いでMapKeyを保持するためのクラス
    /// </summary>
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
            Debug.Log($"Set mapKey: {mapKey}");
        }
    }
}