using System;
using Addler.Runtime.Core.LifetimeBinding;
using Carry.Utility.Interfaces;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

namespace Carry.Utility.Scripts
{
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