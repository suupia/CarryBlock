﻿using System.Linq;
using System.Numerics;
using Carry.CarrySystem.Entity.Scripts;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using Projects.CarrySystem.Block.Scripts;
using TMPro;
using UnityEngine;
using VContainer;

#nullable enable

namespace Carry.EditMapSystem.EditMap.Scripts
{
    public class EditMapUpdater : IMapUpdater
    {
        // ToDo: クラス名を具体的に決める
        public MapKey MapKey => _mapKey;
        public int Index => _index;

        readonly EntityGridMapLoader _gridMapLoader;
        readonly TilePresenterBuilder _tilePresenterBuilder;
        EntityGridMap _map;
        MapKey _mapKey;
        int _index;

        [Inject]
        public EditMapUpdater(EntityGridMapLoader entityGridMapLoader, TilePresenterBuilder tilePresenterBuilder)
        {
            _gridMapLoader = entityGridMapLoader;
            _tilePresenterBuilder = tilePresenterBuilder;
            _mapKey = MapKey.Default;
            _index = -1; // LoadedFileを表示するために初期化が必要
            _map = _gridMapLoader.LoadDefaultEntityGridMap(); // Defaultのマップデータを読み込む
            for (int i = 0; i < _map.GetLength(); i++)
            {
                if (_map.GetSingleEntity<Ground>(i) is { } ground) Debug.Log($"i:{i} ground:{ground}");
            }
        }

        public EntityGridMap GetMap()
        {
            return _map;
        }

        public void InitUpdateMap(MapKey mapKey, int index)
        {
            _map = _gridMapLoader.LoadEntityGridMap(mapKey, index);
            _tilePresenterBuilder.Build(_map);
        }

        public void UpdateMap(MapKey mapKey, int index)
        {
            _map = _gridMapLoader.LoadEntityGridMap(mapKey, index);
            _tilePresenterBuilder.Build(_map);
            _mapKey = mapKey;
            _index = index;
        }

        // ToDo: 以下の関数をMapEditorクラスに移して、このクラスをコンテナの役割に特化させる
        // 以下のメソッドは抽象化したほうがよい

        public void AddRock(Vector2Int gridPos)
        {
            if (!_map.IsInDataRangeArea(gridPos)) return;

            var newRock = new Rock(Rock.Kind.Kind1, gridPos);
            var allEntityList = _map.GetAllEntityList(gridPos).ToList();
            var rockCount = allEntityList.OfType<Rock>().Count();
            var groundCount = allEntityList.OfType<Ground>().Count();
            var othersCount = allEntityList.Count() - rockCount - groundCount;

            Debug.Log($"rockCount:{rockCount} groundCount:{groundCount} othersCount:{othersCount}");
            if (rockCount >= newRock.MaxPlacedBlockCount) return;
            if (othersCount > 0) return;

            _map.AddEntity(gridPos, newRock);
        }

        public void RemoveRock(Vector2Int gridPos)
        {
            var rocks = _map.GetSingleEntityList<Rock>(gridPos);
            if (!rocks.Any()) return;
            var rock = rocks.First();
            if (_map.IsInDataRangeArea(gridPos)) _map.RemoveEntity(gridPos, rock);
        }


        public void AddBasicBlock(Vector2Int gridPos)
        {
            if (!_map.IsInDataRangeArea(gridPos)) return;

            var newBasicBlock = new BasicBlock(BasicBlock.Kind.Kind1, gridPos);
            var allEntityList = _map.GetAllEntityList(gridPos).ToList();
            var basicBlockCount = allEntityList.OfType<BasicBlock>().Count();
            var groundCount = allEntityList.OfType<Ground>().Count();
            var othersCount = allEntityList.Count() - basicBlockCount - groundCount;
            
            Debug.Log($"basicBlockCount:{basicBlockCount} groundCount:{groundCount} othersCount:{othersCount}");

            if (basicBlockCount >= newBasicBlock.MaxPlacedBlockCount) return;
            if (othersCount > 0) return;

            _map.AddEntity(gridPos, newBasicBlock);
        }

        public void RemoveBasicBlock(Vector2Int gridPos)
        {
            var basics = _map.GetSingleEntityList<BasicBlock>(gridPos);
            if (!basics.Any()) return;
            var basic = basics.First();
            if (_map.IsInDataRangeArea(gridPos)) _map.RemoveEntity(gridPos, basic);
        }
    }
}