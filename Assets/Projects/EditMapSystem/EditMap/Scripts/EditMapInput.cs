using System;
using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.Block.Scripts;
using Carry.CarrySystem.Entity.Scripts;
using Carry.CarrySystem.Map.Interfaces;
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

        BlockPlacer _blockPlacer;
        IMapUpdater _editMapUpdater;
        EntityGridMapSaver _entityGridMapSaver;

        CUIState _cuiState = CUIState.Idle;
        IBlock _blockType;

        enum CUIState
        {
            Idle,
            OpenSaveCUI,
            OpenLoadCUI,
        }


        [Inject]
        public void Construct(BlockPlacer blockPlacer, IMapUpdater editMapUpdater)
        {
            _blockPlacer = blockPlacer;
            _editMapUpdater = editMapUpdater;
        }

        void Start()
        {
            this.ObserveEveryValueChanged(_ => editMapCuiSave.IsOpened).Subscribe(isOpened =>
            {
                if (isOpened)
                {
                    _cuiState = CUIState.OpenSaveCUI;
                }
                else if (_cuiState == CUIState.OpenSaveCUI)
                {
                    _cuiState = CUIState.Idle;
                }
            });
            this.ObserveEveryValueChanged(_ => editMapCUILoad.IsOpened).Subscribe(isOpened =>
            {
                if (isOpened)
                {
                    _cuiState = CUIState.OpenLoadCUI;
                }
                else if (_cuiState == CUIState.OpenLoadCUI)
                {
                    _cuiState = CUIState.Idle;
                }
            });
        }

        void Update()
        {
            var mouseXYPos = Input.mousePosition; // xy座標であることに注意
            var cameraHeight = Camera.main.transform.position.y;
            var mousePosOnGround =
                Camera.main.ScreenToWorldPoint(new Vector3(mouseXYPos.x, mouseXYPos.y, cameraHeight));

            if (Input.GetMouseButtonDown(0))
            {
                var mouseGridPosOnGround = GridConverter.WorldPositionToGridPosition(mousePosOnGround);
                Debug.Log($"mouseGridPosOnGround : {mouseGridPosOnGround},  mousePosOnGround: {mousePosOnGround}");

                var map = _editMapUpdater.GetMap();
                
                IBlock block = _blockType switch
                {
                    BasicBlock _ => new BasicBlock(BasicBlock.Kind.Kind1, mouseGridPosOnGround),
                    UnmovableBlock _ => new UnmovableBlock(UnmovableBlock.Kind.Kind1, mouseGridPosOnGround),
                    HeavyBlock _ => new HeavyBlock(HeavyBlock.Kind.Kind1, mouseGridPosOnGround),
                    FragileBlock _ => new FragileBlock(FragileBlock.Kind.Kind1, mouseGridPosOnGround),
                    _ =>  throw new ArgumentOutOfRangeException(nameof(_blockType), _blockType, null),
                };
                _blockPlacer.AddBlock(map, mouseGridPosOnGround, block);
            }

            if (Input.GetMouseButtonDown(1))
            {
                var mouseGridPosOnGround = GridConverter.WorldPositionToGridPosition(mousePosOnGround);
                Debug.Log($"mouseGridPosOnGround : {mouseGridPosOnGround},  mousePosOnGround: {mousePosOnGround}");

                var map = _editMapUpdater.GetMap();
                
                Action action = _blockType switch
                {
                    BasicBlock _ => () => _blockPlacer.RemoveBlock<BasicBlock>(map, mouseGridPosOnGround),
                    UnmovableBlock _ => () => _blockPlacer.RemoveBlock<UnmovableBlock>(map, mouseGridPosOnGround),
                    HeavyBlock _ => () => _blockPlacer.RemoveBlock<HeavyBlock>(map, mouseGridPosOnGround),
                    FragileBlock _ => () => _blockPlacer.RemoveBlock<FragileBlock>(map, mouseGridPosOnGround),
                    _ =>  throw new ArgumentOutOfRangeException(nameof(_blockType), _blockType, null),
                };
                action();
            }

            // 置くブロックを切り替える
            if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
            {
                _blockType = new BasicBlock(BasicBlock.Kind.Kind1, Vector2Int.zero);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
            {
                _blockType = new UnmovableBlock(UnmovableBlock.Kind.Kind1, Vector2Int.zero);
            }

            if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
            {
                _blockType = new HeavyBlock(HeavyBlock.Kind.Kind1, Vector2Int.zero);
            }
            
            if(Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
            {
                _blockType = new FragileBlock(FragileBlock.Kind.Kind1, Vector2Int.zero);
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                if (_cuiState == CUIState.Idle) editMapCuiSave.OpenSaveUI();
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                if (_cuiState == CUIState.Idle) editMapCUILoad.OpenLoadUI();
            }
        }
    }
}