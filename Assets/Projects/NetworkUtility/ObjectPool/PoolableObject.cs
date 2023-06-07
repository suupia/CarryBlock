using Fusion;

namespace  NetworkUtility.ObjectPool
{
    public abstract class PoolableObject : NetworkBehaviour
    {
        void OnDisable()
        {
            OnInactive();
        }

        protected abstract void OnInactive();
    }
}