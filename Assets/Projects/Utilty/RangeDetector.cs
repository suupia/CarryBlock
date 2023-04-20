using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public class RangeDetector : MonoBehaviour
    {
        List<GameObject> gameObjects = new();

        public GameObject[] GameObjects => gameObjects.ToArray();

        private void OnTriggerEnter(Collider other)
        {
            gameObjects.Add(other.gameObject);
        }

        private void OnTriggerExit(Collider other)
        {
            gameObjects.Remove(other.gameObject);
        }
    }

}
