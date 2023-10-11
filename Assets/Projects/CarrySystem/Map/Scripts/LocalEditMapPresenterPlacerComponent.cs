﻿using System.Collections.Generic;
using Carry.CarrySystem.Map.Interfaces;
using VContainer;

namespace Carry.CarrySystem.Map.Scripts
{
    // Composite pattern ?
    public class LocalEditMapPresenterPlacerComponent :IPresenterPlacer
    {
        readonly List<IPresenterPlacer>_presenterBuilders = new ();

        [Inject]
        public  LocalEditMapPresenterPlacerComponent(         
            LocalEditMapBlockPresenterPlacer blockPresenterPlacer,
            RandomWallPresenterPlacerLocal randomWallPresenterPlacerLocal,
            RegularGroundPresenterPlacerLocal regularGroundPresenterPlacerLocal
        )
        {
            _presenterBuilders.Add(blockPresenterPlacer);
            _presenterBuilders.Add(randomWallPresenterPlacerLocal);
            _presenterBuilders.Add(regularGroundPresenterPlacerLocal);
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