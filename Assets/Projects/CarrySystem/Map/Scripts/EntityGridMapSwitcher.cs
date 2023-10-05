#nullable enable
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
        readonly MapKeyDataSelectorNet _mapKeyDataSelectorNet;
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
            MapKeyDataSelectorNet mapKeyDataSelectorNet,
            StageIndexTransporter stageIndexTransporter,
            IPresenterPlacer allPresenterPlacer
            )
        {
            _gridMapLoader = gridMapGridMapLoader;
            _cartBuilder = cartBuilder;
            _floorTimerNet = floorTimerNet;
            _mapKeyDataSelectorNet = mapKeyDataSelectorNet;
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
            Debug.Log($"StageIndex : {_stageIndexTransporter.StageIndex}");
            var mapKeyDataList = _mapKeyDataSelectorNet.SelectMapKeyDataList(_stageIndexTransporter.StageIndex);
            var key =mapKeyDataList[index].mapKey;
            var mapIndex =  mapKeyDataList[index].index;
            _currentMap = _gridMapLoader.LoadEntityGridMap(key, mapIndex);
            _allPresenterPlacer.Place(_currentMap);
            _cartBuilder.Build(_currentMap, this);

            _resetAction();

        }

        public void UpdateMap(MapKey mapKey, int index)
        {
            var mapKeyDataList = _mapKeyDataSelectorNet.SelectMapKeyDataList(_stageIndexTransporter.StageIndex);
            Debug.Log($"次のフロアに変更します nextIndex: {index}");
            if (index < 0)
            {
                Debug.LogWarning($"index is under the min value");
                index = 0; // 0未満にならないようにする
            }
            if (index > mapKeyDataList.Count - 1)
            {
                Debug.LogWarning($"index is over the max value");
                index = mapKeyDataList.Count - 1; // 最大値を超えないようにする
            }
            _currentIndex = index;
            var key = mapKeyDataList[_currentIndex].mapKey;
            var mapIndex = mapKeyDataList[_currentIndex].index;
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