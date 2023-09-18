using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Carry.EditMapSystem.EditMap.Scripts
{
    public class EditMapInputView : MonoBehaviour
    {
        [SerializeField] EditMapInput editMapInput;
        [SerializeField] TextMeshProUGUI blockTypeText;
        [SerializeField] TextMeshProUGUI directionText;

        void Update()
        {
            blockTypeText.text = $"BlockType : {editMapInput.BlockTypeString}"; 
            directionText.text = $"Direction : {editMapInput.DirectionString}";
        }

    }

}

