using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.Player.Interfaces;
using Carry.CarrySystem.Player.Scripts;
using Fusion;
using UnityEngine;
#nullable enable

namespace Projects.CarrySystem.Enemy
{
    [RequireComponent(typeof(EnemyControllerNet))]
    public class EnemyAnimatorPresenterNet: NetworkBehaviour
    {
        public enum AnimationState
        {
            Threat,
            Chase,
        }
        public struct PresentData : INetworkStruct
        {
            public AnimationState AnimationState { get; set; }
        }
        [Networked] public ref PresentData PresentDataRef => ref MakeRef<PresentData>();

        // 差分を判定して、アニメーションを発火させる
        int _threatCount;
        int _chaseCount;

        Animator? _animator;
        
        // public void Init(ICharacter character)
        // {
        //     Debug.Log($"PlayerAnimatorPresenterNet Init");
        //     character.SetPlayerAnimatorPresenter(this);
        // }
        
        public  override void Spawned()
        {
            // ここで取得するのは怠慢かも
            // PlayerはSpawn時にAnimatorを取得しているので、それに合わせたほうがよいかも
            Debug.Log($"EnemyAnimatorPresenterNet Spawned");
            
            var animator = GetComponentInChildren<Animator>();
            SetAnimator(animator);
        }

        public void SetAnimator(Animator animator)
        {
            _animator = animator;
        }

        public override void Render()
        {
            if (_animator == null)
            {
                Debug.LogError($"_animator is null");
                return;
            }
            switch (PresentDataRef.AnimationState)
            {
                case AnimationState.Threat:
                    _animator.SetBool("isThreat", true);
                    _animator.SetBool("isChase", false);
                    break;
                case  AnimationState.Chase:
                    _animator.SetBool("InWalk", false);
                    _animator.SetBool("InDash", true);
                    break;
            }
        }
        
        public void Threat()
        {
            Debug.Log($"Threat");
            PresentDataRef.AnimationState = AnimationState.Threat;
        }
        
        public void Chase()
        {
            Debug.Log($"Chase");
            PresentDataRef.AnimationState = AnimationState.Chase;
        }
        
    }
}