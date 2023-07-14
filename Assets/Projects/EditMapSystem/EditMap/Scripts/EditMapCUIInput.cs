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
        [SerializeField] TextMeshProUGUI messageText;
        [SerializeField] TextMeshProUGUI inputText;
        EditMapManager _editMapManager;
        EntityGridMapSaver _entityGridMapSaver;
        CUIInputState _inputState;

        readonly int _maxDigit = 10; // インデックスの最大の桁数
        bool _isOpened = false;
        int _index = 0;

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

        public void OpenSaveUI()
        {
            CUICanvas.SetActive(true);
            _isOpened = true;
        }

        public void CloseSaveUI()
        {
            CUICanvas.SetActive(false);
            _isOpened = false;
        }

        void Start()
        {
            CloseSaveUI();
        }

        void Update()
        {
            // Escキーでどのような時でも中断する
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CloseSaveUI();
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
            messageText.text = "Please enter the index of the file. (Enter)";
            inputText.text = _index.ToString();
            HandleNumberInput();
        }

        void SaveDone()
        {
            messageText.text = "Saved in.";
        }

        void DecideOverride()
        {
            messageText.text = "Do you want to overwrite the file because it has the same name? (Y/N)";
        }

        void OverrideSaveDone()
        {
            messageText.text = "Overwrite saved.";
        }

        void CancelOverride()
        {
            messageText.text = "Save Overwrite canceled.";
        }

        void Cancel()
        {
            messageText.text = "Canceled";
        }

        void HandleNumberInput()
        {
            for (int i = 0; i <= 9; i++)
            {
                if (Input.GetKeyDown(KeyCode.Alpha0 + i) || Input.GetKeyDown(KeyCode.Keypad0 + i))
                {
                    if(_index.ToString().Length < _maxDigit) _index = _index * 10 + i;
                }
            }

            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                _index = _index / 10;
            }
        }
    }
}