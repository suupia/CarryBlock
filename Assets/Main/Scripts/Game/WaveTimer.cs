using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class WaveTimer : SimulationBehaviour
{
    [Networked] TickTimer waveTimer { get; set; }
    
    public float getRemainingTime =>  waveTimer.RemainingTime(Runner).HasValue ? waveTimer.RemainingTime(Runner).Value : 0f;

    readonly float _waveTime = 60f;

    public void Init()
    {
        waveTimer = TickTimer.CreateFromSeconds(Runner, _waveTime);
    }

    public override void FixedUpdateNetwork()
    {
        if (waveTimer.ExpiredOrNotRunning(Runner))
        {
            Debug.Log($"終了〜〜");
        }
    }
    
}
