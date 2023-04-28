using UnityEngine;

namespace Decoration
{
    public interface IDecoration
    {
        void OnMove();
        void OnDamage();
        void OnDead();
        void OnSpawn();
        void OnAttack(bool value = true);

    }
    
    public interface IDecorationEnemy: IDecoration {}

    public interface IDecorationPlayer: IDecoration
    {
        void OnMainAction();
    }
}