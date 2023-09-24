using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Fusion;
using Carry.NetworkUtility.NetworkRunnerManager.Scripts;
using UnityEngine;

public class SetActiveTestInitializer : SimulationBehaviour
{
    [SerializeField] NetworkPrefabRef setActiveTestPrefabRef;
    bool _isInitialized;
    async void Start()
    {
        var runnerManager = FindObjectOfType<NetworkRunnerManager>();
        // Runner.StartGame() if it has not been run.
        await runnerManager.AttemptStartScene("SetActiveTest");
        runnerManager.Runner.AddSimulationBehaviour(this); // Register this class with the runner
        await UniTask.WaitUntil(() => Runner.SceneManager.IsReady(Runner),
            cancellationToken: new CancellationToken());


        Runner.Spawn(setActiveTestPrefabRef,Vector3.zero, Quaternion.identity,Runner.LocalPlayer);
        
        _isInitialized = true;

    }

    // Update is called once per frame
    void Update()
    {
        if(!_isInitialized) return;
    }
}
