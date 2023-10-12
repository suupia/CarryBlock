using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Carry.CarrySystem.Audio.Scripts
{
    public class AudioSettingsTransporter
    {
        public float BgmVolume { get; private set; }
        public float SeVolume { get; private set; }
        
        public Resolution R1920x1080{ get; private set; }
        public Resolution R1680x1050{ get; private set; }
        public Resolution R1440x900{ get; private set; }
        public Resolution R960x600{ get; private set; }

        public void SetBgmVolume(float bgmVolume)
        {
            BgmVolume = bgmVolume;
        }
        
        public void SetSeVolume(float seVolume)
        {
            SeVolume = seVolume;
        }
        
    }
}