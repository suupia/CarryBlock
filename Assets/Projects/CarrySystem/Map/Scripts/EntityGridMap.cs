using System;
using System.Collections;
using System.Collections.Generic;
using Carry.CarrySystem.Entity.Interfaces;
using UnityEngine;
#nullable enable

namespace Carry.CarrySystem.Map.Scripts
{
    public class NumericHexagonMap
    {
        //数字だけを格納することができる六角形のマップ
        public int Width { get; private set; }
        public int Height { get; private set; }
        private readonly long[] _values = null;
        public int GetLength() => _values.Length;
        public int InitValue { get; } = -1;
        public int OutOfRangeValue { get; } = -88;

        //データが存在する領域、端の領域、それ以外　の３つの領域がある

        //コンストラクタ
        public NumericHexagonMap(int width, int height)
        {
            if (width <= 0 || height <= 0)
            {
                Debug.LogWarning("Mapの幅または高さが0以下になっています");
                return;
            }

            this.Width = width;
            this.Height = height;

            if (width % 2 == 0 || height % 2 == 0)
            {
                _values = new long[(width * height) / 2];
            }
            else
            {
                _values = new long[(width * height + 1) / 2];
            }


            FillAll(InitValue);
        }


        //Getter

        public long GetValue(int index)
        {
            if (index < 0 || index > _values.Length)
            {
                Debug.LogWarning("領域外の値を習得しようとしました");
                return OutOfRangeValue;
            }

            return _values[index];
        }

        public long GetValue(int x, int y)
        {
            if (IsOutOfEdge(x, y))
            {
                //edgeの外側
                Debug.LogWarning($"IsOutOfEdge({x},{y})がtrueです");
                return OutOfRangeValue;
            }

            if (IsOnTheEdge(x, y))
            {
                //edgeの上
                //データは存在しないが、判定のために初期値を使いたい場合
                return InitValue;
            }

            if (x + y % 2 == 1)
            {
                Debug.LogWarning($"x座標とy座標の和が奇数になっているため、対応するマスが存在しません");
            }

            //Debug.Log($"ToSubscript:{ToSubscript(x,y)}, x:{x}, y:{y}, IsOnTheEdge({x},{y}):{IsOnTheEdge(x,y)}, IsOutOfEdge({x},{y}):{IsOutOfEdge(x,y)}, Width:{Width}, Height:{Height}");
            return _values[ToSubscript(x, y)];
        }

        public long GetValue(Vector2Int vector)
        {
            return GetValue(vector.x, vector.y);
        }

        public Vector2Int GetVectorFromIndex(int index)
        {
            return DivideSubscript(index);
        }

        public int GetIndexFromVector(Vector2Int vector)
        {
            if (IsOutOfEdge(vector.x, vector.y)) Debug.LogError($"IsOutOfEdge({vector.x}, {vector.y})がtrueです");
            return ToSubscript(vector.x, vector.y);
        }


        //Setter
        public void SetValue(int index, long value)
        {
            if (index < 0 || index > _values.Length - 1)
            {
                Debug.LogWarning("領域外の値を習得しようとしました");
                return;
            }

            _values[index] = value;
        }

        public void SetValue(int x, int y, long value)
        {
            if (IsOutOfEdge(x, y))
            {
                Debug.LogWarning($"IsOutOfEdge({x},{y})がtrueです");
                return;
            }

            if ((x + y) % 2 == 1)
            {
                Debug.LogWarning($"x座標とy座標の和が奇数になっているため、対応するマスが存在しません");
            }

            _values[ToSubscript(x, y)] = value;
        }

        public void SetValue(Vector2Int vector, long value)
        {
            SetValue(vector.x, vector.y, value);
        }

        public void AdditionSetValue(Vector2Int vector, long value)
        {
            var x = vector.x;
            var y = vector.y;
            SetValue(x, y, GetValue(x, y) + value);
        }

        public void MultiplySetValue(Vector2Int vector, int value)
        {
            var x = vector.x;
            var y = vector.y;

            if (IsOutOfEdge(x, y))
            {
                Debug.LogWarning($"IsOutOfEdge({x},{y})がtrueです");
                return;
            }

            if (x + y % 2 != 0)
            {
                Debug.LogWarning($"x座標とy座標の和が奇数になっているため、対応するマスが存在しません");
            }

            _values[ToSubscript(x, y)] *= value;
        }

