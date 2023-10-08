﻿using System;
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
        IPresenterPlacer _wallPresenterPlacer;
        LocalGroundPresenterPlacer _groundPresenterPlacer;

        readonly int _wallHorizontalNum = 3;
        readonly int _wallVerticalNum = 2;


        public struct PresenterPlacerData : INetworkStruct
        {
            [Networked, Capacity(375)] public NetworkArray<NetworkBool> WallArray => default;
            [Networked, Capacity(375)] public NetworkArray<NetworkBool> GroundArray => default;
        }

        [Inject]
        public void Construct(
            CarryBlockPresenterPlacer blockPresenterPlacer,
            RandomWallPresenterPlacer wallPresenterPlacer,
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
            var presenterPlacerData = new PresenterPlacerData();

            // block
            _blockPresenterPlacer.Place(map);
            
            // ground
            for(int i = 0 ; i < map.Length; i++){
                presenterPlacerData.GroundArray.Set(i, (NetworkBool)map.GetSingleEntityList<Ground>(i).Any()); // 他のやり方ありそうだけど、、
            }
            
            // wall
            // // todo: WallもEntityで管理するのがよさそう
            // var expandedMap = new SquareGridMap(map.Width + 2 * _wallHorizontalNum, map.Height + 2 * _wallVerticalNum);
            // var wallArray = new bool[expandedMap.Length];
            // for(int i = 0; i< wallArray.Length; i++){
            //     // wallArray[i] = map.GetSingleEntityList<Ground>(i).Any();  // todo: Wallの判定方法をLobbyWallPresenterPlacerから持ってくる
            //     groundArray[i] = map.GetSingleEntityList<Ground>(i).Any();
            // }


            RPC_PlacePresenters(presenterPlacerData, map.Width, map.Height);
        }
        
        // bool IsNotPlacingBlock(EntityGridMap map, Vector2Int gridPos)
        // {
        //     // 右端においては、ブロックがない場所には置かない
        //     if (gridPos.x >= map.Width)
        //     {
        //         if (map.GetSingleEntityList<IBlock>(new Vector2Int(gridPos.x, map.Width - 1)).Count == 0) return true;
        //     }
        //     
        //     // 左端においては、真ん中から3マス分の範囲には置かない
        //     if (gridPos.x < 0)
        //     {
        //         if (map.Height / 2 - 1 <= gridPos.y && gridPos.y <= map.Height / 2 + 1) return true;
        //     }
        //
        //     return false;
        // }
        
        
        [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
        public void RPC_PlacePresenters(PresenterPlacerData presenterPlacerData, Int32 width, Int32 height){
            Debug.Log($"RPC_PresenterPlace");
            var wallArray = presenterPlacerData.WallArray;
            var groundArray =  presenterPlacerData.GroundArray;
            // _wallPresenterPlacer.Place(wallArray);
            _groundPresenterPlacer.Place(groundArray,width, height); // todo: bool[]を渡せるようにする
        }
        
        // // todo: test , so delete this method
        // [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority)]
        // public void RPC_PresenterPlace(RpcInfo info = default){
        //     Debug.Log($"RPC_PresenterPlace");
        // }

        
    }
}