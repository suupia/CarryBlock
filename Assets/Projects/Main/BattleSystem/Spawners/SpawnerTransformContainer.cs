using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using UnityEngine;

namespace Main
{
    public class SpawnerTransformContainer: List<Transform>
    {
        const string SpawnerTransformTagName = "SpawnerTransform";

        public void AddRangeByTag(string tagName = SpawnerTransformTagName)
        {
            var transforms = GameObject
                .FindGameObjectsWithTag(tagName)
                .Map(g => g.transform);

            AddRange(transforms);
        }
    }
}