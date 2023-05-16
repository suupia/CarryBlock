using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Main;
using UnityEngine;
using TMPro;
using Cysharp.Threading.Tasks;
using UnityEngine.Serialization;

# nullable enable
public class ShowPlayerHp : MonoBehaviour
{
   [SerializeField] AbstractNetworkPlayerController? playerController;
   [SerializeField] TextMeshProUGUI? hpText;
   Vector3 offset = new Vector3(0, 1.5f, 0);


    RectTransform? _rectTransform;
    async void  Start()
    {
        _rectTransform = gameObject.GetComponent<RectTransform>();
    }
    void Update()
    {
        _rectTransform.position 
            = RectTransformUtility.WorldToScreenPoint(Camera.main, playerController.gameObject.transform.position + offset);
        hpText.text = $"HP = {playerController.PlayerStruct.Hp.ToString()}";
    }
    
    // 動的に生成されたカメラを設定する必要がある....

}
