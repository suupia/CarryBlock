using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.Block.Scripts;
using Carry.CarrySystem.CarriableBlock.Scripts;
using Carry.CarrySystem.Entity.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using UnityEngine;
using Fusion;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Entity.Scripts;
using Projects.CarrySystem.Gimmick.Scripts;
using Projects.CarrySystem.Item.Scripts;
using UnityEngine.Serialization;

#nullable enable

namespace Carry.CarrySystem.Map.Scripts
{
    public class EntityPresenterNet : NetworkBehaviour, IEntityPresenter
    {
        struct PresentData : INetworkStruct
        {
            public int BasicBlockCount;
            public int UnmovableBlockCount;
            public int HeavyBlockCount;
            public int FragileBlockCount;
            public int ConfusionBlockCount;
            public int CannonBlockCount;
            public int TreasureCoinCount;
            public Direction CannonDirection;
            public int SpikeCount;
        }

        [Networked] ref PresentData PresentDataRef => ref MakeRef<PresentData>();

        // このぐらいなら、PrefabLoadするまでもなく直接アタッチした方がよい
        [SerializeField] GameObject basicBlockView = null!;
        [SerializeField] GameObject doubleBasicBlockView = null!;
        [SerializeField] GameObject unmovableBlockView = null!;
        [SerializeField] GameObject doubleUnmovableBlockView = null!;
        [SerializeField] GameObject heavyBlockView = null!;
        [SerializeField] GameObject doubleHeavyBlockView = null!;
        [SerializeField] GameObject fragileBlockView = null!;
        [SerializeField] GameObject confusionBlockView = null!;
        [SerializeField] GameObject treasureCoinView = null!;
        [SerializeField] GameObject cannonBlockView = null!;
        Direction _cannonDirectionLocal;
        [SerializeField] GameObject spikeView = null!; 

        public override void Render()
        {
            // ToDo: 個数で例外を投げる設計でよいのか？　プレゼンターは何個置けるかを知っている必要ないのでは？
            
            // UnmovableBlock
            unmovableBlockView.SetActive(PresentDataRef.UnmovableBlockCount switch
            {
                0 or 2 => false,
                1 => true,
                _ => throw new InvalidOperationException($"RockCount : {PresentDataRef.UnmovableBlockCount}")
            });
            doubleUnmovableBlockView.SetActive(PresentDataRef.UnmovableBlockCount switch
            {
                0 or 1 => false,
                2 => true,
                _ => throw new InvalidOperationException($"RockCount : {PresentDataRef.UnmovableBlockCount}")
            });

            // BasicBlock
            basicBlockView.SetActive(PresentDataRef.BasicBlockCount switch
            {
                0 or 2 => false,
                1 => true,
                _ => throw new InvalidOperationException($"BasicBlockCount : {PresentDataRef.BasicBlockCount}")
            });
            doubleBasicBlockView.SetActive(PresentDataRef.BasicBlockCount switch
            {
                0 or 1 => false,
                2 => true,
                _ => throw new InvalidOperationException($"BasicBlockCount : {PresentDataRef.BasicBlockCount}")
            });

            // HeavyBlock
            heavyBlockView.SetActive(PresentDataRef.HeavyBlockCount switch
            {
                0 or 2 => false,
                1 => true,
                _ => throw new InvalidOperationException($"HeavyBlockCount : {PresentDataRef.HeavyBlockCount}")
            });
            doubleHeavyBlockView.SetActive(PresentDataRef.HeavyBlockCount switch
            {
                0 or 1 => false,
                2 => true,
                _ => throw new InvalidOperationException($"HeavyBlockCount : {PresentDataRef.HeavyBlockCount}")
            });

            // FragileBlock
            fragileBlockView.SetActive(PresentDataRef.FragileBlockCount switch
            {
                0 or 2 => false,
                1 => true,
                _ => throw new InvalidOperationException($"FragileBlockCount : {PresentDataRef.FragileBlockCount}")
            });
            
            // ConfusionBlock
            confusionBlockView.SetActive(PresentDataRef.ConfusionBlockCount switch
            {
                0 or 2 => false,
                1 => true,
                _ => throw new InvalidOperationException($"ConfusionBlockCount : {PresentDataRef.ConfusionBlockCount}")
            });
            
            // TreasureCoinBlock
            treasureCoinView.SetActive(PresentDataRef.TreasureCoinCount switch
            {
                1 => true,
                _ => false
            });
            
            // CannonBlock
            cannonBlockView.SetActive(PresentDataRef.CannonBlockCount switch
            {
                0 or 2 => false,
                1 => true,
                _ => throw new InvalidOperationException($"CannonBlockCount : {PresentDataRef.CannonBlockCount}")
            });
            if (_cannonDirectionLocal != PresentDataRef.CannonDirection)
            {
                _cannonDirectionLocal = PresentDataRef.CannonDirection;
                // Viewの方を回転させても反映されないので、親を回転させる（たぶんNetwork関連のコンポーネントのせい）
                cannonBlockView.transform.parent.Rotate(0,CalcRotationAmount(_cannonDirectionLocal),0);
            }
            
            // Spike
            spikeView.SetActive(PresentDataRef.SpikeCount switch
            {
                0 or 2 => false,
                1 => true,
                _ => throw new InvalidOperationException($"SpikeCount : {PresentDataRef.SpikeCount}")
            });
        }


