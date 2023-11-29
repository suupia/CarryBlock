using System;
using Carry.CarrySystem.Gimmick.Interfaces;
using Carry.CarrySystem.Map.Interfaces;
using UnityEngine;
using VContainer;

namespace Projects.CarrySystem.Gimmick.Scripts
{
    public class GimmickDisposerMono : MonoBehaviour
    {
        IMapGetter _mapGetter;
        
        [Inject]
        public void Construct(IMapGetter mapGetter)
        {
            _mapGetter = mapGetter;
        }
        
        void OnDestroy()
        {
            Debug.Log($"OnDestroy GimmickFinalizerMono");
            EndGimmicks();
        }

        void EndGimmicks()
        {
            for (int i = 0; i < _mapGetter.GetMap().Length; i++)
            {
                var disposables = _mapGetter.GetMap().GetSingleTypeList<IDisposable>(i);

                foreach (var gimmick in disposables)
                {
                    gimmick.Dispose();
                }
                
            }
        }
    }
}