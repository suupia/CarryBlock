using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Carry.EditMapSystem.EditMap.Scripts
{
    public class EditMapInputView : MonoBehaviour
    {
        [SerializeField]
        EditMapInput editMapInput;
        [SerializeField] TMPro.TextMeshProUGUI blockTypeText;

        void Update()
        {
            blockTypeText.text = $"BlockType : {editMapInput.BlockType.ToString()}"; 
        }

    }

}

