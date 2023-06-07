using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Nuts.BattleSystem.Enemy.Scripts.Boss_Previous.Attack.TargetBufferAttack
{

    public class TargetBufferAttack:  AttackWrapper, ITargetAttack
    {
        public struct Context
        {
            [NotNull] public Transform Transform;
            [NotNull] public ISet<Transform> TargetBuffer;
        }

        private Context _context;
        private ITargetAttack TargetAttack => _attack as ITargetAttack;

        public Transform Target
        {
            get => TargetAttack.Target;
            set => TargetAttack.Target = value;
        }


        protected TargetBufferAttack(Context context, ITargetAttack targetAttack)
        {
            _context = context;
            _attack = targetAttack;
        }
        
        public void Attack()
        {
            Debug.Log($"TargetBufferAttack() TargetBuffer.Count={_context.TargetBuffer.Count}");
            Debug.Log($"{ToString()}");
            Debug.Log($"Transform ={_context.Transform}");
            if (_context.TargetBuffer.Count == 0) return;

            TargetAttack.Target = GetTargetTransform(_context);
            TargetAttack.Attack();
        }

        protected virtual Transform GetTargetTransform(Context context)
        {
            throw new NotImplementedException();
        }
    }
}