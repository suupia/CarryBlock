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
#nullable enable

namespace Carry.EditMapSystem.EditMap.Scripts
{
    public class EditMapInput : MonoBehaviour
    {
        [SerializeField] EditMapCUISave editMapCuiSave = null!;
        [SerializeField] EditMapCUILoad editMapCUILoad = null!;

        public  Type BlockType => _blockType;
        
        Direction _direction = Direction.Up; 

        BlockPlacer _blockPlacer = null!;
        IMapUpdater _editMapUpdater = null!;

        CUIState _cuiState = CUIState.Idle;
        Type _blockType = null!;

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

            _blockType = typeof(BasicBlock);

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
                
                IBlock block = _blockType.Name switch
                {
                    nameof(BasicBlock)  => new BasicBlock(BasicBlock.Kind.Kind1, mouseGridPosOnGround),
                    nameof(UnmovableBlock)  => new UnmovableBlock(UnmovableBlock.Kind.Kind1, mouseGridPosOnGround),
                    nameof(HeavyBlock)  => new HeavyBlock(HeavyBlock.Kind.Kind1, mouseGridPosOnGround),
                    nameof(FragileBlock) => new FragileBlock(FragileBlock.Kind.Kind1, mouseGridPosOnGround),
                    nameof(CannonBlock) => new CannonBlock(CannonBlock.Kind.Left, mouseGridPosOnGround),
                    _ => ((Func<IBlock>)(() => 
                    {
                        Debug.LogError($"Unknown block type. _blockType.Name: {_blockType.Name}");
                        return null!;
                    }))(),
                };
                _blockPlacer.AddBlock(map, mouseGridPosOnGround, block);
            }

            if (Input.GetMouseButtonDown(1))
            {
                var mouseGridPosOnGround = GridConverter.WorldPositionToGridPosition(mousePosOnGround);
                Debug.Log($"mouseGridPosOnGround : {mouseGridPosOnGround},  mousePosOnGround: {mousePosOnGround}");

                var map = _editMapUpdater.GetMap();
                
                (_blockType.Name switch
                {
                    nameof(BasicBlock) => () => _blockPlacer.RemoveBlock<BasicBlock>(map, mouseGridPosOnGround),
                    nameof(UnmovableBlock) => () => _blockPlacer.RemoveBlock<UnmovableBlock>(map, mouseGridPosOnGround),
                    nameof(HeavyBlock) => () => _blockPlacer.RemoveBlock<HeavyBlock>(map, mouseGridPosOnGround),
                    nameof(FragileBlock) => () => _blockPlacer.RemoveBlock<FragileBlock>(map, mouseGridPosOnGround),
                    nameof(CannonBlock) => () => _blockPlacer.RemoveBlock<CannonBlock>(map, mouseGridPosOnGround),
                    _ => (Action)(() => Debug.LogError($"Unknown block type. _blockType.Name: {_blockType.Name}") ),
                })();
            }

            // 置くブロックを切り替える
            if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
            {
                _blockType = typeof(BasicBlock);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
            {
                _blockType = typeof(UnmovableBlock);
            }

            if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
            {
                _blockType = typeof(HeavyBlock);
            }
            
            if(Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
            {
                _blockType = typeof(FragileBlock);
            }
            
            if(Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5))
            {
                _blockType =typeof(CannonBlock);
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

        enum Direction
        {
            Up,
            Left,
            Down,
            Right
        }
    }
}