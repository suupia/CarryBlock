using System;
using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;


public class NetworkEnemyContainer : SimulationBehaviour
{
    // [SerializeField] NetworkEnemy[] enemyPrefabs;

    List<NetworkEnemy> enemies = new();

    public NetworkEnemy[] Enemies => enemies.ToArray();
    public int MaxEnemyCount { get; set; } = 128;

    public IEnumerable<NetworkEnemy> Enemies2 => enemies;

    void SpawnEnemy(int index)
    {
        if (MaxEnemyCount > enemies.Count)
        {
            var position = new Vector3(0, 1, 0);
            var enemyPrefabs = Resources.LoadAll<NetworkEnemy>("Prefabs/Enemys");
            var no = Runner.Spawn(enemyPrefabs[index], position, Quaternion.identity, PlayerRef.None);
            var enemy = no.GetComponent<NetworkEnemy>();
            enemy.OnDespawn += () => enemies.Remove(enemy);
            enemies.Add(enemy);
        }
    }

    // IEnumerator SimpleSpawner(int index, float interval)
    // {
    //     while (true)
    //     {
    //         SpawnEnemy(index);
    //         yield return new WaitForSeconds(interval);
    //     }
    // }
    //
    // public void StartSimpleSpawner(int index, float interval)
    // {
    //     var spawner = SimpleSpawner(index, interval);
    //     StartCoroutine(spawner);
    // }
    
    public async void StartSimpleSpawner(int index, float interval)
    {
        while (true)
        {
            SpawnEnemy(index);
            await UniTask.Delay(TimeSpan.FromSeconds(interval));
        }
    }

    public void AddEnemy(NetworkEnemy enemy)
    {
        enemies.Add(enemy);
    }

    public void RemoveEnemy(NetworkEnemy enemy)
    {
        enemies.Remove(enemy);
    }
}

public class NetworkEnemySpawner
{
    readonly NetworkRunner runner;
    readonly CancellationTokenSource cts = new();
    readonly CancellationToken token;

    public NetworkEnemySpawner(NetworkRunner runner)
    {
        this.runner = runner;
        token = cts.Token;
    }

    void SpawnEnemy(int index, NetworkEnemyContainer enemyContainer)
    {
        var position = new Vector3(0, 1, 0);
        var enemyPrefabs = Resources.LoadAll<NetworkEnemy>("Prefabs/Enemys");
        var networkObject = runner.Spawn(enemyPrefabs[index], position, Quaternion.identity, PlayerRef.None);
        var enemy = networkObject.GetComponent<NetworkEnemy>();
        enemy.OnDespawn += () => enemyContainer.RemoveEnemy(enemy);
        enemyContainer.AddEnemy(enemy);
    }
    



}