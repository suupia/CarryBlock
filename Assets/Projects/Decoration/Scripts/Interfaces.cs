using UnityEngine;

namespace Decoration
{
    public interface IForwardDecoration
    {
        void OnChangeDirection(Vector3 direction);
    }

    public interface IMoverDecoration
    {
        void OnMoved();
    }

    public interface IHurtDecoration
    {
        void OnDamaged();
        void OnDead();
    }

    public interface IHealDecoration
    {
        void OnHealed();
    }

    public interface ISpawnedDecoration
    {
        void OnSpawned();
    }

    public interface IAttackerDecoration
    {
        void OnAttacked(bool onStart = true);
    }

    public interface IEntityDecoration : IMoverDecoration, IHurtDecoration, ISpawnedDecoration
    {
        // void OnInput(); //入力に呼応するデコレーションもあると思う
    }

    public interface IEnemyDecoration : IEntityDecoration, IAttackerDecoration
    {
    }

    public interface IPlayerDecoration : IEntityDecoration, IForwardDecoration, IAttackerDecoration
    {
        void OnMainAction();
    }

    public interface IBoss1Decoration : IEntityDecoration
    {
        void OnTackle(bool onStart);
        void OnJump(bool onStart);
    }
}