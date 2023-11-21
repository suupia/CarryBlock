﻿using System.Collections.Generic;
using Carry.CarrySystem.Map.Interfaces;
using VContainer;

namespace Carry.CarrySystem.Map.Scripts
{
    public class EditMapPresenterPlacerComposite :IPresenterPlacer
    {
        readonly List<IPresenterPlacer>_presenterBuilders = new ();

        [Inject]
        public  EditMapPresenterPlacerComposite(         
            PlaceablePresenterPlacer blockPresenterPlacer,
            RandomWallPresenterPlacer randomWallPresenterPlacer,
            RegularGroundPresenterPlacerLocal regularGroundPresenterPlacerLocal
        )
        {
            _presenterBuilders.Add(blockPresenterPlacer);
            _presenterBuilders.Add(randomWallPresenterPlacer);
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