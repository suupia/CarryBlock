#nullable enable
using System.Collections.Generic;
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
            RandomWallPresenterPlacer randomWallPresenterPlacerLocal,
            GroundPresenterPlacer groundPresenterPlacer
        )
        {
            _presenterBuilders.Add(blockPresenterPlacer);
            _presenterBuilders.Add(randomWallPresenterPlacerLocal);
            _presenterBuilders.Add(groundPresenterPlacer);
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