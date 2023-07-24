﻿using System;
using System.Collections.Generic;
using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.SearchRoute.Scripts;
using UnityEngine;
#nullable enable

namespace Projects.CarrySystem.SearchRoute.Scripts
{
    public class SearchShortestRouteExecutor
    {
        readonly WaveletSearchExecutor _waveletSearchExecutor;
        public SearchShortestRouteExecutor(WaveletSearchExecutor waveletSearchExecutor)
        {
            _waveletSearchExecutor = waveletSearchExecutor;
        }
                public List<Vector2Int> NonDiagonalSearchShortestRoute(Vector2Int startPos, Vector2Int endPos,
            Vector2Int[] orderInDirectionArray1, Func<int, int, bool> isWall, SearcherSize searcherSize = SearcherSize.SizeOne)
        {
            List<Vector2Int> shortestRouteList = new List<Vector2Int>();
            Vector2Int[] orderInDirectionArray2 = OrderInDirectionArrayContainer.SwapPairwise(orderInDirectionArray1);
            Vector2Int[] orderInDirectionArray;
            bool orderInDirectionFlag = true;
            long maxDistance = 0;

            var searchedMap = _waveletSearchExecutor.WaveletSearch(startPos, endPos, isWall, searcherSize);

            maxDistance = searchedMap.GetValue(endPos);
            
            //数字をもとに、大きい数字から巻き戻すようにして最短ルートを配列に格納する
            Debug.Log($"StoreRouteAround({endPos},{maxDistance})を実行します");
            StoreShortestRoute(searchedMap, endPos, maxDistance);

            shortestRouteList.Reverse(); //リストを反転させる


            //デバッグ
             Debug.Log($"shortestRouteList:{string.Join(",", shortestRouteList)}");

            return shortestRouteList;
            
            /////////////////////////////
            // 以下はローカル関数
            
            void StoreShortestRoute(NumericGridMap map, Vector2Int centerPos, long distance) //再帰的に呼ぶ
            {
                if (distance == 0)
                {
                    shortestRouteList.Add(centerPos);
                    return;
                }
                if (distance < 0) return; //0までQueに入れれば十分

                Debug.Log($"GetValue({centerPos})は{map.GetValue(centerPos)}、distance:{distance}");

                // 斜めの経路ができるように優先度を切り替える
                if (orderInDirectionFlag)
                {
                    orderInDirectionArray = orderInDirectionArray1;
                    orderInDirectionFlag = false;
                }
                else
                {
                    orderInDirectionArray = orderInDirectionArray2;
                    orderInDirectionFlag = true;
                }
                
                foreach (Vector2Int direction in orderInDirectionArray)
                {
                    if (map.GetValue(centerPos + direction) == distance - 1 && CanMoveDiagonally(map, centerPos, centerPos + direction))
                    {
                        shortestRouteList.Add(centerPos);
                        StoreShortestRoute(map,centerPos + direction, distance - 1);
                        break;
                    }
                }
            }
        }
        bool CanMoveDiagonally(NumericGridMap map, Vector2Int prePos, Vector2Int afterPos)
        {
            Vector2Int directionVector = afterPos - prePos;

            //斜め移動の時にブロックの角を移動することはできない
            if (directionVector.x != 0 && directionVector.y != 0)
            {
                //水平方向の判定
                if (map.GetValue(prePos.x + directionVector.x, prePos.y) == _waveletSearchExecutor.WallValue)
                {
                    return false;
                }

                //垂直方向の判定
                if (map.GetValue(prePos.x, prePos.y + directionVector.y) == _waveletSearchExecutor.WallValue)
                {
                    return false;
                }
            }

            return true;
        }
    }
}