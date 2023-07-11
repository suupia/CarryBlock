using UnityEngine;

namespace Nuts.Utility.Scripts
{
    public interface IPrefabLoader<out T> where T : Object
    {
        T Load();
        T[] LoadAll();
    }


// Other classes can be created by implementing IPrefabLoader, such as PrefabLoaderFromAssetBundle and PrefabLoaderFromStreamingAssets.
    public class PrefabLoaderFromResources<T> : IPrefabLoader<T> where T : Object
    {
        readonly string _folderPath;
        readonly string _prefabName;

        public PrefabLoaderFromResources(string folderPath, string prefabName)
        {
            _folderPath = folderPath;
            _prefabName = prefabName;
        }

        public T Load()
        {
            var result = Resources.Load<T>(_folderPath + "/" + _prefabName);
            if (result == null)
            {
                Debug.LogError($"Failed to load prefab. folderPath ={_folderPath+"/"+_prefabName} prefabName = {_prefabName}");
                return null;
            }
            else
            {
                return result;
            }
        }

        public T[] LoadAll()
        {
            var result = Resources.LoadAll<T>(_folderPath );
            if (result == null)
            {
                Debug.LogError($"Failed to load prefab. folderPath ={_folderPath}");
                return null;
            }
            else
            {
                return result;
            }
        }
    }
}