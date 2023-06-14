using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class SetActiveTestParent : NetworkBehaviour
{
    [SerializeField] GameObject targetObject;

    void Update()
    {
        if (Object.InputAuthority)
        {
            if (Input.GetKeyDown(KeyCode.Space))
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
