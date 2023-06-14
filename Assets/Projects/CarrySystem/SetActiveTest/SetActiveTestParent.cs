using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using Nuts.NetworkUtility.Inputs.Scripts;
using UnityEngine;

namespace Carry.CarrySystem.SetActiveTest
{
    public class SetActiveTestParent : NetworkBehaviour
    {
        [SerializeField] GameObject targetObject;
        [Networked] NetworkButtons PreButtons { get; set; }
        
        [Networked] int ChangeActiveCount { get; set; }
        [Networked] int PreChangeActiveCount { get; set; }

        public override void FixedUpdateNetwork()
        {
            if (!HasStateAuthority) return;

            if (GetInput(out NetworkInputData input))
            {
                if (input.Buttons.WasPressed(PreButtons, PlayerOperation.MainAction))
                {
                    ChangeActiveCount++;
                    Debug.Log($" ChangeActiveCount -> {ChangeActiveCount}");
                }
                
                PreButtons = input.Buttons;
            }
        }

        void Update()
        {
            if (Object.InputAuthority)
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    RPC_ChangeActive();
                }
            }
        }

        public override void Render()
        {
            if (ChangeActiveCount > PreChangeActiveCount)
            {
                PreChangeActiveCount = ChangeActiveCount;
                targetObject.SetActive(!targetObject.activeSelf);
            }

        }

        [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
        public void RPC_ChangeActive()
        {
            targetObject.SetActive(!targetObject.activeSelf);
        }
    }


}