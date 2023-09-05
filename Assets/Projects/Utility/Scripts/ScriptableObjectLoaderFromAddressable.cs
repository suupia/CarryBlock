using System;
using Addler.Runtime.Core.LifetimeBinding;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Projects.Utility.Scripts
{
    public class ScriptableObjectLoaderFromAddressable<T>  where T : ScriptableObject
    {
        readonly string _key;

        public ScriptableObjectLoaderFromAddressable(string key)
        {
            _key = key;
        }

        public (T , AsyncOperationHandle<T>) Load()
        {
            //Addressableでは直接コンポーネントをとってこれない。GameObjectから取得する
            // return typeof(T).IsSubclassOf(typeof(Component)) ? LoadComponent() : LoadDirectory();
            if (typeof(T).IsSubclassOf(typeof(ScriptableObject)))
            {
                return LoadComponent();
            }
            else
            {
                return LoadDirectory();
            }
        }
        
        public void Release(AsyncOperationHandle<T> handler)
        {
            Addressables.Release(handler);
        }
 

        (T , AsyncOperationHandle<T>)  LoadComponent()
        {
            var handler = Addressables.LoadAssetAsync<T>(_key);
            var scriptableObject = handler.WaitForCompletion();
            return (scriptableObject, handler);
        }

        (T , AsyncOperationHandle<T>)  LoadDirectory()
        {
            Debug.LogError($"LoadDirectory key = {_key}");
            var handler = Addressables.LoadAssetAsync<T>(_key);
            var scriptableObject = handler.WaitForCompletion();
            return (scriptableObject, handler);
        }

    }
}