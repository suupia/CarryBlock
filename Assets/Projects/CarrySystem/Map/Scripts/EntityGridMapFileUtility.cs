using System.IO;
using UnityEngine;

#nullable enable

namespace Carry.CarrySystem.Map.Scripts
{
    public static class EntityGridMapFileUtility
    {
        public static string GetFolderPath()
        {
            return Application.streamingAssetsPath + "/JsonFiles/MapData";
        }

        public static string GetFilePath(MapKey key, int index)
        {
            return GetFolderPath() + $"/MapData_{key}_{index}.json";
        }

        public static bool IsExitFile(MapKey key, int index)
        {
            return File.Exists(GetFilePath(key, index));
        }

        public static string GetDefaultFilePath()
        {
            // このパスに白紙のマップデータを必ず置いておく
            return GetFolderPath() + $"/MapData_{MapKey.Default}_{0}.json";
        }
    }
}