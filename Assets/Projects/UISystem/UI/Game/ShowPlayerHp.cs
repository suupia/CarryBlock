using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Main;
using UnityEngine;
using TMPro;
using ExitGames.Client.Photon;
using UnityEngine.Serialization;
using GameSystem.GameScene.Scripts;


# nullable enable
public class ShowPlayerHp : MonoBehaviour
{
    GameInitializer? _gameInitializer;
   [SerializeField] AbstractNetworkPlayerController? playerController;
   [SerializeField] TextMeshProUGUI? hpText;
   
   Vector3 offset = new Vector3(2.0f, 2.2f, 0);
   
    RectTransform? _rectTransform;
    async void  Start()
    {
        _rectTransform = gameObject.GetComponent<RectTransform>();
        _gameInitializer = FindObjectOfType<GameInitializer>();

    }
    void LateUpdate()
    {
        if(_gameInitializer == null ||  !_gameInitializer.IsInitialized)return;
        _rectTransform.position 
            = RectTransformUtility.WorldToScreenPoint(Camera.main, playerController.InterpolationTransform.position + offset);
        hpText.text = $"HP = {playerController.PlayerStruct.Hp.ToString()}";
    }
    
}

