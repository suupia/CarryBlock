using System.Collections.Generic;
using System.Linq;
using Fusion;

namespace Decoration
{
    public struct NetworkDecorationEnemy : INetworkStruct
    {
        public int AttackCount;
    }

    

    public class EnemyDecorationContainer
    {
        private List<IEnemyDecoration> _decorations;
        private int _preAttackCount = 0;

        public EnemyDecorationContainer(params IEnemyDecoration[] decorations)
        {
            _decorations = decorations.ToList();
        }

        public void OnSpawn(NetworkDecorationEnemy networkStruct)
        {
        }
    }
}