        public void DivisionalSetValue(Vector2Int vector, int value)
        {
            var x = vector.x;
            var y = vector.y;

            if (IsOutOfEdge(x, y))
            {
                Debug.LogWarning($"IsOutOfEdge({x},{y})がtrueです");
                return;
            }

            if (x + y % 2 == 1)
            {
                Debug.LogWarning($"x座標とy座標の和が奇数になっているため、対応するマスが存在しません");
            }

            if (GetValue(x, y) % value != 0)
            {
                Debug.LogWarning($"DivisionalSetValue(vector:{vector},value:{value})で余りが出たため実行できません");
                return;
            }

            _values[ToSubscript(x, y)] /= value;
        }


        //添え字を変換する
        protected int ToSubscript(int x, int y)
        {
            //xを常に0からスタートさせるために必要
            if (y % 2 == 1)
            {
                x--;
            }

            if (Width % 2 == 0)
            {
                //偶数×偶数や偶数×奇数の時

                return x / 2 + Width / 2 * y;
            }
            else
            {
                //奇数×偶数や奇数×奇数の時
                if (y % 2 == 0)
                {
                    return x / 2 + Width * y / 2;
                }
                else
                {
                    return x / 2 + (Width * y + 1) / 2;
                }
            }
        }

        protected Vector2Int DivideSubscript(int subscript)
        {
            int preXSub, xSub, ySub;

            if (Width % 2 == 0)
            {
                preXSub = subscript % (Width / 2);
                xSub = preXSub * 2;
                ySub = (subscript - preXSub) / (Width / 2);
                if (ySub % 2 == 1)
                {
                    xSub++;
                }
            }
            else
            {
                int r = subscript % Width;
                int halfWidth = (Width + 1) / 2;


                if (r < halfWidth)
                {
                    preXSub = r;
                    xSub = preXSub * 2;
                    ySub = (subscript - preXSub) / Width * 2;
                }
                else
                {
                    preXSub = r - halfWidth;
                    xSub = preXSub * 2 + 1;
                    ySub = (subscript - preXSub - halfWidth) / Width * 2 + 1;
                }
            }

            //Debug.Log($"subscript:{subscript}preXSub:{preXSub}, xSub:{xSub}, ySub:{ySub}");
            return new Vector2Int(xSub, ySub);
        }

        //判定用関数
        public bool IsOutOfEdge(int x, int y) //edgeの外側。つまり、データがedgeValueすらない
        {
            if (x < -1 || Width < x) return true;
            if (y < -2 || Height + 1 < y) return true;
            if (Width % 2 == 0)
            {
                if (Height % 2 == 0)
                {
                    if (x == -1 && y == Height + 1 || x == Width && y == -2) return true; //左上と右下に空きがある
                }
                else
                {
                    if (x == Width && y == Height + 1 || x == Width && y == -2) return true; //右上と右下に空きがある
                }
            }
            else
            {
                if (Height % 2 == 0)
                {
                    if (x == -1 && y == Height + 1 || x == Width && y == Height + 1) return true; //左上と右上に空きがある
                }
                else
                {
                    //空きはない
                }
            }

            //mapの中
            return false;
        }

        public bool IsOnTheEdge(int x, int y) //データは存在しないが、_initValueを返すマス
        {
            //if ((Mathf.Abs(x)  + Mathf.Abs(y)) % 2 != 0) return false;
            if ((x == -1 || x == Width) && (-1 <= y && y <= Height)) return true; //左右の両端
            if ((y == -1 || y == Height) && (-1 <= x && x <= Width)) return true; //上下の両端（へこんだ方）
            if ((y == -2 || y == Height + 1) && (0 <= x && x <= Width - 1)) return true; //上下の両端（膨らんだ方）

            return false;
        }

        public bool IsOutOfDataRange(int x, int y) //座標(0,0)～(mapWidht-1,mapHeight-1)のデータが存在する領域の外側
        {
            if ((x + y) % 2 != 0) return true; //六角形のマス目上の座標でなければtrue
            //Debug.Log($"IsOutOfDataRange ({x},{y}):{IsOutOfEdge(x, y) || IsOnTheEdge(x, y)}");
            return IsOutOfEdge(x, y) || IsOnTheEdge(x, y);
        }

        //初期化で利用
        public void FillAll(int value)
        {
            for (int i = 0; i < _values.Length; i++)
            {
                _values[i] = value;
            }
        }
    }


