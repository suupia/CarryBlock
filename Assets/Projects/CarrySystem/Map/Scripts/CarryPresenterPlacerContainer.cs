using System.Collections.Generic;
using Carry.CarrySystem.Map.Interfaces;
using VContainer;

namespace Carry.CarrySystem.Map.Scripts
{
    public class CarryPresenterPlacerContainer : IPresenterPlacer
    {
        readonly List<IPresenterPlacer>_presenterBuilders = new ();

        [Inject]
        public  CarryPresenterPlacerContainer(         
            IBlockPresenterPlacer blockPresenterPlacer,
            WallPresenterPlacer wallPresenterPlacer,
            GroundPresenterPlacer groundPresenterPlacer
            )
        {
            _presenterBuilders.Add(blockPresenterPlacer);
            _presenterBuilders.Add(wallPresenterPlacer);
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