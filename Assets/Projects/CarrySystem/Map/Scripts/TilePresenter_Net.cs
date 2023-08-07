using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Carry.CarrySystem.Block.Scripts;
using Carry.CarrySystem.Entity.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using UnityEngine;
using Fusion;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Entity.Scripts;
using UnityEngine.Serialization;

#nullable enable

namespace Carry.CarrySystem.Map.Scripts
{
    public class TilePresenter_Net : NetworkBehaviour, ITilePresenter
    {
        public struct PresentData : INetworkStruct
        {
            public int GroundCount;
            public int BasicBlockCount;
            public int UnmovableBlockCount;
            public int HeavyBlockCount;
            public int FragileBlockCount;
        }

        [Networked] public ref PresentData PresentDataRef => ref MakeRef<PresentData>();

        // このぐらいなら、PrefabLoadするまでもなく直接アタッチした方がよい
        [SerializeField] GameObject groundView = null!;
        [SerializeField] GameObject basicBlockView = null!;
        [SerializeField] GameObject doubleBasicBlockView = null!;
        [SerializeField] GameObject unmovableBlockView = null!;
        [SerializeField] GameObject doubleUnmovableBlockView = null!;
        [SerializeField] GameObject heavyBlockView = null!;
        [SerializeField] GameObject doubleHeavyBlockView = null!;
        [SerializeField] GameObject fragileBlockView = null!;

        public override void Render()
        {
            groundView.SetActive(PresentDataRef.GroundCount switch
            {
                0 => false,
                1 => true,
                _ => throw new InvalidOperationException($"GroundCount : {PresentDataRef.GroundCount}")
            });

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
        }


        public void SetInitAllEntityActiveData(IEnumerable<IEntity> allEntities)
        {
            var allEntityList = allEntities.ToList();
            PresentDataRef.GroundCount = allEntityList.OfType<Ground>().Count();
            PresentDataRef.BasicBlockCount = allEntityList.OfType<BasicBlock>().Count();
            PresentDataRef.UnmovableBlockCount = allEntityList.OfType<UnmovableBlock>().Count();
            PresentDataRef.HeavyBlockCount = allEntityList.OfType<HeavyBlock>().Count();
            PresentDataRef.FragileBlockCount = allEntityList.OfType<FragileBlock>().Count();
        }

        public void SetEntityActiveData(IEntity entity, int count)
        {
            switch (entity)
            {
                case Ground _:
                    PresentDataRef.GroundCount = count;
                    break;
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
                default:
                    throw new System.Exception("想定外のEntityが渡されました");
            }
        }
    }
}