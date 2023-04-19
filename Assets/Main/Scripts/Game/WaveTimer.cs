using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using VContainer;

public class WaveTimer : NetworkBehaviour 
{
    [Networked] TickTimer waveTimer { get; set; }
    GameContext _observer;

    public float getRemainingTime() =>  waveTimer.RemainingTime(Runner).HasValue ? waveTimer.RemainingTime(Runner).Value : 0f;
    
    public bool isExpired() => waveTimer.ExpiredOrNotRunning(Runner);

    readonly float _waveTime = 60f;

    [Inject]
    public void Construct(GameContext observer)
    {
        _observer = observer;
    }

    public void Init()
    {
        waveTimer = TickTimer.CreateFromSeconds(Runner, _waveTime);
    }

    public override void FixedUpdateNetwork()
    {
        _observer.Update(this);
    }
    
}
 