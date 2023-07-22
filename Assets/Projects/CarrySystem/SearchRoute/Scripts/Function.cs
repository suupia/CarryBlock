// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
//
// namespace Carry.CarrySystem.RoutingAlgorithm.Scripts
// {
//     public static class Function
//     {
//         //味方
//         // 8 6 4
//         // 7 * 2
//         // 5 3 1 の優先順位で判定していく
//
//         static Vector2Int[] orderInDirectionArrayForAlly1 = new Vector2Int[]
//         {
//             Vector2Int.right + Vector2Int.down, Vector2Int.right, Vector2Int.down, Vector2Int.right + Vector2Int.up,
//             Vector2Int.left + Vector2Int.down, Vector2Int.up, Vector2Int.left, Vector2Int.left + Vector2Int.up
//         };
//
//         // 8 7 5
//         // 6 * 3
//         // 4 2 1 の優先順位で判定していく
//
//         static Vector2Int[] orderInDirectionArrayForAlly2 = new Vector2Int[]
//         {
//             Vector2Int.right + Vector2Int.down, Vector2Int.down, Vector2Int.right, Vector2Int.left + Vector2Int.down,
//             Vector2Int.right + Vector2Int.up, Vector2Int.left, Vector2Int.up, Vector2Int.left + Vector2Int.up
//         };
//
//         //敵
//         // 1 3 5
//         // 2 * 7
//         // 4 6 8 の優先順位で判定していく
//
//         static Vector2Int[] orderInDirectionArrayForEnemy1 = new Vector2Int[]
//         {
//             Vector2Int.left + Vector2Int.up, Vector2Int.left, Vector2Int.up, Vector2Int.left + Vector2Int.down,
//             Vector2Int.right + Vector2Int.up, Vector2Int.down, Vector2Int.right, Vector2Int.right + Vector2Int.down
//         };
//
//         // 1 2 4
//         // 3 * 6
//         // 5 7 8 の優先順位で判定していく
//
//         static Vector2Int[] orderInDirectionArrayForEnemy2 = new Vector2Int[]
//         {
//             Vector2Int.left + Vector2Int.up, Vector2Int.up, Vector2Int.left, Vector2Int.right + Vector2Int.up,
//             Vector2Int.left + Vector2Int.down, Vector2Int.right, Vector2Int.down, Vector2Int.right + Vector2Int.down
//         };
//
//         public static List<Vector2Int>
//             NonDiagonalSearchShortestRoute(Vector2Int startPos, Vector2Int endPos) //PlaceNumAroundのとき4マス判定する
//         {
//             return NonDiagonalSearchShortestRoute(startPos, endPos, orderInDirectionArrayForEnemy1,
//                 orderInDirectionArrayForEnemy2);
//         }
//
//         public static List<Vector2Int>
//             DiagonalSearchShortestRouteForAlly(Vector2Int startPos,
//                 Vector2Int endPos) //startPosからendPosへの斜め移動ありの最短経路を返す（startPos、endPosどちらも含む）
//         {
//             return ConvertToDiagonalRoute(NonDiagonalSearchShortestRoute(startPos, endPos,
//                 orderInDirectionArrayForAlly1, orderInDirectionArrayForAlly2));
//         }
//
//         public static List<Vector2Int>
//             DiagonalSearchShortestRouteForEnemy(Vector2Int startPos,
//                 Vector2Int endPos) //StoreShortestRouteの優先順位が異なる(乱数で決定)
//         {
//             if (Random.Range(0, 2) == 0)
//             {
//                 return ConvertToDiagonalRoute(NonDiagonalSearchShortestRoute(startPos, endPos,
//                     orderInDirectionArrayForEnemy1, orderInDirectionArrayForEnemy2));
//             }
//             else
//             {
//                 return ConvertToDiagonalRoute(NonDiagonalSearchShortestRoute(startPos, endPos,
//                     orderInDirectionArrayForEnemy2, orderInDirectionArrayForEnemy1));
//             }
//         }
//
//         public static List<Vector2Int> DiagonalSearchShortestRouteForEnemy(Vector2Int startPos, Vector2Int endPos,
//             bool isPriorityToTheSide) //StoreShortestRouteの優先順位が異なる(引数で決定)
//         {
//             if (isPriorityToTheSide)
//             {
//                 return ConvertToDiagonalRoute(NonDiagonalSearchShortestRoute(startPos, endPos,
//                     orderInDirectionArrayForEnemy1, orderInDirectionArrayForEnemy2));
//             }
//             else
//             {
//                 return ConvertToDiagonalRoute(NonDiagonalSearchShortestRoute(startPos, endPos,
//                     orderInDirectionArrayForEnemy2, orderInDirectionArrayForEnemy1));
//             }
//         }
//
//         public static List<Vector2Int> SearchShortestDiagonalRouteToAllyCastle(Vector2Int startPos)
//         {
//             return ConvertToDiagonalRoute(NonDiagonalSearchShortestDiagonalRouteToCastle(
//                 GameManager.instance.mapMGR.GetAllysCastlePos(), startPos, orderInDirectionArrayForAlly1,
//                 orderInDirectionArrayForAlly2));
//         }
//
//         public static List<Vector2Int> SearchShortestDiagonalRouteToEnemyCastle(Vector2Int startPos)
//         {
//             return ConvertToDiagonalRoute(NonDiagonalSearchShortestDiagonalRouteToCastle(
//                 GameManager.instance.mapMGR.GetEnemysCastlePos(), startPos, orderInDirectionArrayForEnemy1,
//                 orderInDirectionArrayForEnemy2));
//         }
//
//         public static bool isWithinTheAttackRange(Vector2Int gridPos, int attackRange, int targetID,
//             out Vector2Int targetPos) //最も近い攻撃対象の座標を返す （存在しないときはVector2Int.zeroを返す）
//         {
//             //攻撃できるかどうかでもう一度ループを回す必要があるため、攻撃できるかどうかと、最も近いターゲットの座標を取得するのを同時に行っている
//
//             int lookingForValue = 1; //索敵範囲の値
//             int notLookingForValue = 0; //索敵範囲外の値
//             int centerValue = 0; //原点の値
//
//             Vector2Int vector; //ループ内で使い、(i,j)をワールド座標に直したもの
//             List<Vector2Int> nearestTargetList = new List<Vector2Int>();
//
//             int[,] searchRangeArray;
//             int maxRange = attackRange; //kはmaxRangeはとらないことに注意
//
//
//             //索敵範囲内の攻撃対象の位置をListに追加する
//             for (int k = 0; k < maxRange; k++) //kは中心のマスから何マスまで歩けるかを表す
//             {
//                 //Debug.Log($"{k}回目のループを開始します");
//
//                 searchRangeArray = CalcSearchRangeArray(k, lookingForValue, notLookingForValue, centerValue);
//                 for (int j = 0; j < searchRangeArray.GetLength(0); j++)
//                 {
//                     for (int i = 0; i < searchRangeArray.GetLength(1); i++)
//                     {
//                         vector = new Vector2Int(gridPos.x - (k + 1) + i, gridPos.y - (k + 1) + j); //ワールド座標に変換する
//
//                         if (vector.x < 0 || vector.y < 0 || vector.x > GameManager.instance.mapMGR.GetMapWidth() ||
//                             vector.y > GameManager.instance.mapMGR.GetMapHeight())
//                         {
//                             continue;
//                         }
//
//                         if (searchRangeArray[i, j] == lookingForValue &&
//                             GameManager.instance.mapMGR.GetMapValue(vector) % targetID == 0)
//                         {
//                             nearestTargetList.Add(vector);
//                         }
//                     }
//                 }
//
//                 if (nearestTargetList.Count > 0)
//                 {
//                     //Debug.Log($"nearestTargetList[0]={nearestTargetList[0]}");
//                     break;
//                 }
//             }
//
//
//             // 2  1
//             // 4  3 と優先順位をつける（攻撃範囲内に存在するかを判定するだけならいらないが、ログを出した時に混乱しないように優先順位をつけておく）
//             //targetが同じ距離にあるときに優先順位をつけて検索するために Listの中身をソートする
//             nearestTargetList.Sort((a, b) => b.y - a.y); //まずy座標に関して降順でソートする
//             nearestTargetList.Sort((a, b) => b.x - a.x); //次にx座標に関して降順でソートする
//
//             if (nearestTargetList.Count > 0)
//             {
//                 targetPos = nearestTargetList[0];
//                 //Debug.Log("nearestTargetList:" + string.Join(",", nearestTargetList) + $"\ntargetPos:{targetPos}");
//                 return true;
//             }
//             else
//             {
//                 targetPos = Vector2Int.zero; //nullの代わり
//                 //Debug.Log("nearestTargetList:" + string.Join(",", nearestTargetList) + $"\ntargetPos:{targetPos}");
//                 return false;
//             }
//         }
//
//         public static bool isWithinTheAttackRangeForFacility(Vector2Int gridPos, int attackRange, int targetID,
//             Vector2Int targetCurrentPos, out Vector2Int targetPos) //最も近い攻撃対象の座標を返す （存在しないときはVector2Int.zeroを返す）
//         {
//             //攻撃できるかどうかでもう一度ループを回す必要があるため、攻撃できるかどうかと、最も近いターゲットの座標を取得するのを同時に行っている
//
//             int lookingForValue = 1; //索敵範囲の値
//             int notLookingForValue = 0; //索敵範囲外の値
//             int centerValue = 0; //原点の値
//
//             Vector2Int vector; //ループ内で使い、(i,j)をワールド座標に直したもの
//             List<Vector2Int> nearestTargetList = new List<Vector2Int>();
//
//             int[,] searchRangeArray;
//             int maxRange = attackRange; //kはmaxRangeはとらないことに注意
//
//             //索敵範囲内の攻撃対象の位置をListに追加する
//             for (int k = 0; k < maxRange; k++) //kは中心のマスから何マスまで歩けるかを表す
//             {
//                 //Debug.Log($"{k}回目のループを開始します");
//
//                 searchRangeArray = CalcSearchRangeArray(k, lookingForValue, notLookingForValue, centerValue);
//                 for (int j = 0; j < searchRangeArray.GetLength(0); j++)
//                 {
//                     for (int i = 0; i < searchRangeArray.GetLength(1); i++)
//                     {
//                         vector = new Vector2Int(gridPos.x - (k + 1) + i, gridPos.y - (k + 1) + j); //ワールド座標に変換する
//
//                         if (vector.x < 0 || vector.y < 0 || vector.x > GameManager.instance.mapMGR.GetMapWidth() ||
//                             vector.y > GameManager.instance.mapMGR.GetMapHeight())
//                         {
//                             continue;
//                         }
//
//                         if (searchRangeArray[i, j] == lookingForValue &&
//                             GameManager.instance.mapMGR.GetMapValue(vector) % targetID == 0)
//                         {
//                             nearestTargetList.Add(vector);
//                         }
//                     }
//                 }
//
//                 if (nearestTargetList.Count > 0)
//                 {
//                     //Debug.Log($"nearestTargetList[0]={nearestTargetList[0]}");
//                     break;
//                 }
//             }
//
//
//             // 2  1
//             // 4  3 と優先順位をつける（攻撃範囲内に存在するかを判定するだけならいらないが、ログを出した時に混乱しないように優先順位をつけておく）
//             //targetが同じ距離にあるときに優先順位をつけて検索するために Listの中身をソートする
//             nearestTargetList.Sort((a, b) => b.y - a.y); //まずy座標に関して降順でソートする
//             nearestTargetList.Sort((a, b) => b.x - a.x); //次にx座標に関して降順でソートする
//
//             if (nearestTargetList.Count > 0)
//             {
//                 foreach (Vector2Int nearestTargetPos in nearestTargetList)
//                 {
//                     if (targetCurrentPos == nearestTargetPos)
//                     {
//                         targetPos = nearestTargetPos;
//                         return true;
//                     }
//                 }
//
//                 targetPos = Vector2Int.zero; //nullの代わり
//                 return false;
//             }
//             else
//             {
//                 targetPos = Vector2Int.zero; //nullの代わり
//                 return false;
//             }
//         }
//
//
//         public static List<Vector2Int> SearchRandomDiagonalRouteForEnemy(Vector2Int startPos, Vector2Int endPos)
//         {
//             //数字を置くとき周り8マスに置いていく
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
//             var queCopyArray = new Vector2Int[100]; //100個作って置けば、キューをコピーできるはず
//             var relayPosList = new List<Vector2Int>(); //分岐数が最大になる中継地点の座標
//             int groupNum = 0; //分岐数が最大になる時のキューを保持するために必要
//             bool[,] groupFlagMap = new bool[height, width]; //同じグループに属しているかどうかを判定するために必要
//             int randRouteNum; //relayPosListの何番目を採用するかを決める
//             bool isAdoptADistantJunction; //分岐数が最大になるもののうち、遠いものと近いもののどちらを採用するか決める
//             bool isPriorityToTheSide; //ルートを格納する時に、（縦、横）か（横、縦）にするかを乱数で決める
//
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
//             //分岐数が最大になるもののうち、遠いものと近いもののどちらを採用するか決める
//             if (Random.Range(0, 2) == 0)
//             {
//                 isAdoptADistantJunction = true;
//             }
//             else
//             {
//                 isAdoptADistantJunction = false;
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
//             //Debug.Log($"relayPosList:{relayPosList.Count}");
//             randRouteNum = Random.Range(0, relayPosList.Count);
//
//             if (Random.Range(0, 2) == 0)
//             {
//                 isPriorityToTheSide = true;
//             }
//             else
//             {
//                 isPriorityToTheSide = false;
//             }
//
//             //中間地点を通るようにルートを作成
//             shortestRouteList =
//                 DiagonalSearchShortestRouteForEnemy(startPos, relayPosList[randRouteNum], isPriorityToTheSide);
//             shortestRouteList.RemoveAt(shortestRouteList.Count - 1); //連結するときに重複してしまうので削除する
//             shortestRouteList.AddRange(
//                 DiagonalSearchShortestRouteForEnemy(relayPosList[randRouteNum], endPos, !isPriorityToTheSide));
//
//             //重なっている部分をまとめる
//             OrganizeRoute();
//
//             //デバッグ
//             //Debug.Log($"shortestRouteList:{string.Join(",", shortestRouteList)}");
//             GameManager.instance.debugMGR.DebugRoute(shortestRouteList);
//
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
//                     int countGroupResult;
//
//                     //Debug.Log($"i:{n}のときloopNum:{loopNum}");
//
//                     //キューに含まれている座標のグループを数える（隣接しているものは同じグループ）
//                     searchQue.CopyTo(queCopyArray, 0);
//                     //Debug.Log(string.Join(",", queCopyArray));
//
//                     countGroupResult = CountGroup();
//
//                     //Debug.Log($"{n - 1}のCountGroup:{countGroupResult}");
//                     if (isAdoptADistantJunction) //グループ数が最大となるものの中で、距離が最大となるものを最終的な分岐点とする
//                     {
//                         if (groupNum <= countGroupResult) //＝を含む
//                         {
//                             groupNum = countGroupResult;
//                             MakeRelayPoint();
//                         }
//                     }
//                     else //グループ数が最大となるものの中で、距離が最小となるものを最終的な分岐点とする
//                     {
//                         if (groupNum < countGroupResult) //＝を含まない
//                         {
//                             groupNum = countGroupResult;
//                             MakeRelayPoint();
//                         }
//                     }
//
//
//                     //Dequeueしつつ、数字を置いていく
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
//             void PlaceNumAround(Vector2Int centerPos)
//             {
//                 Vector2Int inspectPos;
//
//                 //8マス判定する（真ん中のマスの判定は必要ない）
//                 for (int y = -1; y < 2; y++)
//                 {
//                     for (int x = -1; x < 2; x++)
//                     {
//                         if (x == 0 && y == 0) continue; //真ん中のマスは飛ばす
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
//             int CountGroup()
//             {
//                 var representativeList = new List<Vector2Int>(); //代表元のリスト
//                 bool isInTheExistingGroups; //新しく代表元をとるかどうかを判定するために必要
//
//                 representativeList.Add(queCopyArray[0]);
//
//                 //Debug.Log($"{n - 1}のsearchQue.Count:{searchQue.Count}");
//                 for (int i = 1; i < searchQue.Count; i++) //最初のキューに入っている座標は初めに代表元に指定していることに注意
//                 {
//                     //Debug.Log($"{n - 1}のrepresentativeList.Count:{representativeList.Count}, 要素は{string.Join(",", representativeList)}");
//
//                     isInTheExistingGroups = false; //最初は既存のグループに属さないと仮定する
//
//                     for (int k = 0; k < representativeList.Count; k++)
//                     {
//                         if (isInTheSameGroup(queCopyArray[i], representativeList[k]))
//                         {
//                             //queCopyArray[i]はrepresentativeList[k]と同じグループに属する
//                             isInTheExistingGroups = true; //一つでも属するグループがあったらtrueにする
//                         }
//                     }
//
//                     if (isInTheExistingGroups == false)
//                     {
//                         //Debug.Log($"{queCopyArray[i]}は既存のグループに属しません");
//                         representativeList.Add(queCopyArray[i]);
//                     }
//                 }
//
//                 return representativeList.Count;
//             }
//
//             bool isInTheSameGroup(Vector2Int pos1, Vector2Int pos2) //上下左右に同じ値のマスを移動してたどり着くことができる
//             {
//                 int searchValue = GetValueFromVector(pos1); //n-1に等しい
//
//                 //flagMapの初期化
//                 for (int y = 0; y < groupFlagMap.GetLength(0); y++)
//                 {
//                     for (int x = 0; x < groupFlagMap.GetLength(1); x++)
//                     {
//                         groupFlagMap[y, x] = false;
//                     }
//                 }
//
//                 if (GetValueFromVector(pos1) != GetValueFromVector(pos2))
//                     Debug.LogError($"pos1:{pos1}とpos2:{pos2}の値が異なります");
//
//                 if (searchValue != n - 1) Debug.LogError($"searchValue:{searchValue}がn-1:{n - 1}と異なります");
//
//                 PlaceFlagAroud(pos1, searchValue);
//
//                 if (groupFlagMap[pos2.y, pos2.x] == true)
//                 {
//                     return true;
//                 }
//                 else
//                 {
//                     return false;
//                 }
//             }
//
//             void PlaceFlagAroud(Vector2Int pos, int searchValue) //上下左右に同じ数字が入っていた場合にフラグを立てていく（再起関数的に呼ぶ）
//             {
//                 Vector2Int inspectPos;
//
//                 groupFlagMap[pos.y, pos.x] = true;
//
//
//                 //4マス判定する（真ん中のマスの判定は必要ない）
//                 for (int y = -1; y < 2; y++)
//                 {
//                     for (int x = -1; x < 2; x++)
//                     {
//                         if (x == 0 && y == 0) continue; //真ん中のマスは飛ばす
//                         if (x != 0 && y != 0) continue; //斜めのマスの判定は飛ばす
//
//                         inspectPos = pos + new Vector2Int(x, y);
//
//                         if (GetValueFromVector(inspectPos) == searchValue &&
//                             groupFlagMap[inspectPos.y, inspectPos.x] == false)
//                         {
//                             PlaceFlagAroud(inspectPos, searchValue);
//                         }
//                     }
//                 }
//             }
//
//             void MakeRelayPoint() //分岐数が最大となる中継地点を整理して保持しておく
//             {
//                 //relayPosListを初期化
//                 relayPosList.Clear();
//
//                 for (int i = 0; i < searchQue.Count; i++)
//                 {
//                     if (isBetweenPreNumAndInitNum(queCopyArray[i]))
//                     {
//                         relayPosList.Add(queCopyArray[i]);
//                     }
//                 }
//                 //Debug.Log($"relayPosList:{string.Join(",",relayPosList)}");
//             }
//
//             bool isBetweenPreNumAndInitNum(Vector2Int pos) //中継地点としてふさわしいかを判定する
//             {
//                 bool isAdjacentPreNum = false; //n-2と同じならtrue
//                 bool isAdjacentInitNum = false; //_initValueと同じならtrue
//                 //8マス判定する（真ん中のマスの判定は必要ない）
//                 for (int y = -1; y < 2; y++)
//                 {
//                     for (int x = -1; x < 2; x++)
//                     {
//                         if (x == 0 && y == 0) continue; //真ん中のマスは飛ばす
//
//                         if (GetValueFromVector(pos + new Vector2Int(x, y)) == n - 2)
//                         {
//                             isAdjacentPreNum = true;
//                         }
//                         else if (GetValueFromVector(pos + new Vector2Int(x, y)) == _initiValue)
//                         {
//                             isAdjacentInitNum = true;
//                         }
//                     }
//                 }
//
//                 if (isAdjacentPreNum && isAdjacentInitNum)
//                 {
//                     return true;
//                 }
//                 else
//                 {
//                     return false;
//                 }
//             }
//
//             void OrganizeRoute() //連結した時に重なる部分ができることがあるので、それを直す
//             {
//                 int indexWhenFoldedBack; //折り返して重なるところの最後のindex
//
//                 for (int i = 0; i < shortestRouteList.Count; i++)
//                 {
//                     indexWhenFoldedBack = shortestRouteList.IndexOf(shortestRouteList[i], i + 1);
//                     if (indexWhenFoldedBack == -1) //重なる部分がない
//                     {
//                         //なにもしない
//                     }
//                     else
//                     {
//                         shortestRouteList.RemoveRange(i + 1, indexWhenFoldedBack - i);
//                     }
//                 }
//             }
//
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
//
//         //以下、private関数
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
//
//         public static List<Vector2Int> NonDiagonalSearchShortestDiagonalRouteToCastle(Vector2Int castlePos,
//             Vector2Int startPos, Vector2Int[] orderInDirectionArray1, Vector2Int[] orderInDirectionArray2)
//         {
//             //Vector2Int endPos = Vector2Int.zero;
//
//
//             //foreach (Vector2Int direction in orderInDirectionArray1)
//             //{
//             //    if (GameManager.instance.mapMGR.GetMapValue(castlePos + direction) % GameManager.instance.groundID == 0)
//             //    {
//             //        endPos = castlePos + direction;
//             //        break;
//             //    }
//             //}
//
//             //if (endPos == Vector2Int.zero)
//             //{
//             //    Debug.LogError($"castlePos:{castlePos}の周りにgroundIDを含むマスがありません");
//             //    return null;
//             //}
//
//             //return NonDiagonalSearchShortestRoute(startPos, endPos, orderInDirectionArray1, orderInDirectionArray2);
//
//             return NonDiagonalSearchShortestRoute(startPos, castlePos, orderInDirectionArray1, orderInDirectionArray2);
//         }
//
//         static int[,] CalcSearchRangeArray(int advancingDistance, int lookingForValue, int notLookingForValue,
//             int centerValue) //中心からadvancingDstanceだけ進んで（斜め移動はなし）隣接できる外側のマスをlookinForValueにした2次元配列を返す　lookinForValueはドーナッツ状に入る
//         {
//             int t = lookingForValue; //索敵範囲の値
//             int f = notLookingForValue; //索敵範囲外の値
//             int o = centerValue; //原点の値
//
//             int size = 2 * (advancingDistance + 1) + 1;
//             int[,] resultArray = new int[size, size];
//
//             for (int j = 0; j < size; j++)
//             {
//                 for (int i = 0; i < size; i++)
//                 {
//                     if (i + j == advancingDistance || i + j == advancingDistance + 1 //左下
//                                                    || i - j == advancingDistance + 1 ||
//                                                    i - j == advancingDistance + 2 //右下
//                                                    || -i + j == advancingDistance + 1 ||
//                                                    -i + j == advancingDistance + 2 //左上
//                                                    || i + j == 3 * (advancingDistance + 1) ||
//                                                    i + j == 3 * (advancingDistance + 1) + 1 //右上
//                        )
//                     {
//                         resultArray[i, j] = t;
//                     }
//                     else if (i == advancingDistance + 1 && j == advancingDistance + 1)
//                     {
//                         resultArray[i, j] = o;
//                     }
//                     else
//                     {
//                         resultArray[i, j] = f;
//                     }
//                 }
//             }
//
//             return resultArray;
//         }
//
//         static List<Vector2Int> ConvertToDiagonalRoute(List<Vector2Int> nonDiagonalRoute)
//         {
//             var removeIndex = new List<int>(); //nextPosの添え字を記録していき、あとでまとめてnextNextPosとまとめる
//
//             for (int i = 0; i < nonDiagonalRoute.Count; i++)
//             {
//                 if (i >= nonDiagonalRoute.Count - 2) break; //nonDiagonalRoute.Count - 3まで判定をする
//
//                 Vector2Int gridPos = nonDiagonalRoute[i];
//                 Vector2Int nextPos = nonDiagonalRoute[i + 1];
//                 Vector2Int nextNextPos = nonDiagonalRoute[i + 2];
//
//                 //if ((((nextPos - gridPos).x == 0 && (nextNextPos - nextPos).y == 0) || ((nextPos - gridPos).y == 0 && (nextNextPos - nextPos).x == 0)) && CanMoveDiagonally(gridPos, nextNextPos))
//                 //{
//                 //    Debug.Log($"斜め移動が可能なため、nextPos:{nextPos}をnextNextPos:{nextNextPos}で置き換えます gridPos:{gridPos}");
//
//                 //    // nextPos = nextNextPosに対応する
//                 //    nonDiagonalRoute[i + 1] = nonDiagonalRoute[i + 2];
//                 //    nonDiagonalRoute.RemoveAt(i + 1);
//                 //    i++;
//                 //}
//
//
//                 if ((((nextPos - gridPos).x == 0 && (nextNextPos - nextPos).y == 0) ||
//                      ((nextPos - gridPos).y == 0 && (nextNextPos - nextPos).x == 0)) &&
//                     CanMoveDiagonally(gridPos, nextNextPos))
//                 {
//                     if (removeIndex.Count != 0 && removeIndex[removeIndex.Count - 1] + 1 == i + 1)
//                         continue; //除く点が連続してはいけない
//                     removeIndex.Add(i + 1);
//                 }
//             }
//
//             for (int i = 0; i < removeIndex.Count; i++)
//             {
//                 nonDiagonalRoute.RemoveAt(
//                     removeIndex[
//                         removeIndex.Count - 1 - i]); //removeIndexが昇順に数字が入っていることを利用する。nonDiagonalRouteは添え字が大きいほうから削除していく
//             }
//
//             return nonDiagonalRoute;
//
//
//             //以下ローカル関数
//             bool CanMoveDiagonally(Vector2Int prePos, Vector2Int afterPos)
//             {
//                 Vector2Int directionVector = afterPos - prePos;
//
//                 //斜め移動の時にブロックの角を移動することはできない
//                 if (directionVector.x != 0 && directionVector.y != 0)
//                 {
//                     //水平方向の判定
//                     if (GameManager.instance.mapMGR.GetMapValue(prePos.x + directionVector.x, prePos.y) %
//                         GameManager.instance.wallID == 0)
//                     {
//                         return false;
//                     }
//
//                     //垂直方向の判定
//                     if (GameManager.instance.mapMGR.GetMapValue(prePos.x, prePos.y + directionVector.y) %
//                         GameManager.instance.wallID == 0)
//                     {
//                         return false;
//                     }
//                 }
//
//                 return true;
//             }
//         }
//     }
//     
// }
//
//
