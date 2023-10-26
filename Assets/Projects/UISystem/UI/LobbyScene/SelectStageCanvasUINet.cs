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
        IPlayerAnimatorPresenter _playerAnimatorPresenter;
        
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
                    Debug.Log(cart.transform.position.ToString());
                    //ゲームスタート前のアニメーション
                    _lobbyPlayerContainer.PlayerControllers.ForEach(playerController =>
                    {
                        viewObject.SetActive(false);
                        var playerTransform = playerController.transform;
                        Debug.Log(playerTransform.position.ToString());
                        var cartPosition = new Vector3(0f, 0.5f, 0f);
                        _playerAnimatorPresenter.Dash();
                        playerTransform.DOMove(cartPosition, 3f).OnComplete(() =>
                        {
                            Debug.Log("finish move");
                            _playerAnimatorPresenter.Idle();
                            playerTransform.SetParent(cart.transform);

                        });
                        var targetPosition = new Vector3(10f, 0, 0);
                        cart.transform.DOMove(targetPosition, 4f).OnComplete(() =>
                        {
                            _stageIndexTransporter.SetStageIndex(index);
                            lobbyInitializer.TransitionToGameScene();
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

