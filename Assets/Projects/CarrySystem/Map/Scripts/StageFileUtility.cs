using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Carry.CarrySystem.Map.Scripts
{
    [Serializable]
    public struct Stage
    {
        public Stage(string name)
        {
            this.name = name;
            id = Guid.NewGuid().ToString();
            mapInfos = new List<MapInfo>
            {
                new("map1"),
                new("map2"),
                new("map3"),
                new("map4"),
                new("map5"),
            };
        } 
        
        // public string version;
        public string name;
        public string id;
        public List<MapInfo> mapInfos;
    }

    [Serializable]
    public struct MapInfo
    {
        public MapInfo(string name)
        {
            this.name = name;
            using var streamReader = new StreamReader(EntityGridMapFileUtility.GetDefaultFilePath());
            var stringData = streamReader.ReadToEnd();
            streamReader.Close();
            data = JsonUtility.FromJson<EntityGridMapData>(stringData);
        }
        public string name;
        public EntityGridMapData data;
    }

    [Serializable]
    struct World
    {
        public List<string> stageIds;
    }

    public static class StageFileUtility
    {
        static string GetPath(string stageId) => Path.Combine(
            Application.streamingAssetsPath, "JsonFiles", "Stages", $"{stageId}.json");

        static World GetWorld()
        {
            if (PlayerPrefs.HasKey("world"))
            {
                return JsonUtility.FromJson<World>(PlayerPrefs.GetString("world"));
            }

            var stages = SaveInitStages();
            var world = new World
            {
                stageIds = stages.Select(s => s.id).ToList()
            };
            PlayerPrefs.SetString("world",JsonUtility.ToJson(world));
            PlayerPrefs.Save();

            return world;
        }

        public static IReadOnlyList<Stage> GetStages()
        {
            var world = GetWorld();
            var stageIds = world.stageIds;
            var requireRefreshWorld = false;
            var stages = new List<Stage>();

            for (var i = 0; i < stageIds.Count; i++)
            {
                var stage = Load(stageIds[i]);
                if (stage == null)
                {
                    stage = new Stage($"Stage{i}");
                    world.stageIds[i] = stage.Value.id;
                    requireRefreshWorld = true;
                }
                stages.Add(stage.Value);
            }

            if (requireRefreshWorld)
            {
                PlayerPrefs.SetString("world",JsonUtility.ToJson(world));
                PlayerPrefs.Save();
            }
            
            return stages;
        }

        static IEnumerable<Stage> SaveInitStages()
        {
            var stages = new List<Stage>
            {
                new("Stage1"),
                new("Stage2"),
                new("Stage3"),
            };
                
            foreach (var stage in stages)
            {
                Save(stage);
            }
            
            Debug.Log("ステージが新規作成されました");

            return stages;
        }

        static void Save(Stage stage)
        {
            var path = GetPath(stage.id);

            var jsonData = JsonUtility.ToJson(stage);

            // JSONデータをファイルに書き込む
            using var streamWriter = new StreamWriter(path);
            streamWriter.Write(jsonData);
            streamWriter.Flush();
        }
        
        static Stage? Load(string stageId)
         {
             var path = GetPath(stageId);

             if (File.Exists(path))
             {
                 string jsonData = File.ReadAllText(path);
                 return JsonUtility.FromJson<Stage>(jsonData);
             }
             Debug.LogError("File not found.");
             return null;
         }
    }
}