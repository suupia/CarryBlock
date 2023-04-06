using Fusion;
using UnityEngine;

/// <summary>
/// Manage input, now playing unit, or something...
/// This Controller class manages "NetworkPlayerUnit"
/// </summary>
public class NetworkPlayerController : NetworkBehaviour
{
    [SerializeField] GameObject cameraPrefab;

    [SerializeField] NetworkPlayerUnit[] playerUnitPrefabs;

    [Networked] NetworkButtons PreButtons { get; set; }
    [Networked] public NetworkBool IsReady { get; set; }
    [Networked] public NetworkPlayerUnit Unit { get; set; }
    

    public override void Spawned()
    {
        //Spawn init player unit
        if (Object.HasStateAuthority)
        {
            Unit = SpawnPlayerUnit(0);
            Unit.Object.transform.SetParent(Object.transform);
            
                    
            // spawn camera
            var followtarget = Instantiate(cameraPrefab).GetComponent<CameraFollowTarget>();
           followtarget.SetTarget(Unit.Object.transform);
        }
    }

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        if (Object.HasStateAuthority)
        {
            if (Unit != null && Unit.Object != null)
            {
                Runner.Despawn(Unit.Object);
            }
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData input))
        {
            //TODO: Check phase
            if (input.Buttons.GetPressed(PreButtons).IsSet(PlayerOperation.Ready))
            {
                IsReady = !IsReady;
                Debug.Log($"Toggled Ready -> {IsReady}");
            }

            if (input.Buttons.GetPressed(PreButtons).IsSet(PlayerOperation.ChangeUnit))
            {
                //Tmp
                RPC_ChangeUnit(1);
            }

            var direction = new Vector3(input.Horizontal, 0, input.Vertical).normalized;
            // Debug.Log($"Direction:{direction}");
            //Apply input
            Unit.Move(direction);
            Unit.Action(input.Buttons, PreButtons);

            PreButtons = input.Buttons;
        }
    }

    //Deal as RPC for changing unit
    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority)]
    public void RPC_ChangeUnit(int index)
    {
        if (Unit != null)
        {
            Runner.Despawn(Unit.Object);
        }

        Unit = SpawnPlayerUnit(index);
    }

    NetworkPlayerUnit SpawnPlayerUnit(int index)
    {
        var prefab = playerUnitPrefabs[index];
        var position = new Vector3(0, 1, 0);
        var rotation = Quaternion.identity;

        return Runner.Spawn(prefab, position, rotation, Runner.LocalPlayer);
    }
}