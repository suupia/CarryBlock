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

        public override void FixedUpdateNetwork()
        {
            if (!HasStateAuthority) return;

            if (GetInput(out NetworkInputData input))
            {
                if (input.Buttons.WasPressed(PreButtons, PlayerOperation.MainAction))
                {
                    targetObject.SetActive(!targetObject.activeSelf);
                    Debug.Log($"Toggled Active -> {targetObject.activeSelf}");
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

        [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
        public void RPC_ChangeActive()
        {
            targetObject.SetActive(!targetObject.activeSelf);
        }
    }


}