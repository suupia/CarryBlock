using System;
using System.Collections.Generic;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using VContainer;

namespace Projects.MapMakerSystem.Scripts
{
    public class StageMapSwitcher: IMapGetter, IMapSwitcher
    {
        public int Index { get; set;  }
        
        readonly IEntityGridMapBuilder _entityGridMapBuilder;
        readonly IPresenterPlacer _presenterPlacer;
        
        EntityGridMap _currentMap;
        Action _resetAction = () => { };
        Stage _stage;

        public StageMapSwitcher(IEntityGridMapBuilder entityGridMapBuilder, IPresenterPlacer presenterPlacer)
        {
            _entityGridMapBuilder = entityGridMapBuilder;
            _presenterPlacer = presenterPlacer;
        }
        
        public void SetStage(Stage stage)
        {
            _stage = stage;
        }
        
        public EntityGridMap GetMap()
        {
            return _currentMap;
        }

        public void InitSwitchMap()
        {
            var info = _stage.mapInfos[Index];
            _currentMap = _entityGridMapBuilder.BuildEntityGridMap(info.data);
            _presenterPlacer.Place(_currentMap);
            
            _resetAction();
        }

        public void SwitchMap()
        {
            var info = _stage.mapInfos[Index];
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