﻿#nullable enable
using Carry.CarrySystem.Map.Interfaces;
using UnityEngine;

namespace Carry.CarrySystem.Spawners.Interfaces
{
    public interface IWallPresenterSpawner
    {
        public IPresenterMono SpawnPrefab(Vector3 position, Quaternion rotation);
    }
} 