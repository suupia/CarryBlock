using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using VContainer;
using Fusion;
using Carry.CarrySystem.CarryScene.Scripts;
using UnityEngine.Serialization;


namespace Carry.UISystem.UI.CarryScene
{
    public class CarrySceneView : NetworkBehaviour
    {
        // NetworkObject must be attached to the parent of this script.
        [SerializeField] TextMeshProUGUI scoreText;
        [SerializeField] TextMeshProUGUI floorTimerText;
        [SerializeField] TextMeshProUGUI resultText;
        [SerializeField] TextMeshProUGUI remainingTimeToReturnText;


        GameContext _gameContext;

        FloorTimer _floorTimer;

        [Networked] float FloorTimerValue { get; set; }
        [Networked] NetworkString<_16> Result { get; set; }


        [Inject]
        public void Construct(
            GameContext gameContext,
            FloorTimer floorTimer)
        {
            _gameContext = gameContext;
            _floorTimer = floorTimer;
        }

        public override void FixedUpdateNetwork()
        {
            if (!HasStateAuthority) return;
            switch (_gameContext.CurrentState)
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
            // Debug.Log($"_gameContext.gameState: {_gameContext.CurrentState}");
            switch (_gameContext.CurrentState)
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
            // Score = _resourceAggregator.GetAmount;
            FloorTimerValue = Mathf.Floor(_floorTimer.GetRemainingTime(Runner));
        }

        void FixedUpdateNetwork_Result()
        {
            // Result = _resourceAggregator.IsSuccess() ? "Success" : "Failure";
            Result = "Failure"; // とりあえず失敗にしておく
        }

        void Render_Playing()
        {
            // サーバー用のドメインの反映
            // scoreText.text = $"Score : {Score} / {_resourceAggregator.QuotaAmount}";
            floorTimerText.text = $"Time : {FloorTimerValue}";

            // ローカル用のドメインの反映
            // remainingTimeToReturnText.text = _returnToMainBaseGauge.IsReturnToMainBase
            //     ? $"{_returnToMainBaseGauge.RemainingTime}s"
            //     : "";
        }

        void Render_Result()
        {
            resultText.text = $"Result : {Result}";
        }
    }
}