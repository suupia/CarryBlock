using System.Collections.Generic;
using System.Linq;
using Fusion;

namespace Decoration
{
    public class EnemyDecorationContainer
    {
        public struct Data : INetworkStruct
        {
            public int AttackCount;
        }

        private List<IEnemyDecoration> _decorations;
        // private int _preAttackCount = 0;

        public EnemyDecorationContainer(params IEnemyDecoration[] decorations)
        {
            _decorations = decorations.ToList();
        }

        public void OnSpawn(ref Data data)
        {
        }
    }
}