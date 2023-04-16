using Fusion;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.Serialization;

/// <summary>
/// The only NetworkBehaviour to control the character.
/// Note: Objects to which this class is attached do not move themselves.
/// Attachment on the inspector is done to the Info class.
/// </summary>
public class NetworkPlayerController : NetworkBehaviour
{
    [SerializeField] Transform unitObjectParent; // The NetworkCharacterControllerPrototype interpolates this transform.
    [SerializeField] GameObject cameraPrefab;

    [SerializeField] GameObject[] playerUnitPrefabs;
    [SerializeField] UnitType _unitType;

    [FormerlySerializedAs("info")] [SerializeField] PlayerInfo _info;

    [Networked] NetworkButtons PreButtons { get; set; }
    [Networked] public NetworkBool IsReady { get; set; }

    [Networked] TickTimer ShootCooldown { get; set; }
    [Networked] TickTimer ActionCooldown { get; set; }

    IPlayerUnit _unit;
    PlayerShooter _shooter;

    enum UnitType
    {
        Tank = 0,
        Plane = 1,
    }


    public override void Spawned()
    {
        // init info
        _info.Init(Runner);

        // Instantiate the unit.
        _unit = InstantiateUnit(_unitType);
        _shooter = new PlayerShooter(_info);

        if (Object.HasInputAuthority)
        {
            // spawn camera
            var followtarget = Instantiate(cameraPrefab).GetComponent<CameraFollowTarget>();
            followtarget.SetTarget(_info.networkCharacterController.transform);
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (HasStateAuthority)
        {
            if (ShootCooldown.ExpiredOrNotRunning(Runner))
            {
                _shooter.AttemptShootEnemy();
                ShootCooldown = TickTimer.CreateFromSeconds(Runner, _shooter.shootInterval);
            }
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
                RPC_ChangeNextUnit();
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
    public void RPC_ChangeNextUnit()
    {
        _unitType = (UnitType)(((int)_unitType + 1) % Enum.GetValues(typeof(UnitType)).Length);
        for(int i = 0; i < unitObjectParent.transform.childCount; i++)
        {
            Destroy(unitObjectParent.transform.GetChild(i).gameObject);
        }
        _unit = InstantiateUnit(_unitType);
        
        // ToDo: 地面をすり抜けないようにするために、少し上に移動させておく（Spawnとの調整は後回し）
        _info.playerObj.transform.position = new Vector3(0, 30, 0) + _info.playerObj.transform.position;
    }

    IPlayerUnit InstantiateUnit(UnitType unitType)
    {
        // Instantiate the unit.
        var prefab = playerUnitPrefabs[(int)unitType];
        var unitObj = Instantiate(prefab, unitObjectParent);

        return unitType switch
        {
            UnitType.Tank => new Tank(_info),
            UnitType.Plane => new Plane(_info),
            _ => throw new ArgumentOutOfRangeException(nameof(unitType), "Invalid unitType")
        };
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
        Collider[] colliders = Physics.OverlapSphere(_info.playerObj.transform.position, _info.rangeRadius);
        var enemys = colliders.Where(collider => collider.CompareTag("Enemy")).Select(collider => collider.gameObject);

        if (enemys.Any()) ShootEnemy(enemys.First());
    }

    void ShootEnemy(GameObject targetEnemy)
    {
        // Debug.Log($"ShootEnemy() targetEnemy:{targetEnemy}");
        var bulletInitPos =
            _info.bulletOffset * (targetEnemy.gameObject.transform.position - _info.playerObj.transform.position)
            .normalized + _info.playerObj.transform.position;
        var bulletObj = _info.runner.Spawn(_info.bulletPrefab, bulletInitPos, Quaternion.identity, PlayerRef.None);
        var bullet = bulletObj.GetComponent<NetworkBulletController>();
        bullet.Init(targetEnemy);
    }
}