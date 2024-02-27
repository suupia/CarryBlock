using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

namespace Carry.CarrySystem.Map.Scripts
{
    public class MapKeyDataNet : MonoBehaviour
    {
        public IReadOnlyList<MapKeyData> MapKeyDataList => mapKeyDataList;
        [SerializeField] List<MapKeyData> mapKeyDataList = null!;

    }
    
        
    [Serializable]
    public class MapKeyData
    {
        public MapKey mapKey;
        public int index;
    }
}