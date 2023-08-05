﻿using System;
using System.Collections;
using System.Collections.Generic;
using Carry.CarrySystem.Entity.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using UnityEngine;
using Fusion;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Entity.Scripts;
using Carry.CarrySystem.Player.Interfaces;
using UnityEngine.Serialization;
using VContainer;
#nullable enable


namespace Carry.CarrySystem.Player.Scripts
{
    [RequireComponent(typeof(CarryPlayerControllerNet))]
    public class PlayerBlockPresenterNet : NetworkBehaviour, IPlayerBlockPresenter
    {
        // Presenter系のクラスはホストとクライアントで状態を一致させるためにNetworkedプロパティを持つので、
        // ドメインの情報を持ってはいけない
        public struct PresentData : INetworkStruct
        {
            public NetworkBool IsHoldingBlock;
        }
        [Networked] public ref PresentData PresentDataRef => ref MakeRef<PresentData>();

        // このぐらいなら、PrefabLoadするまでもなく直接アタッチした方がよい
        [SerializeField] GameObject holdingRock= null!;
        [SerializeField] GameObject holdingDoubleRock= null!;
        [SerializeField] GameObject holdingBasicBlock= null!;
        
        public void Init(ICharacter character)
        {
            character.SetHoldPresenter(this);
        }

        public override void Render()
        {
            // Debug.Log($"PresentDataRef.IsHoldingRock = {PresentDataRef.IsHoldingRock}");
            
            // Rock
            // if (holdingRock.activeSelf != PresentDataRef.IsHoldingBlock)
            // {
            //     holdingRock.SetActive(PresentDataRef.IsHoldingBlock);
            // }
            
            // BasicBlock
            if (holdingBasicBlock.activeSelf != PresentDataRef.IsHoldingBlock)
            {
                holdingBasicBlock.SetActive(PresentDataRef.IsHoldingBlock);
            }
        }

        // ホストのみで呼ばれることに注意
        // 以下の処理はアニメーション、音、エフェクトの再生を行いたくなったら、それぞれのクラスの対応するメソッドを呼ぶようにするかも
        public void PickUpBlock()
        {
            PresentDataRef.IsHoldingBlock = true;
        }

        public void PutDownBlock()
        {
            PresentDataRef.IsHoldingBlock = false;
        }
        
        public void ReceiveBlock()
        {
            PresentDataRef.IsHoldingBlock = true;
        }
        
        public void PassBlock()
        {
            PresentDataRef.IsHoldingBlock = false;
        }
    }
}