using Carry.CarrySystem.Block.Scripts;
using Carry.CarrySystem.Gimmick.Scripts;
using Carry.Utility.Interfaces;
using Carry.Utility.Scripts;
using Fusion;
using UnityEngine;
using VContainer;
#nullable enable

namespace Projects.CarrySystem.Gimmick.Scripts
{
    public class SpikeBodyBuilder
    {
        readonly NetworkRunner _runner;
        readonly IPrefabLoader<SpikeBodyControllerNet> _canonBallControllerLoader;

        [Inject]
        public SpikeBodyBuilder(NetworkRunner runner)
        {
            _runner = runner;
            // I decided to not DI Loader
            _canonBallControllerLoader = new PrefabLoaderFromAddressable<SpikeBodyControllerNet>("Prefabs/Gimmick/Spike/SpikeBodyNet");
        }
        
        public SpikeBodyControllerNet Build(SpikeGimmick.Kind kind, Vector3 position, Quaternion rotation, PlayerRef playerRef)
        {
            // Load prefab
            var cannonBallController = _canonBallControllerLoader.Load();
            
            // Spawn prefab
            var cannonBallControllerObj = _runner.Spawn(cannonBallController, position, rotation, playerRef,
                (runner, networkObj) =>
                {
                    networkObj.GetComponent<SpikeBodyControllerNet>().Init(kind);
                });
            
            // Setup Info
            // ...
            
            return cannonBallControllerObj;
        }
    }
}