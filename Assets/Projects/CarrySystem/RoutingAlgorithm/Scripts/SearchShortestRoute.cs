// using System.Collections.Generic;
// using UnityEngine;
//
// namespace Carry.CarrySystem.RoutingAlgorithm.Scripts
// {
//     public class SearchShortestRoute
//     {
//         int _width;
//         int _height;
//         int[] _values; // 2次元配列のように扱う
//         int _initiValue = -10; // PlaceNumAroundで重複して数字を置かないようにするために必要
//         int _wallValue = -1; // wallのマス
//         int _errorValue = -88;
//         
//         public SearchShortestRoute(int width, int height)
//         {
//             _width = width;
//             _height = height;
//             _values = new int[_width * _height];
//         }
//         
//         public List<Vector2Int> SearchShortestRouteFromTo(Vector2Int startPos, Vector2Int endPos)
//         {
//             List<Vector2Int> shortestRouteList = new List<Vector2Int>();
//             shortestRouteList = NonDiagonalSearchShortestRoute(startPos, endPos);
//             return shortestRouteList;
//         }
//         
//         
//         private static List<Vector2Int> NonDiagonalSearchShortestRoute(Vector2Int startPos, Vector2Int endPos,
//             Vector2Int[] orderInDirectionArray1, Vector2Int[] orderInDirectionArray2)
//         {
//             int width = GameManager.instance.mapMGR.GetMapWidth();
//             int height = GameManager.instance.mapMGR.GetMapHeight();
//             int[] _values = null; //2次元配列のように扱う
//             int _initiValue = -10; //PlaceNumAroundで重複して数字を置かないようにするために必要
//             int _wallValue = -1; //wallのマス
//             int _errorValue = -88;
//             List<Vector2Int> shortestRouteList;
//
//             Queue<Vector2Int> searchQue = new Queue<Vector2Int>();
//             int n = 1; //1から始まることに注意!!
//             bool isComplete = false;
//             int maxDistance = 0;
//
//             bool orderInDirectionFlag = true; //探索するたびに優先順位を切り替えるのに必要
//             Vector2Int[] orderInDirectionArray;
//
//             //引数が適切かどうかチェックする
//             if (width <= 0 || height <= 0)
//             {
//                 Debug.LogWarning("SearchShortestRouteの幅または高さが0以下になっています");
//                 return null;
//             }
//
//             if (GameManager.instance.mapMGR.GetMapValue(startPos) % GameManager.instance.wallID == 0)
//             {
//                 Debug.LogWarning("SearchShortestRouteのstatPosにwallIDが含まれています");
//                 return null;
//             }
//
//             if (GameManager.instance.mapMGR.GetMapValue(endPos) % GameManager.instance.wallID == 0)
//             {
//                 Debug.LogWarning("SearchShortestRouteのendPosにwallIDが含まれています");
//                 return null;
//             }
//
//
//             //初期化
//             _values = new int[width * height];
//             shortestRouteList = new List<Vector2Int>();
//             FillAll(_initiValue); //mapの初期化は_initiValueで行う
//
//
//             //次にmapをコピーして、壁のマスを-1にする。
//             for (int y = 0; y < height; y++)
//             {
//                 for (int x = 0; x < width; x++)
//                 {
//                     if (GameManager.instance.mapMGR.GetMap().GetValue(x, y) % GameManager.instance.wallID == 0)
//                     {
//                         SetValue(x, y, _wallValue);
//                     }
//                 }
//             }
//
//             //壁でないマスに数字を順番に振っていく
//             Debug.Log($"WaveletSearchを実行します startPos:{startPos}");
//             WaveletSearch();
//
//
//             //デバッグ用
//             string debugCell = "";
//             for (int y = 0; y < height; y++)
//             {
//                 for (int x = 0; x < width; x++)
//                 {
//                     debugCell += $"{GetValue(x, height - y - 1)}".PadRight(3) + ",";
//                 }
//
//                 debugCell += "\n";
//             }
//
//             Debug.Log($"WaveletSearchの結果は\n{debugCell}");
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
//             //Debug.Log($"shortestRouteList:{string.Join(",", shortestRouteList)}");
//             GameManager.instance.debugMGR.DebugRoute(shortestRouteList);
//
//             return shortestRouteList;
//
//             ////////////////////////////////////////////////////////////////////
//
//             //以下ローカル関数
//
//
//             void WaveletSearch()
//             {
//                 SetValueByVector(startPos, 0); //startPosの部分だけ周囲の判定を行わないため、ここで個別に設定する
//                 searchQue.Enqueue(startPos);
//
//                 while (!isComplete)
//                 {
//                     int loopNum = searchQue.Count; //前のループでキューに追加された個数を数える
//                     //Debug.Log($"i:{n}のときloopNum:{loopNum}");
//                     for (int k = 0; k < loopNum; k++)
//                     {
//                         if (isComplete) break;
//                         //Debug.Log($"PlaceNumAround({searchQue.Peek()})を実行します");
//                         PlaceNumAround(searchQue.Dequeue());
//                     }
//
//                     n++; //前のループでキューに追加された文を処理しきれたら、インデックスを増やして次のループに移る
//
//                     if (n > 100) //無限ループを防ぐ用
//                     {
//                         isComplete = true;
//                         Debug.Log("SearchShortestRouteのwhile文でループが100回行われてしまいました");
//                     }
//                 }
//             }
//
//             void StoreShortestRoute(Vector2Int centerPos, int distance) //再帰的に呼ぶ
//             {
//                 if (distance == 0)
//                 {
//                     shortestRouteList.Add(centerPos);
//                     return;
//                 }
//
//                 if (distance < 0) return; //0までQueに入れれば十分
//
//                 Debug.Log($"GetValue({centerPos})は{GetValueFromVector(centerPos)}、distance:{distance}");
//
//
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
//                     if (GetValueFromVector(centerPos + direction) == distance - 1 &&
//                         CanMoveDiagonally(centerPos, centerPos + direction))
//                     {
//                         shortestRouteList.Add(centerPos);
//                         StoreShortestRoute(centerPos + direction, distance - 1);
//                         break;
//                     }
//                 }
//             }
//
//
//             void PlaceNumAround(Vector2Int centerPos)
//             {
//                 Vector2Int inspectPos;
//
//                 //8マス または 4マス判定する（真ん中のマスの判定は必要ない）（isDiagonalSearchに依る）
//                 for (int y = -1; y < 2; y++)
//                 {
//                     for (int x = -1; x < 2; x++)
//                     {
//                         if (x == 0 && y == 0) continue; //真ん中のマスは飛ばす
//                         if (x != 0 && y != 0) continue; //斜めのマスも飛ばす
//
//                         inspectPos = centerPos + new Vector2Int(x, y);
//                         //Debug.Log($"centerPos:{centerPos},inspectPos:{inspectPos}のとき、CanMove(centerPos, inspectPos):{CanMove(centerPos, inspectPos)}");
//                         if (GetValueFromVector(inspectPos) == _initiValue && CanMoveDiagonally(centerPos, inspectPos))
//                         {
//                             SetValueByVector(inspectPos, n);
//                             searchQue.Enqueue(inspectPos);
//                             //Debug.Log($"({inspectPos})を{n}にし、探索用キューに追加しました。");
//                         }
//                         else //このelseはデバッグ用
//                         {
//                             //Debug.Log($"{inspectPos}は初期値が入っていない　または　斜め移動でいけません\nGetValueFromVector({inspectPos}):{GetValueFromVector(inspectPos)}, CanMoveDiagonally({centerPos}, {inspectPos}):{CanMoveDiagonally(centerPos, inspectPos)}");
//                         }
//
//                         if (inspectPos == endPos && CanMoveDiagonally(centerPos, inspectPos))
//                         {
//                             isComplete = true;
//                             SetValueByVector(inspectPos, n);
//                             maxDistance = n;
//                             //Debug.Log($"isCompleteをtrueにしました。maxDistance:{maxDistance}");
//                             break; //探索終了
//                         }
//                     }
//                 }
//             }
//
//             bool CanMoveDiagonally(Vector2Int prePos, Vector2Int afterPos)
//             {
//                 Vector2Int directionVector = afterPos - prePos;
//
//                 //斜め移動の時にブロックの角を移動することはできない
//                 if (directionVector.x != 0 && directionVector.y != 0)
//                 {
//                     //水平方向の判定
//                     if (GetValue(prePos.x + directionVector.x, prePos.y) == _wallValue)
//                     {
//                         return false;
//                     }
//
//                     //垂直方向の判定
//                     if (GetValue(prePos.x, prePos.y + directionVector.y) == _wallValue)
//                     {
//                         return false;
//                     }
//                 }
//
//                 return true;
//             }
//
//
//             //Getter
//             int GetValue(int x, int y)
//             {
//                 if (IsOutOfRange(x, y))
//                 {
//                     Debug.LogError($"領域外の値を取得しようとしました (x,y):({x},{y})");
//                     return _errorValue;
//                 }
//
//                 if (IsOnTheEdge(x, y))
//                 {
//                     Debug.Log($"IsOnTheEdge({x},{y})がtrueです");
//                     return _wallValue;
//                 }
//
//                 return _values[ToSubscript(x, y)];
//             }
//
//             int GetValueFromVector(Vector2Int vector) //ローカル関数はオーバーライドができないことに注意
//             {
//                 return GetValue(vector.x, vector.y);
//             }
//
//
//             //Setter
//             void SetValue(int x, int y, int value)
//             {
//                 if (IsOutOfRange(x, y))
//                 {
//                     Debug.LogError($"領域外に値を設定しようとしました (x,y):({x},{y})");
//                     return;
//                 }
//
//                 _values[ToSubscript(x, y)] = value;
//             }
//
//             void SetValueByVector(Vector2Int vector, int value)
//             {
//                 SetValue(vector.x, vector.y, value);
//             }
//
//             //添え字を変換する
//             int ToSubscript(int x, int y)
//             {
//                 return x + (y * width);
//             }
//
//             bool IsOutOfRange(int x, int y)
//             {
//                 if (x < -1 || x > width)
//                 {
//                     return true;
//                 }
//
//                 if (y < -1 || y > height)
//                 {
//                     return true;
//                 }
//
//                 //mapの中
//                 return false;
//             }
//
//             bool IsOnTheEdge(int x, int y)
//             {
//                 if (x == -1 || x == width)
//                 {
//                     return true;
//                 }
//
//                 if (y == -1 || y == height)
//                 {
//                     return true;
//                 }
//
//                 return false;
//             }
//
//             void FillAll(int value) //edgeValueまでは書き換えられないことに注意
//             {
//                 for (int j = 0; j < height; j++)
//                 {
//                     for (int i = 0; i < width; i++)
//                     {
//                         _values[ToSubscript(i, j)] = value;
//                     }
//                 }
//             }
//         }
//     }
// }