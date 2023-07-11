using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public class TilePresenterAttacher
    { 
        IEnumerable <TilePresenter_Net> _tilePresenters = new List<TilePresenter_Net>();

        // AttachTilePresenter()を呼び出す前にSetTilePresenters()を呼び出す必要がある
        public void SetTilePresenters(IEnumerable<TilePresenter_Net> tilePresenters)
        {
            _tilePresenters = tilePresenters;
        }


        public void AttachTilePresenter(EntityGridMap map)
        {
            for (int i = 0; i < _tilePresenters.Count(); i++)
            {
                var tilePresenter = _tilePresenters.ElementAt(i);
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