#nullable enable
using System.Collections.Generic;
using Fusion;
using UnityEngine;

namespace Carry.CarrySystem.Map.Scripts
{
    /// <summary>
    /// This class is used to select which MapKeyData prefab to use.
    /// This should be placed in the LobbyScene.
    /// </summary>
    public class MapKeyDataSelectorLocal : NetworkBehaviour
    {
        [SerializeField] List<MapKeyDataLocal> mapKeyDataNetList = null!;

        public int MapKeyDataNetListCount => mapKeyDataNetList.Count;

        void Awake()
        {
            // To use this object in LobbyScene and CarryScene, DontDestroyOnLoad is used.
            DontDestroyOnLoad(this);
        }

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