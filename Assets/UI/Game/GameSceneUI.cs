using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using VContainer;
using Main;

namespace  UI
{
    public class GameSceneUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _scoreText;
        ResourceAggregator _resourcesAggregator;
        
        [Inject]
        public void Construct(ResourceAggregator resourceAggregator)
        {
            _resourcesAggregator = resourceAggregator;
        }
        
        
        void Update()
        {
            _scoreText.text = $"Score : {_resourcesAggregator.getAmount}"; 
        }
        


    }
    


}
