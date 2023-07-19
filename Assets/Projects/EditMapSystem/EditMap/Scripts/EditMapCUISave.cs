﻿using System;
using System.Net;
using Carry.CarrySystem.Map.Scripts;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using TMPro;
using UniRx;
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
        readonly float _displayTime = 1.5f; // メッセージを表示する時間
        readonly float _autoSaveInterval = 3f; // オートセーブの間隔
        bool _isOpened = false;


        MapKey _key;
        int _index;

        enum CUIInputState
        {
            InputIndex,
            Save,
            DecideOverwrite,
            CannotOverwriteAutoSave,
            OverwriteSave,
            CancelOverwrite,
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
            _key = FindObjectOfType<MapKeyContainer>().MapKey;
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
            Observable.Interval(TimeSpan.FromSeconds(_autoSaveInterval))
                .Subscribe(_ => AutoSave())
                .AddTo(this);
        }

        void Update()
        {
            if (!_isOpened) return;

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
                case CUIInputState.CannotOverwriteAutoSave:
                    CannotOverwriteAutoSaveProcess();
                    break;
                case CUIInputState.DecideOverwrite:
                    DecideOverwriteProcess();
                    break;
                case CUIInputState.OverwriteSave:
                    OverwriteSaveProcess();
                    break;
                case CUIInputState.CancelOverwrite:
                    CancelOverwriteProcess();
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
                if (_index == 0)
                {
                    _inputState = CUIInputState.CannotOverwriteAutoSave;
                }
                else if (EntityGridMapFileUtility.IsExitFile(_key, _index))
                {
                    _inputState = CUIInputState.DecideOverwrite;
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

        async void CannotOverwriteAutoSaveProcess()
        {
            messageText.text = "<color=\"red\">Index 0 is for auto save and cannot be overwritten</color>";
            await UniTask.Delay(TimeSpan.FromSeconds(_displayTime));
            _inputState = CUIInputState.InputIndex;
        }

        void DecideOverwriteProcess()
        {
            messageText.text = "A file with the same index already exists.\nDo you want to overwrite it? (Y/N)";
            if (Input.GetKeyDown(KeyCode.Y))
            {
                _inputState = CUIInputState.OverwriteSave;
            }
            else if (Input.GetKeyDown(KeyCode.N))
            {
                _inputState = CUIInputState.CancelOverwrite;
            }
        }

        async void OverwriteSaveProcess()
        {
            messageText.text = "Overwrite saved.";
            _entityGridMapSaver.SaveMap(_editMapManager.GetMap(), _key, _index);
            await UniTask.Delay(TimeSpan.FromSeconds(_displayTime));
            _inputState = CUIInputState.End;
        }

        async void CancelOverwriteProcess()
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

        void AutoSave()
        {
            // オートセーブはインデックス0に保存する
            if (_key == MapKey.Default ) return; // ToDo: とりあえずMapKeyがデフォルトの時はオートセーブしない
            _entityGridMapSaver.SaveMap(_editMapManager.GetMap(), _key, 0);
        }
    }
}