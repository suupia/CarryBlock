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
                var nowPlayerNum = 0;
                var nowPlayerPosition=0;
                stageButton.AddListener(() =>
                {
                    CartLobbyControllerNet cart = FindObjectOfType<CartLobbyControllerNet>();
                    EnemyControllerNet enemy = FindObjectOfType<EnemyControllerNet>();
                    var enemyAnimatorPresenter = enemy.GetComponentInChildren<EnemyAnimatorPresenterNet>();
                    Debug.Log(cart.transform.position.ToString());
                    Debug.Log(enemy.transform.position.ToString());
                    Vector3[] PlayerCartPosition = new Vector3[]
                    {
                        cart.transform.position + new Vector3(0.7f, 0, 0.7f),
                        cart.transform.position + new Vector3(0.7f, 0, -0.7f),
                        cart.transform.position + new Vector3(-0.7f, 0, 0.7f),
                        cart.transform.position + new Vector3(-0.7f, 0, -0.7f),
                    };
                    //ゲームスタート前のアニメーション
                    _lobbyPlayerContainer.PlayerControllers.ForEach(playerController =>
                    {
                        viewObject.SetActive(false);
                        var playerTransform = playerController.transform;
                        Debug.Log(playerTransform.position.ToString());
                        // _playerAnimatorPresenter.Dash();
                        playerController.GetCharacter.Dash();
                        // プレイヤーが移動方向に向く
                        var PlayerNumPosition = PlayerCartPosition[nowPlayerPosition];
                        nowPlayerPosition += 1;
                        playerTransform.LookAt(PlayerNumPosition);
                        playerTransform.DOMove(PlayerNumPosition, 2f).OnComplete(() =>
                        {
                            Debug.Log("finish move");
                            // _playerAnimatorPresenter.Idle();
                            playerController.GetCharacter.SwitchToRegularMove();
                            var changePosition = new Vector3(playerTransform.position.x, 1.0f,
                                playerTransform.position.z);
                            playerTransform.position = changePosition;
                            playerTransform.SetParent(cart.transform);
                            nowPlayerNum += 1;
                            if (nowPlayerNum >= _lobbyPlayerContainer.PlayerControllers.Count)
                            {
                                //敵の歩くアニメーション
                                enemyAnimatorPresenter.Chase();
                                enemy.transform.LookAt(cart.transform.position);
                                enemy.transform.DOMove(new Vector3(-6f, 0, 0f), 1.2f).OnComplete(() =>
                                {
                                    //敵の威嚇アニメーション
                                    // enemyAnimatorPresenter.Threat();
                                    var targetPosition = new Vector3(45f, 0, 0);
                                    playerTransform.LookAt(targetPosition);
                                    //敵の歩くアニメーション
                                    
                                    enemy.transform.DOMove(new Vector3(15f, 0, 0f), 3.5f);
                                    cart.transform.DOMove(targetPosition, 3.5f).OnComplete(() =>
                                    {
                                        _stageIndexTransporter.SetStageIndex(index);
                                        lobbyInitializer.TransitionToGameScene();
                                    }); 
                                });
                                
                            }
                        });
                    });
                    
                    
                    
                });
            }
            
            SetupToggleSelectStageCanvas();
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