    public class EntityHexagonMap : NumericHexagonMap
    {
         List<IEntity>[] _entityMaps;
        

        public int AdvancedCells = 0; //緑化したマスの最前線のｘ座標

        //コンストラクタ
        public EntityHexagonMap(int width, int height) : base(width, height)
        {
            _entityMaps = new List<IEntity>[GetLength()];
            for (int i = 0; i < GetLength(); i++)
            {
                _entityMaps[i] = new List<IEntity>();
            }

            // if (GameManager.instance == null) return;
        }

        public EntityHexagonMap CloneMap()
        {
            return (EntityHexagonMap)MemberwiseClone();
        }

        //Getter
        public EntityType GetSingleEntity<EntityType>(Vector2Int vector) where EntityType : IEntity
        {
            int x, y;
            x = vector.x;
            y = vector.y;

            if (IsOutOfDataRange(x, y)) return default(EntityType);

            return GetSingleEntity<EntityType>(ToSubscript(x, y));
        }

        public EntityType GetSingleEntity<EntityType>(int index) where EntityType : IEntity
        {
            var resultEntityList = new List<EntityType>();

            if (index < 0 || index > GetLength())
            {
                Debug.LogWarning("領域外の値を習得しようとしました");
                return default(EntityType);
            }

            // ToDo: GetType()
            // if (_entityMaps[index].Count(s => s.GetType() == typeof(EntityType)) == 0)
            // {
            //     //Debug.Log($"_entityMaps[{index}]の{typeof(EntityType)}のCountが0です");
            //     return default(EntityType);
            // }

            foreach (var entity in _entityMaps[index])
            {
                if (entity.GetType() == typeof(EntityType)) resultEntityList.Add((EntityType)entity);
            }


            ////今はリストであることは使わない
            //return resultEntityList[0];

            foreach (var entity in resultEntityList)
            {
                //探しているEntityTypeの先頭のものを返す
                if (entity.GetType() == typeof(EntityType)) return entity;
            }

            //なかった場合
            return default(EntityType);
        }

        public List<EntityType> GetSingleEntityList<EntityType>(Vector2Int vector) where EntityType : IEntity
        {
            int x, y;
            x = vector.x;
            y = vector.y;

            if (IsOutOfDataRange(x, y)) return default(List<EntityType>);

            return GetSingleEntityList<EntityType>(ToSubscript(x, y));
        }

        public List<EntityType> GetSingleEntityList<EntityType>(int index) where EntityType : IEntity
        {
            var resultEntityList = new List<EntityType>();

            if (index < 0 || index > GetLength())
            {
                Debug.LogWarning("領域外の値を習得しようとしました");
                return default(List<EntityType>);
            }

            // ToDo: GetType()
            // if (_entityMaps[index].Count(s => s.GetType() == typeof(EntityType)) == 0)
            // {
            //     //Debug.Log($"_entityMaps[{index}]の{typeof(EntityType)}のCountが0です");
            //     return default(List<EntityType>);
            // }

            foreach (var entity in _entityMaps[index])
            {
                if (entity.GetType() == typeof(EntityType)) resultEntityList.Add((EntityType)entity);
            }

            return resultEntityList;
        }

        public List<IEntity> GetAllEntityList(Vector2Int vector)
        {
            int x, y;
            x = vector.x;
            y = vector.y;

            if (IsOutOfDataRange(x, y)) return default(List<IEntity>);

            int index = ToSubscript(vector.x, vector.y);

            var resultEntityList = new List<IEntity>();

            if (index < 0 || index > GetLength())
            {
                Debug.LogWarning("領域外の値を習得しようとしました");
                return default(List<IEntity>);
            }

            return _entityMaps[index];
        }

        public void AddEntity<EntityType>(Vector2Int vector, IEntity entity) where EntityType : IEntity
        {
            var x = vector.x;
            var y = vector.y;

            if (IsOutOfDataRange(x, y))
            {
                Debug.LogWarning($"IsOutOfDataRange({x},{y})がtrueです");
                return;
            }

            AddEntity<EntityType>(ToSubscript(x, y), entity);
        }

        public void AddEntity<EntityType>(int index, IEntity entity) where EntityType : IEntity
        {
            if (index < 0 || index > GetLength())
            {
                Debug.LogWarning("領域外に値を設定しようとしました");
                return;
            }

            // ToDo: GetType()
            // if (_entityMaps[index].Count(s => s.GetType() == typeof(EntityType)) > 0)
            // {
            //     Debug.LogWarning(
            //         $"[注意!!] GetEntityList<EntityType>(index).Countが0より大きいため、既に{typeof(EntityType)}が入っています");
            // }

            _entityMaps[index].Add(entity);
        }

