using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

// public interface ITimerObserver
// {
//     void Update(ITimer timer);
// }
//
// public interface ITimer
// {
//     float getRemainingTime();
//     bool isExpired();
// }
public class GameContext
{
    public GameState gameState { get; private set; } = GameState.Playing;
    public enum GameState
    {
        Playing,Result // マップを開いている状態や、ユニット選択の状態などが増えるかも
    }

    public void Update(WaveTimer timer)
    {
        if (timer.isExpired())
        {
            gameState = GameState.Result;
        }
    }
}
