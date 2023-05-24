using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Enemy;
using Main;
using UnityEngine;
using TMPro;

# nullable enable
public class ShowEnemyHp : MonoBehaviour
{
    NetworkRunnerManager? _runnerManger;
    [SerializeField] NetworkEnemyController enemyController;
    [SerializeField] TextMeshProUGUI? hpText;
   
    Transform _playerTransform;
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
            = RectTransformUtility.WorldToScreenPoint(Camera.main, enemyController.InterpolationTransform.position + offset);
        hpText.text = $"HP = {enemyController.Hp.ToString()}";
    }
    
}