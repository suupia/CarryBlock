#nullable enable
using Carry.CarrySystem.Map.Interfaces;
using Fusion;
using UnityEngine;

namespace Carry.CarrySystem.Map.Scripts
{
    public class WallPresenterNet : NetworkBehaviour, IWallPresenter
    {
        public MonoBehaviour GetMonoBehaviour => this;
        struct PresentData : INetworkStruct
        {
            // ランタイムで見た目が変化するようになったときに使用する
            // その時はTilePresenterNetを参考にする
        }

        [Networked] ref PresentData PresentDataRef => ref MakeRef<PresentData>();

        // このぐらいなら、PrefabLoadするまでもなく直接アタッチした方がよい
        [SerializeField] GameObject wallView = null!;
        public void DestroyPresenter() => Runner.Despawn(Object);

        public override void Render()
        {
            // PresentDataRefの状態を監視して、見た目を変える
            /*Random random = new Random();
            int WallType = random.Next(2);
            wall1View.SetActive(WallType == 0);
            wall2View.SetActive(WallType == 1);*/
        }
        
    }
}