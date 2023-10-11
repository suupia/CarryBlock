using System.Collections.Generic;
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
            RandomWallPresenterPlacer randomWallPresenterPlacer,
            RegularGroundPresenterPlacer regularGroundPresenterPlacer
        )
        {
            _presenterBuilders.Add(blockPresenterPlacer);
            _presenterBuilders.Add(randomWallPresenterPlacer);
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