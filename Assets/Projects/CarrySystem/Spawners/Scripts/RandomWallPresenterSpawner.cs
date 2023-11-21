#nullable enable
using System.Collections.Generic;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Spawners.Interfaces;
using UnityEngine;

namespace Carry.CarrySystem.Spawners.Scripts
{
    public class RandomWallPresenterSpawner : IWallPresenterSpawner
    {
        readonly IList<IWallPresenterSpawner> _wallPresenterSpawners = new List<IWallPresenterSpawner>();

        public void AddSpawner(IWallPresenterSpawner wallPresenterSpawner)
        {
            _wallPresenterSpawners.Add(wallPresenterSpawner);
        }

        public IPresenterMono SpawnPrefab(Vector3 position, Quaternion rotation)
        {
            var random = Random.Range(0, _wallPresenterSpawners.Count);
            return _wallPresenterSpawners[random].SpawnPrefab(position, rotation);
        }

    }
}