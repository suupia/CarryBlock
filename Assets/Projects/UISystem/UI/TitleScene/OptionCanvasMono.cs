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
        enum Resolution
        {
            R1920x1080,
            R1680x1050,
            R1440x900,
            R960x600
        }
    
        [SerializeField] Slider bgmSlider;
        [SerializeField] Slider seSlider;
        [SerializeField] Transform resolutionButtonParent;
        
        AudioSettingsTransporter _audioSettingsTransporter;

        [Inject]
        public void Construct(AudioSettingsTransporter audioSettingsTransporter)
        {
            _audioSettingsTransporter = audioSettingsTransporter;
        }

        void Start()
        {
            var buttons = resolutionButtonParent.GetComponentsInChildren<Button>();
            foreach (var button in buttons)
            {
                button.onClick.AddListener(() =>   _audioSettingsTransporter.SetResolution(button));
            }
            
            List<Button> buttonList = new List<Button>(buttons); // ボタンをリストに変換

            for (int i = 0; i < buttonList.Count; i++)
            {
                Button button = buttonList[i];
                button.onClick.AddListener(() => _audioSettingsTransporter.SetResolution(button));

                // インデックスを利用する例
                Debug.Log("Button at index " + i + " is " + button.name);
            }
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