using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;
using Carry.Utility.Scripts;

#nullable enable

namespace Carry.UISystem.UI.Prefabs
{
    public class MultiInputCursor : MonoBehaviour
    {
        [SerializeField] PlayerInput playerInput = null!;
        [SerializeField] RectTransform cursorTransform = null!;
        [SerializeField] Canvas canvas = null!;

        readonly string _mouseScheme = "Mouse";
        readonly string _otherScheme = "Other";
        readonly float _cursorSpeed = 500.0f;
        readonly float _padding = 50.0f;
        
        Mouse? _virtualMouse;
        Mouse _currentMouse = null!;
        Camera _mainCamera = null!;
        InputAction _cursorAction = null!;
        InputAction _selectAction = null!;
        bool _previousMouseState = false;
        string _previousControlScheme = "Mouse";
        
        
        private void OnEnable()
        {
            _mainCamera = Camera.main!;
            _currentMouse = Mouse.current;
            
            // 仮想マウスを作成し，InputSystemに追加する
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

            // InputActionを取得する
            InputActionMap inputActionMap = InputActionMapLoader.GetInputActionMap(InputActionMapLoader.ActionMapName.UI);
            _cursorAction = inputActionMap.FindAction("MoveCursor");
            _selectAction = inputActionMap.FindAction("Select");

            Debug.Log("OnEnable");
        }

        private void OnDisable()
        {
            // 必ず追加したデバイスを削除すること
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

            // 仮想マウスの位置を更新する
            Vector2 deltaValue = _cursorAction.ReadValue<Vector2>();
            deltaValue *= _cursorSpeed * Time.unscaledDeltaTime;
            
            Vector2 currentPos = _virtualMouse.position.ReadValue();
            Vector2 newPos = currentPos + deltaValue;
            
            newPos.x = Mathf.Clamp(newPos.x, _padding, Screen.width - _padding);
            newPos.y = Mathf.Clamp(newPos.y, _padding, Screen.height -_padding);
            
            InputState.Change(_virtualMouse.position, newPos);
            InputState.Change(_virtualMouse.delta, deltaValue);
            cursorTransform.transform.position = newPos;

            // 仮想マウスのボタン(クリック)の状態を更新する
            bool isPressed = _selectAction.IsPressed();
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