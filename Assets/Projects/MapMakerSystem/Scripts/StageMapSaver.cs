using Projects.MapMakerSystem.Scripts;
using UnityEngine;

#nullable enable

namespace Carry.CarrySystem.Map.Scripts
{
    public class StageMapSaver
    {
        readonly EditingMapTransporter _editingMapTransporter;
        readonly EntityGridMapDataConverter _mapDataConverter;
        readonly MapValidator _mapValidator;
        
        public StageMapSaver(EditingMapTransporter editingMapTransporter, EntityGridMapDataConverter mapDataConverter, MapValidator mapValidator)
        {
            _editingMapTransporter = editingMapTransporter;
            _mapDataConverter = mapDataConverter;
            _mapValidator = mapValidator;
        }

        public bool Save(EntityGridMap map)
        {
            if (!_mapValidator.CanSave) return false;
            
            var stage = StageFileUtility.Load(_editingMapTransporter.StageId);

            if (stage != null)
            {
                var data = _mapDataConverter.Convert(map);
                var info = stage.mapInfos[_editingMapTransporter.Index];
                stage.mapInfos[_editingMapTransporter.Index] = info with
                {
                    data = data
                };
                var newStage = stage with
                {
                    mapInfos = stage.mapInfos
                };
                StageFileUtility.Save(newStage);
                    
                Debug.Log("マップを更新したステージを保存しました");
            }
            else
            {
                Debug.LogError("Stageが読み込めません");
            }

            return true;
        }
    }
}