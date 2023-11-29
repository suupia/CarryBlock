using System.Collections;
using System.Collections.Generic;
using Carry.CarrySystem.Entity.Scripts;
using Carry.CarrySystem.Map.Scripts;
using UnityEngine;
using VContainer;

namespace Carry.EditMapSystem.EditMap.Scripts
{
    public class EditMapDefaultInitializer : MonoBehaviour
    {
        [SerializeField] int width;
        [SerializeField] int height;
        EntityGridMapSaver _entityGridMapSaver;

        [Inject]
        public void Construct(EntityGridMapSaver entityGridMapSaver)
        {
            _entityGridMapSaver = entityGridMapSaver;
            
            SaveDefaultMap();

        }

        /// <summary>
        /// Defaultのマップを保存する
        /// </summary>
        void SaveDefaultMap()
        {
            Debug.Log("SaveDefaultMap");
            // インデックスが-1であるデフォルトマップを更新する
            
            var cleanMap = new EntityGridMap(width, height);

            // すべてのマスにGroundを1つ設置する
            for (int i = 0; i < cleanMap.Length; i++)
            {
                var gridPosition = cleanMap.ToVector(i);

                cleanMap.AddEntity(gridPosition, new Ground(Ground.Kind.Kind1));
            }

            _entityGridMapSaver.SaveMap(cleanMap, MapKey.Default, -1);
        }
    }
}