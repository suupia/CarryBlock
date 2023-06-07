using Fusion;
using UnityEngine;

namespace Main
{
    public struct NetworkPlayerStruct : INetworkStruct
    {
        public NetworkBool IsAlive;
        public int Hp { get; set; }
    }

    public interface IUnitStats
    {
        void OnAttacked(ref NetworkPlayerStruct networkPlayerStruct, int damage);
    }

    public class PlayerStats : IUnitStats
    {
        public readonly int MaxHp = 3;

        public PlayerStats(ref NetworkPlayerStruct stats)
        {
            stats.IsAlive = true;
            stats.Hp = MaxHp;
        }

        public void OnAttacked(ref NetworkPlayerStruct stats, int damage)
        {
            stats.Hp -= damage;
            Debug.Log($"Player HP = {stats.Hp}");
            if (stats.Hp <= 0)
                //ToDo : dead
                stats.IsAlive = false;
        }
    }
}