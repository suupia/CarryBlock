using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.CarriableBlock.Interfaces;
using Carry.CarrySystem.Cart.Scripts;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Player.Info;
using Carry.CarrySystem.Player.Interfaces;
using Carry.CarrySystem.Spawners.Interfaces;
using Carry.CarrySystem.Spawners.Scripts;
using Fusion;
using DG.Tweening;
using UnityEngine;
#nullable enable

namespace Carry.CarrySystem.Player.Scripts
{
    public class PassBlockMoveExecutorNet : NetworkBehaviour
    {
        public struct PassBlockMoveData : INetworkStruct
        {
            public bool IsPassing;
        }
        [Networked] ref PassBlockMoveData Data => ref MakeRef<PassBlockMoveData>();
        public void Init(ICharacter character)
        {
             character.SetPassBlockMoveExecutor(this);
        }

        public override void Render()
        {
            
        }

        public void PassBlockMove(ICarriableBlock block, Transform playerTransform, Transform targetTransform)
        {
            Debug.Log("PassBlockMove Start");
            var spawner = new EntityPresenterSpawner(Runner);
            var entityPresenter = spawner.SpawnPrefabNet(playerTransform.position, Quaternion.identity);
            entityPresenter.SetEntityActiveData(block,count:1);
            Debug.Log("PassBlockMove Spawned");
            entityPresenter.Object.transform.DOJump(targetTransform.position, jumpPower: 3f, numJumps: 1, duration: 1f)
                .OnComplete(() =>
                {
                    entityPresenter.DestroyPresenter();
                    Debug.Log("PassBlockMove Complete");
                });
        }

    }
}