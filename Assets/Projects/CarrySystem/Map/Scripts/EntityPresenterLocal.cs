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
    public class EntityPresenterLocal : MonoBehaviour, IEntityPresenter
    {
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
        
        int _basicBlockCount;
        int _unmovableBlockCount;
        int _heavyBlockCount;
        int _fragileBlockCount;
        int _confusionBlockCount;
        int _cannonBlockCount;
        int _treasureCoinCount;
        Direction _cannonDirection;
        int _spikeCount;
        
        public void DestroyPresenter()
        {
            Destroy(this);
        }

void Update()
{
    // UnmovableBlock
    unmovableBlockView.SetActive(_unmovableBlockCount switch
    {
        0 or 2 => false,
        1 => true,
        _ => throw new InvalidOperationException($"RockCount : {_unmovableBlockCount}")
    });
    doubleUnmovableBlockView.SetActive(_unmovableBlockCount switch
    {
        0 or 1 => false,
        2 => true,
        _ => throw new InvalidOperationException($"RockCount : {_unmovableBlockCount}")
    });

    // BasicBlock
    basicBlockView.SetActive(_basicBlockCount switch
    {
        0 or 2 => false,
        1 => true,
        _ => throw new InvalidOperationException($"BasicBlockCount : {_basicBlockCount}")
    });
    doubleBasicBlockView.SetActive(_basicBlockCount switch
    {
        0 or 1 => false,
        2 => true,
        _ => throw new InvalidOperationException($"BasicBlockCount : {_basicBlockCount}")
    });

    // HeavyBlock
    heavyBlockView.SetActive(_heavyBlockCount switch
    {
        0 or 2 => false,
        1 => true,
        _ => throw new InvalidOperationException($"HeavyBlockCount : {_heavyBlockCount}")
    });
    doubleHeavyBlockView.SetActive(_heavyBlockCount switch
    {
        0 or 1 => false,
        2 => true,
        _ => throw new InvalidOperationException($"HeavyBlockCount : {_heavyBlockCount}")
    });

    // FragileBlock
    fragileBlockView.SetActive(_fragileBlockCount switch
    {
        0 or 2 => false,
        1 => true,
        _ => throw new InvalidOperationException($"FragileBlockCount : {_fragileBlockCount}")
    });

    // ConfusionBlock
    confusionBlockView.SetActive(_confusionBlockCount switch
    {
        0 or 2 => false,
        1 => true,
        _ => throw new InvalidOperationException($"ConfusionBlockCount : {_confusionBlockCount}")
    });

    // TreasureCoinBlock
    treasureCoinView.SetActive(_treasureCoinCount switch
    {
        1 => true,
        _ => false
    });

    // CannonBlock
    cannonBlockView.SetActive(_cannonBlockCount switch
    {
        0 or 2 => false,
        1 => true,
        _ => throw new InvalidOperationException($"CannonBlockCount : {_cannonBlockCount}")
    });
    if (_cannonDirectionLocal != _cannonDirection)
    {
        _cannonDirectionLocal = _cannonDirection;
        cannonBlockView.transform.parent.Rotate(0,CalcRotationAmount(_cannonDirectionLocal),0);
    }

    // Spike
    spikeView.SetActive(_spikeCount switch
    {
        0 or 2 => false,
        1 => true,
        _ => throw new InvalidOperationException($"SpikeCount : {_spikeCount}")
    });
}



        public void SetInitAllEntityActiveData(IEnumerable<IEntity> allEntities)
        {
            // Debug.Log($"allEntities : {string.Join(", ", allEntities)}");
            var allEntityList = allEntities.ToList();
            _basicBlockCount = allEntityList.OfType<BasicBlock>().Count();
            _unmovableBlockCount = allEntityList.OfType<UnmovableBlock>().Count();
            _heavyBlockCount = allEntityList.OfType<HeavyBlock>().Count();
            _fragileBlockCount = allEntityList.OfType<FragileBlock>().Count();
            _cannonBlockCount = allEntityList.OfType<CannonBlock>().Count();
            _confusionBlockCount = allEntityList.OfType<ConfusionBlock>().Count();
            _treasureCoinCount = allEntityList.OfType<TreasureCoin>().Count();
            _cannonDirection = allEntityList.OfType<CannonBlock>().FirstOrDefault()?.KindValue switch
            {
                CannonBlock.Kind.Up => Direction.Up,
                CannonBlock.Kind.Left => Direction.Left,
                CannonBlock.Kind.Down => Direction.Down,
                CannonBlock.Kind.Right => Direction.Right,
                null => Direction.Up,  // 存在しない場合は上向きにしておく
                _ => throw new InvalidOperationException()
            };
            _spikeCount = allEntityList.OfType<Spike>().Count();
        }

        public void SetEntityActiveData(IEntity entity, int count)
        {
            // Debug.Log($"BlockPresenterNet.SetBlockActiveData block : {block} count : {count}");
            switch (entity)
            {
                case BasicBlock _:
                    _basicBlockCount = count;
                    break;
                case UnmovableBlock _:
                    _unmovableBlockCount = count;
                    break;
                case HeavyBlock _:
                    _heavyBlockCount = count;
                    break;
                case FragileBlock _:
                    _fragileBlockCount = count;
                    break;
                case ConfusionBlock _:
                    _confusionBlockCount = count;
                    break;
                case TreasureCoin _:
                    _treasureCoinCount = count;
                    break;
                case CannonBlock cannonBlock:
                    Debug.Log($"CannonBlock KindValue : {cannonBlock.KindValue} ");
                    _cannonBlockCount = count;
                   _cannonDirection = cannonBlock.KindValue switch
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
                    _spikeCount = count;
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