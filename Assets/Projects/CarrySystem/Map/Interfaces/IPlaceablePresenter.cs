﻿using System.Collections.Generic;
using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.Entity.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using UnityEngine;

#nullable enable

namespace Carry.CarrySystem.Map.Interfaces
{
    public interface  IPlaceablePresenter : IPresenterMono
    {
        public void SetInitAllEntityActiveData(IEnumerable<IEntity> allEntities);
        public void SetEntityActiveData(IEntity entity, int count);

    }
}