using System;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Map.Scripts;

#nullable enable

namespace Projects.MapMakerSystem.Scripts
{
    public class StageMapSwitcher: IMapGetter, IMapSwitcher
    {
        public int Index { get; set;  }
        
        public Stage Stage { get; set; }
        
        readonly IEntityGridMapBuilder _entityGridMapBuilder;
        readonly IPresenterPlacer _presenterPlacer;
        
        EntityGridMap _currentMap;
        Action _resetAction = () => { };

        public StageMapSwitcher(IEntityGridMapBuilder entityGridMapBuilder, IPresenterPlacer presenterPlacer)
        {
            _entityGridMapBuilder = entityGridMapBuilder;
            _presenterPlacer = presenterPlacer;
        }
        
        public EntityGridMap GetMap()
        {
            return _currentMap;
        }

        public void InitSwitchMap()
        {
            var info = Stage.mapInfos[Index];
            _currentMap = _entityGridMapBuilder.BuildEntityGridMap(info.data);
            _presenterPlacer.Place(_currentMap);
            
            _resetAction();
        }

        public void SwitchMap()
        {
            var info = Stage.mapInfos[Index];
            _currentMap = _entityGridMapBuilder.BuildEntityGridMap(info.data);
            _presenterPlacer.Place(_currentMap);
            
            _resetAction();
        }

        public void RegisterResetAction(Action action)
        {
            _resetAction += action;
        }
    }
}