        public void RemoveEntity(int x, int y, IEntity entity)
        {
            if (IsOutOfDataRange(x, y))
            {
                Debug.LogWarning($"IsOutOfDataRange({x},{y})がtrueです");
                return;
            }

            _entityMaps[ToSubscript(x, y)].Remove(entity);
        }

        public void RemoveEntity(Vector2Int vector, IEntity entity)
        {
            RemoveEntity(vector.x, vector.y, entity);
        }

        public bool IsTouchingAnotherEntity<EntityType>(Vector2Int pos) where EntityType : IEntity
        {
            bool result = false;
            Vector2Int[] aroundVectors = new Vector2Int[6];

            for (int i = 0; i < aroundVectors.Length; i++)
            {
                aroundVectors[i] = HexagonFunction.GetUnitDirectionVector(i);
            }

            foreach (var directionVector in aroundVectors)
            {
                if (directionVector == Vector2Int.zero) continue;

                var inspectPos = pos + directionVector;


                if (GetSingleEntity<EntityType>(inspectPos) == null) continue;

                result = true;
            }

            return result;
        }


        public List<Vector2Int> SearchShortestRoute(Vector2Int startPos, Vector2Int endPos,
            System.Func<Vector2Int, bool> InspectPosIsWallID)
        {
            int _initiValue = -10; //PlaceNumAroundで重複して数字を置かないようにするために必要
            int _wallValue = -1; //wallのマス
            List<Vector2Int> shortestRouteList;

            Queue<Vector2Int> searchQue = new Queue<Vector2Int>();
            int n = 1; //1から始まることに注意!!
            bool isComplete = false;
            int maxDistance = 0;

            var numericHexagonMap = new NumericHexagonMap(Width, Height);

            //引数が適切かどうかチェックする


            //初期化
            shortestRouteList = new List<Vector2Int>();
            numericHexagonMap.FillAll(_initiValue);

            //次にmapをコピーして、壁のマスを-1にする。
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    if ((x + y) % 2 == 1) continue;
                    if (InspectPosIsWallID(new Vector2Int(x, y)))
                    {
                        //Debug.Log($"x:{x}, y:{y} に_wallValueをセットします");
                        numericHexagonMap.SetValue(x, y, _wallValue);
                    }
                }
            }

            //壁でないマスに数字を順番に振っていく
            //Debug.Log($"WaveletSearchを実行します startPos:{startPos}");
            var successFlag = WaveletSearch();
            if (successFlag == false) return new List<Vector2Int>();

            //デバッグ用
            //string debugCell = "";
            //for (int y = 0; y < Height; y++)
            //{
            //    for (int x = 0; x < Width; x++)
            //    {
            //        debugCell += $"{GetValue(x, Height - y - 1)}".PadRight(3) + ",";
            //    }
            //    debugCell += "\n";
            //}
            //Debug.Log($"WaveletSearchの結果は\n{debugCell}");


            //数字をもとに、大きい数字から巻き戻すようにして最短ルートを配列に格納する
            //Debug.Log($"StoreRouteAround({endPos},{maxDistance})を実行します");
            StoreShortestRoute(endPos, maxDistance);

            shortestRouteList.Reverse(); //リストを反転させる


            //デバッグ
            //Debug.Log($"shortestRouteList:{string.Join(",", shortestRouteList)}");

            return shortestRouteList;

            ////////////////////////////////////////////////////////////////////

            //以下ローカル関数


