using System.Collections.Generic;
using Carry.CarrySystem.Map.Interfaces;
using VContainer;

namespace Carry.CarrySystem.Map.Scripts
{
    // CarryPresenterPlacerContainer とまったく一緒
    public class EditMapPresenterPlacerContainer :IPresenterPlacer
    {
        readonly List<IPresenterPlacer>_presenterBuilders = new ();

        [Inject]
        public  EditMapPresenterPlacerContainer(         
            EditMapBlockPresenterPlacer blockPresenterPlacer,
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