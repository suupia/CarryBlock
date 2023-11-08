using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Carry.CarrySystem.Map.Scripts;
using Fusion;
using Carry.GameSystem.LobbyScene.Scripts;
using Carry.NetworkUtility.Inputs.Scripts;
using Carry.UISystem.UI;
using Carry.Utility;
using Carry.Utility.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;
using Carry.CarrySystem.Player.Scripts;
using DG.Tweening;
using Carry.CarrySystem.Player.Info;
using Carry.CarrySystem.Player.Interfaces;
using Carry.CarrySystem.Cart.Scripts;
using log4net.Appender;
using Projects.CarrySystem.Enemy;


#nullable enable

namespace Carry.UISystem.UI.LobbyScene
{
    public class SelectStageCanvasUINet : NetworkBehaviour
    {
        [SerializeField] GameObject viewObject = null!;
        [SerializeField] Transform buttonParent = null!;
        [SerializeField] CustomButton buttonPrefab;

        [Networked] protected NetworkButtons PreButtons { get; set; }

        MapKeyDataSelectorNet  _mapKeyDataSelectorNet = null!;
        StageIndexTransporter _stageIndexTransporter =  null!;
        InputAction _toggleSelectStageCanvas = null!;
        LobbyPlayerContainer _lobbyPlayerContainer = null!;
        IPlayerAnimatorPresenter? _playerAnimatorPresenter;
        
        //PlayerNearCartHandlerNet _playerNearCartHandler = null!;
        
        [Inject]
        public void Construct(
            MapKeyDataSelectorNet mapKeyDataSelectorNet,
            StageIndexTransporter stageIndexTransporter,
            LobbyPlayerContainer lobbyPlayerContainer
            //IPlayerAnimatorPresenter? _playerAnimatorPresenter
            )
        {
            
            _mapKeyDataSelectorNet = mapKeyDataSelectorNet;
            _stageIndexTransporter = stageIndexTransporter;
            _lobbyPlayerContainer = lobbyPlayerContainer;
        }

        public override void Spawned()
        {

            //var enemySpawner = new EnemySpawner(Runner);
            //enemySpawner.SpawnPrefab(new Vector3(-12f,0f,0f), Quaternion.Euler(0f, 180f, 0f));
            viewObject.SetActive(false);

            if (!HasStateAuthority)return;

            var buttonCount = _mapKeyDataSelectorNet.MapKeyDataNetListCount;
            var stageButtons = buttonParent.GetComponentsInChildren<CustomButton>().ToList();
            for (int i = 0; i < buttonCount; i++)
            {
                var button = Instantiate(buttonPrefab, buttonParent);
                button.Init();
                stageButtons.Add(button);
            }
            
            var lobbyInitializer = FindObjectOfType<LobbyInitializer>();
            
            for(int i = 0; i< stageButtons.Count; i++)
            {
                var stageButton = stageButtons[i];
                stageButton.SetText($"Stage {i + 1}");
                var index = i;
                stageButton.AddListener(() =>
                {
                    CartLobbyControllerNet cart = FindObjectOfType<CartLobbyControllerNet>();
                    EnemyControllerNet enemy = FindObjectOfType<EnemyControllerNet>();
                    var enemyAnimatorPresenter = enemy.GetComponentInChildren<EnemyAnimatorPresenterNet>();

                    //ゲームスタート前のアニメーション
                    viewObject.SetActive(false);
                    for (int playerIndex = 0;
                         playerIndex < _lobbyPlayerContainer.PlayerControllers.Count;
                         playerIndex++)
                    {
                        var playerController = _lobbyPlayerContainer.PlayerControllers[playerIndex];
                        var playerTransform = playerController.transform;
                        
                        DOTween.Sequence()
                            .Append(playerTransform.DOLookAt(CalcPlayerPositionInCart(playerIndex, cart.transform.position), 0))
                            .AppendCallback(() => playerController.GetCharacter.Dash())
                            .Append(playerTransform.DOMove(CalcPlayerPositionInCart(playerIndex, cart.transform.position), 2f))
                            .AppendCallback(() =>
                            {
                                playerController.GetCharacter.SwitchToRegularMove();
                                var changePosition = new Vector3(playerTransform.position.x, 1.0f,
                                    playerTransform.position.z);
                                playerTransform.position = changePosition;
                                playerTransform.SetParent(cart.transform);
                            })
                            .Append(enemy.transform.DOLookAt(cart.transform.position, 0))
                            .AppendCallback(() => enemyAnimatorPresenter.Chase())
                            .Append(enemy.transform.DOMove(new Vector3(-6f, 0, 0f), 1.5f))
                            .AppendCallback(() =>   enemyAnimatorPresenter.Threat())
                            .Append(enemy.transform.DOMove(new Vector3(-5.5f, 0, 0f), 1.5f))
                            .Append(playerTransform.DOLookAt(new Vector3(40f, 0, 0), 0)) // var targetPosition = new Vector3(40f, 0, 0);
                            .AppendCallback(() => enemyAnimatorPresenter.Chase())
                            .Append(enemy.transform.DOMove(new Vector3(15f, 0, 0f), 2.5f))
                            .Join(cart.transform.DOMove(new Vector3(40f, 0, 0), 2.5f))
                            .OnComplete(() =>
                            {
                                _stageIndexTransporter.SetStageIndex(index);
                                lobbyInitializer.TransitionToGameScene();
                            }).Play();

                    }
                    
                    
                });
            }
            
            SetupToggleSelectStageCanvas();
        }

        Vector3 CalcPlayerPositionInCart(int playerIndex, Vector3 cartPosition)
        {
            return playerIndex switch 
            {
                0 => cartPosition + new Vector3(0.7f, 0, 0.7f),
                1 => cartPosition + new Vector3(0.7f, 0, -0.7f),
                2 => cartPosition + new Vector3(-0.7f, 0, 0.7f),
                3 => cartPosition + new Vector3(-0.7f, 0, -0.7f),
                _ => throw new System.ArgumentOutOfRangeException()
            };
        }


        // 以下の処理はInputAction系を初期化するところに移動させた方がよいかもしれない
        void SetupToggleSelectStageCanvas()
        {
            var inputActionMap = InputActionMapLoader.GetInputActionMap();
            _toggleSelectStageCanvas = inputActionMap.FindAction("ToggleSelectStageCanvas");
            _toggleSelectStageCanvas.performed += OnToggleSelectStageCanvas;
        }
        
        void OnToggleSelectStageCanvas(InputAction.CallbackContext context)
        {
            if(!HasStateAuthority)return;
            if (context.performed)
            {
                viewObject.SetActive(!viewObject.activeSelf);
            }
        }
    }
}

