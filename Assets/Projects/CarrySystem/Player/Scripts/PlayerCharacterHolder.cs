using System.Collections.Generic;
using Carry.CarrySystem.Player.Scripts;
using Fusion;
using UnityEngine;

namespace Carry.CarrySystem.Player.Scripts
{
    public class PlayerCharacterHolder
    {
        readonly Dictionary<PlayerRef, PlayerColorType> _colorDictionary = new Dictionary<PlayerRef, PlayerColorType>();

        public PlayerColorType GetPlayerColorType(PlayerRef playerRef)
        {
            if (_colorDictionary.TryGetValue(playerRef, out PlayerColorType colorType))
            {
                // Debug.Log($"GetPlayerColorType playerRef:{playerRef} is {colorType}");
                return colorType;
            }
            return (PlayerColorType)0;
        }
        public void SetColor(PlayerRef playerRef, PlayerColorType colorType)
        {
            // Debug.Log($"Registering playerRef:{playerRef} as {colorType}");
            _colorDictionary[playerRef] = colorType;
        }
    }
}