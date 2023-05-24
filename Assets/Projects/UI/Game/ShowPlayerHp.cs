using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Main;
using UnityEngine;
using TMPro;
using Cysharp.Threading.Tasks;
using ExitGames.Client.Photon;
using UnityEngine.Serialization;

# nullable enable
public class ShowPlayerHp : MonoBehaviour
{
    NetworkRunnerManager? _runnerManger;
   [SerializeField] AbstractNetworkPlayerController? playerController;
   [SerializeField] TextMeshProUGUI? hpText;
   
   Vector3 offset = new Vector3(2.0f, 2.2f, 0);
   
    RectTransform? _rectTransform;
    async void  Start()
    {
        _rectTransform = gameObject.GetComponent<RectTransform>();
        _runnerManger = FindObjectOfType<NetworkRunnerManager>();
        
    }
    void LateUpdate()
    {
        if(_runnerManger == null ||  !_runnerManger.IsReady)return;
        _rectTransform.position 
            = RectTransformUtility.WorldToScreenPoint(Camera.main, playerController.InterpolationTransform.position + offset);
        hpText.text = $"HP = {playerController.PlayerStruct.Hp.ToString()}";
    }
    
}

