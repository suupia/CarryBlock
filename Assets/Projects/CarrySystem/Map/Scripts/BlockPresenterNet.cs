using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Carry.CarrySystem.Block.Interfaces;
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
    public class BlockPresenterNet : NetworkBehaviour, IBlockPresenter
    {
        public struct PresentData : INetworkStruct
        {
            public int BasicBlockCount;
            public int UnmovableBlockCount;
            public int HeavyBlockCount;
            public int FragileBlockCount;
        }

        [Networked] public ref PresentData PresentDataRef => ref MakeRef<PresentData>();

        // このぐらいなら、PrefabLoadするまでもなく直接アタッチした方がよい
        [SerializeField] GameObject basicBlockView = null!;
        [SerializeField] GameObject doubleBasicBlockView = null!;
        [SerializeField] GameObject unmovableBlockView = null!;
        [SerializeField] GameObject doubleUnmovableBlockView = null!;
        [SerializeField] GameObject heavyBlockView = null!;
        [SerializeField] GameObject doubleHeavyBlockView = null!;
        [SerializeField] GameObject fragileBlockView = null!;

        public override void Render()
        {
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
            // Debug.Log($"allEntities : {string.Join(", ", allEntities)}");
            var allEntityList = allEntities.ToList();
            PresentDataRef.BasicBlockCount = allEntityList.OfType<BasicBlock>().Count();
            PresentDataRef.UnmovableBlockCount = allEntityList.OfType<UnmovableBlock>().Count();
            PresentDataRef.HeavyBlockCount = allEntityList.OfType<HeavyBlock>().Count();
            PresentDataRef.FragileBlockCount = allEntityList.OfType<FragileBlock>().Count();
        }

        public void SetBlockActiveData(IBlock block, int count)
        {
            // Debug.Log($"BlockPresenterNet.SetBlockActiveData block : {block} count : {count}");
            switch (block)
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
                default:
                    throw new System.Exception($"想定外のEntityが渡されました block : {block}");
            }
        }
    }
}