using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

namespace Carry.UISystem.UI.EditMap
{
    public class EditMapBlockSelectCanvas : MonoBehaviour
    {
        [SerializeField] Transform buttonParent = null!;
        [SerializeField] CustomButton buttonPrefab;
        
        void Start()
        {
            Assert.IsNotNull(buttonParent);
            
            InstantiateBlockSelectButton("Basic", () => Debug.Log("Basic"));
            InstantiateBlockSelectButton("Wall", () => Debug.Log("Wall"));
            InstantiateBlockSelectButton("Floor", () => Debug.Log("Floor"));
            InstantiateBlockSelectButton("Door", () => Debug.Log("Door"));
            InstantiateBlockSelectButton("Stairs", () => Debug.Log("Stairs"));
            InstantiateBlockSelectButton("Trap", () => Debug.Log("Trap"));
        }

        
        void InstantiateBlockSelectButton(string blockName, UnityAction action)
        {
            var customButton = Instantiate(buttonPrefab, buttonParent);
            customButton.Init();
            customButton.SetText(blockName);
            customButton.AddListener(action);
        }
    }
}