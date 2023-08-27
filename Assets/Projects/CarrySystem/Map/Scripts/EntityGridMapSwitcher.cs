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
        readonly CartBuilder _cartBuilder;
        readonly FloorTimerNet _floorTimerNet;
        readonly EntityGridMapLoader _gridMapLoader;
        readonly MapKeyDataNet _mapKeyDataNet;
        readonly AllPresenterBuilder _allPresenterBuilder;
        int _currentIndex;
        EntityGridMap? _currentMap;
        
        Action _resetAction = () => { };

        [Inject]
        public EntityGridMapSwitcher(
            EntityGridMapLoader gridMapGridMapLoader,
            CartBuilder cartBuilder,
            FloorTimerNet floorTimerNet,
            MapKeyDataNet mapKeyDataNet,
            AllPresenterBuilder allPresenterBuilder)
        {
            _gridMapLoader = gridMapGridMapLoader;
            _cartBuilder = cartBuilder;
            _floorTimerNet = floorTimerNet;
            _mapKeyDataNet = mapKeyDataNet;
            _allPresenterBuilder　= allPresenterBuilder;
        }

        public EntityGridMap GetMap()
        {
            return _currentMap;
        }

        public void InitUpdateMap(MapKey mapKey, int index)
        {
            var key = _mapKeyDataNet.MapKeyDataList[_currentIndex].mapKey;
            var mapIndex =  _mapKeyDataNet.MapKeyDataList[_currentIndex].index;
            _currentMap = _gridMapLoader.LoadEntityGridMap(key, mapIndex);
            _allPresenterBuilder.Build(_currentMap);
            _cartBuilder.Build(_currentMap, this);
            
            var players = Object.FindObjectsByType<CarryPlayerControllerNet>(FindObjectsSortMode.None);
            foreach (var player in players) player.Reset(_currentMap);
            
        }

        public void UpdateMap(MapKey mapKey, int index = 0)
        {
            Debug.Log($"次のフロアに変更します nextIndex: {_currentIndex + 1}");
            _currentIndex++;
            var key = _mapKeyDataNet.MapKeyDataList[_currentIndex].mapKey;
            var mapIndex = _mapKeyDataNet.MapKeyDataList[_currentIndex].index;
            var nextMap = _gridMapLoader.LoadEntityGridMap(key, mapIndex);
            _currentMap = nextMap;
            _allPresenterBuilder.Build(_currentMap);


            // 以下リセット処理
            var players = Object.FindObjectsByType<CarryPlayerControllerNet>(FindObjectsSortMode.None);
            foreach (var player in players) player.Reset(_currentMap);

            _cartBuilder.Build(_currentMap, this);

            _floorTimerNet.StartTimer();
            
            // 上にある処理はResetに登録するようにする
            
            _resetAction();
        }
        
        public void RegisterResetAction(Action action)
        {
            _resetAction += action;
        }
    }

}