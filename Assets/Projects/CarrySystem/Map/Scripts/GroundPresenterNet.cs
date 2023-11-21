#nullable enable
using Carry.CarrySystem.Map.Interfaces;
using Fusion;
using UnityEngine;

namespace Carry.CarrySystem.Map.Scripts
{
    public class GroundPresenterNet :NetworkBehaviour , IPresenterMono
    {
        public MonoBehaviour GetMonoBehaviour => this;
        struct PresentData : INetworkStruct
        {
            // ランタイムで見た目が変化するようになったときに使用する
            // その時はTilePresenterNetを参考にする
        }

        [Networked] ref PresentData PresentDataRef => ref MakeRef<PresentData>();

        // このぐらいなら、PrefabLoadするまでもなく直接アタッチした方がよい
        // [SerializeField] GameObject wallView = null!;

        public void DestroyPresenter() => Runner.Despawn(Object);

        public override void Render()
        {   
            // PresentDataRefの状態を監視して、見た目を変える
        }

        
    }
}