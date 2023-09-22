﻿using System.Collections.Generic;
using Carry.CarrySystem.Map.Interfaces;
using VContainer;

namespace Carry.CarrySystem.Map.Scripts
{
    public class CarryPresenterPlacerContainer : IPresenterPlacer
    {
        readonly List<IPresenterPlacer>_presenterBuilders = new ();

        [Inject]
        public  CarryPresenterPlacerContainer(         
            CarryBlockPresenterPlacer blockPresenterPlacer,
            RegularWallPresenterPlacer regularWallPresenterPlacer,
            RegularGroundPresenterPlacer regularGroundPresenterPlacer
            )
        {
            _presenterBuilders.Add(blockPresenterPlacer);
            _presenterBuilders.Add(regularWallPresenterPlacer);
            _presenterBuilders.Add(regularGroundPresenterPlacer);
        }
        
        public void Place(EntityGridMap map)
        {
            foreach (var presenterBuilder in _presenterBuilders)
            {
                presenterBuilder.Place(map);
            }
        }
    }
}