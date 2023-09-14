using System.Collections.Generic;
using Carry.CarrySystem.Player.Scripts;
using Fusion;
using UnityEngine;

namespace Carry.CarrySystem.Player.Scripts
{
    /// <summary>
    /// ロビーシーンからゲームシーンに遷移する際に、PlayerRefとPlayerColorTypeの組を保持するクラス
    /// 他にも保持したい状態があったらこのクラスに追加する
    /// 追加:PlayerRefとPlayerIndexの組を保持する
    /// </summary>
    public class PlayerCharacterHolder
    {
        readonly Dictionary<PlayerRef, PlayerColorType> _colorDictionary = new Dictionary<PlayerRef, PlayerColorType>();
        readonly Dictionary<PlayerRef, int> _playerIndexDictionary = new Dictionary<PlayerRef, int>();

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
             Debug.Log($"Registering playerRef:{playerRef} as {colorType}");
            _colorDictionary[playerRef] =  colorType;
        }
        
        public int GetPlayerIndex(PlayerRef playerRef)
        {
            if (_playerIndexDictionary.TryGetValue(playerRef, out int index))
            {
                // Debug.Log($"GetPlayerIndex playerRef:{playerRef} is {index+1}P");
                return index;
            }
            return 0;
        }
        public void SetIndex(PlayerRef playerRef)
        {
             Debug.Log($"Registering playerRef:{playerRef} as {_playerIndexDictionary.Count+1}P");
            _playerIndexDictionary.Add(playerRef, _playerIndexDictionary.Count+1);
        }
    }
}