            bool WaveletSearch()
            {
                numericHexagonMap.SetValue(startPos, 0); //startPosの部分だけ周囲の判定を行わないため、ここで個別に設定する
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
                        return false;
                    }
                }

                return true;
            }

            void StoreShortestRoute(Vector2Int centerPos, int distance) //再帰的に呼ぶ
            {
                if (distance == 0)
                {
                    shortestRouteList.Add(centerPos);
                    return;
                }

                if (distance < 0) return; //0までQueに入れれば十分

                //Debug.Log($"GetValue({centerPos})は{numericHexagonMap.GetValue(centerPos)}、distance:{distance}");


                foreach (Vector2Int direction in HexagonFunction.GetCircumferenceVector(1))
                {
                    if (numericHexagonMap.GetValue(centerPos + direction) == distance - 1)
                    {
                        shortestRouteList.Add(centerPos);
                        StoreShortestRoute(centerPos + direction, distance - 1);
                        break;
                    }
                }
            }


            void PlaceNumAround(Vector2Int centerPos)
            {
                Vector2Int inspectPos;

                foreach (var directionVector in HexagonFunction.GetCircumferenceVector(1))
                {
                    if (directionVector == Vector2Int.zero) continue;


                    inspectPos = centerPos + directionVector;
                    //Debug.Log($"centerPos:{centerPos},inspectPos:{inspectPos}のとき");
                    if (numericHexagonMap.GetValue(inspectPos) == _initiValue)
                    {
                        numericHexagonMap.SetValue(inspectPos, n);
                        searchQue.Enqueue(inspectPos);
                        //Debug.Log($"({inspectPos})を{n}にし、探索用キューに追加しました。");
                    }
                    else //このelseはデバッグ用
                    {
                        //Debug.Log($"{inspectPos}は初期値が入っていない　または　斜め移動でいけません\nGetValueFromVector({inspectPos}):{GetValueFromVector(inspectPos)}, CanMoveDiagonally({centerPos}, {inspectPos}):{CanMoveDiagonally(centerPos, inspectPos)}");
                    }

                    if (inspectPos == endPos)
                    {
                        isComplete = true;
                        numericHexagonMap.SetValue(inspectPos, n);
                        maxDistance = n;
                        //Debug.Log($"isCompleteをtrueにしました。maxDistance:{maxDistance}");
                        break; //探索終了
                    }
                }
            }
        }
    }
    
    
    public static class HexagonFunction
{
    public enum Direction
    {
        Up,
        UpLeft,
        DownLeft,
        Down,
        DownRight,
        UpRight,
    }

    //真上から始まって時計回りで6方向ある
    public static Vector2Int GetUnitDirectionVector(int index)
    {
        var direction = index switch
        {
            0 => new Vector2Int(0, 2),
            1 => new Vector2Int(1, 1),
            2 => new Vector2Int(1, -1),
            3 => new Vector2Int(0, -2),
            4 => new Vector2Int(-1, -1),
            5 => new Vector2Int(-1, 1),
            _ => throw new InvalidOperationException()
        };
        return direction;
    }
    public static float GetAngleFromUnitDirectionVector(Direction direction)
    {

        //Debug.Log($"direction:{direction}");

        return direction switch
        {
            Direction.Up => Vector2.SignedAngle(Vector2.up, Quaternion.Euler(0, 0, 0) * Vector2.up),
            Direction.UpLeft => Vector2.SignedAngle(Vector2.up, Quaternion.Euler(0, 0, 57.850638470217f) * Vector2.up),
            Direction.DownLeft => Vector2.SignedAngle(Vector2.up, Quaternion.Euler(0, 0, 122.149361529783f) * Vector2.up),
            Direction.Down => Vector2.SignedAngle(Vector2.up, Quaternion.Euler(0, 0, 180) * Vector2.up),
            Direction.DownRight => Vector2.SignedAngle(Vector2.up, Quaternion.Euler(0, 0, 237.850638470217f) * Vector2.up),
            Direction.UpRight => Vector2.SignedAngle(Vector2.up, Quaternion.Euler(0, 0, 302.149361529783f) * Vector2.up),
            _ => throw new InvalidOperationException()
        };
    }

    public static Direction ToDirection(Vector2 unitDirectionVector)
    {
        //六角形のマス目に沿う単位ベクトルから六角形のマス目上の向きを求める
        if (unitDirectionVector == Vector2.zero)
        {
            //Debug.LogError($"unitDirectionVector:{unitDirectionVector}");
            return Direction.Up;
        }

        if (unitDirectionVector == Vector2.left || unitDirectionVector == Vector2.right)
        {
            //単位ベクトルが真横になることはない
            Debug.LogError($"unitDirectionVector:{unitDirectionVector}");
            return Direction.Up;
        }

        var direction = Direction.Up;
        float angle = Vector2.SignedAngle(Vector2.up, unitDirectionVector);

        //Debug.Log($"angle:{angle}");

        if (-30.0f <= angle && angle < 30.0f)
        {
            direction = Direction.Up;

        }
        else if (30.0f <= angle && angle < 90.0f)
        {
            direction = Direction.UpLeft;

        }
        else if (90.0f <= angle && angle < 150.0f)

        {
            direction = Direction.DownLeft;

        }
        else if (150.0f <= angle || angle < -150.0f)
        {
            direction = Direction.Down;

        }
        else if (-150.0f <= angle && angle < -90.0f)
        {
            direction = Direction.DownRight;
            ;
        }
        else if (-90.0f <= angle && angle < -30.0f)
        {
            direction = Direction.UpRight;

        }
        else
        {
            Debug.LogError($"angle:{angle}");
            throw new System.Exception();
        }

        //Debug.Log($"ToDirection({unitDirectionVector}):{direction}");

        return direction;
    }

