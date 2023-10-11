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

        AudioSettingsTransporter _audioSettingsTransporter;

        [Inject]
        public void Construct(AudioSettingsTransporter audioSettingsTransporter)
        {
            _audioSettingsTransporter = audioSettingsTransporter;
        }

        void Awake()
        {
            bgmSlider.onValueChanged.AddListener(OnBgmSliderValueChanged);
        }

        void OnBgmSliderValueChanged(float value)
        {
            _audioSettingsTransporter.SetBgmVolume(value);
        }
        
    }
}