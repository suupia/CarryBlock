using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Carry.CarrySystem.Entity.Scripts;
using Carry.CarrySystem.Spawners;
using UnityEngine;
using Fusion;
using Nuts.Utility.Scripts;
using Projects.CarrySystem.Block.Scripts;

#nullable enable

namespace Carry.CarrySystem.Map.Scripts
{
    /// <summary>
    /// TilePresentersをドメインのEntityGridMapに紐づける
    /// </summary>
    public class TilePresenterAttacher
    {
        // TilePresenterBuilderのみに依存するようにする
        // なんならメソッドにしてメンバにした方がよいまである
        public void AttachTilePresenter(IEnumerable<TilePresenter_Net> tilePresenters , EntityGridMap map)
        {
            for (int i = 0; i < tilePresenters.Count(); i++)
            {
                var tilePresenter = tilePresenters.ElementAt(i);
                // ReSharper disable once NotAccessedVariable
                var presentData = tilePresenter.PresentDataRef;

                // RegisterTilePresenter()の前なのでSetEntityActiveData()を実行する必要がある
                // Presenterの初期化処理みたいなもの
                var existGround = map.GetSingleEntity<Ground>(i) != null;
                var existRock = map.GetSingleEntity<Rock>(i) != null;
                var existBasicBlock = map.GetSingleEntity<BasicBlock>(i) != null;
                
                if(existRock) Debug.Log($"existGround: {existGround}, existRock: {existRock}, existBasicBlock: {existBasicBlock}");

                if(map.GetSingleEntity<Ground>(i) is {} ground) tilePresenter.SetInitEntityActiveData(ground, existGround);

                if(map.GetSingleEntity<Rock>(i) is {} rock) tilePresenter.SetInitEntityActiveData(rock, existRock);
                
                if(map.GetSingleEntity<BasicBlock>(i) is {} basicBlock) tilePresenter.SetInitEntityActiveData(basicBlock, existBasicBlock);

                // mapにTilePresenterを登録
                map.RegisterTilePresenter(tilePresenter, i);
            }
        }
    }
}