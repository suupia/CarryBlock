﻿using Fusion;
using UnityEngine;

namespace Main.Tests
{
    public class TestInitializer : SimulationBehaviour
    {
        readonly NetworkEnemyContainer _networkEnemyContainer = new();
        readonly SpawnerTransformContainer _enemySpawnerTransformContainer = new();
        EnemySpawnersController _enemySpawnersController;

        [SerializeField] string overrideSessionName;

        /// <summary>
        /// スポーン位置を決定するためのTransformの配列
        ///
        /// Why not?
        /// シリアライズで受け取っても良いが、ヒューマンエラーを防ぐため、現在は最初にタグから探す方針
        /// タグから探す責任はSpawnerTransformContainerが持っている
        ///
        /// When to consider?
        /// タグで取得するとヒエラルキーが変わったときにTransformの順番が変わる
        /// 順番を一意にしたい場合は、シリアライズのほうが都合がいいだろう
        /// </summary>
        // [SerializeField] List<Transform> spawnTransforms;
        async void Start()
        {
            //Fusion関連の初期化
            var runnerManager = FindObjectOfType<NetworkRunnerManager>();
            await runnerManager.AttemptStartScene(overrideSessionName);
            runnerManager.Runner.AddSimulationBehaviour(this); // Register this class with the runner

            //スポナーの位置を決定しているTransformを取得し、Controllerにわたす
            //ControllerによってTransformの数だけEnemySpawnerがインスタンス化され
            //Controllerがそれらの責任を負う
            _enemySpawnerTransformContainer.AddRangeByTag();
            _enemySpawnersController = new EnemySpawnersController(Runner, _enemySpawnerTransformContainer);

            Debug.Log("Please press F1 to start spawning");
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                _enemySpawnersController.StartSimpleSpawner(_networkEnemyContainer);
                Debug.Log("Spawn Loop was Started");
            }
            else if (Input.GetKeyDown(KeyCode.F2))
            {
                _enemySpawnersController.CancelSpawning();
                Debug.Log("Spawn Loop was Canceled");
            }
            else if (Input.GetKeyDown(KeyCode.F3))
            {
                _enemySpawnersController.StartSimpleSpawner(_networkEnemyContainer,
                    startSimpleSpawnerDelegate: (i, _) => new StartSimpleSpawnerRecord()
                    {
                        Index = 0,
                        Interval = i + 2f
                    });
            }
        }
    }
}