using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.SearchRoute.Scripts;
using JetBrains.Annotations;
using Projects.CarrySystem.RoutingAlgorithm.Interfaces;
using UnityEngine;

#nullable enable

namespace Carry.CarrySystem.Map.Scripts
{
    public enum SearcherSize
    {
        SizeOne,
        SizeThree
    }

    public class WaveletSearchExecutor
    {
        public int WallValue => _wallValue;
        NumericGridMap _map;
        readonly int _initValue = -10; // PlaceNumAroundで重複して数字を置かないようにするために必要
        readonly int _wallValue = -5; // wallのマス

        IRoutePresenter?[] _routePresenters;


        public WaveletSearchExecutor(IGridMap girdMap)
        {
            _map = new NumericGridMap(girdMap.Width, girdMap.Height, _initValue, -8, -88);
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

        public bool[] SearchAccessibleArea(Vector2Int startPos, Vector2Int endPos,
            Func<int, int, bool> isWall, SearcherSize searcherSize = SearcherSize.SizeOne)
        {
            var searchedMap = WaveletSearch(startPos, endPos, isWall, searcherSize);
            var resultBoolArray = CalcAccessibleArea(searchedMap, searcherSize);

            UpdatePresenter(resultBoolArray);

            return resultBoolArray;
        }


        public NumericGridMap WaveletSearch(Vector2Int startPos, Vector2Int endPos,
            Func<int, int, bool> isWall, SearcherSize searcherSize = SearcherSize.SizeOne)
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

            if (isWall(endPos.x, endPos.y))
            {
                Debug.Log($"endPos:{endPos}は壁です");
                return _map; // _initValueのみが入ったmap
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
                        if (_map.GetValue(inspectPos) == _initValue && CanMoveDiagonally(centerPos, inspectPos)) // _edgeValueが帰ってくることもある
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
                    }else if(x == 0 || x == _map.Width -1 || y == 0 || y == _map.Height -1)
                    {
                        // 壁の周りのマスを壁にする
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
            var tmpBoolArray = new bool[map.Length];
            var resultBoolArray = new bool[map.Length];
            var waveletResult = map;
            // 数字がある部分をtrueにする
            for (int i = 0; i < tmpBoolArray.Length; i++)
            {
                tmpBoolArray[i] = waveletResult.GetValue(i) != _wallValue &&
                                  waveletResult.GetValue(i) != _initValue;
            }

            switch (searcherSize)
            {
                case SearcherSize.SizeOne:
                    resultBoolArray = tmpBoolArray;
                    break;
                case SearcherSize.SizeThree:
                    // set true to the squares around ture
                    for (int i = 0; i < tmpBoolArray.Length; i++)
                    {
                        if (tmpBoolArray[i])
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
        
        void UpdatePresenter(bool[] resultBoolArray)
        {
            for (int i = 0; i < resultBoolArray.Length; i++)
            {
                _routePresenters[i]?.SetPresenterActive(resultBoolArray[i]);
            }
        }
    }
}