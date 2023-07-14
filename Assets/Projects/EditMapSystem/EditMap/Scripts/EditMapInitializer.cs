

using System;
using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.Spawners;
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

        void Start()
        {
            var tilePresenters = _tilePresenterBuilder.Build(_entityGridMapSwitcher.GetMap());
            _tilePresenterAttacher.SetTilePresenters(tilePresenters);
            _entityGridMapSwitcher.RegisterTilePresenterContainer(_tilePresenterAttacher);
        }
    }
}