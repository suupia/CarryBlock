using System.Collections.Generic;
using Fusion;
#nullable enable

namespace Carry.CarrySystem.CarryScene.Scripts
{
    public class CarryInitializersReady
    {
        readonly Dictionary<PlayerRef, bool> _initializersReady = new Dictionary<PlayerRef, bool>();
        
        public bool IsAllInitializersReady()
        {
            foreach (var initializerReady in _initializersReady.Values)
            {
                if (!initializerReady)
                {
                    return false;
                }
            }

            return true;
        }
        
        public void AddInitializerReady(PlayerRef playerRef)
        {
            _initializersReady[playerRef] = false;
        }
        
        public void SetInitializerReady(PlayerRef playerRef)
        {
            _initializersReady[playerRef] = true;
        }
    }
}