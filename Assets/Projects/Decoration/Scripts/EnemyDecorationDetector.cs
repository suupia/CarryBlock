using System.Collections.Generic;
using System.Linq;
using Fusion;

namespace Decoration
{
    public class EnemyDecorationDetector
    {
        List<IEnemyDecoration> _decorations;
        // private int _preAttackCount = 0;

        public EnemyDecorationDetector(params IEnemyDecoration[] decorations)
        {
            _decorations = decorations.ToList();
        }

        public void OnSpawn(ref Data data)
        {
        }

        public struct Data : INetworkStruct
        {
            public int AttackCount;
        }
    }
}