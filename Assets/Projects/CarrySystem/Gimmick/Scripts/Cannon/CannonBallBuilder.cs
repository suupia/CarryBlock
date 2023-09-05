using Projects.Utility.Interfaces;
using Carry.CarrySystem.Gimmick.Scripts;
using Fusion;
using Projects.Utility.Scripts;
using UnityEngine;
using VContainer;

namespace Carry.CarrySystem.Gimmick.Scripts
{
    public class CannonBallBuilder
    {
        // ToDo: CannonBallControllerをインスタンス化する
        readonly NetworkRunner _runner;
        readonly IPrefabLoader<CannonBallControllerNet> _canonBallControllerLoader;

        [Inject]
        public CannonBallBuilder()
        {
            
            // LoaderはDIしなくてもよいのではと思った
            _canonBallControllerLoader = new PrefabLoaderFromAddressable<CannonBallControllerNet>("Prefabs/Gimmick/Cannon/CannonBallNet");
        }
        
        public CannonBallControllerNet Build()
        {
            // Load prefab
            var cannonBallController = _canonBallControllerLoader.Load();
            
            // Spawn prefab
            var position = new Vector3(5, 5, 3);  // ToDo: 適当
            var cannonBallControllerObj = _runner.Spawn(cannonBallController, position, Quaternion.identity, PlayerRef.None,
                (runner, networkObj) =>
                {
                    networkObj.GetComponent<CannonBallControllerNet>().Init();
                });
            
            // Setup Info
            // ...
            
            return cannonBallControllerObj;
        }
    }
}