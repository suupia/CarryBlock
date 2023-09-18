using System.Collections;
using System.Collections.Generic;
using Carry.CarrySystem.FloorTimer.Scripts;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using DG.Tweening;
using UniRx;
using VContainer;

#nullable enable


namespace Carry.UISystem.UI.CarryScene
{
    // 使う場合は Main Camera の設定でPostProcessingにチェックを入れること
    
    public class VignetteBlinker : MonoBehaviour
    { 
        [SerializeField] Volume volume = null!;
        [SerializeField] float period;
        [SerializeField] [Range(0.0f,1.0f)] float blinkTimeRate;
        [SerializeField] [Range(0.0f,1.0f)] float maxIntensity;

        Vignette  _vignette = null!;
        Sequence _sequence = null!;
        
        bool _stopFlag = true;
        
        readonly float _playThreshold = 0.20f;  // 残り時間が2割の時に点滅を開始する
        
        [Inject]
        public void Construct(FloorTimerNet floorTimerNet)
        {
            this.ObserveEveryValueChanged(_ => floorTimerNet.FloorRemainingTimeRatio)
                .Subscribe(_ =>
                {
                    if (floorTimerNet.FloorRemainingTimeRatio <= _playThreshold)
                    {
                        if (_stopFlag)Play();
                    }
                    else
                    {
                        if(!_stopFlag)Stop();
                    }
                });
        }
        
        void Start()
        {
            volume.profile.TryGet(out _vignette);

            _vignette.intensity.value = 0.0f;

            _sequence = DOTween.Sequence()
                .Append
                (
                    DOTween.To
                    (
                        () => _vignette.intensity.value,
                        (x) => _vignette.intensity.value = x,
                        maxIntensity,
                        period * blinkTimeRate
                    ).SetEase(Ease.OutFlash, 2)
                )
                .SetLoops(-1, LoopType.Restart)
                .AppendInterval(period * (1 - blinkTimeRate))
                .AppendCallback
                (
                    () =>
                    {
                        if (_stopFlag)
                        {
                            _sequence.Pause();
                        }
                    }
                );

            // DOTween.defaultAutoPlay = AutoPlay.None;
            _sequence.Pause();  // 自動再生を止める
        }

        public void Stop()
        {
            _stopFlag = true;
        }
        
        public void Play()
        {
            _stopFlag = false;
            _sequence.Play();
        }
    }    
}
