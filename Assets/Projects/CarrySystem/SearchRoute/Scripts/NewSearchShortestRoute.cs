﻿using System;
using System.Collections.Generic;
using System.Text;
using Carry.CarrySystem.SearchRoute.Scripts;
using UnityEngine;

namespace Carry.CarrySystem.Map.Scripts
{
    public class NewSearchShortestRoute
    {
        public int InitValue => _initValue; 
        public int WallValue => _wallValue;
        int _width;
        int _height;
        int[] _values;
        int _initValue = -10; // PlaceNumAroundで重複して数字を置かないようにするために必要
        int _wallValue = -1; // wallのマス
        int _errorValue = -88;
        
        public NewSearchShortestRoute(int width, int height)
        {
            _width = width;
            _height = height;
            _values = new int[width * height];
        }

        public int[] WaveletSearch(Vector2Int startPos, Vector2Int endPos,
            Func<int, int, bool> isWall)
        {
            var searchQue = new Queue<Vector2Int>();
            int n = 1; //1から始まることに注意!!
            bool isComplete = false;
            int maxDistance = 0;

            bool orderInDirectionFlag = true; //探索するたびに優先順位を切り替えるのに必要
            Vector2Int[] orderInDirectionArray;

            // 初期化
            FillAll(_initValue); //mapの初期化は_initValueで行う


            if (isWall(startPos.x, startPos.y))
            {
                Debug.LogError($"startPos:{startPos}は壁です");
                return _values; // 空の配列
            }

            if (isWall(startPos.x, startPos.y))
            {
                Debug.LogError($"endPos:{endPos}は壁です");
                return _values; // 空の配列
            }


            //mapをコピーして、壁のマスを-1にする。
            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    if (isWall(x, y))
                    {
                        SetValue(x, y, _wallValue);
                    }
                }
            }

            //壁でないマスに数字を順番に振っていく
            Debug.Log($"WaveletSearchを実行します startPos:{startPos}");
            WaveletSearch();


            //デバッグ用
            StringBuilder debugCell = new StringBuilder();
            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    int value = GetValue(x, _height - y - 1);
                    debugCell.AppendFormat("{0,4},",
                        (value >= 0 ? " " : "") + value.ToString("D2")); // 桁数をそろえるために0を追加していると思う
                }

                debugCell.AppendLine();
            }

            Debug.Log($"WaveletSearchの結果は\n{debugCell}");


            return _values;

            ////////////////////////////////////////////////////////////////////

            //以下ローカル関数


            void WaveletSearch()
            {
                SetValueByVector(startPos, 0); //startPosの部分だけ周囲の判定を行わないため、ここで個別に設定する
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
                for (int y = -1; y < 2; y++)
                {
                    for (int x = -1; x < 2; x++)
                    {
                        if (x == 0 && y == 0) continue; //真ん中のマスは飛ばす
                        if (x != 0 && y != 0) continue; //斜めのマスも飛ばす

                        inspectPos = centerPos + new Vector2Int(x, y);
                        //Debug.Log($"centerPos:{centerPos},inspectPos:{inspectPos}のとき、CanMove(centerPos, inspectPos):{CanMove(centerPos, inspectPos)}");
                        if (GetValueFromVector(inspectPos) == _initValue && CanMoveDiagonally(centerPos, inspectPos))
                        {
                            SetValueByVector(inspectPos, n);
                            searchQue.Enqueue(inspectPos);
                            //Debug.Log($"({inspectPos})を{n}にし、探索用キューに追加しました。");
                        }
                        else //このelseはデバッグ用
                        {
                            //Debug.Log($"{inspectPos}は初期値が入っていない　または　斜め移動でいけません\nGetValueFromVector({inspectPos}):{GetValueFromVector(inspectPos)}, CanMoveDiagonally({centerPos}, {inspectPos}):{CanMoveDiagonally(centerPos, inspectPos)}");
                        }

                        if (inspectPos == endPos && CanMoveDiagonally(centerPos, inspectPos))
                        {
                            isComplete = true;
                            SetValueByVector(inspectPos, n);
                            maxDistance = n;
                            //Debug.Log($"isCompleteをtrueにしました。maxDistance:{maxDistance}");
                            break; //探索終了
                        }
                    }
                }
            }

            bool CanMoveDiagonally(Vector2Int prePos, Vector2Int afterPos)
            {
                Vector2Int directionVector = afterPos - prePos;

                //斜め移動の時にブロックの角を移動することはできない
                if (directionVector.x != 0 && directionVector.y != 0)
                {
                    //水平方向の判定
                    if (GetValue(prePos.x + directionVector.x, prePos.y) == _wallValue)
                    {
                        return false;
                    }

                    //垂直方向の判定
                    if (GetValue(prePos.x, prePos.y + directionVector.y) == _wallValue)
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        void FillAll(int value) //edgeValueまでは書き換えられないことに注意
        {
            for (int j = 0; j < _height; j++)
            {
                for (int i = 0; i < _width; i++)
                {
                    _values[ToSubscript(i, j)] = value;
                }
            }
        }

        //添え字を変換する
        int ToSubscript(int x, int y)
        {
            return x + (y * _width);
        }

        //Getter
        int GetValue(int x, int y)
        {
            if (IsOutOfRange(x, y))
            {
                Debug.LogError($"領域外の値を取得しようとしました (x,y):({x},{y})");
                return _errorValue;
            }

            if (IsOnTheEdge(x, y))
            {
                Debug.Log($"IsOnTheEdge({x},{y})がtrueです");
                return _wallValue;
            }

            return _values[ToSubscript(x, y)];
        }

        int GetValueFromVector(Vector2Int vector) //ローカル関数はオーバーライドができないことに注意
        {
            return GetValue(vector.x, vector.y);
        }


        //Setter
        void SetValue(int x, int y, int value)
        {
            if (IsOutOfRange(x, y))
            {
                Debug.LogError($"領域外に値を設定しようとしました (x,y):({x},{y})");
                return;
            }

            _values[ToSubscript(x, y)] = value;
        }

        void SetValueByVector(Vector2Int vector, int value)
        {
            SetValue(vector.x, vector.y, value);
        }


        bool IsOutOfRange(int x, int y)
        {
            if (x < -1 || x > _width)
            {
                return true;
            }

            if (y < -1 || y > _height)
            {
                return true;
            }

            //mapの中
            return false;
        }

        bool IsOnTheEdge(int x, int y)
        {
            if (x == -1 || x == _width)
            {
                return true;
            }

            if (y == -1 || y == _height)
            {
                return true;
            }

            return false;
        }
    }
}

