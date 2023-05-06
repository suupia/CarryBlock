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

        private readonly State[] _detectedStates =  { State.ChargingJump, State.Tackling, State.SpitOut, State.Vacuuming };

        //Define Template Moves 
        private IMove DefaultMove => new SimpleMove(new SimpleMove.Context()
        {
            GameObject = gameObject,
            Acceleration = 20f,
            MaxVelocity = 1f
        });

        private IMove SpeedyMove => new SimpleMove(new SimpleMove.Context()
        {
            GameObject = gameObject,
            Acceleration = 30f,
            MaxVelocity = 2.5f
        });

        private IMove WanderingMove => new WanderingMove(
            context: new WanderingMove.Context()
            {
                InputSimulationFrequency = 2f
            },
            move: DefaultMove
        );

        private IMove ToTargetMove =>
            new ToTargetMove(
                new ToTargetMove.Context()
                {
                    Transform = transform
                }, SpeedyMove);

        private IMove LookAtTargetMove => new LookAtTargetMove(transform);

        //Define Search
        private ISearch DefaultSearch => new RangeSearch(transform: transform, radius: SearchRadius,
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
                        Search = DefaultSearch
                    }
                },
                {
                    State.Tackling, () => new StateContext
                    {
                        Attack = new ToNearestAttack(
                            new TargetBufferAttack.Context()
                            {
                                Transform = transform,
                                TargetBuffer = _targetBuffer
                            },
                            new ToTargetAttack(
                                gameObject,
                                new MockAttack()
                            )
                        ),
                        Move = ToTargetMove,
                        Search = DefaultSearch
                    }
                },
                {
                    State.Jumping, () =>
                    {
                        GameObject o;
                        return new StateContext
                        {
                            Attack = new ToNearestAttack(
                                new TargetBufferAttack.Context()
                                {
                                    Transform = transform,
                                    TargetBuffer = _targetBuffer
                                },
                                new ToTargetAttack(
                                    (o = gameObject),
                                    new DelayAttack(
                                        JumpTime,
                                        new RangeAttack(o, JumpAttackRadius)
                                    )
                                )
                            ),
                            Move = ToTargetMove,
                            Search = DefaultSearch
                        };
                    }
                },
                {
                    State.ChargingJump, () => new StateContext
                    {
                        Move = LookAtTargetMove,
                        Search = DefaultSearch
                    }
                },
                {
                    State.SpitOut, () => new StateContext()
                    {
                        Attack = new ToFurthestAttack(
                            new TargetBufferAttack.Context()
                            {
                                Transform = transform,
                                TargetBuffer = _targetBuffer
                            },
                            new ToTargetAttack(
                                gameObject,
                                new DelayAttack(
                                    ChargeSpitOutTime,
                                    new LaunchNetworkObjectAttack(
                                        new LaunchNetworkObjectAttack.Context()
                                        {
                                            Runner = Runner,
                                            From = finSpawnerTransform,
                                            Prefab = Resources.Load<GameObject>("Prefabs/Attacks/Fin"),
                                        }
                                    )
                                )
                            )
                        ),
                        Move = LookAtTargetMove,
                        Search = DefaultSearch,
                    }
                },
                {
                    State.Vacuuming, () => new StateContext()
                    {
                        Attack = new ToNearestAttack(
                            new TargetBufferAttack.Context()
                            {
                                Transform = transform,
                                TargetBuffer = _targetBuffer
                            },
                            new ToTargetAttack(gameObject, new MockAttack())
                        ),
                        Move = LookAtTargetMove,
                        Search = DefaultSearch
                    }
                }
            };
        }
    }
}