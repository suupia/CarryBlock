using UnityEngine;

namespace Main
{

    public class ToFurthestAttack: TargetBufferAttack
    {
        public ToFurthestAttack(Context context, ITargetAttack targetAttack) : base(context, targetAttack)
        {
        }
        
        protected override Transform GetTargetTransform(Context context)
        {
            Transform maxTransform = null;
            float maxDistance = float.NegativeInfinity;
            foreach (var transform in context.TargetBuffer)
            {
                var distance = Vector3.Distance(transform.position, context.Transform.position);
                if (distance > maxDistance)
                {
                    maxTransform = transform;
                    maxDistance = distance;
                }
            }
            
            return maxTransform;
        }
    }
}