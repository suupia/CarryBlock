using System.Collections;
using System.Collections.Generic;
using Carry.CarrySystem.Entity.Scripts;
using Carry.CarrySystem.Spawners;
using UnityEngine;
using Fusion;
using Nuts.Utility.Scripts;

#nullable enable

namespace Carry.CarrySystem.Map.Scripts
{
    /// <summary>
    /// TilePresenterを保持しておく
    /// </summary>
    public class TilePresenterContainer
    {
        readonly List<TilePresenter_Net> _tilePresenters = new List<TilePresenter_Net>();
        bool _isInitialized;

        // ToDo: AttachTilePresenter()を呼び出す前にSpawnTilePresenter()を呼び出す必要がある

        public void SpawnTilePresenter(NetworkRunner runner, EntityGridMap map)
        {
            var tilePresenterSpawner = new TilePresenterSpawner(runner);

            for (int i = 0; i < map.GetLength(); i++)
            {
                var girdPos = map.GetVectorFromIndex(i);
                var worldPos = GridConverter.GridPositionToWorldPosition(girdPos);
                var tilePresenter = tilePresenterSpawner.SpawnPrefab(worldPos, Quaternion.identity);
                _tilePresenters.Add(tilePresenter);
            }
        }


        public void AttachTilePresenter(EntityGridMap map)
        {
            for (int i = 0; i < _tilePresenters.Count; i++)
            {
                var tilePresenter = _tilePresenters[i];
                // ReSharper disable once NotAccessedVariable
                var presentData = tilePresenter.PresentDataRef;

                // RegisterTilePresenter()の前なのでSetEntityActiveData()を実行する必要がある
                // Presenterの初期化処理みたいなもの
                var existGround = map.GetSingleEntity<Ground>(i) != null;
                var existRock = map.GetSingleEntity<Rock>(i) != null;

                tilePresenter.SetInitEntityActiveData(map.GetSingleEntity<Ground>(i), existGround);

                tilePresenter.SetInitEntityActiveData(map.GetSingleEntity<Rock>(i), existRock);


                // mapにTilePresenterを登録
                map.RegisterTilePresenter(tilePresenter, i);
            }
        }
    }
}