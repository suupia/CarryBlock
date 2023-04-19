using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class GameContext
{
    WaveTimer _waveTimer;
    public GameState gameState { get; private set; } = GameState.Playing;
    public enum GameState
    {
        Playing,Result // マップを開いている状態や、ユニット選択の状態などが増えるかも
    }

    
    [Inject]
    public  GameContext(WaveTimer waveTimer)
    {
        _waveTimer = waveTimer;
    }
}
