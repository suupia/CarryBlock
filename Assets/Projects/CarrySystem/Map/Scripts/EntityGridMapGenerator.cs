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
            gridMapData ??= new DefaultGridMapData(); // nullだったら代入

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

        GridMapData? Load(int mapDataIndex)
        {
            GridMapData? gridMapData;

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
                Debug.LogError($"パス:{filePath}にjsonファイルが存在しません");
                gridMapData = null;
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

        readonly int _width = 20;
        readonly int _height = 11;
        public DefaultGridMapData()
        {
            var length = _width * _height;
            rockRecords = new RockRecord[length];
            
            PlaceRock();
        }

        void PlaceRock()
        {
            rockRecords[0].kind = Rock.Kind.Kind1;
            rockRecords[2].kind = Rock.Kind.Kind1;
            rockRecords[_width + 1].kind = Rock.Kind.Kind1;
        }
    }
}