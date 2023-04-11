using System;
using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;


public class NetworkEnemySpawner
{
    readonly NetworkRunner _runner;
    readonly CancellationTokenSource _cts = new();
    readonly CancellationToken _token;

    public NetworkEnemySpawner(NetworkRunner runner)
    {
        this._runner = runner;
        _token = _cts.Token;
    }

    public void CancelSpawning()
    {
        _cts.Cancel();
    }
    
    public async UniTask StartSimpleSpawner(int index, float interval, NetworkEnemyContainer enemyContainer)
    {
        while (true)
        {
            SpawnEnemy(index,enemyContainer);
            await UniTask.Delay(TimeSpan.FromSeconds(interval),cancellationToken: _token);
        }
    }
    void SpawnEnemy(int index, NetworkEnemyContainer enemyContainer)
    {
        if(enemyContainer.Enemies.Count() >= enemyContainer.MaxEnemyCount)return;;
        var position = new Vector3(0, 1, 0);
        var enemyPrefabs = Resources.LoadAll<NetworkEnemyController>("Prefabs/Enemys");
        var networkObject = _runner.Spawn(enemyPrefabs[index], position, Quaternion.identity, PlayerRef.None);
        var enemy = networkObject.GetComponent<NetworkEnemyController>();
        enemy.onDespawn += () => enemyContainer.RemoveEnemy(enemy);
        enemyContainer.AddEnemy(enemy);
    }
    

}

public class NetworkEnemyContainer
{
    List<NetworkEnemyController> enemies = new();
    public int MaxEnemyCount { get; set; } = 128;
    public IEnumerable<NetworkEnemyController> Enemies => enemies;

    public void AddEnemy(NetworkEnemyController enemyController)
    {
        enemies.Add(enemyController);
    }

    public void RemoveEnemy(NetworkEnemyController enemyController)
    {
        enemies.Remove(enemyController);
    }
}