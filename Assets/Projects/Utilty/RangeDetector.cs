using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public class RangeDetector : MonoBehaviour
    {
        readonly List<GameObject> gameObjects = new();

        public GameObject[] GameObjects => gameObjects.ToArray();

        void OnTriggerEnter(Collider other)
        {
            gameObjects.Add(other.gameObject);
        }

        void OnTriggerExit(Collider other)
        {
            gameObjects.Remove(other.gameObject);
        }
    }
}