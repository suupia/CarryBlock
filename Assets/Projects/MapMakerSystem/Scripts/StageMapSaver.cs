#nullable enable

using Projects.MapMakerSystem.Scripts;
using UnityEngine;

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
            if (!_mapValidator.CanSave)
            {
                Debug.LogWarning("テストプレイをクリアしないとセーブできません");
                return false;
            }

            SaveInternal(map, _editingMapTransporter.StageId, _editingMapTransporter.Index);
            return true;
        }

        public void SaveTmpMap(EntityGridMap map)
        {
            SaveInternal(map, StageFileUtility.TMPStageID, _editingMapTransporter.Index);
        }

        void SaveInternal(EntityGridMap map, string stageId, int index)
        {
            var stage = StageFileUtility.Load(stageId);

            if (stage != null)
            {
                var data = _mapDataConverter.Convert(map);
                var info = stage.mapInfos[index];
                stage.mapInfos[index] = info with
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