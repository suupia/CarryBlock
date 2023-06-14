using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Nuts.BattleSystem.GameScene.Scripts;
using Nuts.Utility.Scripts;
using Nuts.BattleSystem.Boss.Scripts;
using UnityEngine;
using TMPro;


namespace  Nuts.UISystem.Game.Scripts
{
    public class ShowBoss1Hp : MonoBehaviour
    {
        GameInitializer _gameInitializer;
        [SerializeField] Monster1Controller_Net enemyController;
        [SerializeField] TextMeshProUGUI hpText;
   
        Transform _playerTransform;
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
                = RectTransformUtility.WorldToScreenPoint(Camera.main, enemyController.InterpolationTransform.position + _offset);
            hpText.text = $"HP = {enemyController.Hp.ToString()}";
        }
    
    }
}
