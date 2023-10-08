using System;
using System.Linq;
using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.CarriableBlock.Scripts;
using Carry.CarrySystem.Entity.Scripts;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Spawners;
using Fusion;
using UnityEngine;
using VContainer;

namespace Carry.CarrySystem.Map.Scripts
{
    public class PresenterPlacerNet : NetworkBehaviour
    {
        [Inject] NetworkRunner _runner;
        IPresenterPlacer _blockPresenterPlacer;
        LocalWallPresenterPlacer _wallPresenterPlacer;
        LocalGroundPresenterPlacer _groundPresenterPlacer;

        readonly int _wallHorizontalNum = 3;
        readonly int _wallVerticalNum = 2;


        public struct PresenterPlacerData : INetworkStruct
        {
            [Networked, Capacity(442)] public NetworkArray<NetworkBool> WallArray => default;
            [Networked, Capacity(375)] public NetworkArray<NetworkBool> GroundArray => default;
        }

        [Inject]
        public void Construct(
            CarryBlockPresenterPlacer blockPresenterPlacer,
            LocalWallPresenterPlacer wallPresenterPlacer,
            LocalGroundPresenterPlacer groundPresenterPlacer
            )
        {
            _blockPresenterPlacer = blockPresenterPlacer;
            _wallPresenterPlacer = wallPresenterPlacer;
            _groundPresenterPlacer = groundPresenterPlacer;
        }
        

        /// <summary>
        /// This method should be called from host only.
        /// </summary>
        /// <param name="map"></param>
        public void Place(EntityGridMap map)
        {
            Debug.Log($"PresenterPlacerNet.Place");
            var presenterPlacerData = new PresenterPlacerData();

            // block
            _blockPresenterPlacer.Place(map);
            
            // ground
            for(int i = 0 ; i < map.Length; i++){
                presenterPlacerData.GroundArray.Set(i, (NetworkBool)map.GetSingleEntityList<Ground>(i).Any());
            }
            
            // wall
            // todo: WallもEntityで管理するのもよさそう
            var expandedMap = new SquareGridMap(map.Width + 2 * _wallHorizontalNum, map.Height + 2 * _wallVerticalNum);
            var wallArray = new bool[expandedMap.Length];
            for(int i = 0; i< wallArray.Length; i++){
                var gridPos = expandedMap.ToVector(i);
                var convertedGridPos = new Vector2Int(gridPos.x - _wallHorizontalNum, gridPos.y - _wallVerticalNum);
                if (map.IsInDataRangeArea(convertedGridPos))
                {
                    presenterPlacerData.WallArray.Set(i, false);
                    continue;
                }
                if (IsNotPlacingBlock(map, convertedGridPos))
                {
                    presenterPlacerData.WallArray.Set(i, false);
                    continue;
                }
                Debug.Log($"true!! gridPos: {gridPos}, convertedGridPos: {convertedGridPos}, i: {i}");
                presenterPlacerData.WallArray.Set(i, true);
            }


            RPC_PlacePresenters(presenterPlacerData, map.Width, map.Height);

        }
        
        bool IsNotPlacingBlock(EntityGridMap map, Vector2Int gridPos)
        {
            // 右端においては、ブロックがない場所には置かない
            if (gridPos.x >= map.Width)
            {
                if (map.GetSingleEntityList<IBlock>(new Vector2Int(gridPos.x, map.Width - 1)).Count == 0) return true;
            }
            
            // 左端においては、真ん中から3マス分の範囲には置かない
            if (gridPos.x < 0)
            {
                if (map.Height / 2 - 1 <= gridPos.y && gridPos.y <= map.Height / 2 + 1) return true;
            }

            return false;
        }
        
        
        [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
        public void RPC_PlacePresenters(PresenterPlacerData presenterPlacerData, Int32 width, Int32 height){
            Debug.Log($"RPC_PresenterPlace");
            var wallArray = presenterPlacerData.WallArray;
            var groundArray =  presenterPlacerData.GroundArray;
            _wallPresenterPlacer.Place(wallArray,width,height);
            _groundPresenterPlacer.Place(groundArray,width, height); // todo: bool[]を渡せるようにする
        }

    }
}