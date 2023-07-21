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

        EditMapUpdater _editMapUpdater;
        EntityGridMapSaver _entityGridMapSaver;
        
        CUIState _cuiState = CUIState.Idle;
        EntityType _entityType;

        enum CUIState
        {
            Idle,
            OpenSaveCUI,
            OpenLoadCUI,
        }
        
        enum EntityType
        {
            BasicBlock,
            Rock,
        }
        
        [Inject]
        public void Construct(EditMapUpdater editMapUpdater)
        {
            _editMapUpdater = editMapUpdater;
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
           this.ObserveEveryValueChanged(_ => _editMapUpdater.MapKey).Subscribe(key =>
           {
                FormatLoadedFileText(key, _editMapUpdater.Index);
           });
           this.ObserveEveryValueChanged(_ => _editMapUpdater.Index).Subscribe(index =>
           {
               FormatLoadedFileText(_editMapUpdater.MapKey,index);
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
                
                 switch (_entityType)
                 {
                     case EntityType.BasicBlock:
                         _editMapUpdater.AddBasicBlock(mouseGridPosOnGround);
                         break;
                     case EntityType.Rock:
                         _editMapUpdater.AddRock(mouseGridPosOnGround);
                         break;
                 }
            }
            
            if (Input.GetMouseButtonDown(1))
            {
                var mouseGridPosOnGround = GridConverter.WorldPositionToGridPosition(mousePosOnGround);
                Debug.Log($"mouseGridPosOnGround : {mouseGridPosOnGround},  mousePosOnGround: {mousePosOnGround}");
                
                switch (_entityType)
                {
                    case EntityType.BasicBlock:
                        _editMapUpdater.RemoveBasicBlock(mouseGridPosOnGround);
                        break;
                    case EntityType.Rock:
                        _editMapUpdater.RemoveRock(mouseGridPosOnGround);
                        break;
                }
            }
            
            if(Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
            {
                _entityType = EntityType.BasicBlock;
            }
            if(Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
            {
                _entityType = EntityType.Rock;
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