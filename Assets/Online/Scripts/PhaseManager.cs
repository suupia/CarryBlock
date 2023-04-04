using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Phase
{
    Matching,
    Starting, //カウントダウン中
    Gaming,
    Ending, //リザルトに行く前の何か
    Result,
}

/// <summary>
/// Phase will be set
/// - SetPhase
/// - OnTimeExpired
/// - SceneLoadDone
/// </summary>

public class PhaseManager : SimulationBehaviour, ISceneLoadDone
{
    [Networked] Phase Phase { get; set; }

    [Networked] TickTimer Timer { get; set; }

    public override void FixedUpdateNetwork()
    {
        if (Runner.IsServer)
        {
            OnTimerExpired();
        }
    }

    void OnTimerExpired()
    {
        if (Timer.Expired(Runner))
        {
            switch (Phase)
            {
                case Phase.Starting:
                    Phase = Phase.Gaming;
                    break;

                case Phase.Ending:
                    Runner.SetActiveScene(SceneName.LobbyScene);
                    break;

                default: break;
            }

        }
    }

    public void SetPhase(Phase phase)
    {
        if (Runner.IsServer)
        {
            Phase = phase;
            Debug.Log(Phase);

            switch (Phase)
            {
                case Phase.Starting:
                    Runner.SetActiveScene(SceneName.GameScene);
                    break;

                case Phase.Ending:
                    //Timer = TickTimer.CreateFromSeconds(Runner, 3);
                    Timer = TickTimer.CreateFromSeconds(Runner, 0.1f);
                    break;

                default: break;
            }
        }
    }

    public void SceneLoadDone()
    {
        var currentScene = SceneManager.GetActiveScene();

        Debug.Log($"Loaded {currentScene.name}");

        if (currentScene.name == SceneName.GameScene)
        {
            Timer = TickTimer.CreateFromSeconds(Runner, 3);
        }
        else if (currentScene.name == SceneName.LobbyScene)
        {
            Phase = Phase.Matching;
        }
    }
}
