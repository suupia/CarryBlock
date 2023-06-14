using Carry.CarrySystem.Player.Interfaces;
using UnityEngine;

namespace Carry.CarrySystem.Player.Scripts
{
    public class CharacterAction : ICharacterAction
    {
        public void Action()
        {
           Debug.Log($"ものを拾ったり、置いたりします");
        }
    }
}