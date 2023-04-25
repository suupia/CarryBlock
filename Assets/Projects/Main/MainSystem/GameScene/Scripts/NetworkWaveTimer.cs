using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using VContainer;

public class NetworkWaveTimer : NetworkBehaviour 
{
    [Networked] public TickTimer tickTimer { get; set; }
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
        tickTimer = TickTimer.CreateFromSeconds(Runner, _waveTime); // ここに書くの良くないかも
    }

    public override void FixedUpdateNetwork()
    {
        if(_isInitialized == false)return;
        // if(Object?.IsValid == false) return;
        // if(tickTimer.ExpiredOrNotRunning(Runner)) tickTimer = TickTimer.CreateFromSeconds(Runner, _waveTime);
        _waveTimer.tickTimer = tickTimer;
        _waveTimer.NotifyObservers(Runner);
    }
    
}

public class WaveTimer : ITimer
{
    GameContext _gameContext;
    public TickTimer tickTimer { get; set; }

    [Inject]
    public WaveTimer(GameContext gameContext)
    {
        _gameContext = gameContext;
    }  
    
    public void NotifyObservers(NetworkRunner runner)
    {
        _gameContext.Update(runner, this);
    }

    public float getRemainingTime(NetworkRunner Runner) =>  tickTimer.RemainingTime(Runner).HasValue ? tickTimer.RemainingTime(Runner).Value : 0f;
    
    public bool isExpired(NetworkRunner Runner) => tickTimer.ExpiredOrNotRunning(Runner);
}
 