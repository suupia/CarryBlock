using System;
using System.Collections.Generic;
using System.Linq;
using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.Block.Scripts;
using Carry.CarrySystem.Entity.Interfaces;
using Carry.CarrySystem.Entity.Scripts;
using Fusion;
using UnityEngine;
using Random = System.Random;

namespace Carry.CarrySystem.Map.Scripts
{
    public class WallPresenterNet : NetworkBehaviour
    {
        public struct PresentData : INetworkStruct
        {
            // ランタイムで見た目が変化するようになったときに使用する
            // その時はTilePresenterNetを参考にする
        }

        [Networked] public ref PresentData PresentDataRef => ref MakeRef<PresentData>();

        // このぐらいなら、PrefabLoadするまでもなく直接アタッチした方がよい
        [SerializeField] GameObject wallView = null!;

        public override void Render()
        {
            // PresentDataRefの状態を監視して、見た目を変える
            /*Random random = new Random();
            int WallType = random.Next(2);
            wall1View.SetActive(WallType == 0);
            wall2View.SetActive(WallType == 1);*/
        }


        public void SetInitAllEntityActiveData(IEnumerable<IEntity> allEntities)
        {

        }

        public void SetEntityActiveData(IEntity entity, int count)
        {

        }
    }
}