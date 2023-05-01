using Decoration;
using Fusion;
using Main;
using UnityEngine;

namespace Boss
{
    public class BossController : NetworkBehaviour
    {
        [SerializeField] private GameObject modelObject;
        
        [Networked] ref BossDecorationDetector.Data DecorationDataRef => ref MakeRef<BossDecorationDetector.Data>();
        private BossDecorationDetector _decorationDetector;

        private IMove _move;

        public override void Spawned()
        {
            _move = new WanderingMove(new RegularMove()
            {
                transform = modelObject.transform,
                rd = GetComponent<Rigidbody>(),
                maxVelocity = 1,
                maxAngularVelocity = 10,
            });
        }

        public override void FixedUpdateNetwork()
        {
            if(!HasStateAuthority) return;
            
            _move.Move();
        }
    }
}