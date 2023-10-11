using System;
using System.Collections;
using System.Collections.Generic;
using Carry.CarrySystem.Audio.Scripts;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Carry.UISystem.UI.TitleScene
{
    public class OptionCanvasMono : MonoBehaviour
    {
        [SerializeField] Slider bgmSlider;
        [SerializeField] Slider seSlider;

        AudioSettingsTransporter _audioSettingsTransporter;

        [Inject]
        public void Construct(AudioSettingsTransporter audioSettingsTransporter)
        {
            _audioSettingsTransporter = audioSettingsTransporter;
        }

        void Awake()
        {
            bgmSlider.onValueChanged.AddListener(OnBgmSliderValueChanged);
            seSlider.onValueChanged.AddListener(OnSeSliderValueChanged);
        }

        void OnBgmSliderValueChanged(float value)
        {
            _audioSettingsTransporter.SetBgmVolume(value);
        }
        
        void OnSeSliderValueChanged(float value)
        {
            _audioSettingsTransporter.SetSeVolume(value);
        }

    }
}