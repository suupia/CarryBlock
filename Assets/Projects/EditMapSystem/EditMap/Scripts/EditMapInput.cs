using System;
using Carry.CarrySystem.Map.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using VContainer;
using UniRx;

namespace Carry.EditMapSystem.EditMap.Scripts
{
    public class EditMapInput : MonoBehaviour
    {
        [SerializeField] EditMapCUISave editMapCuiSave;
        [SerializeField] EditMapCUILoad editMapCUILoad;
        [SerializeField] TextMeshProUGUI loadedFileText;

        EditMapManager _editMapManager;
        EntityGridMapSaver _entityGridMapSaver;
        
        CUIState _cuiState = CUIState.Idle;

        enum CUIState
        {
            Idle,
            OpenSaveCUI,
            OpenLoadCUI,
        }
        
        [Inject]
        public void Construct(EditMapManager editMapManager)
        {
            _editMapManager = editMapManager;
        }

        void Start()
        {
           this.ObserveEveryValueChanged(_ =>  editMapCuiSave.IsOpened).Subscribe(isOpened =>
           {
               if (isOpened)
               {
                   _cuiState = CUIState.OpenSaveCUI;
               }else if (_cuiState == CUIState.OpenSaveCUI)
               {
                   _cuiState = CUIState.Idle;
               }
           });
           this.ObserveEveryValueChanged(_ => editMapCUILoad.IsOpened).Subscribe(isOpened =>
           {
               if (isOpened)
               {
                   _cuiState = CUIState.OpenLoadCUI;
               }else if (_cuiState == CUIState.OpenLoadCUI)
               {
                   _cuiState = CUIState.Idle;
               }
           });
           this.ObserveEveryValueChanged(_ => _editMapManager.MapKey).Subscribe(key =>
           {
                FormatLoadedFileText(key, _editMapManager.Index);
           });
           this.ObserveEveryValueChanged(_ => _editMapManager.Index).Subscribe(index =>
           {
               FormatLoadedFileText(_editMapManager.MapKey,index);
           });
           
        }

        void Update()
        {
            var mouseXYPos = Input.mousePosition; // xy座標であることに注意
            var cameraHeight = Camera.main.transform.position.y;
            var mousePosOnGround = Camera.main.ScreenToWorldPoint(new Vector3(mouseXYPos.x, mouseXYPos.y, cameraHeight));
            
            if (Input.GetMouseButtonDown(0))
            {
                var mouseGridPosOnGround = GridConverter.WorldPositionToGridPosition(mousePosOnGround);
                Debug.Log($"mouseGridPosOnGround : {mouseGridPosOnGround},  mousePosOnGround: {mousePosOnGround}");
                
                // とりあえずRockを足す
                _editMapManager.AddRock(mouseGridPosOnGround);
            }
            
            if (Input.GetMouseButtonDown(1))
            {
                var mouseGridPosOnGround = GridConverter.WorldPositionToGridPosition(mousePosOnGround);
                Debug.Log($"mouseGridPosOnGround : {mouseGridPosOnGround},  mousePosOnGround: {mousePosOnGround}");
                
                // とりあえずRockを消す
                _editMapManager.RemoveRock(mouseGridPosOnGround);
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
               if(_cuiState == CUIState.Idle) editMapCuiSave.OpenSaveUI();
            }
            
            if (Input.GetKeyDown(KeyCode.L))
            {
                if(_cuiState == CUIState.Idle) editMapCUILoad.OpenLoadUI();
            }
        }
        
        void  FormatLoadedFileText(MapKey mapKey, int index)
        {
            loadedFileText.text = $"Loaded File: {mapKey}_{index}";
        }
    }
}