using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using UnityEngine.UI;

namespace Carry.CarrySystem.Audio.Scripts
{
    public class AudioSettingsTransporter
    {
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

        public void SetResolution(Button button)
        {
           
        }
        
    }
}