using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using VContainer;

public class NetworkWaveTimer : NetworkBehaviour 
{
    [Networked] TickTimer tickTimer { get; set; }
    WaveTimer _waveTimer;

    readonly float _waveTime = 60f;
    bool _isInitialized = false;

    [Inject]
    public void Construct(WaveTimer waveTimer)
    {
        _waveTimer = waveTimer;
        Debug.Log($"_waveTimer : {_waveTimer}");
    }

    public void Init()
    {
        _isInitialized = true;
    }

    public override void FixedUpdateNetwork()
    {
        if(_isInitialized == false)return;
        if(tickTimer.ExpiredOrNotRunning(Runner)) tickTimer = TickTimer.CreateFromSeconds(Runner, _waveTime);
        _waveTimer.tickTimer = tickTimer;
    }
    
}

public class WaveTimer
{
    public TickTimer tickTimer { get; set; }
    
    public float getRemainingTime(NetworkRunner Runner) =>  tickTimer.RemainingTime(Runner).HasValue ? tickTimer.RemainingTime(Runner).Value : 0f;
    
    public bool isExpired(NetworkRunner Runner) => tickTimer.ExpiredOrNotRunning(Runner);
}
 