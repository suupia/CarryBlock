using UnityEngine;

namespace Carry.Utility.Interfaces
{
    public interface IPrefabLoader<out T> where T : Object
    {
        T Load();
        T[] LoadAll();
    }

}