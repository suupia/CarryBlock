using Fusion;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Main;
using NetworkUtility.ObjectPool;

namespace Enemy
{
    [RequireComponent(typeof(NetworkObject))]
    public class NetworkResourceController : PoolableObject
    {
        public enum State
        {
            Idle,
            Reserved,
            Held
        }

        Transform _holder;
        bool _isInitialized;
        ResourceAggregator _resourceAggregator;

        public State state { get; private set; } = State.Idle;
        public bool canAccess => state == State.Idle;


        public override void Spawned() // 必要であればInit()にして外部から呼び出せるようにする
        {
            _isInitialized = true;
            _resourceAggregator = FindObjectOfType<LifetimeScope>().Container.Resolve<ResourceAggregator>();
        }

        public override void Render()
        {
            switch (state)
            {
                case State.Idle:
                    break;
                case State.Reserved:
                    break;
                case State.Held:
                    transform.position = _holder.position;
                    break;
            }
        }

        public void OnReserved()
        {
            state = State.Reserved;
        }

        public void OnHeld(Transform holder)
        {
            state = State.Held;
            _holder = holder;
        }


        protected override void OnInactive()
        {
            if (!_isInitialized) return;
            if (state == State.Held) _resourceAggregator.AddResource(1);
            state = State.Idle;
        }
    }

    public class ResourceAggregator
    {
        public readonly int QuotaAmount = 15;
        public int GetAmount { get; private set; }

        public bool IsQuotaReached => GetAmount >= QuotaAmount;

        public void AddResource(int amount)
        {
            GetAmount += amount;
        }

        public bool IsSuccess()
        {
            return GetAmount >= QuotaAmount;
        }
    }
}