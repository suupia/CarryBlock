using System;
using System.Collections.Generic;
using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.Block.Scripts;
using Carry.CarrySystem.Player.Interfaces;
using Carry.CarrySystem.Player.Scripts;
using Fusion;
using UnityEngine;
#nullable  enable

namespace Carry.CarrySystem.Player.Scripts
{
    public class PlayerAidKitPresenterNet : NetworkBehaviour
    {
        struct PresentData : INetworkStruct
        {
            public bool HoldingAidKit; 
        }

        [Networked] ref PresentData PresentDataRef => ref MakeRef<PresentData>();

        // このぐらいなら、PrefabLoadするまでもなく直接アタッチした方がよい

        [SerializeField] GameObject aidKitView = null!;
        
        bool _holdingAidKitLocal = false;
        
        public void Init(ICharacter character)
        {
            // tie presenter to domain
            character.SetAidKitPresenter(this);
        }

        void Awake()
        {
            aidKitView.SetActive(false);
        }

        public override void Render()
        {
            if (_holdingAidKitLocal != PresentDataRef.HoldingAidKit)
            {
                _holdingAidKitLocal = PresentDataRef.HoldingAidKit;
                aidKitView.SetActive(_holdingAidKitLocal);
            }
        }
        

        public void PickUpAidKit()
        {
            PresentDataRef.HoldingAidKit = true;
        }
        
        public void UseAidKit()
        {
            PresentDataRef.HoldingAidKit = false;
        }

        public void DisableAidKit()
        {
            // blockを持つことによって上書きされたときに呼ばれる想定
            PresentDataRef.HoldingAidKit = false;
        }
         
    }
}