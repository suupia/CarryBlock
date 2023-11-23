#nullable enable

using System;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Map.Scripts;

namespace Projects.MapMakerSystem.Scripts
{
    public class StageMapSwitcher: IMapGetter, IMapSwitcher
    {
        public int Index { get;}

        Stage _stage;
        
        readonly IEntityGridMapBuilder _entityGridMapBuilder;
        readonly IPresenterPlacer _presenterPlacer;
        
        EntityGridMap? _currentMap;
        Action _resetAction = () => { };

        public StageMapSwitcher(
            EditingMapTransporter editingMapTransporter,
            IEntityGridMapBuilder entityGridMapBuilder, 
            IPresenterPlacer presenterPlacer)
        {
            _entityGridMapBuilder = entityGridMapBuilder;
            _presenterPlacer = presenterPlacer;

            var stage = StageFileUtility.Load(editingMapTransporter.StageId)!;
            Index = editingMapTransporter.Index;
            _stage = stage;

        }
        
        public EntityGridMap GetMap()
        {
            if (_currentMap == null) throw new Exception("Please call InitSwitchMap first");
            return _currentMap;
        }

        public void InitTmpMap()
        {
            var preStage = _stage;
            _stage = StageFileUtility.Load(StageFileUtility.TMPStageID) ??
                     new Stage("Tmp", StageFileUtility.TMPStageID);
            InitSwitchMap();
            _stage = preStage;
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