#nullable enable
using System.Linq;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.Player.Interfaces;
using Carry.GameSystem.LobbyScene.Scripts;
using Carry.Utility.Scripts;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

namespace Carry.UISystem.UI.LobbyScene
{
    public class SelectStageCanvasUILocal : MonoBehaviour
    {
        [SerializeField] GameObject viewObject = null!;
        [SerializeField] Transform buttonParent = null!;
        [SerializeField] CustomButton buttonPrefab;

        IMapKeyDataSelector  _mapKeyDataSelector = null!;
        StageIndexTransporter _stageIndexTransporter =  null!;
        InputAction _toggleSelectStageCanvas = null!;
        IPlayerAnimatorPresenter? _playerAnimatorPresenter;
        
        LobbyStartGameTheater _lobbyStartGameTheater = null!;
        
        
        [Inject]
        public void Construct(
            IMapKeyDataSelector mapKeyDataSelector,
            StageIndexTransporter stageIndexTransporter,
            LobbyStartGameTheater lobbyStartGameTheater
            )
        {
            
            _mapKeyDataSelector = mapKeyDataSelector;
            _stageIndexTransporter = stageIndexTransporter;
            _lobbyStartGameTheater = lobbyStartGameTheater;
        }

        void Start()
        {
            viewObject.SetActive(false);


            var buttonCount = _mapKeyDataSelector.MapKeyDataNetListCount;
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
                string buttonText;

                switch (i)
                {
                    case 0:
                        buttonText = "Easy";
                        break;
                    case 1:
                        buttonText = "Normal";
                        break;
                    case 2:
                        buttonText = "Hard";
                        break;
                    case 3:
                        buttonText = "Expert";
                        break;
                    case 4:
                        buttonText = "Endless";
                        break;
                    default:
                        buttonText = "NotFound";
                        break;
                }

                stageButton.SetText(buttonText);
                var index = i;
                
                stageButton.AddListener(() =>
                {
                    viewObject.SetActive(false);

                    _lobbyStartGameTheater.PlayLobbyTheater(() =>
                    {
                        _stageIndexTransporter.SetStageIndex(index);
                        lobbyInitializer.TransitionToGameScene();
                    });
                });
            }
            
            SetupToggleSelectStageCanvas();
        }
        

        // 以下の処理はInputAction系を初期化するところに移動させた方がよいかもしれない
        void SetupToggleSelectStageCanvas()
        {
            var inputActionMap = InputActionMapLoader.GetInputActionMap(InputActionMapLoader.ActionMapName.Default);
            _toggleSelectStageCanvas = inputActionMap.FindAction("ToggleSelectStageCanvas");
            _toggleSelectStageCanvas.performed += OnToggleSelectStageCanvas;
        }
        
        void OnToggleSelectStageCanvas(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                viewObject.SetActive(!viewObject.activeSelf);
            }
        }
    }
}