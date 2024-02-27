#nullable enable
using System;
using System.Collections.Generic;
using Carry.CarrySystem.Cart.Scripts;
using Carry.CarrySystem.FloorTimer.Scripts;
using Carry.CarrySystem.Gimmick.Interfaces;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Player.Scripts;
using Projects.CarrySystem.Cart.Interfaces;
using UnityEngine;
using VContainer;
using Object = UnityEngine.Object;

namespace Carry.CarrySystem.Map.Scripts
{
    /// <summary>
    ///     フロアごとに別のマップを生成し、返すクラス
    /// </summary>
    public class EntityGridMapSwitcher: IMapSwitcher , IMapGetter
    {
        public int Index => _currentIndex; 
        
        readonly ICartBuilder _cartBuilder;
        readonly FloorTimerNet _floorTimerNet;
        readonly EntityGridMapLoader _gridMapLoader;
        readonly IMapKeyDataSelector _mapKeyDataSelector;
        readonly StageIndexTransporter _stageIndexTransporter;
        readonly IPresenterPlacer _presenterPlacer;
        int _currentIndex;
        EntityGridMap? _currentMap;
        
        Action _resetAction = () => { };

        [Inject]
        public EntityGridMapSwitcher(
            EntityGridMapLoader gridMapGridMapLoader,
            ICartBuilder cartBuilder,
            FloorTimerNet floorTimerNet,
            IMapKeyDataSelector mapKeyDataSelector,
            StageIndexTransporter stageIndexTransporter,
            IPresenterPlacer presenterPlacer
            )
        {
            _gridMapLoader = gridMapGridMapLoader;
            _cartBuilder = cartBuilder;
            _floorTimerNet = floorTimerNet;
            _mapKeyDataSelector = mapKeyDataSelector;
            _stageIndexTransporter = stageIndexTransporter;
            _presenterPlacer = presenterPlacer;   

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


        public void InitSwitchMap()
        {
            var firstIndex = 0;
            Debug.Log($"StageIndex : {_stageIndexTransporter.StageIndex}");
            var mapKeyDataList = _mapKeyDataSelector.SelectMapKeyDataList(_stageIndexTransporter.StageIndex);
            var key =mapKeyDataList[firstIndex].mapKey;
            var mapIndex =  mapKeyDataList[firstIndex].index;
            _currentMap = _gridMapLoader.LoadEntityGridMap(key, mapIndex);
            _presenterPlacer.Place(_currentMap);
            StartGimmicks();
            _cartBuilder.Build(_currentMap, this);

            _resetAction();

        }

        public void SwitchMap()
        {
            SwitchToNextMap();
        }

        public void SwitchToNextMap()
        {
            PrivateUpdateMap(_currentIndex + 1);
        }
        public void SwitchToPreviousMap()
        {
            PrivateUpdateMap(_currentIndex - 1);
        }
        
        void PrivateUpdateMap(int index)
        {
            var mapKeyDataList = _mapKeyDataSelector.SelectMapKeyDataList(_stageIndexTransporter.StageIndex);
            Debug.Log($"次のフロアに変更します nextIndex: {index}");
            
            //この呼び出しが_floorTimerNet.IsCleared = trueより後になるとクリアした最後のフロアの時間が加算されない
            _floorTimerNet.SumRemainingTime();
            
            if (index < 0)
            {
                Debug.LogWarning($"index is under the min value");
                index = 0; // 0未満にならないようにする
            }
            if (index > mapKeyDataList.Count - 1)
            {
                Debug.LogWarning($"index is over the max value");
                index = mapKeyDataList.Count - 1; // 最大値を超えないようにする
                _floorTimerNet.IsCleared = true;//次のフロアがないためクリアフラグをtrueにする
            }
            _currentIndex = index;
            var key = mapKeyDataList[_currentIndex].mapKey;
            var mapIndex = mapKeyDataList[_currentIndex].index;
            var nextMap = _gridMapLoader.LoadEntityGridMap(key, mapIndex);
            EndGimmicks();  // EntityGridMapを更新する前にGimmickを終了させることに注意
            _currentMap = nextMap;
            _presenterPlacer.Place(_currentMap);
            StartGimmicks();
            _cartBuilder.Build(_currentMap, this);

            _floorTimerNet.StartTimer();
            
            
            _resetAction();
        }
        
        public void RegisterResetAction(Action action)
        {
            _resetAction += action;
        }

        void StartGimmicks()
        {
            for (int i = 0; i < GetMap().Length; i++)
            {
                var gimmicks = GetMap().GetSingleTypeList<IGimmick>(i);
                var pos = GetMap().ToVector(i);

                foreach (var gimmick in gimmicks)
                {
                    gimmick.StartGimmick(pos);
                }
                
            }
        }
        
        void EndGimmicks()
        {
            for (int i = 0; i < GetMap().Length; i++)
            {
                var gimmicks = GetMap().GetSingleTypeList<IGimmick>(i);

                foreach (var gimmick in gimmicks)
                {
                    gimmick.Dispose();
                }
                
            }
        }

        
    }

}