using System;
using Carry.CarrySystem.Map.Scripts;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using VContainer;

namespace Carry.EditMapSystem.EditMap.Scripts
{
    public class EditMapCUILoad : MonoBehaviour
    {
        [SerializeField] GameObject CUILoadCanvas;
        [SerializeField] TextMeshProUGUI messageText;
        [SerializeField] TextMeshProUGUI inputText;
        
        public bool IsOpened => _isOpened;

        EditMapManager _editMapManager;
        CUIHandleNumber _handleNumber;
        CUIInputState _inputState;


        readonly float _displayTime = 1.5f; // メッセージを表示する時間
        bool _isOpened = false;

        MapKey _key;
        int _index;
        

        enum CUIInputState
        {
            InputIndex,
            Load,
            NotExist,
            End,
        }
        
        
        [Inject]
        public void Construct(
            EditMapManager editMapManager,
            CUIHandleNumber handleNumber)
        {
            _editMapManager = editMapManager;
            _handleNumber = handleNumber;
        }
        public void OpenLoadUI()
        {
            CUILoadCanvas.SetActive(true);
            _inputState = CUIInputState.InputIndex;
            _key =  FindObjectOfType<MapKeyContainer>().MapKey;
            _index = 0;
            _isOpened = true;
        }
        
        void CloseLoadUI()
        {
            CUILoadCanvas.SetActive(false);
            _isOpened = false;
        }
        
        void Start()
        {
            CloseLoadUI();
        }

        void Update()
        {
            if(!_isOpened) return;

            // Escキーでどのような時でも中断する
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CloseLoadUI();
            }

            switch (_inputState)
            {
                case CUIInputState.InputIndex:
                    InputIndexProcess();
                    break;
                case CUIInputState.Load:
                    LoadProcess();
                    break;
                case CUIInputState.NotExist:
                    NotExistProcess();
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
            messageText.text = "Enter the index of the file to be loaded and press Enter.";
            inputText.text = _index.ToString();
            _index = _handleNumber. HandleNumberInput(_index);
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                if (EntityGridMapFileUtility.IsExitFile(_key, _index))
                {
                    _inputState = CUIInputState.Load;
                }
                else
                {
                    _inputState = CUIInputState.NotExist;
                }
            }
        }

        async void LoadProcess()
        {
            messageText.text = "Loaded.";
            _editMapManager.UpdateMap(_key,_index);
            await UniTask.Delay(TimeSpan.FromSeconds(_displayTime));
            _inputState = CUIInputState.End;
        }

        async void NotExistProcess()
        {
            messageText.text = "The file does not exist.";
            await UniTask.Delay(TimeSpan.FromSeconds(_displayTime));
            _inputState = CUIInputState.End;
        }
        
        void EndProcess()
        {
            CloseLoadUI();
        }
    }
}