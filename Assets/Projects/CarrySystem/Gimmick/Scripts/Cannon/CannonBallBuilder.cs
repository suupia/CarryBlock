using Carry.CarrySystem.Block.Scripts;
using Carry.Utility.Interfaces;
using Carry.CarrySystem.Gimmick.Scripts;
using Fusion;
using Carry.Utility.Scripts;
using UnityEngine;
using VContainer;
#nullable enable

namespace Carry.CarrySystem.Gimmick.Scripts
{
    public class CannonBallBuilder
    {
        readonly IPrefabLoader<CannonBallControllerNet> _canonBallControllerLoader;
        NetworkRunner? _runner;

        [Inject]
        public CannonBallBuilder()
        {
            // LoaderはDIしなくてもよいのではと思った
            _canonBallControllerLoader = new PrefabLoaderFromAddressable<CannonBallControllerNet>("Prefabs/Gimmick/Cannon/CannonBallNet");
        }
        
        public CannonBallControllerNet Build(CannonBlock.Kind kind, Vector3 position, Quaternion rotation, PlayerRef playerRef)
        {
            _runner ??= GameObject.FindWithTag("NetworkRunner").GetComponent<NetworkRunner>();
            if(_runner == null) Debug.LogError("NetworkRunner is not found");
            
            // Load prefab
            var cannonBallController = _canonBallControllerLoader.Load();
            
            // Spawn prefab
            var cannonBallControllerObj = _runner.Spawn(cannonBallController, position, rotation, playerRef,
                (runner, networkObj) =>
                {
                    networkObj.GetComponent<CannonBallControllerNet>().Init(kind);
                });
            
            // Setup Info
            // ...
            
            return cannonBallControllerObj;
        }
    }
}