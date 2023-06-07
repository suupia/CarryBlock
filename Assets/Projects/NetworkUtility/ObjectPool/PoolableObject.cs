using Fusion;

namespace  Nuts.NetworkUtility.ObjectPool.Scripts
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