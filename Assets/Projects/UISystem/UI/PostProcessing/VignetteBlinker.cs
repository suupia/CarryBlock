using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using DG.Tweening;
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

        private Vignette  _vigette = null!;
        private Sequence _sequence = null!;
        
        private bool _stopFlag = true;
        
        
        void Start()
        {
            volume.profile.TryGet(out _vigette);

            _vigette.intensity.value = 0.0f;

            _sequence = DOTween.Sequence()
                .Append
                (
                    DOTween.To
                    (
                        () => _vigette.intensity.value,
                        (x) => _vigette.intensity.value = x,
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
