using UnityEngine;

#nullable enable

namespace Carry.CarrySystem.Map.Scripts
{
    public class StageMapSaver
    {
        readonly EditingMapTransporter _editingMapTransporter;
        readonly EntityGridMapDataConverter _mapDataConverter;
        
        public StageMapSaver(EditingMapTransporter editingMapTransporter, EntityGridMapDataConverter mapDataConverter)
        {
            _editingMapTransporter = editingMapTransporter;
            _mapDataConverter = mapDataConverter;
        }

        public void Save(EntityGridMap map){
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
        }
    }
}