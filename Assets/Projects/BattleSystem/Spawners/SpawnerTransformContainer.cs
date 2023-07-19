using System.Collections.Generic;
using Fusion;
using UnityEngine;

namespace Projects.BattleSystem.Spawners.Scripts
{
    public class SpawnerTransformContainer
    {
        readonly string _spawnerTransformTagName = "SpawnerTransform";

        readonly List<Transform> _transforms;
        public IEnumerable<Transform> Transforms => _transforms;

        public SpawnerTransformContainer(string spawnerTransformTagName)
        {
            _spawnerTransformTagName = spawnerTransformTagName;
            _transforms = new List<Transform>();
        }

        public SpawnerTransformContainer(List<Transform> transforms)
        {
            _transforms = transforms;
        }

        public void AddRangeByTag()
        {
            var transforms = GameObject
                .FindGameObjectsWithTag(_spawnerTransformTagName)
                .Map(g => g.transform);

            _transforms.AddRange(transforms);
        }
    }

    //Why not?
    //個人的には以下のが楽で良いが、たしかにListの機能をすべて受け継いでしまい、
    //いつでも変更可能になったり、チーム開発においてバグへの第一歩という感じはする
    //よって、森田くんの実装に習って、機能を絞り、上記のように実装した
    // public class SpawnerTransformContainer: List<Transform>
    // {
    //     const string SpawnerTransformTagName = "SpawnerTransform";
    //
    //     public void AddRangeByTag(string tagName = SpawnerTransformTagName)
    //     {
    //         var transforms = GameObject
    //             .FindGameObjectsWithTag(tagName)
    //             .Map(g => g.transform);
    //
    //         AddRange(transforms);
    //     }
    // }
}