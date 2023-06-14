using UnityEngine;

namespace Nuts.Utility.Scripts
{
    public interface IPrefabLoader<out T> where T : Object
    {
        T Load(string prefabName);
    }


// Other classes can be created by implementing IPrefabLoader, such as PrefabLoaderFromAssetBundle and PrefabLoaderFromStreamingAssets.
    public class PrefabLoaderFromResources<T> : IPrefabLoader<T> where T : Object
    {
        readonly string _folderPath;

        public PrefabLoaderFromResources(string folderPath)
        {
            _folderPath = folderPath;
        }

        public T Load(string prefabName)
        {
            var result = Resources.Load<T>(_folderPath + "/" + prefabName);
            if (result == null)
            {
                Debug.LogError($"Failed to load prefab. folderPath ={_folderPath+"/"+prefabName} prefabName = {prefabName}");
                return null;
            }
            else
            {
                return result;
            }
        }
    }
}