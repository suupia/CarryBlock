﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.SearchRoute.Scripts;
using Carry.CarrySystem.RoutingAlgorithm.Interfaces;
using Cysharp.Threading.Tasks;
using Fusion;
using UnityEngine;
using UnityEngine.Assertions;

#nullable enable

namespace Carry.CarrySystem.Map.Scripts
{
    public enum SearcherSize
    {
        SizeOne = 1,
        SizeThree = 3,
    }

    /// <summary>
    /// このクラスはDIせずにnewする
    /// </summary>
    public class WaveletSearchExecutor
    {
        public int WallValue => _wallValue;
        NumericGridMap _map;
        readonly int _initValue = -10; // PlaceNumAroundで重複して数字を置かないようにするために必要
        readonly int _wallValue = -5; // wallのマス
        readonly int _edgeValue = -8;
        readonly int _outOfRangeValue = -88;

        IRoutePresenter?[] _routePresenters;


        public WaveletSearchExecutor(IGridMap girdMap)
        {
            _map = new NumericGridMap(girdMap.Width, girdMap.Height, _initValue, _edgeValue, _outOfRangeValue);
            _routePresenters = new IRoutePresenter[_map.Length];
        }

        public void RegisterRoutePresenters(IReadOnlyList<RoutePresenter_Net> routePresenters)
        {
            if (routePresenters.Count() != _map.Length)
            {
                Debug.LogError($"routePresentersの数がmapのマスの数と一致しません。" +
                               $"routePresentersの数: {routePresenters.Count()} " +
                               $"mapのマスの数: {_map.Length}");
            }

            for (int i = 0; i < routePresenters.Count(); i++)
            {
                _routePresenters[i] = routePresenters[i];
            }
        }

        public bool[] SearchAccessibleArea(Vector2Int startPos, Func<int, int, bool> isWall,
            SearcherSize searcherSize = SearcherSize.SizeOne)
        {
            var searchedMap = WaveletSearch(startPos, isWall, searcherSize);
            var accessibleAreaArray = CalcAccessibleArea(searchedMap, searcherSize);

            var extendedMap =  ExtendToAccessibleNumericMap(searchedMap, searcherSize);

            // UpdatePresenter(accessibleAreaArray);
            UpdatePresenter(extendedMap);

            return accessibleAreaArray;
        }


        public NumericGridMap WaveletSearch(Vector2Int startPos, Func<int, int, bool> isWall,
            SearcherSize searcherSize = SearcherSize.SizeOne)
        {
            var searchQue = new Queue<Vector2Int>();
            int n = 1; //1から始まることに注意!!
            bool isComplete = false;

            // 初期化
            _map.FillAll(_initValue);


            if (isWall(startPos.x, startPos.y))
            {
                Debug.Log($"startPos:{startPos}は壁です");
                return _map; // _initValueのみが入ったmap
            }

            ExpandVirtualWall(isWall, searcherSize);
            
            
            //壁でないマスに数字を順番に振っていく
            // Debug.Log($"WaveletSearchを実行します startPos:{startPos}");
            WaveletSearch();

            return _map;

            ////////////////////////////////////////////////////////////////////

            //以下ローカル関数


            void WaveletSearch()
            {
                _map.SetValue(startPos, 0); //startPosの部分だけ周囲の判定を行わないため、ここで個別に設定する
                searchQue.Enqueue(startPos);

                while (!isComplete)
                {
                    int loopNum = searchQue.Count; //前のループでキューに追加された個数を数える
                    //Debug.Log($"i:{n}のときloopNum:{loopNum}");
                    for (int k = 0; k < loopNum; k++)
                    {
                        if (isComplete) break;
                        //Debug.Log($"PlaceNumAround({searchQue.Peek()})を実行します");
                        PlaceNumAround(searchQue.Dequeue());
                    }

                    n++; //前のループでキューに追加された文を処理しきれたら、インデックスを増やして次のループに移る

                    if (n > 100) //無限ループを防ぐ用
                    {
                        isComplete = true;
                        Debug.Log("SearchShortestRouteのwhile文でループが100回行われてしまいました");
                    }
                }
            }


            void PlaceNumAround(Vector2Int centerPos)
            {
                Vector2Int inspectPos;

                //8マス または 4マス判定する（真ん中のマスの判定は必要ない）（isDiagonalSearchに依る）
                for (int y = -1; y <= 1; y++)
                {
                    for (int x = -1; x <= 1; x++)
                    {
                        if (x == 0 && y == 0) continue; //真ん中のマスは飛ばす
                        // if (x != 0 && y != 0) continue; //斜めのマスも飛ばす

                        inspectPos = centerPos + new Vector2Int(x, y);
                        if (_map.GetValue(inspectPos) == _initValue &&
                            CanMoveDiagonally(centerPos, inspectPos)) // _edgeValueが帰ってくることもある
                        {
                            _map.SetValue(inspectPos, n);
                            searchQue.Enqueue(inspectPos);
                            //Debug.Log($"({inspectPos})を{n}にし、探索用キューに追加しました。");
                        }

                        // すべての範囲を探索したいので、コメントアウト
                        // endPosに到達したら探索終了
                        // if (inspectPos == endPos )
                        // {
                        //     isComplete = true;
                        //     SetValueByVector(inspectPos, n);
                        //     maxDistance = n;
                        //     //Debug.Log($"isCompleteをtrueにしました。maxDistance:{maxDistance}");
                        //     break; //探索終了
                        // }
                    }
                }

                // endPosに到達したらではなく、キューが空になったら探索終了
                if (searchQue.Count == 0)
                {
                    isComplete = true;
                    // Debug.Log("探索キューが空になりました");
                }
            }
        }

