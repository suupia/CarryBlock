

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
        TilePresenterAttacher _tilePresenterAttacher;
        EditMapManager _editMapManager;
        
        [Inject]
        public void Construct(
            TilePresenterBuilder tilePresenterBuilder,
            TilePresenterAttacher tilePresenterAttacher,
            EditMapManager editMapManager)
        {
            _tilePresenterBuilder = tilePresenterBuilder;
            _tilePresenterAttacher = tilePresenterAttacher;
            _editMapManager = editMapManager;

        }

        async void Start()
        {
            var runner = FindObjectOfType<NetworkRunner>();
            if(runner == null) Debug.LogError($"NetworkRunner is not found.");
            await UniTask.WaitUntil(() => runner.SceneManager.IsReady(runner));
            
            var tilePresenters = _tilePresenterBuilder.Build(_editMapManager.GetMap());
            _tilePresenterAttacher.SetTilePresenters(tilePresenters);
            _editMapManager.RegisterTilePresenterContainer(_tilePresenterAttacher);
        }
    }
}