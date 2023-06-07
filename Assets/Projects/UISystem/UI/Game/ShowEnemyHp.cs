using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Nuts.Utility.Scripts;
using Nuts.BattleSystem.Enemy.Scripts;
using UnityEngine;
using TMPro;
using Nuts.GameSystem.GameScene.Scripts;

# nullable enable
namespace  Nuts.UISystem.Game.Scripts
{
    public class ShowEnemyHp : MonoBehaviour
    {
        GameInitializer? _gameInitializer;
        [SerializeField] NetworkEnemyController enemyController;
        [SerializeField] TextMeshProUGUI? hpText;
   
        Transform _playerTransform;
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
                = RectTransformUtility.WorldToScreenPoint(Camera.main, enemyController.InterpolationTransform.position + offset);
            hpText.text = $"HP = {enemyController.Hp.ToString()}";
        }
    
    }
}
