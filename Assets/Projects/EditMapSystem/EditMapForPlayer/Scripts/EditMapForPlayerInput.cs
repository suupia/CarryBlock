using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UniRx;
using VContainer;
using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.Block.Scripts;
using Carry.CarrySystem.CarriableBlock.Scripts;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using Carry.EditMapSystem.EditMap.Scripts;
using Carry.Utility.Scripts;
using Projects.CarrySystem.Gimmick.Scripts;
using Projects.CarrySystem.Item.Scripts;


#nullable enable

namespace Carry.EditMapSystem.EditMapForPlayer.Scripts
{
    public class EditMapForPlayerInput : MonoBehaviour
    {
        [SerializeField] EditMapCUISave editMapCuiSave = null!;
        [SerializeField] EditMapCUILoad editMapCuiLoad = null!;

        public string BlockTypeString => _blockType?.Name ?? "(None)";
        public string DirectionString => _direction.ToString();
        Direction _direction = Direction.Up;

        MemorableEditMapBlockAttacher _editMapBlockAttacher = null!;
        IMapGetter _mapGetter = null!;

        CuiState _cuiState = CuiState.Idle;
        Type _blockType = null!;

        Vector2Int _respawnAreaOrigin = new Vector2Int(0, 4);
        readonly int _respawnSize = 3;

        InputAction _leftClickAction = null!;
        InputAction _rightClickAction = null!;
        InputAction _moveAction = null!;

        enum CuiState
        {
            Idle,
            OpenSaveCui,
            OpenLoadCui,
        }

        enum Direction
        {
            Up,
            Left,
            Down,
            Right
        }


        [Inject]
        public void Construct(MemorableEditMapBlockAttacher editMapBlockAttacher, IMapGetter mapGetter)
        {
            _editMapBlockAttacher = editMapBlockAttacher;
            _mapGetter = mapGetter;
        }


        
        void Start()
        {
            this.ObserveEveryValueChanged(_ => editMapCuiSave.IsOpened).Subscribe(isOpened =>
            {
                if (isOpened)
                {
                    _cuiState = CuiState.OpenSaveCui;
                }
                else if (_cuiState == CuiState.OpenSaveCui)
                {
                    _cuiState = CuiState.Idle;
                }
            });
            this.ObserveEveryValueChanged(_ => editMapCuiLoad.IsOpened).Subscribe(isOpened =>
            {
                if (isOpened)
                {
                    _cuiState = CuiState.OpenLoadCui;
                }
                else if (_cuiState == CuiState.OpenLoadCui)
                {
                    _cuiState = CuiState.Idle;
                }
            });

            _blockType = typeof(BasicBlock);

            InputActionMap inputActionMap =
                InputActionMapLoader.GetInputActionMap(InputActionMapLoader.ActionMapName.UI);
            
            _leftClickAction = inputActionMap.FindAction("LeftClick");
            _rightClickAction = inputActionMap.FindAction("RightClick");
            _moveAction = inputActionMap.FindAction("Point");
        }

        void Update()
        {
            var mouseXYPos = _moveAction.ReadValue<Vector2>(); // xy座標であることに注意
            var cameraHeight = Camera.main.transform.position.y;
            Vector3 mousePosOnGround =
                Camera.main.ScreenToWorldPoint(new Vector3(mouseXYPos.x, mouseXYPos.y, cameraHeight));
            Vector2Int mouseGridPosOnGround = GridConverter.WorldPositionToGridPosition(mousePosOnGround);

            if (_leftClickAction.WasPressedThisFrame())
            {
                Debug.Log($"mouseGridPosOnGround : {mouseGridPosOnGround},  mousePosOnGround: {mousePosOnGround}");

                TryToAddPlaceable(mouseGridPosOnGround);
            }

            if (_rightClickAction.WasPerformedThisFrame())
            {
                //上とまとめる
                Debug.Log($"mouseGridPosOnGround : {mouseGridPosOnGround},  mousePosOnGround: {mousePosOnGround}");

                var map = _mapGetter.GetMap();

                _editMapBlockAttacher.RemovePlaceable(map, mouseGridPosOnGround);
            }

            GetInput();
        }

        void TryToAddPlaceable(Vector2Int mouseGridPosOnGround)
        {
            if (IsInsideRespawnArea(mouseGridPosOnGround))
            {
                FindObjectOfType<HopUpCanvasGenerator>().PopMessage("Blocks cannot be placed in respawn areas");
                return;
            }

            var map = _mapGetter.GetMap();
            IPlaceable placeable = _blockType.Name switch
            {
                nameof(BasicBlock) => new BasicBlock(BasicBlock.Kind.Kind1, mouseGridPosOnGround),
                nameof(UnmovableBlock) => new UnmovableBlock(UnmovableBlock.Kind.Kind1, mouseGridPosOnGround),
                nameof(HeavyBlock) => new HeavyBlock(HeavyBlock.Kind.Kind1, mouseGridPosOnGround),
                nameof(FragileBlock) => new FragileBlock(FragileBlock.Kind.Kind1, mouseGridPosOnGround),
                nameof(SpikeGimmick) => new SpikeGimmick(SpikeGimmick.Kind.Kind1, mouseGridPosOnGround),
                nameof(ConfusionBlock) => new ConfusionBlock(ConfusionBlock.Kind.Kind1, mouseGridPosOnGround),
                nameof(CannonBlock) => CreateCannonBlock(),
                nameof(TreasureCoin) => new TreasureCoin(TreasureCoin.Kind.Kind1, mouseGridPosOnGround, map,
                    new TreasureCoinCounter()),
                _ => CreateUnknownBlock(),
            };
            _editMapBlockAttacher.AddPlaceable(map, mouseGridPosOnGround, placeable);

            // local function
            IBlock CreateCannonBlock()
            {
                var kind = _direction switch
                {
                    Direction.Up => CannonBlock.Kind.Up,
                    Direction.Left => CannonBlock.Kind.Left,
                    Direction.Down => CannonBlock.Kind.Down,
                    Direction.Right => CannonBlock.Kind.Right,
                    _ => throw new ArgumentOutOfRangeException(),
                };
                return new CannonBlock(kind, mouseGridPosOnGround);
            }

            IBlock CreateUnknownBlock()
            {
                Debug.LogError($"Unknown block type. _blockType.Name: {_blockType.Name}");
                return null!;
            }
        }

        // ToDo: Input.~~は使用せずにInputActionを使用するようにする
        void GetInput()
        {
            // 方向を切り替える
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                _direction = Direction.Up;
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                _direction = Direction.Left;
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                _direction = Direction.Down;
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                _direction = Direction.Right;
            }

            // Open Save CUI
            if (Input.GetKeyDown(KeyCode.S))
            {
                if (_cuiState == CuiState.Idle) editMapCuiSave.OpenSaveUI();
            }

            // Open Load CUI
            if (Input.GetKeyDown(KeyCode.L))
            {
                if (_cuiState == CuiState.Idle) editMapCuiLoad.OpenLoadUI();
            }
        }

        bool IsInsideRespawnArea(Vector2Int mouseGridPos)
        {
            // 指定した座標がグリッド内にあるかどうかを判定します。
            bool isInsideX = mouseGridPos.x >= _respawnAreaOrigin.x &&
                             mouseGridPos.x < _respawnAreaOrigin.x + _respawnSize;
            bool isInsideY = mouseGridPos.y >= _respawnAreaOrigin.y &&
                             mouseGridPos.y < _respawnAreaOrigin.y + _respawnSize;

            return isInsideX && isInsideY;
        }
    }
}