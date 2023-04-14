using UnityEngine;

public interface IAnimatorMove
{
    /// <summary>
    /// Needs some param
    /// </summary>
    /// <param name="direction">Used for manage body animation</param>
    void OnMove(Vector3 direction);
}

public interface IAnimatorDead
{
    void OnDead();
}

public interface IAnimatorSpawn
{
    void OnSpawn();
}

public interface IAnimatorAttack
{
    void OnAttack();
}

public interface IAnimatorPlayerUnit: IAnimatorAttack, IAnimatorDead, IAnimatorMove, IAnimatorSpawn
{
    void OnMainAction();
}

public interface IAnimatorSimpleEnemyUnit : IAnimatorAttack, IAnimatorDead, IAnimatorMove, IAnimatorSpawn
{
    
}
