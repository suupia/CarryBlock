using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Users;

#nullable enable

namespace Carry.UISystem.UI.Prefabs
{
    public class MultiInputCursor : MonoBehaviour
    {
        [SerializeField] PlayerInput playerInput = null!;
        [SerializeField] RectTransform cursorTransform = null!;
        [SerializeField] Canvas canvas = null!;

        Mouse? _virtualMouse;
        Mouse _currentMouse = null!;
        bool _previousMouseState = false;
        Camera _mainCamera = null!;
        
        string _previousControlScheme = "";
        readonly string _mouseScheme = "Mouse";
        readonly string _otherScheme = "other";

        readonly float _cursorSpeed = 500.0f;
        readonly float _padding = 50.0f;
        
        private void OnEnable()
        {
            _mainCamera = Camera.main!;
            _currentMouse = Mouse.current;
            
            if (_virtualMouse == null)
            {
                _virtualMouse = (Mouse)InputSystem.AddDevice("VirtualMouse");  
            }
            else if (!_virtualMouse.added)
            {
                InputSystem.AddDevice(_virtualMouse);
            }
            
            InputUser.PerformPairingWithDevice(_virtualMouse,playerInput.user);

            if (cursorTransform != null)
            {
                Vector2 pos = cursorTransform.anchoredPosition;
                InputState.Change(_virtualMouse.position, pos);
            }

            InputSystem.onAfterUpdate += UpdateMotion;
            playerInput.onControlsChanged += OnControlsChanged;
            
            Debug.Log("OnEnable");
        }

        private void OnDisable()
        {
            if(_virtualMouse != null && _virtualMouse.added)InputSystem.RemoveDevice(_virtualMouse);
            
            InputSystem.onAfterUpdate -= UpdateMotion;
            playerInput.onControlsChanged -= OnControlsChanged;
            
            Debug.Log("OnDisable");
        }

        private void UpdateMotion()
        {
            if (_virtualMouse == null)
            {
                Debug.Log("UpdateMotion: _virtualMouse == null");
                return;
            }

            Vector2 deltaValue = Gamepad.current.leftStick.ReadValue(); //todo InputActionで取得するようにする
            deltaValue *= _cursorSpeed * Time.unscaledDeltaTime;
            
            Vector2 currentPos = _virtualMouse.position.ReadValue();
            Vector2 newPos = currentPos + deltaValue;
            
            newPos.x = Mathf.Clamp(newPos.x, _padding, Screen.width - _padding);
            newPos.y = Mathf.Clamp(newPos.y, _padding, Screen.height -_padding);
            
            InputState.Change(_virtualMouse.position, newPos);
            InputState.Change(_virtualMouse.delta, deltaValue);
            cursorTransform.transform.position = newPos;

            bool isPressed = Gamepad.current.aButton.IsPressed();   //todo InputActionで取得するようにする
            if (_previousMouseState != isPressed)
            {
                _virtualMouse.CopyState<MouseState>(out var mouseState);
                mouseState.WithButton(MouseButton.Left, isPressed);
                InputState.Change(_virtualMouse, mouseState);
                _previousMouseState = isPressed;
            }
        }

        void OnControlsChanged(PlayerInput input)
        {
            if(_virtualMouse == null) { return; }
         
            // 入力方法が切り替わった時，カーソルの表示を切り替える．その際に，切り替わるカーソルの位置を現在のカーソルの位置に合わせる．
            if (playerInput.currentControlScheme == _mouseScheme && _previousControlScheme != _mouseScheme)
            {
                Debug.Log("Change to Mouse");
                cursorTransform.gameObject.SetActive(false);
                Cursor.visible = true;
                _currentMouse.WarpCursorPosition(_virtualMouse.position.ReadValue());
                _previousControlScheme = _mouseScheme;
            }
            else if (playerInput.currentControlScheme != _mouseScheme && _previousControlScheme == _mouseScheme)
            {
                Debug.Log("Change to Gamepad or Keyboard");
                cursorTransform.gameObject.SetActive(true);
                Cursor.visible = false;
                InputState.Change(_virtualMouse.position, _currentMouse.position.ReadValue());
                SetVirtualCursorPosByAnchor(_virtualMouse.position.ReadValue());
                _previousControlScheme = _otherScheme;
            }
        }
        
        private void SetVirtualCursorPosByAnchor(Vector2 pos)
        {
            Vector2 anchoredPosition = pos;
            
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.GetComponent<RectTransform>(),
                pos,
                canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : _mainCamera,
                out anchoredPosition
            );
            
            cursorTransform.anchoredPosition = anchoredPosition;
        }
    }    
}