using Carry.CarrySystem.Block.Scripts;
using Projects.Utility.Interfaces;
using Carry.CarrySystem.Gimmick.Scripts;
using Fusion;
using Projects.Utility.Scripts;
using UnityEngine;
using VContainer;
#nullable enable

namespace Carry.CarrySystem.Gimmick.Scripts
{
    public class CannonBallBuilder
    {
        // ToDo: CannonBallControllerをインスタンス化する
        readonly NetworkRunner _runner;
        readonly IPrefabLoader<CannonBallControllerNet> _canonBallControllerLoader;

        [Inject]
        public CannonBallBuilder(NetworkRunner runner)
        {
            _runner = runner;
            // LoaderはDIしなくてもよいのではと思った
            _canonBallControllerLoader = new PrefabLoaderFromAddressable<CannonBallControllerNet>("Prefabs/Gimmick/Cannon/CannonBallNet");
        }
        
        public CannonBallControllerNet Build(CannonBlock.Kind kind, Vector3 position, Quaternion rotation, PlayerRef playerRef)
        {
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