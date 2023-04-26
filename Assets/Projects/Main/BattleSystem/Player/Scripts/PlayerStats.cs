using Fusion;
using UnityEngine;

namespace Main
{
    public struct NetworkPlayerStruct : INetworkStruct
    {
        public int hp { get; set; }
    }

    public class PlayerStats : IUnitStats
    {
        public readonly int MaxHp = 3;
        
        public PlayerStats(ref NetworkPlayerStruct stats)
        {
            stats.hp = MaxHp;
        }
        
        public void OnAttacked(ref NetworkPlayerStruct stats,int damage)
        {
            stats.hp -= damage;
            Debug.Log($"Player HP = {stats.hp}");
            if (stats.hp <= 0)
            {
                //ToDo : dead
                return;
            }
        }
    }
}