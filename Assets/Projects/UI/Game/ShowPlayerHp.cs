using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Main;
using UnityEngine;
using TMPro;
using Cysharp.Threading.Tasks;
using UnityEngine.Serialization;

# nullable enable
public class ShowPlayerHp : MonoBehaviour, ICameraSetter
{
    NetworkRunnerManager? _runnerManger;
   [SerializeField] AbstractNetworkPlayerController? playerController;
   [SerializeField] TextMeshProUGUI? hpText;
   
   Camera? _playerCamera;
   Vector3 offset = new Vector3(0, 1.5f, 0);
   
    RectTransform? _rectTransform;
    async void  Start()
    {
        _rectTransform = gameObject.GetComponent<RectTransform>();
        _runnerManger = FindObjectOfType<NetworkRunnerManager>();
        
    }
    void Update()
    {
        if(_runnerManger == null ||  !_runnerManger.IsReady)return;
        if(_playerCamera == null) return;
        _rectTransform.position 
            = RectTransformUtility.WorldToScreenPoint(_playerCamera, playerController.gameObject.transform.position + offset);
        hpText.text = $"HP = {playerController.PlayerStruct.Hp.ToString()}";
    }

    public void SetCamera(Camera playerCamera)
    {
        _playerCamera = playerCamera;
    }
    
    // 動的に生成されたカメラを設定する必要がある....

}

