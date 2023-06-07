using Fusion;
using UnityEngine;

namespace Main
{
    public class ComponentPrefabInstantiate<T> where  T : Component
    {
        readonly IPrefabLoader<T> _prefabLoader;
        readonly string _prefabName;

        public ComponentPrefabInstantiate(IPrefabLoader<T> prefabLoader, string prefabName)
        {
            _prefabLoader = prefabLoader;
            _prefabName = prefabName;
        }

        public T InstantiatePrefab(Vector3 position, Quaternion rotation, Transform parent = null)
        {
            var prefab = _prefabLoader.Load(_prefabName);
            var gameObj = Object.Instantiate(prefab, position, rotation, parent);
            var component = gameObj.GetComponent<T>();
            if(component == null) Debug.LogError($"Instantiated object does not have component. T = {typeof(T)}");
            return component;
        }
        public T InstantiatePrefab(Transform parent)
        {
            var prefab = _prefabLoader.Load(_prefabName);
            var gameObj = Object.Instantiate(prefab, parent);
            var component = gameObj.GetComponent<T>();
            if(component == null) Debug.LogError($"Instantiated object does not have component. T = {typeof(T)}");
            return component;
        }
    }

}