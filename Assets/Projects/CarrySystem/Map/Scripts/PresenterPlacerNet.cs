using System.Linq;
using Carry.CarrySystem.CarriableBlock.Scripts;
using Carry.CarrySystem.Entity.Scripts;
using Carry.CarrySystem.Map.Interfaces;
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
        IPresenterPlacer _groundPresenterPlacer;

        // public struct PresenterPlacerData : INetworkStruct
        // {
        //    public bool[] wallArray;
        //    public bool[] groundArray;
        // }

        [Inject]
        public void Construct(
            CarryBlockPresenterPlacer blockPresenterPlacer,
            RandomWallPresenterPlacer wallPresenterPlacer,
            RegularGroundPresenterPlacer groundPresenterPlacer
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
            _blockPresenterPlacer.Place(map);
            
            var wallArray = new bool[map.Length];
            var groundArray = new bool[map.Length];
            for(int i = 0; i< map.Length; i++){
                // wallArray[i] = map.GetSingleEntityList<Ground>(i).Any();  // todo: Wallの判定方法をLobbyWallPresenterPlacerから持ってくる
                groundArray[i] = map.GetSingleEntityList<Ground>(i).Any();
            }
            // var presenterPlacerData = new PresenterPlacerData{
            //     wallArray = wallArray,
            //     groundArray = groundArray
            // };
           // RPC_PlacePresenters();
        }
        
        // [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
        // public void RPC_PlacePresenters(){
        //     Debug.Log($"RPC_PresenterPlace");
        //     var presenterPlacerData = new PresenterPlacerData{
        //         wallArray = new bool[0],
        //         groundArray = new bool[0]
        //     };
        //     var wallArray = presenterPlacerData.wallArray;
        //     var groundArray = presenterPlacerData.groundArray;
        //     // _wallPresenterPlacer.Place(wallArray);
        //     // _groundPresenterPlacer.Place(groundArray);
        // }
        
        // todo: test , so delete this method
        [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority)]
        public void RPC_PresenterPlace(RpcInfo info = default){
            Debug.Log($"RPC_PresenterPlace");
        }

        
    }
}