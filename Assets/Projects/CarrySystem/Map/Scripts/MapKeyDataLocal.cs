#nullable enable
using System.Collections.Generic;
using UnityEngine;

namespace Carry.CarrySystem.Map.Scripts
{
    public class MapKeyDataLocal : MonoBehaviour
    {
        public IReadOnlyList<MapKeyData> MapKeyDataList => mapKeyDataList;
        [SerializeField] List<MapKeyData> mapKeyDataList = null!;

    }
}