using System;
using System.Collections;
using System.Collections.Generic;
using Carry.CarrySystem.Map.Scripts;
using Cysharp.Threading.Tasks;
using Fusion;
using UnityEngine;
using VContainer;

namespace Carry.CarrySystem.SearchRoute.Scripts
{
    public class SearchRouteInitializer : MonoBehaviour
    {
        SearchRouteUpdater _searchRouteUpdater;
        [Inject]
        public void Construct(SearchRouteUpdater searchRouteUpdater)
        {
            _searchRouteUpdater = searchRouteUpdater;
        }

       async  void Start()
        {
            var runner = FindObjectOfType<NetworkRunner>();
            if(runner == null) Debug.LogError($"NetworkRunner is not found.");
            await UniTask.WaitUntil(() => runner.SceneManager.IsReady(runner));
            
            _searchRouteUpdater.InitUpdateMap(); 

        }
    }

}
