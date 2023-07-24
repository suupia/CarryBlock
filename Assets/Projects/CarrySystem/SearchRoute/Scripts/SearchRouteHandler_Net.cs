﻿using System;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using Fusion;
using Projects.CarrySystem.Block.Interfaces;
using UnityEngine;
using VContainer;
using VContainer.Unity;

#nullable enable


namespace Carry.CarrySystem.SearchRoute.Scripts
{
    /// <summary>
    /// テスト用にSearchShortestRouteを実行するクラス
    /// </summary>
    public class SearchRouteHandler_Net : NetworkBehaviour
    {
        [SerializeField] Vector2Int startPos;
        [SerializeField] Vector2Int endPos;
        WaveletSearchBuilder _waveletSearchBuilder = null!;
        IMapUpdater _entityGridMapSwitcher = null!;

        [Inject]
        public void Construct(IMapUpdater entityGridMapSwitcher,
            WaveletSearchBuilder waveletSearchBuilder)
        {
            _waveletSearchBuilder = waveletSearchBuilder;
            _entityGridMapSwitcher = entityGridMapSwitcher;
        }


        void Update()
        {
            if (Runner == null) return;
            if (Runner.IsServer && Input.GetKeyDown(KeyCode.S))
            {
                Search();
            }
        }

        void Search()
        {
            var map = _entityGridMapSwitcher.GetMap();
            Func<int, int, bool> isWall = (x, y) => map.GetSingleEntityList<IBlock>(new Vector2Int(x, y)).Count > 0;

            var waveletSearchExecutor = _waveletSearchBuilder.Build(_entityGridMapSwitcher.GetMap());
            waveletSearchExecutor.SearchAccessibleArea(startPos, endPos, isWall);
        }
    }
}