using System;
using System.Collections;
using System.Collections.Generic;
using Carry.CarrySystem.Entity.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using UnityEngine;
using Fusion;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Entity.Scripts;
using Carry.CarrySystem.Player.Interfaces;


namespace Carry.CarrySystem.Player.Scripts
{
    [RequireComponent(typeof(CarryPlayerController_Net))]
    public class HoldPresenter_Net : NetworkBehaviour, IHoldActionPresenter
    {
        // Presenter系のクラスはホストとクライアントで状態を一致させるためにNetworkedプロパティを持つので、
        // ドメインの情報を持ってはいけない
        public struct PresentData : INetworkStruct
        {
            public NetworkBool IsHoldingRock;
        }
        [Networked] public ref PresentData PresentDataRef => ref MakeRef<PresentData>();

        // このぐらいなら、PrefabLoadするまでもなく直接アタッチした方がよい
        [SerializeField] GameObject holdingRock;
        [SerializeField] GameObject holdingDoubleRock;

        public override void Render()
        {
            // Debug.Log($"PresentDataRef.IsHoldingRock = {PresentDataRef.IsHoldingRock}");

            if (holdingRock.activeSelf != PresentDataRef.IsHoldingRock)
            {
                holdingRock.SetActive(PresentDataRef.IsHoldingRock);
            }
            
        }

        // ホストのみで呼ばれることに注意
        // 以下の処理はアニメーション、音、エフェクトの再生を行いたくなったら、それぞれのクラスの対応するメソッドを呼ぶようにするかも
        public void PickUpRock()
        {
            PresentDataRef.IsHoldingRock = true;
        }

        public void PutDownRock()
        {
            PresentDataRef.IsHoldingRock = false;
        }
    }
}