using System;
using Carry.CarrySystem.Map.Scripts;
using UnityEngine;
using VContainer;

namespace Carry.EditMapSystem.EditMap.Scripts
{
    public class EditMapInput : MonoBehaviour
    {
        EditMapManager _editMapManager;
        [Inject]
        public void Construct(EditMapManager editMapManager)
        {
            _editMapManager = editMapManager;
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
        }
    }
}