        public void SetInitAllEntityActiveData(IEnumerable<IEntity> allEntities)
        {
            // Debug.Log($"allEntities : {string.Join(", ", allEntities)}");
            var allEntityList = allEntities.ToList();
            PresentDataRef.BasicBlockCount = allEntityList.OfType<BasicBlock>().Count();
            PresentDataRef.UnmovableBlockCount = allEntityList.OfType<UnmovableBlock>().Count();
            PresentDataRef.HeavyBlockCount = allEntityList.OfType<HeavyBlock>().Count();
            PresentDataRef.FragileBlockCount = allEntityList.OfType<FragileBlock>().Count();
            PresentDataRef.CannonBlockCount = allEntityList.OfType<CannonBlock>().Count();
            PresentDataRef.ConfusionBlockCount = allEntityList.OfType<ConfusionBlock>().Count();
            PresentDataRef.TreasureCoinCount = allEntityList.OfType<TreasureCoin>().Count();
            PresentDataRef.CannonDirection = allEntityList.OfType<CannonBlock>().FirstOrDefault()?.KindValue switch
            {
                CannonBlock.Kind.Up => Direction.Up,
                CannonBlock.Kind.Left => Direction.Left,
                CannonBlock.Kind.Down => Direction.Down,
                CannonBlock.Kind.Right => Direction.Right,
                null => Direction.Up,  // 存在しない場合は上向きにしておく
                _ => throw new InvalidOperationException()
            };
            PresentDataRef.SpikeCount = allEntityList.OfType<Spike>().Count();
            Debug.Log($"SpikeCount : {PresentDataRef.SpikeCount}");
        }

        public void SetEntityActiveData(IEntity entity, int count)
        {
            // Debug.Log($"BlockPresenterNet.SetBlockActiveData block : {block} count : {count}");
            switch (entity)
            {
                case BasicBlock _:
                    PresentDataRef.BasicBlockCount = count;
                    break;
                case UnmovableBlock _:
                    PresentDataRef.UnmovableBlockCount = count;
                    break;
                case HeavyBlock _:
                    PresentDataRef.HeavyBlockCount = count;
                    break;
                case FragileBlock _:
                    PresentDataRef.FragileBlockCount = count;
                    break;
                case ConfusionBlock _:
                    PresentDataRef.ConfusionBlockCount = count;
                    break;
                case TreasureCoin _:
                    PresentDataRef.TreasureCoinCount = count;
                    break;
                case CannonBlock cannonBlock:
                    Debug.Log($"CannonBlock KindValue : {cannonBlock.KindValue} ");
                    PresentDataRef.CannonBlockCount = count;
                    PresentDataRef.CannonDirection = cannonBlock.KindValue switch
                    {
                        CannonBlock.Kind.Up => Direction.Up,
                        CannonBlock.Kind.Left => Direction.Left,
                        CannonBlock.Kind.Down => Direction.Down,
                        CannonBlock.Kind.Right => Direction.Right,
                        _ => throw new System.Exception($"想定外のCannonBlock.Kindが渡されました cannonBlock.KindValue : {cannonBlock.KindValue}")
                    };
                    break;
                default:
                    throw new System.Exception($"想定外のEntityが渡されました block : {entity}");
                case Spike _:
                    PresentDataRef.SpikeCount = count;
                    break;
            }
        }
        
        float CalcRotationAmount(Direction direction)
        {
            return direction switch
            {
                Direction.Up => 0f,
                Direction.Left => -90f,
                Direction.Down => -180f,
                Direction.Right => 90f,
                _ => throw new System.Exception($"想定外のDirectionが渡されました direction : {direction}")
            };
        }
        
        enum Direction
        {
            Up,
            Left,
            Down,
            Right
        }
    }
}