﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Carry.CarrySystem.SearchRoute.Scripts;
using JetBrains.Annotations;
using Projects.CarrySystem.RoutingAlgorithm.Interfaces;
using UnityEngine;
#nullable enable

namespace Carry.CarrySystem.Map.Scripts
{
    public class WaveletSearchExecutor
    {
        NumericGridMap _map;
        readonly int _initValue = -10; // PlaceNumAroundで重複して数字を置かないようにするために必要
        readonly int _wallValue = -5; // wallのマス
        
        IRoutePresenter?[] _routePresenters;

        enum SearcherSize
        {
            SizeOne,
            SizeThree
        }

        public WaveletSearchExecutor(NumericGridMap numericGridMap)
        {
            _map = numericGridMap;
            _routePresenters = new IRoutePresenter[numericGridMap.GetLength()];
        }
        
        public void RegisterRoutePresenters( IEnumerable<RoutePresenter_Net> routePresenters)
        {
            var routePresentersArray = routePresenters.ToArray();
            if (routePresentersArray.Count() != _map.GetLength())
            {
                Debug.LogError($"routePresentersの数がmapのマスの数と一致しません。" +
                               $"routePresentersの数: {routePresentersArray.Count()} " +
                               $"mapのマスの数: {_map.GetLength()}");
            }
            for(int i = 0 ; i< routePresentersArray.Count(); i++)
            {
                _routePresenters[i] = routePresentersArray[i];
            }
        }

        public bool[] SearchAccessibleAreaSizeOne(Vector2Int startPos, Vector2Int endPos,
            Func<int, int, bool> isWall)
        {
            var resultBoolArray = new bool[_map.GetLength()];
            var waveletResult = WaveletSearch(startPos, endPos, isWall);
            for (int i = 0; i < resultBoolArray.Length; i++)
            {
                resultBoolArray[i] = waveletResult.GetValue(i) != _wallValue &&
                                     waveletResult.GetValue(i) != _initValue;
            }
            
            UpdatePresenter(resultBoolArray);


            return resultBoolArray;
        }

        public bool[] SearchAccessibleAreaSizeThree(Vector2Int startPos, Vector2Int endPos,
            Func<int, int, bool> isWall)
        {
            var tmpBoolArray =
                new bool[_map
                    .GetLength()]; // Declare an another array so that the resultBoolArray is not affected when it is rewritten.
            var resultBoolArray = new bool[_map.GetLength()];
            var waveletResult = WaveletSearch(startPos, endPos, isWall, SearcherSize.SizeThree);
            // 数字がある部分をtrueにする
            for (int i = 0; i < tmpBoolArray.Length; i++)
            {
                tmpBoolArray[i] = waveletResult.GetValue(i) != _wallValue &&
                                  waveletResult.GetValue(i) != _initValue;
            }

            // tureの周囲のマスをtrueにする
            for (int i = 0; i < tmpBoolArray.Length; i++)
            {
                if (tmpBoolArray[i])
                {
                    for (int y = -1; y <= 1; y++)
                    {
                        for (int x = -1; x <= 1; x++)
                        {
                            var pos = _map.ToVector(i);
                            var newX = pos.x + x;
                            var newY = pos.y + y;
                            if (newX < 0 || newX >= _map.Width || newY < 0 || newY >= _map.Height)
                                continue; // SetValueを使いたい
                            resultBoolArray[_map.ToSubscript(pos.x + x, pos.y + y)] = true;
                        }
                    }
                }
            }
            
            UpdatePresenter(resultBoolArray);

            return resultBoolArray;
        }
        
        void UpdatePresenter(bool[] resultBoolArray)
        {
            for (int i = 0; i < resultBoolArray.Length; i++)
            {
                if (resultBoolArray[i])
                {
                    _routePresenters[i]?.SetPresenterActive(true);
                }
                else
                {
                    _routePresenters[i]?.SetPresenterActive(false);
                }
            }
        }


        NumericGridMap WaveletSearch(Vector2Int startPos, Vector2Int endPos,
            Func<int, int, bool> isWall, SearcherSize searcherSize = SearcherSize.SizeOne)
        {
            var searchQue = new Queue<Vector2Int>();
            int n = 1; //1から始まることに注意!!
            bool isComplete = false;

            // 初期化
            _map.FillAll(_initValue);


            if (isWall(startPos.x, startPos.y))
            {
                // Debug.LogError($"startPos:{startPos}は壁です");
                return _map; // 空の配列
            }

            if (isWall(endPos.x, endPos.y))
            {
                // Debug.LogError($"endPos:{endPos}は壁です");
                return _map; // 空の配列
            }

            switch (searcherSize)
            {
                case SearcherSize.SizeOne:
                    BasicSetWall(isWall);
                    break;
                case SearcherSize.SizeThree:
                    SetWallSizeThree(isWall);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(searcherSize), searcherSize, null);
            }

            BasicSetWall(isWall);

            //壁でないマスに数字を順番に振っていく
            // Debug.Log($"WaveletSearchを実行します startPos:{startPos}");
            WaveletSearch();


            //デバッグ用
            StringBuilder debugCell = new StringBuilder();
            for (int y = 0; y < _map.Height; y++)
            {
                for (int x = 0; x < _map.Width; x++)
                {
                    long value = _map.GetValue(x, _map.Height - y - 1);
                    debugCell.AppendFormat("{0,4},",
                        (value >= 0 ? " " : "") + value.ToString("D2")); // 桁数をそろえるために0を追加していると思う
                }
            
                debugCell.AppendLine();
            }
            Debug.Log($"WaveletSearchの結果は\n{debugCell}");


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
                        if (x != 0 && y != 0) continue; //斜めのマスも飛ばす

                        inspectPos = centerPos + new Vector2Int(x, y);
                        if (_map.GetValue(inspectPos) == _initValue) // _edgeValueが帰ってくることもある
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

        // 探索者の大きさが1*1の場合
        void BasicSetWall(Func<int, int, bool> isWall)
        {
            //mapをコピーして、壁のマスを-1にする。
            for (int y = 0; y < _map.Height; y++)
            {
                for (int x = 0; x < _map.Width; x++)
                {
                    if (isWall(x, y))
                    {
                        _map.SetValue(x, y, _wallValue);
                    }
                }
            }
        }

        // 探索者の大きさが3*3の場合
        void SetWallSizeThree(Func<int, int, bool> isWall)
        {
            //mapをコピーして、壁のマスを-1にする。
            for (int y = 0; y < _map.Height; y++)
            {
                for (int x = 0; x < _map.Width; x++)
                {
                    if (isWall(x, y))
                    {
                        // 壁を中心として3*3の範囲を壁にする
                        for (int j = -1; j <= 1; j++)
                        {
                            for (int i = -1; i <= 1; i++)
                            {
                                _map.SetValue(x + i, y + j, _wallValue);
                            }
                        }
                    }
                }
            }
        }
    }
}