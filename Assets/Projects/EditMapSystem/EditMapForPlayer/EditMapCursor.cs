using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Carry.Utility.Scripts;
using UnityEngine.Assertions;

#nullable enable

namespace Carry.EditMapSystem.EDitMapForPlayer
{
    public class EditMapCursor : MonoBehaviour
    {
        [SerializeField] private Image cursorImage = null!;
        
        InputActionMap? _inputActionMap;
        InputAction _moveCursorAction = null!;
        //InputAction _clickCursorAction = null!;

        void Start()
        {
            var loader =
                new ScriptableObjectLoaderFromAddressable<InputActionAsset>("InputActionAssets/EditMapInputAction");

            (var inputActionAsset, var handler) = loader.Load();
            Assert.IsNotNull(inputActionAsset, "InputActionを設定してください。Pathが間違っている可能性があります");

            _inputActionMap = inputActionAsset.FindActionMap("UI");
            Assert.IsNotNull(_inputActionMap, "FindActionMap()の引数が間違っている可能性があります");

            loader.Release(handler);

            _moveCursorAction = _inputActionMap.FindAction("MoveCursor");

            _inputActionMap.Enable();
            //_clickCursorAction = _inputActionMap.FindAction("ClickCursor");
        }

        void Update()
        {
            //インプットアクションからカーソルの移動量を取得
            Vector2 moveVector = _moveCursorAction.ReadValue<Vector2>();

            // マウスカーソルの移動量を取得
            float mouseInputX = Input.GetAxis("Mouse X");
            float mouseInputY = Input.GetAxis("Mouse Y");
            Vector2 mouseMoveVector = new Vector2(mouseInputX, mouseInputY);
            
            //移動量の比較を行う
            if (mouseMoveVector != Vector2.zero)
            {
                Vector3 mousePosition = Input.mousePosition;
            
                cursorImage.transform.position = mousePosition;
            }
            else if (moveVector != Vector2.zero)
            {
                //インプットアクションの移動量を加算
                cursorImage.transform.position += (Vector3)moveVector;
            }
        }
    }
}