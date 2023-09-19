﻿#nullable enable
using System;
using System.Collections.Generic;
using Carry.CarrySystem.Cart.Scripts;
using Carry.CarrySystem.FloorTimer.Scripts;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Player.Scripts;
using UnityEngine;
using VContainer;
using Object = UnityEngine.Object;

namespace Carry.CarrySystem.Map.Scripts
{
    /// <summary>
    ///     フロアごとに別のマップを生成し、返すクラス
    /// </summary>
    public class EntityGridMapSwitcher : IMapUpdater
    {
        public int Index => _currentIndex; 
        
        readonly CartBuilder _cartBuilder;
        readonly FloorTimerNet _floorTimerNet;
        readonly EntityGridMapLoader _gridMapLoader;
        readonly MapKeyDataNet _mapKeyDataNet;
        readonly StageIndexTransporter _stageIndexTransporter;
        readonly IPresenterPlacer _allPresenterPlacer;
        int _currentIndex;
        EntityGridMap? _currentMap;
        
        Action _resetAction = () => { };

        [Inject]
        public EntityGridMapSwitcher(
            EntityGridMapLoader gridMapGridMapLoader,
            CartBuilder cartBuilder,
            FloorTimerNet floorTimerNet,
            MapKeyDataNet mapKeyDataNet,
            StageIndexTransporter stageIndexTransporter,
            IPresenterPlacer allPresenterPlacer
            )
        {
            _gridMapLoader = gridMapGridMapLoader;
            _cartBuilder = cartBuilder;
            _floorTimerNet = floorTimerNet;
            _mapKeyDataNet = mapKeyDataNet;
            _stageIndexTransporter = stageIndexTransporter;
            _allPresenterPlacer　= allPresenterPlacer;
        }

        public EntityGridMap GetMap()
        {
            if (_currentMap == null)
            {
                Debug.LogError($"_currentMap is null");
                return null!;
            }
            return _currentMap;
        }

        public void InitUpdateMap(MapKey mapKey, int index = 0)
        {
            var key = _mapKeyDataNet.MapKeyDataList[index].mapKey;
            var mapIndex =  _mapKeyDataNet.MapKeyDataList[index].index;
            _currentMap = _gridMapLoader.LoadEntityGridMap(key, mapIndex);
            _allPresenterPlacer.Place(_currentMap);
            _cartBuilder.Build(_currentMap, this);

            _resetAction();

        }

        public void UpdateMap(MapKey mapKey, int index)
        {
            Debug.Log($"StageIndex : {_stageIndexTransporter.StageIndex}");
            Debug.Log($"次のフロアに変更します nextIndex: {index}");
            if (index < 0)
            {
                Debug.LogWarning($"index is under the min value");
                index = 0; // 0未満にならないようにする
            }
            if (index > _mapKeyDataNet.MapKeyDataList.Count - 1)
            {
                Debug.LogWarning($"index is over the max value");
                index = _mapKeyDataNet.MapKeyDataList.Count - 1; // 最大値を超えないようにする
            }
            _currentIndex = index;
            var key = _mapKeyDataNet.MapKeyDataList[_currentIndex].mapKey;
            var mapIndex = _mapKeyDataNet.MapKeyDataList[_currentIndex].index;
            var nextMap = _gridMapLoader.LoadEntityGridMap(key, mapIndex);
            _currentMap = nextMap;
            _allPresenterPlacer.Place(_currentMap);
            _cartBuilder.Build(_currentMap, this);

            _floorTimerNet.StartTimer();
            
            
            _resetAction();
        }
        
        public void RegisterResetAction(Action action)
        {
            _resetAction += action;
        }
    }

}