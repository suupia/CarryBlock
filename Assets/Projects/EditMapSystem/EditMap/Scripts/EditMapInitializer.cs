

using System;
using System.Linq;
using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.Spawners;
using Cysharp.Threading.Tasks;
using Fusion;
using UnityEngine;
using VContainer;

namespace Carry.EditMapSystem.EditMap.Scripts
{
    public class EditMapInitializer : MonoBehaviour
    {
        TilePresenterBuilder _tilePresenterBuilder;
        EditMapManager _editMapManager;
        
        [Inject]
        public void Construct(
            TilePresenterBuilder tilePresenterBuilder,
            EditMapManager editMapManager)
        {
            _tilePresenterBuilder = tilePresenterBuilder;
            _editMapManager = editMapManager;

        }

        async void Start()
        {
            var runner = FindObjectOfType<NetworkRunner>();
            if(runner == null) Debug.LogError($"NetworkRunner is not found.");
            await UniTask.WaitUntil(() => runner.SceneManager.IsReady(runner));
            
            _tilePresenterBuilder.Build(_editMapManager.GetMap());

            var mapKeyContainer = FindObjectOfType<MapKeyContainer>();
            _editMapManager.SetMapKey(mapKeyContainer.MapKey);
        }
    }
}