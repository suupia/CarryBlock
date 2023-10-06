using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Carry.Utility.Scripts;

#nullable enable

namespace Carry.EditMapSystem.EditMap.Scripts
{
    public class EditMapCursor : MonoBehaviour
    {
        [SerializeField] private Image cursorImage = null!;
        
        InputAction _moveCursorAction = null!;
        InputAction _clickCursorAction = null!;
        
        void Start()
        {
            var inputActionMap = InputActionMapLoader.GetInputActionMap();
        }
        
        enum CursorType
        {
            Mouse,
            GamePad,
        }
        
        void Update()
        {
            // マウスカーソルの移動量を取得
            float mouseInputX = Input.GetAxis("Mouse X");
            float mouseInputY = Input.GetAxis("Mouse Y");
            
            //マウスカーソルの位置を取得
            Vector3 mousePosition = Input.mousePosition;
            
            cursorImage.transform.position = mousePosition;
        }
    }

}