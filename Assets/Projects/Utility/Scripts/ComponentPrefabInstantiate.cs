using Carry.Utility.Interfaces;
using Carry.Utility.Scripts;
using UnityEngine;

namespace Carry.GameSystem.Spawners.Scripts
{
    public class ComponentPrefabInstantiate<T> where  T : Component
    {
        readonly IPrefabLoader<T> _prefabLoader;

        public ComponentPrefabInstantiate(IPrefabLoader<T> prefabLoader)
        {
            _prefabLoader = prefabLoader;
        }

        public T InstantiatePrefab(Vector3 position, Quaternion rotation, Transform parent = null)
        {
            var prefab = _prefabLoader.Load();
            var gameObj = Object.Instantiate(prefab, position, rotation, parent);
            var component = gameObj.GetComponent<T>();
            if(component == null) Debug.LogError($"Instantiated object does not have component. T = {typeof(T)}");
            return component;
        }
        public T InstantiatePrefab(Transform parent)
        {
            var prefab = _prefabLoader.Load();
            var gameObj = Object.Instantiate(prefab, parent);
            var component = gameObj.GetComponent<T>();
            if(component == null) Debug.LogError($"Instantiated object does not have component. T = {typeof(T)}");
            return component;
        }
    }

}