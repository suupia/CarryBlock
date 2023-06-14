using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Nuts.Utility.Scripts;
using UnityEngine;
using TMPro;
using ExitGames.Client.Photon;
using Nuts.BattleSystem.Player.Scripts;
using UnityEngine.Serialization;
using Nuts.BattleSystem.GameScene.Scripts;


namespace Nuts.UISystem.Game.Scripts
{
    public class ShowPlayerHp : MonoBehaviour
    {
        GameInitializer _gameInitializer;
        [SerializeField] AbstractNetworkPlayerController playerController;
        [SerializeField] TextMeshProUGUI hpText;
   
        readonly Vector3 _offset = new Vector3(2.0f, 2.2f, 0);
   
        RectTransform _rectTransform;
         void  Start()
        {
            _rectTransform = gameObject.GetComponent<RectTransform>();
            _gameInitializer = FindObjectOfType<GameInitializer>();

        }
        void LateUpdate()
        {
            if(_gameInitializer == null ||  !_gameInitializer.IsInitialized)return;
            _rectTransform.position 
                = RectTransformUtility.WorldToScreenPoint(Camera.main, playerController.InterpolationTransform.position + _offset);
            hpText.text = $"HP = {playerController.PlayerStruct.Hp.ToString()}";
        }
    
    }


}
