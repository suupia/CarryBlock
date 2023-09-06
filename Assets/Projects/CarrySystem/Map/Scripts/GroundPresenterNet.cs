using System.Collections.Generic;
using Carry.CarrySystem.Entity.Interfaces;
using Fusion;
using UnityEngine;

namespace Carry.CarrySystem.Map.Scripts
{
    public class GroundPresenterNet :NetworkBehaviour
    {
        public struct PresentData : INetworkStruct
        {
            // ランタイムで見た目が変化するようになったときに使用する
            // その時はTilePresenterNetを参考にする
        }

        // [Networked] public ref PresentData PresentDataRef => ref MakeRef<PresentData>();

        // このぐらいなら、PrefabLoadするまでもなく直接アタッチした方がよい
        // [SerializeField] GameObject wallView = null!;

        public override void Render()
        {   
            // PresentDataRefの状態を監視して、見た目を変える
        }


        public void SetInitAllEntityActiveData(IEnumerable<IEntity> allEntities)
        {

        }

        public void SetEntityActiveData(IEntity entity, int count)
        {

        }
    }
}