using System;
using Carry.CarrySystem.Map.Scripts;
using UnityEngine;
using UnityEngine.Serialization;
using VContainer;

namespace Carry.EditMapSystem.EditMap.Scripts
{
    public class EditMapInput : MonoBehaviour
    {
        [FormerlySerializedAs("editMapCUIInput")] [SerializeField] EditMapCUISave editMapCuiSave;
        [SerializeField] EditMapCUILoad editMapCUILoad;
        EditMapManager _editMapManager;
        EntityGridMapSaver _entityGridMapSaver;
        
        [Inject]
        public void Construct(EditMapManager editMapManager, EntityGridMapSaver entityGridMapSaver)
        {
            _editMapManager = editMapManager;
            _entityGridMapSaver = entityGridMapSaver;
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
                editMapCuiSave.OpenSaveUI();
            }
            
            if (Input.GetKeyDown(KeyCode.L))
            {
                Debug.Log($"LoadMap()を実行します");  

                editMapCUILoad.OpenLoadUI();
            }
        }
    }
}