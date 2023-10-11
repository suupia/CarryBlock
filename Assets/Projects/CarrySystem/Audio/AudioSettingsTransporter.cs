using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Carry.CarrySystem.Audio.Scripts
{
    public class AudioSettingsTransporter
    {
        public float BgmVolume { get; private set; }
        
        public void SetBgmVolume(float bgmVolume)
        {
            BgmVolume = bgmVolume;
        }
        
        
    }
}