using System;
using System.Collections;
using System.Collections.Generic;
using Carry.CarrySystem.Audio.Scripts;
using Fusion;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;
using VContainer;

namespace Carry.UISystem.UI.TitleScene
{
    public class OptionCanvasMono : MonoBehaviour
    {
        
        [SerializeField] Slider bgmSlider;
        [SerializeField] Slider seSlider;
        [SerializeField] Transform resolutionButtonParent;
        
        AudioSettingsTransporter _audioSettingsTransporter;

        [Inject]
        public void Construct(AudioSettingsTransporter audioSettingsTransporter)
        {
            _audioSettingsTransporter = audioSettingsTransporter;
            Initialize();
        }

        void Initialize()
        {
            var buttons = resolutionButtonParent.GetComponentsInChildren<Button>();
            
            for (int i = 0; i < buttons.Length; i++)
            {
                Button button = buttons[i];
                AudioSettingsTransporter.Resolution resolution = (AudioSettingsTransporter.Resolution)i;
                button.onClick.AddListener(() => _audioSettingsTransporter.SetResolution(resolution));

                // インデックスを利用する例
                Debug.Log("Button at index " + i + " is " + button.name);
            }
            
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