using Fusion;
using System;
using System.Linq;
using UnityEngine;

namespace Main
{
    public interface IEnemy : IMove
    {
        void Move(Vector3 direction);
        void Action();
        bool InAction();
        float ActionCooldown();
    }

    public interface IEnemyStats
    {
        void OnAttacked(ref NetworkPlayerStruct networkPlayerStruct,int damage);
    }
    public class EnemyInfo
    {
        
    }
}