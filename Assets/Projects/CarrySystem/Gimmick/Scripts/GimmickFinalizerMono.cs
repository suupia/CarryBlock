using System;
using Carry.CarrySystem.Gimmick.Interfaces;
using Carry.CarrySystem.Map.Interfaces;
using UnityEngine;
using VContainer;

namespace Projects.CarrySystem.Gimmick.Scripts
{
    public class GimmickFinalizerMono : MonoBehaviour
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
                var gimmicks = _mapGetter.GetMap().GetSingleEntityList<IGimmick>(i);

                foreach (var gimmick in gimmicks)
                {
                    gimmick.Dispose();
                }
                
            }
        }
    }
}