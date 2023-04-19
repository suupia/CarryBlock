using System;
using Fusion;
using UnityEngine;
using TMPro;
using Main.VContainer;
using Main;
using VContainer;

namespace  UI
{
    public class GameSceneView : NetworkBehaviour
    {
        // NetworkObject must be attached to the parent of this script.
        [SerializeField] TextMeshProUGUI _scoreText;
        [SerializeField] TextMeshProUGUI _waveTimerText;
        [SerializeField] TextMeshProUGUI _resultText;

        [Networked] public int score { get; set; }

        GameContext _gameContext;
        ResourceAggregator _resourceAggregator;
        WaveTimer _waveTimer;
        

        [Inject]
        public void Construct(ResourceAggregator resourceAggregator, GameContext gameContext, WaveTimer waveTimer)
        {
            _resourceAggregator = resourceAggregator;
            _gameContext = gameContext;
            _waveTimer = waveTimer;
        }

        public override void FixedUpdateNetwork()
        {
            if(!HasStateAuthority)return;
            switch (_gameContext.gameState)
            {
                case GameContext.GameState.Playing:
                    PlayingView();
                    break;
                case GameContext.GameState.Result:
                    ResultView();
                    break;
            }
        }


        public override void Render()
        {
             Debug.Log($"_score : {score}, runner : {Runner}");
            _scoreText.text = $"Score : {score}";
            _waveTimerText.text = $"Time : {Mathf.Floor(_waveTimer.getRemainingTime(Runner))}";
        }
        
        
        void PlayingView()
        {
            score = _resourceAggregator.getAmount; // クライアントに反映させるためにNetworkedで宣言した変数に値を代入する
        }
        
        void ResultView()
        {
            _resultText.text = $"Result : {score}";
        }


    }
    


}
