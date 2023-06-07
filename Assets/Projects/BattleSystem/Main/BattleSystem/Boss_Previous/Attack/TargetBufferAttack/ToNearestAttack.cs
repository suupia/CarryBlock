using UnityEngine;

namespace Nuts.Projects.BattleSystem.Main.BattleSystem.Boss_Previous.Attack.TargetBufferAttack
{

    /// <summary>
    /// targetBufferを共有して、一番transformに近いものをtargetとする
    /// 使用される攻撃はtargetAttackで指定する
    /// </summary>
    public class ToNearestAttack : TargetBufferAttack
    {

        public ToNearestAttack(Context context, ITargetAttack targetAttack): base(context, targetAttack)
        {
        }

        protected override Transform GetTargetTransform(Context context)
        {
            Transform minTransform = null;
            float minDistance = float.PositiveInfinity;
            foreach (var transform in context.TargetBuffer)
            {
                var distance = Vector3.Distance(transform.position, context.Transform.position);
                if (distance < minDistance)
                {
                    minTransform = transform;
                    minDistance = distance;
                }
            }
            
            return minTransform;
        }
    }

}