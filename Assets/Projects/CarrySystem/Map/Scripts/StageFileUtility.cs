#nullable enable

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Carry.CarrySystem.Map.Scripts
{
    [Serializable]
    public record Stage
    {
                
        // public string version;
        public string name;
        public string id;
        public List<MapInfo> mapInfos;
        
        public Stage(string name): this(name, Guid.NewGuid().ToString())
        {
        } 
        
        public Stage(string name, string id)
        {
            this.name = name;
            this.id = id;
            mapInfos = new List<MapInfo>
            {
                new("map1"),
                new("map2"),
                new("map3"),
                new("map4"),
                new("map5"),
            };
        } 

    }

    [Serializable]
    public record MapInfo
    {
        public string name;
        public EntityGridMapData data;
        
        public MapInfo(string name)
        {
            this.name = name;
            using var streamReader = new StreamReader(EntityGridMapFileUtility.GetDefaultFilePath());
            var stringData = streamReader.ReadToEnd();
            streamReader.Close();
            data = JsonUtility.FromJson<EntityGridMapData>(stringData);
        }

    }

    [Serializable]
    struct World
    {
        public List<string> stageIds;
    }

    public static class StageFileUtility
    {
        public const string TMPStageID = "tmp";
        public static void Save(Stage stage)
        {
            var path = GetPath(stage.id);

            var jsonData = JsonUtility.ToJson(stage);

            // JSONデータをファイルに書き込む
            using var streamWriter = new StreamWriter(path);
            streamWriter.Write(jsonData);
            streamWriter.Flush();

            Debug.Log("ステージ" + stage.id + " を保存しました");
        }

        public static Stage? Load(string stageId)
        {
            var path = GetPath(stageId);

            if (!File.Exists(path))
            {
                Debug.LogWarning(path + " が存在しません");
                return null;
            }

            var jsonData = File.ReadAllText(path);
            return JsonUtility.FromJson<Stage>(jsonData);
        }
        
        public static IReadOnlyList<Stage> GetStages()
        {
            var world = LoadWorld();
            var stageIds = world.stageIds;
            var needsRefreshWorld = false;
            var stages = new List<Stage>();

            for (var i = 0; i < stageIds.Count; i++)
            {
                var stage = Load(stageIds[i]);
                if (stage == null)
                {
                    stage = new Stage($"Stage{i + 1}");
                    Save(stage);
                    world.stageIds[i] = stage.id;
                    needsRefreshWorld = true;
                }
                stages.Add(stage);
            }

            if (needsRefreshWorld)
            {
                SaveWorld(world);
            }
            
            return stages;
        }
        
        static string GetPath(string stageId) => Path.Combine(
            Application.streamingAssetsPath, "JsonFiles", "Stages", $"{stageId}.json");

        static void SaveWorld(World world)
        {
            PlayerPrefs.SetString("world",JsonUtility.ToJson(world));
            PlayerPrefs.Save();
            Debug.Log("新しいWorldを作成しました " + JsonUtility.ToJson(world));
        }

        static World LoadWorld()
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
            SaveWorld(world);

            return world;
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
            Save(new Stage("Tmp", TMPStageID));

            Debug.Log("初期ステージが新規作成されました");

            return stages;
        }

    }
}