        public bool CanMoveDiagonally(Vector2Int prePos, Vector2Int afterPos)
        {
            Vector2Int directionVector = afterPos - prePos;

            //斜め移動の時にブロックの角を移動することはできない
            if (directionVector.x != 0 && directionVector.y != 0)
            {
                //水平方向の判定
                if (_map.GetValue(prePos.x + directionVector.x, prePos.y) == _wallValue)
                {
                    return false;
                }

                //垂直方向の判定
                if (_map.GetValue(prePos.x, prePos.y + directionVector.y) == _wallValue)
                {
                    return false;
                }
            }

            return true;
        }
        
        void ExpandVirtualWall(Func<int , int, bool> isWall, SearcherSize searcherSize)
        {
            var searcherSizeInt = (int) searcherSize;
            UnityEngine.Assertions.Assert.IsTrue(searcherSizeInt % 2 ==  1, "searcherSize must be odd number");
            
            var expandSize = (searcherSizeInt-1) / 2;
            
            for (int y = 0; y < _map.Height; y++)
            {
                for (int x = 0; x < _map.Width; x++)
                {
                    // expand wall
                    if (isWall(x, y)) 
                    {
                        for (int j = -expandSize; j <= expandSize; j++)
                        {
                            for (int i = -expandSize; i <= expandSize; i++)
                            {
                                _map.SetValue(x + i, y + j, _wallValue);
                            }
                        }
                    }
                    // expand edge
                    else if (x <= -1 + expandSize
                             || x >= _map.Width - expandSize
                             || y <= -1 + expandSize ||
                             y >= _map.Height - expandSize) 
                    {
                        _map.SetValue(x, y, _wallValue);
                    }
                }
            }
        }


        /// <summary>
        /// 仮置きでおいた壁を戻すために数字のマスに接しているマスをtrueにする
        /// </summary>
        /// <param name="map"></param>
        /// <param name="searcherSize"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        bool[] CalcAccessibleArea(NumericGridMap map, SearcherSize searcherSize)
        {
            var routeArray = new bool[map.Length];
            var resultBoolArray = new bool[map.Length];
            var waveletResult = map;
            // 数字がある部分をtrueにする
            for (int i = 0; i < routeArray.Length; i++)
            {
                routeArray[i] = waveletResult.GetValue(i) != _wallValue &&
                                  waveletResult.GetValue(i) != _initValue;
            }

            switch (searcherSize)
            {
                case SearcherSize.SizeOne:
                    resultBoolArray = routeArray;
                    break;
                case SearcherSize.SizeThree:
                    // set true to the squares around ture
                    for (int i = 0; i < routeArray.Length; i++)
                    {
                        if (routeArray[i])
                        {
                            for (int y = -1; y <= 1; y++)
                            {
                                for (int x = -1; x <= 1; x++)
                                {
                                    var pos = map.ToVector(i);
                                    var newX = pos.x + x;
                                    var newY = pos.y + y;
                                    if (!map.IsInDataRangeArea(newX, newY)) continue;
                                    resultBoolArray[map.ToSubscript(pos.x + x, pos.y + y)] = true;
                                }
                            }
                        }
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(searcherSize), searcherSize, null);
            }

            return resultBoolArray;
        }

        NumericGridMap ExtendToAccessibleNumericMap(NumericGridMap map, SearcherSize searcherSize)
        {
            var extendedMap = new NumericGridMap(map.Width, map.Height, _initValue, _edgeValue, _outOfRangeValue);
            var routeArray = new bool[map.Length];
            // 数字がある部分をtrueにする
            for (int i = 0; i < routeArray.Length; i++)
            {
                routeArray[i] = map.GetValue(i) != _wallValue &&
                                map.GetValue(i) != _initValue;
            }

            switch (searcherSize)
            {
                case SearcherSize.SizeOne:
                    for (int i = 0; i < routeArray.Length; i++)
                    {
                        extendedMap.SetValue(i, map.GetValue(i));
                    }
                    break;
                case SearcherSize.SizeThree:
                    // set true to the squares around ture
                    for (int i = 0; i < routeArray.Length; i++)
                    {
                        if (routeArray[i])
                        {
                            for (int y = -1; y <= 1; y++)
                            {
                                for (int x = -1; x <= 1; x++)
                                {
                                    var pos = map.ToVector(i);
                                    var newX = pos.x + x;
                                    var newY = pos.y + y;
                                    if (!map.IsInDataRangeArea(newX, newY)) continue;
                                    extendedMap.SetValue(newX,newY, map.GetValue(i) + 1);
                                }
                            }
                        }
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(searcherSize), searcherSize, null);
            }

            return extendedMap;
        }

        public void DebugAccessibleArea(int height, int width,bool[] accessibleAreaArray )
        {
            //デバッグ用
            StringBuilder debugCell = new StringBuilder();
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    bool value = accessibleAreaArray[x + (height - y - 1) * width];
                    debugCell.AppendFormat("{0,4},", value.ToString()); // 桁数をそろえるために0を追加していると思う
                }
            
                debugCell.AppendLine();
            }
            Debug.Log($"すべてのresultBoolArrayの結果は\n{debugCell}");
        }

        // 時間差でpresenterをupdateする
        void UpdatePresenter(NumericGridMap numericGridMap)
        {
            for (int i = 0; i < numericGridMap.Length; i++)
            {
                DelayUpdate(_routePresenters[i], numericGridMap.GetValue(i)).Forget();
            }
        }

        async UniTaskVoid DelayUpdate(IRoutePresenter? routePresenter, long value)
        {
            if(routePresenter == null) return;
            if(value < 0)return;

            await UniTask.Delay((int)value * 250);
            routePresenter.SetPresenterActive(true);
        
        }
    }
}