using System;
using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using UnityEngine;

#nullable enable

namespace Projects.MapMakerSystem.Scripts
{
    public class StageMapSwitcher: IMapGetter, IMapSwitcher
    {
        public int Index { get;}

        public Stage Stage { get; private set; }
        
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
            
            var stage = StageFileUtility.Load(editingMapTransporter.StageId);
            Index = editingMapTransporter.Index;
            Stage = stage;

        }
        
        public EntityGridMap GetMap()
        {
            return _currentMap;
        }

        public void InitTmpMap()
        {
            var preStage = Stage;
            Stage = StageFileUtility.Load(StageFileUtility.TMPStageID)!;
            InitSwitchMap();
            Stage = preStage;
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