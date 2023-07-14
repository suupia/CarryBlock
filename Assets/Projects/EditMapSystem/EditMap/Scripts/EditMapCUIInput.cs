using System;
using Carry.CarrySystem.Map.Scripts;
using UnityEngine;
using VContainer;
using TMPro;
using UnityEngine.Serialization;

namespace Carry.EditMapSystem.EditMap.Scripts
{
    public class EditMapCUIInput : MonoBehaviour
    {
        [SerializeField] GameObject CUICanvas;
        TextMeshProUGUI messageText;
        EditMapManager _editMapManager;
        EntityGridMapSaver _entityGridMapSaver;
        CUIInputState _inputState;
        enum CUIInputState
        {
            InputIndex,
            SaveDone,
            DecideOverride,
            OverrideSaveDone,
            CancelOverride,
            Cancel,
        }
        [Inject]
        public void Construct(EditMapManager editMapManager, EntityGridMapSaver entityGridMapSaver)
        {
            _editMapManager = editMapManager;
            _entityGridMapSaver = entityGridMapSaver;
        }

        void OnEnable()
        {
            CUICanvas.SetActive(true);
        }

        void OnDisable()
        {
            CUICanvas.SetActive(false);
        }

        void Update()
        {
            // Escキーでどのような時でも中断する
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                gameObject.SetActive(false);
            }
            
            switch (_inputState)
            {
                case CUIInputState.InputIndex:
                    InputIndex();
                    break;
                case CUIInputState.SaveDone:
                    SaveDone();
                    break;
                case CUIInputState.DecideOverride:
                    DecideOverride();
                    break;
                case CUIInputState.OverrideSaveDone:
                    OverrideSaveDone();
                    break;
                case CUIInputState.CancelOverride:
                    CancelOverride();
                    break;
                case CUIInputState.Cancel:
                    Cancel();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        void InputIndex()
        {
            messageText.text = "ファイルのインデックスを入力してください";
        }
        void SaveDone()
        {
            messageText.text = "保存しました";
        }
        void DecideOverride()
        {
            messageText.text = "同一名のファイルが存在します\n上書きしますか？";
        }
        void OverrideSaveDone()
        {
            messageText.text = "上書き保存しました";
        }
        void CancelOverride()
        {
            messageText.text = "上書き保存をキャンセルしました";
        }
        void Cancel()
        {
            messageText.text = "キャンセルしました";
        }
    }
}