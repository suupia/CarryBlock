using Fusion;
using UnityEngine;
using System.Linq;

/// <summary>
/// The only NetworkBehaviour to control the character.
/// Note: Objects to which this class is attached do not move themselves.
/// Attachment on the inspector is done to the Info class.
/// </summary>
public class NetworkPlayerController : NetworkBehaviour
{
    [SerializeField] GameObject cameraPrefab;

    [SerializeField] GameObject[] playerUnitPrefabs;

    [SerializeField] PlayerInfo info;

    [Networked] NetworkButtons PreButtons { get; set; }
    [Networked] public NetworkBool IsReady { get; set; }

    [Networked] TickTimer ShootCooldown { get; set; }
    [Networked] TickTimer ActionCooldown { get; set; }

    IPlayerUnit _unit;
    PlayerShooter _shooter;


    public override void Spawned()
    {
        // Instantiate the tank.
        var prefab = playerUnitPrefabs[0];
        var unitObj = Instantiate(prefab, info.unitObjectParent);

        info.Init(Runner, unitObj);
        _unit = new Tank(info);
        _shooter = new PlayerShooter(info);

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
        if (!HasStateAuthority) return;

        if (ShootCooldown.ExpiredOrNotRunning(Runner))
        {
            _shooter.AttemptShootEnemy();
            ShootCooldown = TickTimer.CreateFromSeconds(Runner, _shooter.shootInterval);
        }

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
                if (ActionCooldown.ExpiredOrNotRunning(Runner))
                {
                    _unit.Action();
                    ActionCooldown = TickTimer.CreateFromSeconds(Runner, _unit.ActionCooldown());
                }
            }

            var direction = new Vector3(input.Horizontal, 0, input.Vertical).normalized;

            _unit.Move(direction);

            PreButtons = input.Buttons;
        }
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

public class PlayerShooter
{
    PlayerInfo _info;

    public float shootInterval = 0.5f;

    public PlayerShooter(PlayerInfo info)
    {
        _info = info;
    }

    public void AttemptShootEnemy()
    {
        Collider[] colliders = Physics.OverlapSphere(_info.unitObject.transform.position, _info.rangeRadius);
        var enemys = colliders.Where(collider => collider.CompareTag("Enemy")).Select(collider => collider.gameObject);

        if (enemys.Any()) ShootEnemy(enemys.First());
    }

    void ShootEnemy(GameObject targetEnemy)
    {
        // Debug.Log($"ShootEnemy() targetEnemy:{targetEnemy}");
        var bulletInitPos =
            _info.bulletOffset * (targetEnemy.gameObject.transform.position - _info.unitObject.transform.position)
            .normalized + _info.unitObject.transform.position;
        var bulletObj = _info.runner.Spawn(_info.bulletPrefab, bulletInitPos, Quaternion.identity, PlayerRef.None);
        var bullet = bulletObj.GetComponent<NetworkBulletController>();
        bullet.Init(targetEnemy);
    }
}