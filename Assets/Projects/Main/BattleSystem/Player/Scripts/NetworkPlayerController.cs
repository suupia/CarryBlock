using Fusion;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.Serialization;
using Animations;

namespace Main
{
    /// <summary>
    /// The only NetworkBehaviour to control the character.
    /// Note: Objects to which this class is attached do not move themselves.
    /// Attachment on the inspector is done to the Info class.
    /// </summary>
    public class NetworkPlayerController : NetworkBehaviour
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

        [Networked(OnChanged = nameof(OnHpChanged))]
        byte Hp { get; set; } = 1;

        int _preMainActionCount = 0;
        int _preAttackCount = 0;
        Vector3 preDirection = Vector3.zero;

        GameObject _unitObj;
        IUnit _unit;
        [Networked] ref NetworkPlayerStruct PlayerStruct => ref MakeRef<NetworkPlayerStruct>();
        IUnitStats _unitStats;
        UnitShooter _shooter;

        enum UnitType
        {
            Tank = 0,
            CollectResourcePlane = 1,
            EstablishSubBasePlane = 2,
        }


        public override void Spawned()
        {
            // init info
            _info.Init(Runner, gameObject);

            // Instantiate the unit.
            InstantiateUnit(_unitType);
            _shooter = new UnitShooter(_info);

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
                var attacked = _shooter.AttemptShootEnemy();
                if (attacked) AttackCount++;
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

                if (input.Buttons.WasPressed(PreButtons, PlayerOperation.MainAction))
                {
                    if (ActionCooldown.ExpiredOrNotRunning(Runner))
                    {
                        _unit.Action();
                        ActionCooldown = TickTimer.CreateFromSeconds(Runner, _unit.ActionCooldown());
                        MainActionCount++;
                    }
                }

                var direction = new Vector3(input.Horizontal, 0, input.Vertical).normalized;

                _unit.Move(direction);

                PreButtons = input.Buttons;
            }
        }

        void Update()
        {
            if (Object. HasInputAuthority)
            {
                if (Input.GetKeyDown(KeyCode.C))
                {
                    RPC_ChangeNextUnit();
                }
            }

            if (Object.HasInputAuthority)
            {
                if (Input.GetKeyDown(KeyCode.H))
                {
                    _unitStats.OnAttacked(ref PlayerStruct,1);
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

            // ToDo: 地面をすり抜けないようにするために、少し上に移動させておく（Spawnとの調整は後回し）
            _info.playerObj.transform.position = new Vector3(0, 5, 0);
            _info.playerRd.velocity = Vector3.zero;
        }

        void InstantiateUnit(UnitType unitType)
        {
            // Instantiate the unit.
            var prefab = playerUnitPrefabs[(int)unitType];
            _unitObj = Instantiate(prefab, unitObjectParent);

            // Set the unit domain
            _unit = unitType switch
            {
                UnitType.Tank => new Tank(_info),
                UnitType.CollectResourcePlane => new CollectResourcePlane(_info),
                UnitType.EstablishSubBasePlane => new EstablishSubBasePlane(_info),
                _ => throw new ArgumentOutOfRangeException(nameof(unitType), "Invalid unitType")
            };
            // とりあえず共通のスタッツにする
            _unitStats = new PlayerStats(ref PlayerStruct);

            // Set the animator.
            var animator = _unitObj.GetComponentInChildren<Animator>();
            _animatorSetter = unitType switch
            {
                UnitType.Tank => new TankAnimatorSetter(new TankAnimatorSetterInfo()
                {
                    Animator = animator,
                }),
                UnitType.CollectResourcePlane => new PlaneAnimatorSetter(new PlaneAnimatorSetterInfo()
                {
                    Animator = animator,
                }),
                UnitType.EstablishSubBasePlane => new PlaneAnimatorSetter(new PlaneAnimatorSetterInfo()
                {
                    Animator = animator,
                }),
                _ => throw new ArgumentOutOfRangeException(nameof(unitType), "Invalid unitType")
            };

            // Play spawn animation
            _animatorSetter.OnSpawn();
        }

        public void OnAttacked(int damage)
        {
            _unitStats.OnAttacked(ref PlayerStruct, damage);
        }

        public static void OnHpChanged(Changed<NetworkPlayerController> changed)
        {
            var hp = changed.Behaviour.Hp;
            if (hp <= 0)
            {
                changed.Behaviour._animatorSetter.OnDead();
            }
        }
    }

    public class UnitShooter
    {
        PlayerInfo _info;

        public float shootInterval = 0.5f;

        public UnitShooter(PlayerInfo info)
        {
            _info = info;
        }

        public bool AttemptShootEnemy()
        {
            Collider[] colliders = Physics.OverlapSphere(_info.playerObj.transform.position, _info.rangeRadius);
            var enemys = colliders.Where(collider => collider.CompareTag("Enemy"))
                .Select(collider => collider.gameObject);

            if (enemys.Any())
            {
                ShootEnemy(enemys.First());
                return true;
            }
            else
            {
                return false;
            }
        }

        void ShootEnemy(GameObject targetEnemy)
        {
            // Debug.Log($"ShootEnemy() targetEnemy:{targetEnemy}");
            var bulletInitPos =
                _info.bulletOffset * (targetEnemy.gameObject.transform.position - _info.playerObj.transform.position)
                .normalized + _info.playerObj.transform.position;
            var bulletObj = _info._runner.Spawn(_info.bulletPrefab, bulletInitPos, Quaternion.identity, PlayerRef.None);
            var bullet = bulletObj.GetComponent<NetworkBulletController>();
            bullet.Init(targetEnemy);
        }
    }
}