using Fusion;
using UnityEngine;
using System;
using Animations;
using VContainer;
using VContainer.Unity;

namespace Main
{
    /// <summary>
    /// The only NetworkBehaviour to control the character.
    /// Note: Objects to which this class is attached do not move themselves.
    /// Attachment on the inspector is done to the Info class.
    /// </summary>
    public class LobbyNetworkPlayerController : AbstractNetworkPlayerController
    {
    }
}