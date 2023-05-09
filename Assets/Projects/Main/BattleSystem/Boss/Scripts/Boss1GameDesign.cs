using System;
using System.Collections.Generic;
using Main;
using UnityEngine;

namespace Boss
{
    public partial class Boss1Controller
    {
        //Define Const value
        private const float JumpTime = 2f;
        private const float ChargeJumpTime = 0.5f;
        private const float JumpAttackRadius = 3f;
        private const float ChargeSpitOutTime = 1.5f;
        private const float SearchRadius = 6f;
        private const float DefaultAttackCoolTime = 4f;

        private readonly State[] _detectedStates =
            { State.ChargingJump, State.Tackling, State.SpitOut, State.Vacuuming };

        //Define Template Moves 
        IEnemyMove DefaultMove => new SimpleMove(new SimpleMove.Record
        {
            GameObject = gameObject,
            Acceleration = 20f,
            MaxVelocity = 1f
        });

        IEnemyMove SpeedyMove => new SimpleMove(new SimpleMove.Record
        {
            GameObject = gameObject,
            Acceleration = 30f,
            MaxVelocity = 2.5f
        });

        IEnemyMove WanderingMove => new WanderingMove(
            new WanderingMove.Record
            {
                InputSimulationFrequency = 2f
            },
            move: DefaultMove
        );

        IEnemyMove ToTargetMove =>
            new ToTargetMove(
                new ToTargetMove.Record
                {
                    Transform = transform
                }, SpeedyMove);

        IEnemyMove LookAtTargetMove => new LookAtTargetMove(transform);

        //Define Search
        IEnemySearch DefaultSearch => new RangeSearch(transform, SearchRadius,
            layerMask: LayerMask.GetMask("Player"));

        //Define State
        private Dictionary<State, Func<StateContext>> _get;


        private void Start()
        {
            _get = new Dictionary<State, Func<StateContext>>
            {
                {
                    State.None, () => default
                },
                {
                    State.Lost, () => new StateContext
                    {
                        Move = WanderingMove,
                        Search = DefaultSearch,
                        Attack = null,
                        AttackCoolTime = 0
                    }
                },
                {
                    State.Tackling, () => new StateContext
                    {
                        Attack = new ToNearestAttack(new TargetBufferAttack.Context
                            {
                                Transform = transform,
                                TargetBuffer = _targetBuffer
                            },
                            new ToTargetAttack(
                                gameObject,
                                new RangeAttack(new RangeAttack.Context
                                {
                                    Transform = transform,
                                    Radius = 2f,
                                    AttackSphereLifeTime = 0.5f
                                })
                            )
                        ),
                        AttackCoolTime = 1,
                        Move = ToTargetMove,
                        Search = DefaultSearch
                    }
                },
                {
                    State.Jumping, () => new StateContext
                    {
                        Attack = new ToNearestAttack(new TargetBufferAttack.Context
                            {
                                Transform = transform,
                                TargetBuffer = _targetBuffer
                            },
                            new ToTargetAttack(
                                gameObject,
                                new DelayAttack(
                                    JumpTime,
                                    new RangeAttack(new RangeAttack.Context
                                    {
                                        Transform = transform,
                                        Radius = JumpAttackRadius
                                    })
                                )
                            )
                        ),
                        AttackCoolTime = DefaultAttackCoolTime,
                        Move = ToTargetMove,
                        Search = DefaultSearch
                    }
                },
                {
                    State.ChargingJump, () => new StateContext
                    {
                        Move = LookAtTargetMove,
                        Search = DefaultSearch,
                        Attack = null,
                        AttackCoolTime = 0
                    }
                },
                {
                    State.SpitOut, () => new StateContext
                    {
                        Attack = new ToFurthestAttack(new TargetBufferAttack.Context
                            {
                                Transform = transform,
                                TargetBuffer = _targetBuffer
                            },
                            new ToTargetAttack(
                                gameObject,
                                new DelayAttack(
                                    ChargeSpitOutTime,
                                    new LaunchNetworkObjectAttack(new LaunchNetworkObjectAttack.Context
                                        {
                                            Runner = Runner,
                                            From = finSpawnerTransform,
                                            Prefab = Resources.Load<GameObject>("Prefabs/Attacks/Fin"),
                                        }
                                    )
                                )
                            )
                        ),
                        AttackCoolTime = DefaultAttackCoolTime,
                        Move = LookAtTargetMove,
                        Search = DefaultSearch,
                    }
                },
                {
                    State.Vacuuming, () => new StateContext
                    {
                        Attack = new ToNearestAttack(new TargetBufferAttack.Context
                            {
                                Transform = transform,
                                TargetBuffer = _targetBuffer
                            },
                            new ToTargetAttack(gameObject, new MockAttack())
                        ),
                        AttackCoolTime = DefaultAttackCoolTime,
                        Move = LookAtTargetMove,
                        Search = DefaultSearch
                    }
                }
            };
        }
    }
}