using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

namespace Main
{
    public abstract class PoolableObject : NetworkBehaviour
    {
        protected abstract void OnInactive();
        void OnDisable()
        {
            OnInactive();
        }
    
    }

}
