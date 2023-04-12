using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using System;
using System.Linq;

public interface IPoolableObject
{
    void OnInactive(); // Please make it sure that it is called by Monobehaviour.OnDisable().
}
public class NetworkObjectPoolDefault : MonoBehaviour, INetworkObjectPool
{
    [SerializeField] Transform _poolParent;

    [Tooltip("The objects to be pooled, leave it empty to pool every Network Object spawned")] 
    [SerializeField] List<NetworkPrefabRef> _poolableObjects; // Todo: Check the attached elements implements IPoolableObject.

    readonly Dictionary<NetworkPrefabId, Stack<NetworkObject>> _free = new();
    
    public NetworkObject AcquireInstance(NetworkRunner runner, NetworkPrefabInfo info)
    {
        if (ShouldPool(runner, info))
        {
            var instance = GetObjectFromPool(runner, info);

            instance.transform.position = Vector3.zero;

            return instance;
        }

        return InstantiateNonPoolableObject(runner, info);
    }

    public void ReleaseInstance(NetworkRunner runner, NetworkObject instance, bool isSceneObject)
    {
        if (isSceneObject)
        {
            Destroy(instance.gameObject);
            return;
        }

        if (runner.Config.PrefabTable.TryGetId(instance.NetworkGuid, out var prefabId))
        {
            if (_free.TryGetValue(prefabId, out var stack))
            {
                instance.gameObject.SetActive(false);

                // reset parenting
                instance.transform.SetParent(_poolParent);
                
                stack.Push(instance);
            }
            else
            {
                Destroy(instance.gameObject);
            }

            return;
        }

        // If no prefabId was found
        Destroy(instance.gameObject);
    }
    NetworkObject GetObjectFromPool(NetworkRunner runner, NetworkPrefabInfo info)
    {
        NetworkObject instance = null;

        if (_free.TryGetValue(info.Prefab, out var stack))
        {
            while (stack.Count > 0 && instance == null)
            {
                instance = stack.Pop();
            }
        }

        if (instance == null)
            instance = GetNewInstance(runner, info);

        instance.gameObject.SetActive(true);
        return instance;
    }
    NetworkObject GetNewInstance(NetworkRunner runner, NetworkPrefabInfo info)
    {
        NetworkObject instance = InstantiatePoolableObject(runner, info);

        if (_free.TryGetValue(info.Prefab, out var stack) == false)
        {
            stack = new Stack<NetworkObject>();
            _free.Add(info.Prefab, stack);
        }

        return instance;
    }
    NetworkObject InstantiatePoolableObject(NetworkRunner runner, NetworkPrefabInfo info)
    {
        if (runner.Config.PrefabTable.TryGetPrefab(info.Prefab, out var prefab))
        {
            return Instantiate(prefab, _poolParent);
        }

        Debug.LogError("No prefab for " + info.Prefab);
        return null;
    }
    NetworkObject InstantiateNonPoolableObject(NetworkRunner runner, NetworkPrefabInfo info)
    {
        if (runner.Config.PrefabTable.TryGetPrefab(info.Prefab, out var prefab))
        {
            return Instantiate(prefab);
        }

        Debug.LogError("No prefab for " + info.Prefab);
        return null;
    }
    bool ShouldPool(NetworkRunner runner, NetworkPrefabInfo info)
    {
        if (runner.Config.PrefabTable.TryGetPrefab(info.Prefab, out var networkObject))
        {
            if (_poolableObjects.Count == 0)
            {
                return true;
            }

            if (IsPoolableObject(networkObject))
            {
                return true;
            }
        }
        else
        {
            Debug.LogError("No prefab found.");
        }

        return false;
    }
    bool IsPoolableObject(NetworkObject networkObject)
    {
        foreach (var poolableObject in _poolableObjects)
        {
            if ((Guid)poolableObject == (Guid)networkObject.NetworkGuid)
                return true;
        }

        return false;
    }
}