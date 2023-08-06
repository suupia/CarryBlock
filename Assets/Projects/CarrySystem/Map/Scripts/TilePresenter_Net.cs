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

#nullable enable

namespace Carry.CarrySystem.Map.Scripts
{
    public class TilePresenter_Net : NetworkBehaviour, ITilePresenter
    {
        public struct PresentData : INetworkStruct
        {
            public int GroundCount;
            public int BasicBlockCount;
            public int RockCount;
        }

        [Networked] public ref PresentData PresentDataRef => ref MakeRef<PresentData>();
        
        // このぐらいなら、PrefabLoadするまでもなく直接アタッチした方がよい
        [SerializeField] GameObject groundObject = null!;
        [SerializeField] GameObject rockObject= null!;
        [SerializeField] GameObject doubleRockObject= null!;
        [SerializeField] GameObject basicBlockObject= null!;
        [SerializeField] GameObject doubleBasicBlockObject= null!;
        
        public override void Render()
        {
            groundObject.SetActive(PresentDataRef.GroundCount switch
            {
                0 => false,
                1 => true,
                _ => throw new InvalidOperationException($"GroundCount : {PresentDataRef.GroundCount}")
            });
            
            rockObject.SetActive(PresentDataRef.RockCount switch
            {
                0 or 2 => false,
                1 => true,
                _ => throw new InvalidOperationException($"RockCount : {PresentDataRef.RockCount}")
            });

            doubleRockObject.SetActive(PresentDataRef.RockCount switch
            {
                0 or 1 => false,
                2 => true,
                _ => throw new InvalidOperationException($"RockCount : {PresentDataRef.RockCount}")
            });
            
            basicBlockObject.SetActive(PresentDataRef.BasicBlockCount switch
            {
                0 or 2 => false,
                1 => true,
                _ => throw new InvalidOperationException($"BasicBlockCount : {PresentDataRef.BasicBlockCount}")
            });
            doubleBasicBlockObject.SetActive(PresentDataRef.BasicBlockCount switch
            {
                0 or 1 => false,
                2 => true,
                _ => throw new InvalidOperationException($"BasicBlockCount : {PresentDataRef.BasicBlockCount}")
            });

        }
        

        public void SetInitAllEntityActiveData(IEnumerable<IEntity> allEntities)
        {
            var allEntityList = allEntities.ToList();
            PresentDataRef.GroundCount = allEntityList.OfType<Ground>().Count();
            PresentDataRef.RockCount = allEntityList.OfType<UnmovableBlock>().Count();
            PresentDataRef.BasicBlockCount = allEntityList.OfType<BasicBlock>().Count();
        }

        public void SetEntityActiveData(IEntity entity, int count) 
        {
            switch (entity)
            {
                case Ground _:
                    PresentDataRef.GroundCount = count;
                    break;
                case UnmovableBlock _:
                    PresentDataRef.RockCount = count;
                    break;
                case BasicBlock _:
                    PresentDataRef.BasicBlockCount = count;
                    break;
                default:
                    throw new System.Exception("想定外のEntityが渡されました");
            }
        }

    }

}
