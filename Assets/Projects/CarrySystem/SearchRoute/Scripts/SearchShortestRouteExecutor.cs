// using System.Collections.Generic;
// using Carry.CarrySystem.SearchRoute.Scripts;
// using UnityEngine;
// #nullable enable
//
// namespace Projects.CarrySystem.SearchRoute.Scripts
// {
//     public class SearchShortestRouteExecutor
//     {
//                 public List<Vector2Int> NonDiagonalSearchShortestRoute(Vector2Int startPos, Vector2Int endPos,
//             Vector2Int[] orderInDirectionArray1)
//         {
//             List<Vector2Int> shortestRouteList = new List<Vector2Int>();
//             Vector2Int[] orderInDirectionArray2 = OrderInDirectionArrayContainer.SwapPairwise(orderInDirectionArray1);
//             Vector2Int[] orderInDirectionArray;
//             bool orderInDirectionFlag = false;
//             int maxDistance = 0;
//
//             
//             //数字をもとに、大きい数字から巻き戻すようにして最短ルートを配列に格納する
//             Debug.Log($"StoreRouteAround({endPos},{maxDistance})を実行します");
//             StoreShortestRoute(endPos, maxDistance);
//
//             shortestRouteList.Reverse(); //リストを反転させる
//
//
//             //デバッグ
//             // Debug.Log($"shortestRouteList:{string.Join(",", shortestRouteList)}");
//             // GameManager.instance.debugMGR.DebugRoute(shortestRouteList);
//
//             return shortestRouteList;
//             
//             void StoreShortestRoute(Vector2Int centerPos, int distance) //再帰的に呼ぶ
//             {
//                 if (distance == 0)
//                 {
//                     shortestRouteList.Add(centerPos);
//                     return;
//                 }
//                 if (distance < 0) return; //0までQueに入れれば十分
//
//                 Debug.Log($"GetValue({centerPos})は{GetValueFromVector(centerPos)}、distance:{distance}");
//
//                 // 斜めに探索するように優先度を切り替える
//                 if (orderInDirectionFlag)
//                 {
//                     orderInDirectionArray = orderInDirectionArray1;
//                     orderInDirectionFlag = false;
//                 }
//                 else
//                 {
//                     orderInDirectionArray = orderInDirectionArray2;
//                     orderInDirectionFlag = true;
//                 }
//                 
//                 foreach (Vector2Int direction in orderInDirectionArray)
//                 {
//                     if (GetValueFromVector(centerPos + direction) == distance - 1 && CanMoveDiagonally(centerPos, centerPos + direction))
//                     {
//                         shortestRouteList.Add(centerPos);
//                         StoreShortestRoute(centerPos + direction, distance - 1);
//                         break;
//                     }
//                 }
//             }
//         }
//         bool CanMoveDiagonally(Vector2Int prePos, Vector2Int afterPos)
//         {
//             Vector2Int directionVector = afterPos - prePos;
//
//             //斜め移動の時にブロックの角を移動することはできない
//             if (directionVector.x != 0 && directionVector.y != 0)
//             {
//                 //水平方向の判定
//                 if (GetValue(prePos.x + directionVector.x, prePos.y) == _wallValue)
//                 {
//                     return false;
//                 }
//
//                 //垂直方向の判定
//                 if (GetValue(prePos.x, prePos.y + directionVector.y) == _wallValue)
//                 {
//                     return false;
//                 }
//             }
//
//             return true;
//         }
//     }
// }