using System;
using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.Block.Scripts;
using Carry.CarrySystem.CarriableBlock.Scripts;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using Carry.EditMapSystem.EditMap.Scripts;
using Projects.CarrySystem.Gimmick.Scripts;
using Projects.CarrySystem.Item.Scripts;
using UnityEngine;
using VContainer;
using UniRx;

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

        EditMapBlockAttacher _editMapBlockAttacher = null!;
        IMapUpdater _editMapUpdater = null!;

        CuiState _cuiState = CuiState.Idle;
        Type _blockType = null!;

        Vector2Int _respawnAreaOrigin = new Vector2Int(0, 4);
        int _respawnSize = 3;

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
        public void Construct(EditMapBlockAttacher editMapBlockAttacher, IMapUpdater editMapUpdater)
        {
            _editMapBlockAttacher = editMapBlockAttacher;
            _editMapUpdater = editMapUpdater;
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

                TryToAddPlaceable(mouseGridPosOnGround);
            }

            if (Input.GetMouseButtonDown(1))
            {
                var mouseGridPosOnGround = GridConverter.WorldPositionToGridPosition(mousePosOnGround);
                Debug.Log($"mouseGridPosOnGround : {mouseGridPosOnGround},  mousePosOnGround: {mousePosOnGround}");

                TryToRemovePlaceable(mouseGridPosOnGround);
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

            var map = _editMapUpdater.GetMap();
            IPlaceable placeable = _blockType.Name switch
            {
                nameof(BasicBlock) => new BasicBlock(BasicBlock.Kind.Kind1, mouseGridPosOnGround),
                nameof(UnmovableBlock) => new UnmovableBlock(UnmovableBlock.Kind.Kind1, mouseGridPosOnGround),
                nameof(HeavyBlock) => new HeavyBlock(HeavyBlock.Kind.Kind1, mouseGridPosOnGround),
                nameof(FragileBlock) => new FragileBlock(FragileBlock.Kind.Kind1, mouseGridPosOnGround),
                nameof(Spike) => new Spike(Spike.Kind.Kind1, mouseGridPosOnGround),
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

        void TryToRemovePlaceable(Vector2Int mouseGridPosOnGround)
        {
            var map = _editMapUpdater.GetMap();

            (_blockType.Name switch
            {
                nameof(BasicBlock) => () =>
                    _editMapBlockAttacher.RemovePlaceable<BasicBlock>(map, mouseGridPosOnGround),
                nameof(UnmovableBlock) => () =>
                    _editMapBlockAttacher.RemovePlaceable<UnmovableBlock>(map, mouseGridPosOnGround),
                nameof(HeavyBlock) => () =>
                    _editMapBlockAttacher.RemovePlaceable<HeavyBlock>(map, mouseGridPosOnGround),
                nameof(FragileBlock) => () =>
                    _editMapBlockAttacher.RemovePlaceable<FragileBlock>(map, mouseGridPosOnGround),
                nameof(ConfusionBlock) => () =>
                    _editMapBlockAttacher.RemovePlaceable<ConfusionBlock>(map, mouseGridPosOnGround),
                nameof(CannonBlock) => () =>
                    _editMapBlockAttacher.RemovePlaceable<CannonBlock>(map, mouseGridPosOnGround),
                nameof(TreasureCoin) => () =>
                    _editMapBlockAttacher.RemovePlaceable<TreasureCoin>(map, mouseGridPosOnGround),
                nameof(Spike) => () => _editMapBlockAttacher.RemovePlaceable<Spike>(map, mouseGridPosOnGround),
                _ => (Action)(() => Debug.LogError($"Unknown block type. _blockType.Name: {_blockType.Name}")),
            })();
        }


        // ToDo: Input.~~は使用せずにInputActionを使用するようにする
        void GetInput()
        {
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

            if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
            {
                _blockType = typeof(FragileBlock);
            }

            if (Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5))
            {
                _blockType = typeof(ConfusionBlock);
            }

            if (Input.GetKeyDown(KeyCode.Alpha6) || Input.GetKeyDown(KeyCode.Keypad6))
            {
                _blockType = typeof(TreasureCoin);
            }

            if (Input.GetKeyDown(KeyCode.Alpha7) || Input.GetKeyDown(KeyCode.Keypad7))
            {
                _blockType = typeof(CannonBlock);
            }

            if (Input.GetKeyDown(KeyCode.Alpha8) || Input.GetKeyDown(KeyCode.Keypad8))
            {
                _blockType = typeof(Spike);
            }

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