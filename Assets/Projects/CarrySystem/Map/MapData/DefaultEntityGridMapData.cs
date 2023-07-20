// using System.Collections;
// using System.Collections.Generic;
// using System.IO;
// using Carry.CarrySystem.Entity.Scripts;
// using Carry.CarrySystem.Spawners;
// using UnityEngine;
// using Carry.CarrySystem.Map.Scripts;
//
// #nullable enable
//
// namespace Carry.CarrySystem.Map.MapData
// {
//     public record DefaultEntityGridMapData : EntityGridMapData
//     {
//         // 適当に作っている
//         // 必要があれば、マップ制作シーンで作れるようにする
//         readonly int _length;
//
//         public DefaultEntityGridMapData()
//         {
//             // 親クラスのフィールドを書き換えていることに注意
//             width = 20;
//             height = 11;
//             _length = width * height;
//             groundRecords = new GroundRecord[_length];
//             rockRecords = new RockRecord[_length];
//
//             FillAll();
//             PlaceGround();
//             PlaceRock();
//         }
//
//         void FillAll()
//         {
//             for (int i = 0; i < _length; i++)
//             {
//                 groundRecords[i] = new GroundRecord();
//                 rockRecords[i] = new RockRecord();
//             }
//         }
//
//         void PlaceGround()
//         {
//             // すべてのマスに対してGroundを配置する
//             for (int i = 0; i < _length; i++)
//             {
//                 groundRecords[i].kind = Ground.Kind.Kind1;
//             }
//         }
//
//         void PlaceRock()
//         {
//             List<int> rockPosList = new List<int>();
//
//             rockPosList.Add(0); //原点に置く
//             for (int i = 1; i < height; i++)
//             {
//                 if (i % 2 == 0)
//                 {
//                     rockPosList.Add(i * width - 1);
//                     rockPosList.Add(i * width - 3);
//                 }
//                 else
//                 {
//                     rockPosList.Add(i * width - 2);
//                     rockPosList.Add(i * width - 4);
//                 }
//             }
//             // Debug.Log($"rockPosList : {string.Join(",", rockPosList)}");
//
//             for (int i = 0; i < _length; i++)
//             {
//                 if (rockPosList.Contains(i))
//                 {
//                     rockRecords[i].kind = Rock.Kind.Kind1;
//                 }
//             }
//         }
//     }
// }