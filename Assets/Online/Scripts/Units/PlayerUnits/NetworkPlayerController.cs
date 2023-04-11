using Fusion;
using UnityEngine;

/// <summary>
/// The only NetworkBehaviour to control the character.
/// Note: Objects to which this class is attached do not move themselves.
/// Attachment on the inspector is done to the Info class.
/// </summary>
public class NetworkPlayerController : NetworkBehaviour
{
    [SerializeField] GameObject cameraPrefab;

    [SerializeField] GameObject[] playerUnitPrefabs;

    [SerializeField] NetworkPlayerInfo info;

    [Networked] NetworkButtons PreButtons { get; set; }
    [Networked] public NetworkBool IsReady { get; set; }
    
    [Networked]  TickTimer _actionCooldown { get; set; }
    
    public NetworkPlayerUnit Unit { get; set; }


    public override void Spawned()
    {

        // Instantiate the tank.
        var prefab = playerUnitPrefabs[0];
        var unitObj = Instantiate(prefab, info.unitObjectParent);
 
        info.Init(Runner,unitObj);
        Unit = new Tank(info);

        if (Object.HasInputAuthority)
        {
            // spawn camera
            var followtarget = Instantiate(cameraPrefab).GetComponent<CameraFollowTarget>();
            followtarget.SetTarget(info.unitObjectParent);
            Debug.Log($"target.name = {info.unitObjectParent}");
        }
    }
    
    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData input))
        {
            //TODO: Check phase
            if (input.Buttons.WasPressed(PreButtons, PlayerOperation.Ready))
            {
                IsReady = !IsReady;
                Debug.Log($"Toggled Ready -> {IsReady}");
            }

            if (input.Buttons.WasPressed(PreButtons, PlayerOperation.ChangeUnit))
            {
                //Tmp
                RPC_ChangeUnit(1);
            }

            if (input.Buttons.WasPressed(PreButtons, PlayerOperation.MainAction))
            {
                // RPC_MainAction();
                if (_actionCooldown.ExpiredOrNotRunning(Runner))
                {
                    Unit.Action();
                    _actionCooldown = TickTimer.CreateFromSeconds(Runner, Unit.DelayBetweenActions);
                }
            }
            var direction = new Vector3(input.Horizontal, 0, input.Vertical).normalized;
            // Debug.Log($"Direction:{direction}");
            //Apply input
            // if (Runner.IsForward)
            // {
            //     Unit.Move(direction);
            // }
            Unit.Move(direction);

            PreButtons = input.Buttons;
        }
    }
    
    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.All)]
    private void RPC_MainAction()
    {
        Unit.Action();
    }

    //Deal as RPC for changing unit
    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.All)]
    public void RPC_ChangeUnit(int index)
    {
        // Todo : ChangeUnitの実装
        
        // if (Unit != null)
        // {
        //     Runner.Despawn(playerObjectParent);
        // }
        //
        // playerObjectParent = SpawnPlayerUnit(index);
    }
    
}