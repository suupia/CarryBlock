using Fusion;

namespace Main
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