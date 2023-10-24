using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.CarriableBlock.Interfaces;
using Carry.CarrySystem.Cart.Scripts;
using Carry.CarrySystem.Player.Info;
using Carry.CarrySystem.Player.Interfaces;
using Fusion;
using DG.Tweening;
using UnityEngine;
#nullable enable

namespace Carry.CarrySystem.Player.Scripts
{
    public class PassBlockMoveExecutor : NetworkBehaviour
    {
        public struct PassBlockMoveData : INetworkStruct
        {
            public bool IsPassing;
            public Vector3 PassTargetPosition;
        }
        [Networked] ref PassBlockMoveData Data => ref MakeRef<PassBlockMoveData>();
        public void Init(ICharacter character)
        {
            character.SetPassBlockMoveExecutor(this);
        }

        public override void Render()
        {
            base.Render();
        }

        public void PassBlockMove(ICarriableBlock block, ICharacter character)
        {
            this.transform.DOJump(Data.PassTargetPosition, jumpPower: 3f, numJumps: 1, duration: 1f);
        }

    }
}