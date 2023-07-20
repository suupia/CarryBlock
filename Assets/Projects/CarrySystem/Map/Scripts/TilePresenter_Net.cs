using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Carry.CarrySystem.Entity.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using UnityEngine;
using Fusion;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Entity.Scripts;
using Projects.CarrySystem.Block.Scripts;

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
            Debug.Log($"GroundCount: {PresentDataRef.GroundCount}, RockCount: {PresentDataRef.RockCount}, BasicBlockCount: {PresentDataRef.BasicBlockCount}");
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
            groundObject.SetActive(allEntityList.OfType<Ground>().Count() switch
            {
                0 => false,
                1 => true,
                _ => throw new InvalidOperationException($"GroundCount : {PresentDataRef.GroundCount}")
            });
            
            rockObject.SetActive(allEntityList.OfType<Rock>().Count()  switch
            {
                0 or 2 => false,
                1 => true,
                _ => throw new InvalidOperationException($"RockCount : {PresentDataRef.RockCount}")
            });

            doubleRockObject.SetActive(allEntityList.OfType<Ground>().Count()  switch
            {
                0 or 1 => false,
                2 => true,
                _ => throw new InvalidOperationException($"RockCount : {PresentDataRef.RockCount}")
            });
            
            basicBlockObject.SetActive(allEntityList.OfType<BasicBlock>().Count() switch
            {
                0 or 2 => false,
                1 => true,
                _ => throw new InvalidOperationException($"BasicBlockCount : {PresentDataRef.BasicBlockCount}")
            });
            doubleBasicBlockObject.SetActive(allEntityList.OfType<BasicBlock>().Count() switch
            {
                0 or 1 => false,
                2 => true,
                _ => throw new InvalidOperationException($"BasicBlockCount : {PresentDataRef.BasicBlockCount}")
            });
        }

        public void SetEntityActiveData(IEntity entity, int count) 
        {
            switch (entity)
            {
                case Ground _:
                    groundObject.SetActive(count switch
                    {
                        0 => false,
                        1 => true,
                        _ => throw new InvalidOperationException($"GroundCount : {PresentDataRef.GroundCount}")
                    });
                    break;
                case Rock _:
                    rockObject.SetActive(count  switch
                    {
                        0 or 2 => false,
                        1 => true,
                        _ => throw new InvalidOperationException($"RockCount : {PresentDataRef.RockCount}")
                    });

                    doubleRockObject.SetActive(count  switch
                    {
                        0 or 1 => false,
                        2 => true,
                        _ => throw new InvalidOperationException($"RockCount : {PresentDataRef.RockCount}")
                    });
                    break;
                case BasicBlock _:
                    basicBlockObject.SetActive(count switch
                    {
                        0 or 2 => false,
                        1 => true,
                        _ => throw new InvalidOperationException($"BasicBlockCount : {PresentDataRef.BasicBlockCount}")
                    });
                    doubleBasicBlockObject.SetActive(count switch
                    {
                        0 or 1 => false,
                        2 => true,
                        _ => throw new InvalidOperationException($"BasicBlockCount : {PresentDataRef.BasicBlockCount}")
                    });
                    break;
                default:
                    throw new System.Exception("想定外のEntityが渡されました");
            }
        }

    }

}
