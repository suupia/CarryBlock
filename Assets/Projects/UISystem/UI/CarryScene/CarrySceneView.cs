using System;
using Carry.CarrySystem.FloorTimer.Scripts;
using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using Carry.CarrySystem.Map.Interfaces;

namespace Carry.UISystem.UI.CarryScene
{
    public class CarrySceneView : NetworkBehaviour
    {
        // NetworkObject must be attached to the parent of this script.
        [SerializeField] TextMeshProUGUI scoreText;
        [SerializeField] TextMeshProUGUI floorNumberText;  // 現在のフロア数
        [SerializeField] TextMeshProUGUI floorTimerText;
        [SerializeField] TextMeshProUGUI resultText;

        [SerializeField] Image floorTimerImage;

        [Networked] int FloorNumber { get; set; }
        [Networked] NetworkString<_16> Result { get; set; }
        
        FloorTimerNet _floorTimerNet;
        GameContext _gameContext;
        IMapUpdater _mapUpdater;


        [Inject]
        public void Construct(
            GameContext gameContext,
            FloorTimerNet floorTimerNet,
            IMapUpdater mapUpdater
            )
        {
            _gameContext = gameContext;
            _floorTimerNet = floorTimerNet;
            _mapUpdater = mapUpdater;
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
            // FloorTimerValue = Mathf.Floor(_floorTimer.GetRemainingTime(Runner));
            
            FloorNumber = _mapUpdater.Index + 1;
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
            var remainingTime = _floorTimerNet.FloorRemainingTime;
            floorNumberText.text = $"{FloorNumber} F";
            floorTimerText.text = $"Time : {Mathf.Floor(remainingTime)}";

            floorTimerImage.fillAmount = remainingTime / _floorTimerNet.FloorLimitTime;

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