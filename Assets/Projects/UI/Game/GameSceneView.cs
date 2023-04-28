using System;
using Fusion;
using UnityEngine;
using TMPro;
using Main.VContainer;
using Main;
using UnityEngine.Serialization;
using VContainer;

namespace UI
{
    public class GameSceneView : NetworkBehaviour
    {
        // NetworkObject must be attached to the parent of this script.
        [SerializeField] TextMeshProUGUI scoreText;
        [SerializeField] TextMeshProUGUI waveTimerText;
        [SerializeField] TextMeshProUGUI resultText;
        [SerializeField] TextMeshProUGUI remainingTimeToReturnText;

        [Networked] int Score { get; set; }
        [Networked] NetworkString<_16> Result { get; set; }

        GameContext _gameContext;
        ResourceAggregator _resourceAggregator;
        WaveTimer _waveTimer;
        ReturnToMainBaseGauge _returnToMainBaseGauge;

        NetworkPlayerController _networkPlayerController;


        [Inject]
        public void Construct(
            ResourceAggregator resourceAggregator,
            GameContext gameContext,
            WaveTimer waveTimer,
            ReturnToMainBaseGauge returnToMainBaseGauge)
        {
            _resourceAggregator = resourceAggregator;
            _gameContext = gameContext;
            _waveTimer = waveTimer;
            _returnToMainBaseGauge = returnToMainBaseGauge;
        }

        public override void FixedUpdateNetwork()
        {
            if (!HasStateAuthority) return;
            switch (_gameContext.gameState)
            {
                case GameContext.GameState.Playing:
                    PlayingNetworkView();
                    break;
                case GameContext.GameState.Result:
                    ResultNetworkView();
                    break;
            }
        }


        public override void Render()
        {
            switch (_gameContext.gameState)
            {
                case GameContext.GameState.Playing:
                    PlayingLocalView();
                    break;
                case GameContext.GameState.Result:
                    ResultLocalView();
                    break;
            }
            
        }


        void PlayingNetworkView()
        {
            Score = _resourceAggregator.getAmount; // クライアントに反映させるためにNetworkedで宣言した変数に値を代入する
        }

        void ResultNetworkView()
        {
            Result = _resourceAggregator.IsSuccess() ? "Success" : "Failure";
        }
        
        void PlayingLocalView()
        {
            Debug.Log($"_score : {Score}, runner : {Runner}");
            scoreText.text = $"Score : {Score}";
            waveTimerText.text = $"Time : {Mathf.Floor(_waveTimer.getRemainingTime(Runner))}";
            remainingTimeToReturnText.text = _returnToMainBaseGauge.IsReturnToMainBase
                ? $"{_returnToMainBaseGauge.RemainingTime}s"
                : "";
        }
        
        void ResultLocalView()
        {
            resultText.text = $"Result : {Result}";
        }
    }
}