using System.Collections.Generic;
using Carry.CarrySystem.Map.Interfaces;
using VContainer;

namespace Carry.CarrySystem.Map.Scripts
{
    public class LobbyPresenterPlacerContainer : IPresenterPlacer
    {
        readonly List<IPresenterPlacer>_presenterBuilders = new ();

        [Inject]
        public  LobbyPresenterPlacerContainer(         
            LobbyWallPresenterPlacer regularWallPresenterPlacer,
            LobbyGroundPresenterPlacer regularGroundPresenterPlacer
        )
        {
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