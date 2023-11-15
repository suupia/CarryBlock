﻿
#nullable enable
using Carry.CarrySystem.Map.Scripts;

namespace Carry.CarrySystem.Map.Interfaces
{
    public interface IMapUpdater
    {
        void InitUpdateMap(MapKey mapKey, int index);
        void UpdateMap(MapKey mapKey, int index = 0);
        
        void RegisterResetAction(System.Action action);
    }
}