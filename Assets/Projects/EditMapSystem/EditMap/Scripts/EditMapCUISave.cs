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
        [SerializeField] GameObject CUICanvas;
        [SerializeField] TextMeshProUGUI messageText;
        [SerializeField] TextMeshProUGUI inputText;
        EditMapManager _editMapManager;
        EntityGridMapSaver _entityGridMapSaver;
        CUIInputState _inputState;

        readonly int _maxDigit = 10; // インデックスの最大の桁数
        readonly float _displayTime = 2.0f; // メッセージを表示する時間
        bool _isOpened = false;
        int _index = 0;

        MapKey _key = MapKey.Morita; // ToDo: とりあえずKokiで固定
        

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
        public void Construct(EditMapManager editMapManager, EntityGridMapSaver entityGridMapSaver)
        {
            _editMapManager = editMapManager;
            _entityGridMapSaver = entityGridMapSaver;
        }

        public void OpenSaveUI()
        {
            CUICanvas.SetActive(true);
            _inputState = CUIInputState.InputIndex;
            _index = 0;
            _isOpened = true;
        }

        void CloseSaveUI()
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
            HandleNumberInput();
        }

        async void SaveProcess()
        {
            messageText.text = "Saved in.";
            _entityGridMapSaver.SaveMap( _editMapManager.GetMap(), _key, _index);
            await UniTask.Delay(TimeSpan.FromSeconds(_displayTime));
            _inputState = CUIInputState.End;
        }

        void DecideOverrideProcess()
        {
            messageText.text = "A file with the same index already exists. Do you want to overwrite it? (Y/N)";
            if (Input.GetKeyDown(KeyCode.Y))
            {
                _inputState = CUIInputState.OverrideSave;
            }else if (Input.GetKeyDown(KeyCode.N))
            {
                _inputState = CUIInputState.CancelOverride;
            }
        }

        async void OverrideSaveProcess()
        {
            messageText.text = "Overwrite saved.";
            _entityGridMapSaver.SaveMap( _editMapManager.GetMap(), _key, _index);
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

            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                if (EntityGridMapFileUtility.IsExitFile(MapKey.Morita, _index))
                {
                    _inputState = CUIInputState.DecideOverride;
                }
                else
                {
                    _inputState = CUIInputState.Save;
                }
            }
        }
    }
}