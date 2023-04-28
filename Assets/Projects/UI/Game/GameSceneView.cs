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

        [Networked] NetworkBool IsReturnToMainBase { get; set; }
        [Networked] float RemainingTimeToReturn { get; set; }

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
                    FixedUpdateNetwork_Playing();
                    break;
                case GameContext.GameState.Result:
                    FixedUpdateNetwork_Result();
                    break;
            }
        }


        public override void Render()
        {
            switch (_gameContext.gameState)
            {
                case GameContext.GameState.Playing:
                    Render_Playing();
                    break;
                case GameContext.GameState.Result:
                    Render_Result();
                    break;
            }
        }


        void FixedUpdateNetwork_Playing()
        {
            // クライアントに反映させるためにNetworkedで宣言した変数に値を代入する
            Score = _resourceAggregator.getAmount;
            IsReturnToMainBase = _returnToMainBaseGauge.IsReturnToMainBase;
            RemainingTimeToReturn = _returnToMainBaseGauge.RemainingTime;
        }

        void FixedUpdateNetwork_Result()
        {
            Result = _resourceAggregator.IsSuccess() ? "Success" : "Failure";
        }

        void Render_Playing()
        {
            // Debug.Log($"_score : {Score}, runner : {Runner}");
            scoreText.text = $"Score : {Score}";
            waveTimerText.text = $"Time : {Mathf.Floor(_waveTimer.getRemainingTime(Runner))}";

            if (Runner.LocalPlayer)
            {
                remainingTimeToReturnText.text = IsReturnToMainBase ? $"{RemainingTimeToReturn}s" : "";
            }
        }

        void Render_Result()
        {
            resultText.text = $"Result : {Result}";
        }
    }
}