using System;
using System.Collections.Generic;
using System.Linq;
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

        public List<Vector2Int> DiagonalSearchShortestRoute(Vector2Int startPos, Vector2Int endPos,
            Vector2Int[] orderInDirectionArray, Func<int, int, bool> isWall,
            SearcherSize searcherSize = SearcherSize.SizeOne)
        {
            List<Vector2Int> shortestRouteList = new List<Vector2Int>();
            bool orderInDirectionFlag = true;
            long maxDistance = 0;

            var searchedMap = _waveletSearchExecutor.WaveletSearch(startPos, endPos, isWall, searcherSize);

            maxDistance = searchedMap.GetValue(endPos);

            //数字をもとに、大きい数字から巻き戻すようにして最短ルートを配列に格納する
            Debug.Log($"StoreRouteAround({endPos},{maxDistance})を実行します");
            StoreShortestRoute(searchedMap, endPos, maxDistance);

            shortestRouteList.Reverse(); //リストを反転させる

            var completeShortestRouteList = RemoveZigzagRoute(searchedMap, shortestRouteList);
            
            //デバッグ
            Debug.Log($"shortestRouteList:{string.Join(",", completeShortestRouteList)}");

            return completeShortestRouteList;

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

                foreach (Vector2Int direction in orderInDirectionArray)
                {
                    if (map.GetValue(centerPos + direction) == distance - 1 &&
                        _waveletSearchExecutor.CanMoveDiagonally(centerPos, centerPos + direction))
                    {
                        shortestRouteList.Add(centerPos);
                        StoreShortestRoute(map, centerPos + direction, distance - 1);
                        break;
                    }
                }
            }
        }

        List<Vector2Int> RemoveZigzagRoute(NumericGridMap map, IReadOnlyList<Vector2Int> diagonalRoute)
        {
            // ジグザグの移動をまっすぐにする
            var resultList = diagonalRoute.ToList();
            var directionList = new List<Vector2Int>() { Vector2Int.up , Vector2Int.left,Vector2Int.down, Vector2Int.right };

            //nonDiagonalRoute.Count - 3まで判定をする
            for (int i = 0; i < diagonalRoute.Count - 2; i++)
            {

                Vector2Int gridPos = diagonalRoute[i];
                Vector2Int nextPos = diagonalRoute[i + 1];
                Vector2Int nextNextPos = diagonalRoute[i + 2];

                foreach (var direction in directionList)
                {
                    // 直線に移動しているならば
                    if (nextNextPos == gridPos + 2 * direction)
                    {
                        if (map.GetValue(gridPos + direction) != _waveletSearchExecutor.WallValue)
                        {
                            // nextPosを書き換える
                            resultList[i + 1] = gridPos + direction;
                            i++;  // nextPosからの直線かどうかの判定はしない（動かしたため）
                        }
                    }
                }
            }
            return resultList;
        }
    }
}