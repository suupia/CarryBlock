using System;
using Fusion;
using UnityEngine;
using TMPro;
using Main.VContainer;
using Main;
using VContainer;

namespace  UI
{
    public class GameSceneUI : NetworkBehaviour
    {
        [SerializeField] TextMeshProUGUI _scoreText;

        [Networked] int _score { get; set; }

        ResourceAggregator _resourceAggregator;


        void Awake()
        {
            _resourceAggregator = FindObjectOfType<GameSceneLifetimeScope>().Container.Resolve<ResourceAggregator>();
        }

        public override void FixedUpdateNetwork()
        {
            if(!HasStateAuthority)return;
            _score = _resourceAggregator.getAmount; // クライアントに反映させるためにNetworkedで宣言した変数に値を代入する
        }


        public override void Render()
        {
             Debug.Log($"_score : {_score}");
            _scoreText.text = $"Score : {_score}"; 
        }
        


    }
    


}
