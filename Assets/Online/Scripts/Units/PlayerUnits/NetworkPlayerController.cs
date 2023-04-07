using Fusion;
using UnityEngine;

/// <summary>
/// Manage input, now playing unit, or something...
/// This Controller class manages "NetworkPlayerUnit"
/// </summary>
public class NetworkPlayerController : NetworkBehaviour
{
    [SerializeField] GameObject cameraPrefab;

    [SerializeField] NetworkPrefabRef[] playerUnitPrefabs;

    [SerializeField] NetworkPlayerInfo info;

    [Networked] NetworkButtons PreButtons { get; set; }
    [Networked] public NetworkBool IsReady { get; set; }
    public NetworkPlayerUnit Unit { get; set; }


    [SerializeField] NetworkObject playerObjectParent; // このオブジェクトの子に3DモデルやCircleDetectorをつける

    public override void Spawned()
    {
        //Spawn init player unit
        if (Object.HasStateAuthority)
        {
            // Unit = SpawnPlayerUnit(0);
            // Unit.Object.transform.SetParent(Object.transform);
            

            // とりあえずTankとしてスポーンさせる
            var networkUnit = SpawnPlayerUnit(0);
            networkUnit.transform.SetParent(playerObjectParent.transform);


            // Unit = playerObj.transform.gameObject.AddComponent<Tank>();
            // Debug.Log($"Unit:{Unit}");
            // Unit = playerObj.GetComponent<Tank>();


        }
        info.Init(playerObjectParent);
        Unit = new Tank(info); // Todo : new でインスタンス化する

        if (Object.HasInputAuthority)
        {
            // spawn camera
            var followtarget = Instantiate(cameraPrefab).GetComponent<CameraFollowTarget>();
            followtarget.SetTarget(playerObjectParent.transform);
            Debug.Log($"target.name = {playerObjectParent.transform.name}");
        }
    }

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        if (Object.HasStateAuthority)
        {
            if (Unit != null && playerObjectParent != null)
            {
                Runner.Despawn(playerObjectParent);
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
            Runner.Despawn(playerObjectParent);
        }

        playerObjectParent = SpawnPlayerUnit(index);
    }

    // NetworkPlayerUnit SpawnPlayerUnit(int index)
    // {
    //     var prefab = playerUnitPrefabs[index];
    //     var position = new Vector3(0, 1, 0);
    //     var rotation = Quaternion.identity;
    //
    //     return Runner.Spawn(prefab, position, rotation, Runner.LocalPlayer);
    // }
    
    NetworkObject SpawnPlayerUnit(int index)
    {
        var prefab = playerUnitPrefabs[0];
        var position = new Vector3(0, 1, 0);
        var rotation = Quaternion.identity;
       return   Runner.Spawn(prefab, position, rotation, Runner.LocalPlayer);
    }
}