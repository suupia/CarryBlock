# nullable  enable
using System.Collections.Generic;
using UnityEngine;

namespace Nuts.BattleSystem.Enemy.Monster.Interfaces
{


    public interface IEnemySearch
    {
        Transform[]?  Search();
        Transform? DetermineTarget(IEnumerable<Transform> targetUnits); // Moveのターゲットなどに使われる
        // もし、Action側で複数のTransformを受け取る必要がある場合は、IEnemyTargetsActionExecutorとDetermineTargetsを作成する

    }

}