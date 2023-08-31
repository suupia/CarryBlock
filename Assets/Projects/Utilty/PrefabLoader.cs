﻿using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;
using Addler.Runtime.Core.LifetimeBinding;

namespace Projects.Utility.Scripts
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

    public class PrefabLoaderFromAddressable<T> : IPrefabLoader<T> where T : Object
    {
        readonly string _path;

        public PrefabLoaderFromAddressable(string path)
        {
            _path = path;
        }

        public T Load()
        {
            //Addressableでは直接コンポーネントをとってこれない。GameObjectから取得する
            // return typeof(T).IsSubclassOf(typeof(Component)) ? LoadComponent() : LoadDirectory();
                if (typeof(T).IsSubclassOf(typeof(Component)))
                {
                    return LoadComponent();
                }
                else
                {
                    return LoadDirectory();
                }
        }
 

        T LoadComponent()
        {
            var handler = Addressables.LoadAssetAsync<GameObject>(_path);
            var gameObject = handler.WaitForCompletion();
            var component = gameObject.GetComponent<T>();
             handler.BindTo(gameObject);
            return component;
        }

        T LoadDirectory()
        {
            Debug.LogWarning($"LoadDirectory key = {_path}");
            var handler = Addressables.LoadAssetAsync<T>(_path);
            var value = handler.WaitForCompletion();
            Addressables.Release(handler);
            return value;
        }

        public T[] LoadAll()
        {
            throw new NotImplementedException();
        }
    }
    
}