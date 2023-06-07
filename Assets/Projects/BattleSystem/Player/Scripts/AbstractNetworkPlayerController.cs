using System;
using Fusion;
using Nuts.Utility.Scripts;
using Nuts.NetworkUtility.Inputs.Scripts;
using Nuts.BattleSystem.Decoration.Scripts;
using Nuts.BattleSystem.Enemy.Scripts;
using Nuts.Projects.BattleSystem.Decoration.Scripts;
using UnityEngine;

namespace Nuts.BattleSystem.Player.Scripts
{
    /// <summary>
    ///     The only NetworkBehaviour to control the character.
    ///     Note: Objects to which this class is attached do not move themselves.
    ///     Attachment on the inspector is done to the Info class.
    /// </summary>
    public abstract class AbstractNetworkPlayerController : NetworkBehaviour, IPlayerOnAttacked
    {
        [SerializeField]  Transform unitObjectParent; // The NetworkCharacterControllerPrototype interpolates this transform.
        public Transform InterpolationTransform => unitObjectParent;

        [SerializeField] GameObject cameraPrefab;
        
        [SerializeField] GameObject[] playerUnitPrefabs;
        [SerializeField] UnitType _unitType;

        [SerializeField] PlayerInfo _info;

        [Networked] NetworkButtons PreButtons { get; set; }
        [Networked] public NetworkBool IsReady { get; set; }

        [Networked] TickTimer ShootCooldown { get; set; }
        [Networked] TickTimer ActionCooldown { get; set; }

        // Detector
        [Networked]
        protected ref PlayerDecorationDetector.Data DecorationDataRef => ref MakeRef<PlayerDecorationDetector.Data>();

        [Networked] public ref NetworkPlayerStruct PlayerStruct => ref MakeRef<NetworkPlayerStruct>();

        PlayerDecorationDetector _decorationDetector;
        IUnitAttack _shooter;
        IUnit _unit;

        GameObject _unitObj;
        IUnitStats _unitStats;
        
        protected void Update()
        {
            if (Object.HasInputAuthority)
                if (Input.GetKeyDown(KeyCode.C))
                    RPC_ChangeNextUnit();
        }


        public override void Spawned()
        {
            // init info
            _info.Init(Runner, gameObject);

            // Instantiate the unit.
            InstantiateUnit(_unitType);

            if (Object.HasInputAuthority)
            {
                // spawn camera
                var cameraObj = Instantiate(cameraPrefab);
                var followtarget = cameraObj.GetComponent<CameraFollowTarget>();
                var playerCamera = cameraObj.GetComponent<Camera>();
                followtarget.SetTarget(unitObjectParent.transform);

            }
        }

        public override void FixedUpdateNetwork()
        {
            if (!HasStateAuthority) return;

            if (ShootCooldown.ExpiredOrNotRunning(Runner))
                if (PlayerStruct.IsAlive)
                {
                    var attacked = _shooter.AttemptAttack();
                    if (attacked) _decorationDetector.OnAttacked(ref DecorationDataRef);
                    ShootCooldown = TickTimer.CreateFromSeconds(Runner, _shooter.AttackCooldown());
                }

            if (GetInput(out NetworkInputData input))
            {
                //TODO: Check phase
                if (input.Buttons.WasPressed(PreButtons, PlayerOperation.Ready))
                {
                    IsReady = !IsReady;
                    Debug.Log($"Toggled Ready -> {IsReady}");
                }

                if (input.Buttons.WasPressed(PreButtons, PlayerOperation.MainAction))
                    if (ActionCooldown.ExpiredOrNotRunning(Runner))
                        if (PlayerStruct.IsAlive)
                        {
                            _unit.Action();
                            ActionCooldown = TickTimer.CreateFromSeconds(Runner, _unit.ActionCooldown());
                            _decorationDetector.OnMainAction(ref DecorationDataRef);
                        }


                var direction = new Vector3(input.Horizontal, 0, input.Vertical).normalized;

                _unit.Move(direction);

                PreButtons = input.Buttons;
            }
        }

        public override void Render()
        {
            _decorationDetector.OnRendered(DecorationDataRef, PlayerStruct.Hp);
        }

        public void OnAttacked(int damage)
        {
            if (!HasStateAuthority) return;
            _unitStats.OnAttacked(ref PlayerStruct, damage);
        }
        
        //Deal as RPC for changing unit
        [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
        public void RPC_ChangeNextUnit()
        {
            _unitType = (UnitType)(((int)_unitType + 1) % Enum.GetValues(typeof(UnitType)).Length);
            Destroy(_unitObj);
            InstantiateUnit(_unitType);

            SetToOrigin();
        }

        [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
        public void RPC_SetToOrigin()
        {
            SetToOrigin();
        }

        void InstantiateUnit(UnitType unitType)
        {
            // Instantiate the unit.
            var prefab = playerUnitPrefabs[(int)unitType];
            _unitObj = Instantiate(prefab, unitObjectParent);

            switch (_unitType)
            {
                case UnitType.Tank:
                    _unit = new Tank(_info);
                    _shooter = new UnitShooter(_info);
                    _decorationDetector = new PlayerDecorationDetector(
                        new TankAnimatorSetter(_unitObj)
                    );
                    _unitStats = new PlayerStats(ref PlayerStruct);
                    break;
                case UnitType.CollectResourcePlane:
                    _unit = new CollectResourcePlane(_info);
                    _shooter = new UnitShooter(_info);
                    _decorationDetector = new PlayerDecorationDetector(
                        new PlaneAnimatorSetter(_unitObj)
                    );
                    _unitStats = new PlayerStats(ref PlayerStruct);
                    break;
                case UnitType.EstablishSubBasePlane:
                    _unit = new EstablishSubBasePlane(_info);
                    _shooter = new UnitShooter(_info);
                    _decorationDetector = new PlayerDecorationDetector(
                        new PlaneAnimatorSetter(_unitObj)
                    );
                    _unitStats = new PlayerStats(ref PlayerStruct);
                    break;
                case UnitType.NoneAttackTank:
                    _unit = new Tank(_info);
                    _shooter = new NoneAttack();
                    _decorationDetector = new PlayerDecorationDetector(
                        new TankAnimatorSetter(_unitObj)
                    );
                    _unitStats = new PlayerStats(ref PlayerStruct);
                    break;
            }

            // Play spawn animation
            _decorationDetector.OnSpawned();
        }



        void SetToOrigin()
        {
            // ToDo: 地面をすり抜けないようにするために、少し上に移動させておく（Spawnとの調整は後回し）
            _info.playerObj.transform.position = new Vector3(0, 5, 0);
            _info.playerRd.velocity = Vector3.zero;
        }


        enum UnitType
        {
            Tank = 0,
            CollectResourcePlane = 1,
            EstablishSubBasePlane = 2,
            NoneAttackTank = 3
        }
    }
}