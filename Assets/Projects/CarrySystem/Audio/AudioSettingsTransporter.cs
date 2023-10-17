using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using UnityEngine.UI;

namespace Carry.CarrySystem.Audio.Scripts
{
    public class AudioSettingsTransporter
    {
        public enum Resolution
        {
            R1920x1080,
            R1680x1050,
            R1440x900,
            R960x600
        }
        
        public float BgmVolume { get; private set; }
        public float SeVolume { get; private set; }
        
        public Resolution ResolutionValue { get; private set; }
        
        public void SetBgmVolume(float bgmVolume)
        {
            BgmVolume = bgmVolume;
        }
        
        public void SetSeVolume(float seVolume)
        {
            SeVolume = seVolume;
        }

        public void SetResolution(Resolution resolution)
        {
            ResolutionValue = resolution;
            Debug.Log($"Resolution;{resolution}");
        }
        
    }
}