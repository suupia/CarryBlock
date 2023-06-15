using System.Collections;
using System.Collections.Generic;
using System.IO;
using Carry.CarrySystem.Entity.Scripts;
using UnityEngine;

#nullable enable

namespace Carry.CarrySystem.Map.Scripts
{
    public class EntityGridMapGenerator
    {
        readonly string _folderPath;

        public EntityGridMapGenerator()
        {
            _folderPath = Application.streamingAssetsPath + "/JsonFiles";
            Debug.Log($"セーブデータのファイルパスは {_folderPath}");
        }

        public EntityGridMap GenerateEntityGridMap(int mapDataIndex)
        {
            var gridMapData = Load(mapDataIndex);

            var map = new EntityGridMap(gridMapData.width, gridMapData.height);
            
            for (int i = 0; i < map.GetLength(); i++)
            {
                //Rock
                if (gridMapData.rockRecords[i].kind != Rock.Kind.None)
                {
                    map.AddEntity<Rock>(i, new Rock(gridMapData.rockRecords[i].kind, map.GetVectorFromIndex(i)));
                }
            }

            return map;
        }

        GridMapData Load(int mapDataIndex)
        {
            GridMapData gridMapData;

            string filePath = GetFilePath(mapDataIndex);

            if (IsExitFile(mapDataIndex))
            {
                using (StreamReader streamReader = new StreamReader(filePath))
                {
                    string data = streamReader.ReadToEnd();
                    streamReader.Close();
                    gridMapData = JsonUtility.FromJson<GridMapData>(data);
                }
            }
            else
            {
                Debug.LogWarning($"パス:{filePath}にjsonファイルが存在しません");
                gridMapData = new DefaultGridMapData();
            }

            return gridMapData;
        }

        string GetFilePath(int index)
        {
            return _folderPath + $"/HexagonMapData{index}.json";
        }

        bool IsExitFile(int index)
        {
            return File.Exists(GetFilePath(index));
        }
    }
    
    public class DefaultGridMapData : GridMapData
    {
        // 適当に作っている
        // エクセルからデータを読み込めるようになるまではこれを使用する
        
        readonly int _length;
        public DefaultGridMapData()
        {
            // 親クラスのフィールドを書き換えていることに注意
            width = 20;
            height = 11;
            _length = width * height;
            rockRecords = new RockRecord[_length];

            FillAll();
            PlaceRock();
        }
        
        void FillAll()
        {
            for (int i = 0; i < _length; i++)
            {
                rockRecords[i] = new RockRecord();
            }
        }

        void PlaceRock()
        {
            rockRecords[0].kind = Rock.Kind.Kind1;
            rockRecords[2].kind = Rock.Kind.Kind1;
            rockRecords[width + 1].kind = Rock.Kind.Kind1;
        }
    }
}