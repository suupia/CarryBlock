using Fusion;
using UnityEngine;
using System;
using Animations;
using VContainer;
using VContainer.Unity;

namespace Main
{
    /// <summary>
    /// The only NetworkBehaviour to control the character.
    /// Note: Objects to which this class is attached do not move themselves.
    /// Attachment on the inspector is done to the Info class.
    /// </summary>
    public abstract class AbstractNetworkPlayerController : NetworkBehaviour
    {
        [SerializeField]
        Transform unitObjectParent; // The NetworkCharacterControllerPrototype interpolates this transform.

        [SerializeField] GameObject cameraPrefab;

        [SerializeField] GameObject[] playerUnitPrefabs;
        [SerializeField] UnitType _unitType;
        IAnimatorPlayerUnit _animatorSetter;

        [SerializeField] PlayerInfo _info;

        [Networked] NetworkButtons PreButtons { get; set; }
        [Networked] public NetworkBool IsReady { get; set; }

        [Networked] TickTimer ShootCooldown { get; set; }
        [Networked] TickTimer ActionCooldown { get; set; }

        // Animation

        [Networked] int MainActionCount { get; set; }
        [Networked] int AttackCount { get; set; }
        
        int _preMainActionCount = 0;
        int _preAttackCount = 0;
        Vector3 preDirection = Vector3.zero;

        GameObject _unitObj;
        IUnit _unit;
        [Networked] protected ref NetworkPlayerStruct PlayerStruct => ref MakeRef<NetworkPlayerStruct>();
        IUnitStats _unitStats;
        IUnitAttack _shooter;


        enum UnitType
        {
            Tank = 0,
            CollectResourcePlane = 1,
            EstablishSubBasePlane = 2,
            NoneAttackTank = 3,
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
                var followtarget = Instantiate(cameraPrefab).GetComponent<CameraFollowTarget>();
                followtarget.SetTarget(unitObjectParent.transform);
                
            }


        }

        public override void FixedUpdateNetwork()
        {
            if (!HasStateAuthority) return;

            if (ShootCooldown.ExpiredOrNotRunning(Runner))
            {
                if (PlayerStruct.IsAlive)
                {
                    var attacked = _shooter.AttemptAttack();
                    if (attacked) AttackCount++;
                    ShootCooldown = TickTimer.CreateFromSeconds(Runner, _shooter.AttackCooldown());
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

                if (input.Buttons.WasPressed(PreButtons, PlayerOperation.MainAction))
                {
                    if (ActionCooldown.ExpiredOrNotRunning(Runner))
                    {
                        if (PlayerStruct.IsAlive)
                        {
                            _unit.Action();
                            ActionCooldown = TickTimer.CreateFromSeconds(Runner, _unit.ActionCooldown());
                            MainActionCount++;   
                        }
                    }
                }
                

                var direction = new Vector3(input.Horizontal, 0, input.Vertical).normalized;

                _unit.Move(direction);

                PreButtons = input.Buttons;
            }
            
            
        }

        protected void Update()
        {
        
            if (Object. HasInputAuthority)
            {
                if (Input.GetKeyDown(KeyCode.C))
                {
                    RPC_ChangeNextUnit();
                }
            }



        }

        public override void Render()
        {
            var deltaAngle = Vector3.SignedAngle(preDirection, _info.playerObj.transform.forward, Vector3.up);
            preDirection = _info.playerObj.transform.forward;
            var vector = deltaAngle switch
            {
                < 0 => new Vector3(-1, 0, 0),
                > 0 => new Vector3(1, 0, 0),
                _ => Vector3.zero
            };
            _animatorSetter.OnMove(vector);
            // Debug.Log("_info.playerObj.transform.forward = " + _info.playerObj.transform.forward);
            
            if (MainActionCount > _preMainActionCount)
            {
                _animatorSetter.OnMainAction();
                _preMainActionCount = MainActionCount;
            }
            
            if (AttackCount > _preAttackCount)
            {
                //現状、攻撃時のアニメーションがないので何も効果はありません
                // print("Attacked");
                _animatorSetter.OnAttack();
                _preAttackCount = AttackCount;
            }
            
        }


        //Deal as RPC for changing unit
        [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.All)]
        public void RPC_ChangeNextUnit()
        {
            _unitType = (UnitType)(((int)_unitType + 1) % Enum.GetValues(typeof(UnitType)).Length);
            Destroy(_unitObj);
            InstantiateUnit(_unitType);
            
            SetToOrigin();
        }
        
        [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority)]
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
                    _animatorSetter = new TankAnimatorSetter(new TankAnimatorSetterInfo()
                    {
                        Animator = _unitObj.GetComponentInChildren<Animator>(),
                    });
                    _unitStats = new PlayerStats(ref PlayerStruct, _animatorSetter);
                    break;
                case UnitType.CollectResourcePlane:
                    _unit = new CollectResourcePlane(_info);
                    _shooter = new UnitShooter(_info);
                    _animatorSetter = new PlaneAnimatorSetter(new PlaneAnimatorSetterInfo()
                    {
                        Animator = _unitObj.GetComponentInChildren<Animator>(),
                    });
                    _unitStats = new PlayerStats(ref PlayerStruct,_animatorSetter);
                    break;
                case UnitType.EstablishSubBasePlane:
                    _unit = new EstablishSubBasePlane(_info);
                    _shooter = new UnitShooter(_info);
                    _animatorSetter = new PlaneAnimatorSetter(new PlaneAnimatorSetterInfo()
                    {
                        Animator = _unitObj.GetComponentInChildren<Animator>(),
                    });
                    _unitStats = new PlayerStats(ref PlayerStruct,_animatorSetter);
                    break;
                case UnitType.NoneAttackTank:
                    _unit = new Tank(_info);
                    _shooter = new NoneAttack();
                    _animatorSetter = new TankAnimatorSetter(new TankAnimatorSetterInfo()
                    {
                        Animator = _unitObj.GetComponentInChildren<Animator>(),
                    });
                    _unitStats = new PlayerStats(ref PlayerStruct,_animatorSetter);
                    break;
            }

            // Play spawn animation
            _animatorSetter.OnSpawn();
        }

        public void OnAttacked(int damage)
        {
            if (!HasStateAuthority) return;
            _unitStats.OnAttacked(ref PlayerStruct, damage);
        }
        
        void SetToOrigin()
        {
            // ToDo: 地面をすり抜けないようにするために、少し上に移動させておく（Spawnとの調整は後回し）
            _info.playerObj.transform.position = new Vector3(0, 5, 0);
            _info.playerRd.velocity = Vector3.zero;
        }
    }

}