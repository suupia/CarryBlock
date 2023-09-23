using System.Collections.Generic;
using Fusion;
using UnityEngine;

namespace Carry.CarrySystem.Map.Scripts
{
    /// <summary>
    /// This class is used to select which MapKeyData prefab to use.
    /// </summary>
    public class MapKeyDataSelectorNet : NetworkBehaviour
    {
        [SerializeField] List<MapKeyDataNet> mapKeyDataNetList = null!;

        public IReadOnlyList<MapKeyData> SelectMapKeyDataList(int index)
        {
            if (index < 0 || index >= mapKeyDataNetList.Count)
            {
                Debug.LogError($"index:{index} is out of range");
                return mapKeyDataNetList[0].MapKeyDataList;
            }
            return mapKeyDataNetList[index].MapKeyDataList;
        }
    }
}