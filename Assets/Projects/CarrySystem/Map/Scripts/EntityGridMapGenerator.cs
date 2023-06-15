using System.Collections;
using System.Collections.Generic;
using System.IO;
using Carry.CarrySystem.Entity.Scripts;
using Carry.CarrySystem.Spawners;
using UnityEngine;

#nullable enable

namespace Carry.CarrySystem.Map.Scripts
{
    public class EntityGridMapGenerator
    {
        readonly string _folderPath;

        public  EntityGridMapGenerator()
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
                // Ground
                if (gridMapData.groundRecords[i].kind != Ground.Kind.None)
                {
                    map.AddEntity<Ground>(i, new Ground(gridMapData.groundRecords[i].kind, map.GetVectorFromIndex(i)));
                }
                // Rock
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
            groundRecords = new GroundRecord[_length];
            rockRecords = new RockRecord[_length];

            FillAll();
            PlaceGround();
            PlaceRock();
        }
        
        void FillAll()
        {
            for (int i = 0; i < _length; i++)
            {
                groundRecords[i] = new GroundRecord();
                rockRecords[i] = new RockRecord();
            }
        }

        void PlaceGround()
        {
            // すべてのマスに対してGroundを配置する
            for (int i = 0; i < _length; i++)
            {
                groundRecords[i].kind = Ground.Kind.Kind1;
            }
        }

        void PlaceRock()
        {
            List<int> rockPosList = new List<int>();
            
            rockPosList.Add(0); //原点に置く
            for (int i = 1 ;i < height; i++)
            {
                if (i % 2 == 0)
                {
                    rockPosList.Add(i * width -1);
                    rockPosList.Add(i * width - 3);
                }
                else
                {
                    rockPosList.Add(i * width -2);
                    rockPosList.Add(i * width - 4);
                }

            }
            // Debug.Log($"rockPosList : {string.Join(",", rockPosList)}");

            for (int i = 0; i < _length; i++)
            {
                if (rockPosList.Contains(i))
                {
                    rockRecords[i].kind = Rock.Kind.Kind1;
                }
            }

        }
    }
}