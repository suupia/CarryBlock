using Fusion;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Main;

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
        readonly int _quotaAmount = 15;
        public int getAmount { get; private set; }

        public bool IsQuotaReached => getAmount >= _quotaAmount;

        public void AddResource(int amount)
        {
            getAmount += amount;
        }

        public bool IsSuccess()
        {
            return getAmount >= _quotaAmount;
        }
    }
}