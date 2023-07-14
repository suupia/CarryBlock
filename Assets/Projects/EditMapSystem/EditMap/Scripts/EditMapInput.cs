using System;
using Carry.CarrySystem.Map.Scripts;
using UnityEngine;
using VContainer;

namespace Carry.EditMapSystem.EditMap.Scripts
{
    public class EditMapInput : MonoBehaviour
    {
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
                var clickGridPos = GridConverter.WorldPositionToGridPosition(mousePosOnGround);
                Debug.Log($"mousePosOnGround : {mousePosOnGround},  clickGridPos: {clickGridPos}");
                
                // とりあえずRockを足す
                Debug.Log($"_editMapManager : {_editMapManager}");
                _editMapManager.AddRock(clickGridPos);
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                Debug.Log($"Saveします");  
                var key = MapKey.Default;
                var index = 11;
                _entityGridMapSaver.SaveMap(_editMapManager.GetMap(),key,index );
            }
        }
    }
}