    public static Vector2Int GetDirectionVector(Direction direction)
    {
        return direction switch
        {
            Direction.Up => new Vector2Int(0, 2),
            Direction.UpLeft => new Vector2Int(-1, 1),
            Direction.DownLeft => new Vector2Int(-1, -1),
            Direction.Down => new Vector2Int(0, -2),
            Direction.DownRight => new Vector2Int(1, -1),
            Direction.UpRight => new Vector2Int(1, 1),
            _ => throw new InvalidOperationException()
        };

    }


    public static List<Vector2Int> GetCircumferenceVector(int radius)
    {
        //時計回りの順番でリストに入れる
        var resultList = new List<Vector2Int>();

        if (radius == 0)
        {
            resultList.Add(Vector2Int.zero);
            return resultList;
        }

        //上の頂点
        resultList.Add(new Vector2Int(0, 2 * radius));
        //右上の辺
        for (int i = 0; i < radius - 1; i++)
        {
            resultList.Add(new Vector2Int(1 + i, 2 * radius - 1 - i));
        }
        //右上の頂点
        resultList.Add(new Vector2Int(radius, radius));
        //右の辺
        for (int i = 0; i < radius - 1; i++)
        {
            resultList.Add(new Vector2Int(radius, radius - 2 - 2 * i));
        }
        //右下の頂点
        resultList.Add(new Vector2Int(radius, -radius));
        //右下の辺
        for (int i = 0; i < radius - 1; i++)
        {
            resultList.Add(new Vector2Int(radius - 1 - i, -radius - 1 - i));
        }
        //下の頂点
        resultList.Add(new Vector2Int(0, -2 * radius));
        //左下の辺
        for (int i = 0; i < radius - 1; i++)
        {
            resultList.Add(new Vector2Int(-1 - i, -2 * radius + 1 + i));
        }
        //左下の頂点
        resultList.Add(new Vector2Int(-radius, -radius));
        //左の辺
        for (int i = 0; i < radius - 1; i++)
        {
            resultList.Add(new Vector2Int(-radius, -radius + 2 + 2 * i));
        }
        //左上の頂点
        resultList.Add(new Vector2Int(-radius, radius));
        //左上の辺
        for (int i = 0; i < radius - 1; i++)
        {
            resultList.Add(new Vector2Int(-radius + 1 + i, radius + 1 + i));
        }

        return resultList;
    }

    public static List<Vector2Int> GetSurroundingVector(Vector2Int centerPos, int radius)
    {
        var circlePosList = new List<Vector2Int>();

        for (int r = 0; r <= radius; r++)
        {
            circlePosList.AddRange(GetCircumferenceVector(r));
        }

        return new List<Vector2Int>(circlePosList).ConvertAll(vec => vec + centerPos);
    }

    public static Vector2Int CalcMoveVectorInHexagon(Vector2Int gridPos, Vector2Int inputVector)
    {

        //Debug.Log($"gridPos:{gridPos}, inputVector:{inputVector}");
        Vector2Int resultVector = Vector2Int.zero;
        if (inputVector.x == 0 && inputVector.y == 0)
        {
            //何もしない
        }
        else if (inputVector.x == 0 && inputVector.y != 0)
        {
            resultVector = new Vector2Int(0, inputVector.y * 2);
        }
        else if (inputVector.x != 0 && inputVector.y == 0)
        {
            if (gridPos.x % 2 == 0)
            {
                resultVector = new Vector2Int(inputVector.x, 1);
            }
            else
            {
                resultVector = new Vector2Int(inputVector.x, -1);
            }
        }
        else
        {
            resultVector = new Vector2Int(inputVector.x, inputVector.y);
        }

        return resultVector;
    }

}
}