using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Carry.CarrySystem.Map.Scripts
{
    /// <summary>
    /// シーン上にマップを生成するクラス
    /// ホストで呼ばれることを想定している
    /// </summary>
    public class MapGenerator
    {
        readonly EntityGridMap _map;
        public  MapGenerator()
        {

        }

        // public void SetupMap(ICameraPos iCameraPosInstance, Peripheral peripheral, int stageIndex)
        // {
        //     _cameraPosInstance = iCameraPosInstance;
        //     StageIndex = stageIndex;
        //
        //     //子オブジェクトをすべて削除
        //     foreach (Transform child in _hexagonTilesParent.transform)
        //     {
        //         Destroy(child.gameObject);
        //     }
        //
        //     foreach (Transform child in _visualizationParent.transform)
        //     {
        //         Destroy(child.gameObject);
        //     }
        //
        //     //マップを生成するたびにする処理
        //     _map = new EntityGridMap(_width, _height);
        //     var hexagonMapData = new HexagonMapDataMGR();
        //
        //
        //     //stageIndexからmapDataIndexを計算する
        //     var mapDataIndexToLoad = _mapDataIndexesToLoad[stageIndex];
        //
        //     if (_isLoadMap)
        //     {
        //         hexagonMapData.LoadMap(Map, mapDataIndexToLoad, _edgeWidth, _edgeHeight);
        //     }
        //     else
        //     {
        //         hexagonMapData.DefaultMap(Map, _edgeWidth, _edgeHeight);
        //     }
        //
        //
        //     _visualization = new HexagonTileVisualization(Map, this, _visualizationParent);
        //     
        //
        //
        //     //マス目にスプライトを置くためのゲームオブジェクトを生成する
        //     _poolContainer.InstantiatePoolObjects(_hexagonTilesParent);
        //     var neighborhoodPoss = Peripheral.GetPeripheralPositions(_cameraPosInstance.GetCameraAnchorGridPos(), Map);
        //     _poolContainer.InitializePoolGridPositions(neighborhoodPoss);
        //     _poolContainer.UpdatePoolObjects(neighborhoodPoss);
        //
        //
        //     
        //     //Pen3の位置を設定する
        //     _character.Initialization(this);
        //
        //
        //     UpdateAppearanceOfMap(_cameraPosInstance);
        // }
    }
}