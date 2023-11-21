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
            PlaceablePresenterPlacer blockPresenterPlacer,
            RandomWallPresenterPlacerNet randomWallPresenterPlacerNet,
            RegularGroundPresenterPlacerLocal regularGroundPresenterPlacerLocal
        )
        {
            _presenterBuilders.Add(blockPresenterPlacer);
            _presenterBuilders.Add(randomWallPresenterPlacerNet);
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