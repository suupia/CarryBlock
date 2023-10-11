using System;
using Carry.CarrySystem.Map.Scripts;
using TMPro;
using UnityEngine;

namespace Carry.EditMapSystem.EditMap.Scripts
{
    public class LoadedFilePresenter : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI loadedFileText;

       public  void  FormatLoadedFileText(MapKey mapKey, int index)
        {
            loadedFileText.text = $"Loaded File: {mapKey}_{index}";
        }
    }
}