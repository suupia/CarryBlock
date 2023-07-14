

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
        EntityGridMapSwitcher _entityGridMapSwitcher;
        
        [Inject]
        public void Construct(
            TilePresenterBuilder tilePresenterBuilder,
            TilePresenterAttacher tilePresenterAttacher,
            EntityGridMapSwitcher entityGridMapSwitcher)
        {
            _tilePresenterBuilder = tilePresenterBuilder;
            _tilePresenterAttacher = tilePresenterAttacher;
            _entityGridMapSwitcher = entityGridMapSwitcher;

        }

        async void Start()
        {
            var runner = FindObjectOfType<NetworkRunner>();
            await UniTask.WaitUntil(() => runner.SceneManager.IsReady(runner));
            
            var tilePresenters = _tilePresenterBuilder.Build(_entityGridMapSwitcher.GetMap());
            _tilePresenterAttacher.SetTilePresenters(tilePresenters);
            _entityGridMapSwitcher.RegisterTilePresenterContainer(_tilePresenterAttacher);
        }
    }
}