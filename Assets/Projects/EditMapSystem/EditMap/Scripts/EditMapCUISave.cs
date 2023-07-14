using System;
using System.Net;
using Carry.CarrySystem.Map.Scripts;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using TMPro;
using UnityEngine.Serialization;

namespace Carry.EditMapSystem.EditMap.Scripts
{
    public class EditMapCUISave : MonoBehaviour
    {
        [SerializeField] GameObject CUISaveCanvas;
        [SerializeField] TextMeshProUGUI messageText;
        [SerializeField] TextMeshProUGUI inputText;
        
        public bool IsOpened => _isOpened;
        
        EditMapManager _editMapManager;
        EntityGridMapSaver _entityGridMapSaver;
        CUIHandleNumber _handleNumber;
        CUIInputState _inputState;

        readonly int _maxDigit = 10; // インデックスの最大の桁数
        readonly float _displayTime = 2.0f; // メッセージを表示する時間
        bool _isOpened = false;


        MapKey _key;
        int _index;

        enum CUIInputState
        {
            InputIndex,
            Save,
            DecideOverride,
            OverrideSave,
            CancelOverride,
            Cancel,
            End,
        }

        [Inject]
        public void Construct(
            EditMapManager editMapManager,
            EntityGridMapSaver entityGridMapSaver,
            CUIHandleNumber handleNumber)
        {
            _editMapManager = editMapManager;
            _entityGridMapSaver = entityGridMapSaver;
            _handleNumber = handleNumber;
        }

        public void OpenSaveUI()
        {
            CUISaveCanvas.SetActive(true);
            _inputState = CUIInputState.InputIndex;
            _key = _editMapManager.MapKey;
            _index = 0;
            _isOpened = true;
        }

        void CloseSaveUI()
        {
            CUISaveCanvas.SetActive(false);
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
                    InputIndexProcess();
                    break;
                case CUIInputState.Save:
                    SaveProcess();
                    break;
                case CUIInputState.DecideOverride:
                    DecideOverrideProcess();
                    break;
                case CUIInputState.OverrideSave:
                    OverrideSaveProcess();
                    break;
                case CUIInputState.CancelOverride:
                    CancelOverrideProcess();
                    break;
                case CUIInputState.Cancel:
                    CancelProcess();
                    break;
                case CUIInputState.End:
                    EndProcess();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        void InputIndexProcess()
        {
            messageText.text = "Please enter the index of the file and press Enter.";
            inputText.text = _index.ToString();
            _index = _handleNumber.HandleNumberInput(_index);
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                if (EntityGridMapFileUtility.IsExitFile(_key, _index))
                {
                    _inputState = CUIInputState.DecideOverride;
                }
                else
                {
                    _inputState = CUIInputState.Save;
                }
            }
        }

        async void SaveProcess()
        {
            messageText.text = "Saved in.";
            _entityGridMapSaver.SaveMap(_editMapManager.GetMap(), _key, _index);
            await UniTask.Delay(TimeSpan.FromSeconds(_displayTime));
            _inputState = CUIInputState.End;
        }

        void DecideOverrideProcess()
        {
            messageText.text = "A file with the same index already exists. Do you want to overwrite it? (Y/N)";
            if (Input.GetKeyDown(KeyCode.Y))
            {
                _inputState = CUIInputState.OverrideSave;
            }
            else if (Input.GetKeyDown(KeyCode.N))
            {
                _inputState = CUIInputState.CancelOverride;
            }
        }

        async void OverrideSaveProcess()
        {
            messageText.text = "Overwrite saved.";
            _entityGridMapSaver.SaveMap(_editMapManager.GetMap(), _key, _index);
            await UniTask.Delay(TimeSpan.FromSeconds(_displayTime));
            _inputState = CUIInputState.End;
        }

        async void CancelOverrideProcess()
        {
            messageText.text = "Overwrite cancelled.";
            await UniTask.Delay(TimeSpan.FromSeconds(_displayTime));
            _inputState = CUIInputState.End;
        }

        async void CancelProcess()
        {
            messageText.text = "Operation cancelled.";
            await UniTask.Delay(TimeSpan.FromSeconds(_displayTime));
            _inputState = CUIInputState.End;
        }

        void EndProcess()
        {
            CloseSaveUI();
        }
    }
}