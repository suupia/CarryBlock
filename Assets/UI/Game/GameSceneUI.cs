using System;
using Fusion;
using UnityEngine;
using TMPro;
using Main.VContainer;
using Main;
using VContainer;

namespace  UI
{
    public class GameSceneUI : SimulationBehaviour
    {
        [SerializeField] TextMeshProUGUI _scoreText;
        [SerializeField] TextMeshProUGUI _waveTimerText;

        [Networked] UInt16 score { get; set; }

        [SerializeField] WaveTimer _waveTimer;
        ResourceAggregator _resourceAggregator;
        
        // 登録しないとRunnerはnullであることに注意
        

        [Inject]
        public void Construct(ResourceAggregator resourceAggregator)
        {
            _resourceAggregator = resourceAggregator;
        }

        public override void FixedUpdateNetwork()
        {
            if(!HasStateAuthority)return;
            score = (UInt16)_resourceAggregator.getAmount; // クライアントに反映させるためにNetworkedで宣言した変数に値を代入する
            Debug.Log($"_score : {score}, runner : {Runner}");
        }


        public override void Render()
        {
             Debug.Log($"_score : {score}, runner : {Runner}");
            _scoreText.text = $"Score : {score}";
            _waveTimerText.text = $"Time : {Mathf.Floor(_waveTimer.getRemainingTime)}";
        }
        


    }
    


}
