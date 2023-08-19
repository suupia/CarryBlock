using Carry.CarrySystem.Player.Info;
using Carry.CarrySystem.Player.Scripts;
using Fusion;
using UnityEngine;

#nullable enable

namespace Carry.CarrySystem.Player.Interfaces
{
    public abstract class AbstractNetworkPlayerController : NetworkBehaviour
    {
        [SerializeField] protected Transform unitObjectParent= null!; // The NetworkCharacterControllerPrototype interpolates this transform.

        [SerializeField] protected GameObject[] playerUnitPrefabs= null!;

        [SerializeField] protected PlayerInfo info= null!;

        [Networked] protected NetworkButtons PreButtons { get; set; }
        [Networked] public NetworkBool IsReady { get; set; }

        [Networked] protected PlayerColorType ColorType { get; set; } // ローカルに反映させるために必要

        protected GameObject _characterObj= null!;
        public ICharacter Character => character;

        protected ICharacter? character;

    }
}