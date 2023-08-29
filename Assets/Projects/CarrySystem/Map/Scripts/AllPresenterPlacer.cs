using System.Collections.Generic;
using Carry.CarrySystem.Map.Interfaces;
using VContainer;

namespace Carry.CarrySystem.Map.Scripts
{
    public class AllPresenterPlacer : IPresenterPlacer
    {
        List< IPresenterPlacer>presenterBuilders = new List<IPresenterPlacer>();

        [Inject]
        public  AllPresenterPlacer(         
            IBlockPresenterPlacer blockPresenterPlacer,
            WallPresenterPlacer wallPresenterPlacer,
            GroundPresenterPlacer groundPresenterPlacer
            )
        {
            presenterBuilders.Add(blockPresenterPlacer);
            presenterBuilders.Add(wallPresenterPlacer);
            presenterBuilders.Add(groundPresenterPlacer);
        }
        
        public void Place(EntityGridMap map)
        {
            foreach (var presenterBuilder in presenterBuilders)
            {
                presenterBuilder.Place(map);
            }
        }
    }
}