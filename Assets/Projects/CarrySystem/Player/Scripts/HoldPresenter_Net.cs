using System;
using System.Collections;
using System.Collections.Generic;
using Carry.CarrySystem.Entity.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using UnityEngine;
using Fusion;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Entity.Scripts;


namespace Carry.CarrySystem.Player.Scripts
{
    [RequireComponent(typeof(CarryPlayerController_Net))]
    public class HoldPresenter_Net : NetworkBehaviour
    {
        // Presenter系のクラスはホストとクライアントで状態を一致させるためにNetworkedプロパティを持つので、
        // ドメインの情報を持ってはいけない
        public struct PresentData : INetworkStruct
        {
            public NetworkBool IsHoldingRock;
            public NetworkBool IsHoldingDoubleRock;
        }

        [Networked] public bool IsInitialized { get; set; } // ホストで初期化されるのでNetworkedである必要がある
        [Networked] public ref PresentData PresentDataRef => ref MakeRef<PresentData>();

        // 一旦、べた貼り付けにする
        [SerializeField] GameObject holdingRock;
        [SerializeField] GameObject holdingDoubleRock;

        HoldAction _holdAction;


        public void Init(HoldAction holdAction)
        {
            _holdAction = holdAction;
            IsInitialized = true;
        }

        public void FixedUpdate()
        {
            if (!IsInitialized) return;
            if (!HasStateAuthority) return;

            PresentDataRef.IsHoldingRock = _holdAction.IsHoldingRock;

            Debug.Log($"PresentDataRef.IsHoldingRock = {_holdAction.IsHoldingRock}");
        }

        public override void Render()
        {
            if (!IsInitialized) return;

            Debug.Log($"PresentDataRef.IsHoldingRock = {PresentDataRef.IsHoldingRock}");

            if (holdingRock.activeSelf != PresentDataRef.IsHoldingRock)
            {
                holdingRock.SetActive(PresentDataRef.IsHoldingRock);
            }

            if (holdingDoubleRock.activeSelf != PresentDataRef.IsHoldingDoubleRock)
            {
                holdingDoubleRock.SetActive(PresentDataRef.IsHoldingDoubleRock);
            }
        }

        public void SetHoldData(IEntity entity, bool isActive)
        {
            // DoubleRockクラスがないからどう書くか。。。
        }
    }
}