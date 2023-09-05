using System;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Fusion;
using Projects.CarrySystem.Block.Info;
using Projects.CarrySystem.Block.Scripts;
using UnityEngine;
using UniRx;
#nullable enable

namespace Projects.CarrySystem.Block
{
    [RequireComponent(typeof(BlockControllerNet))]
    public class BlockMaterialSetter : NetworkBehaviour
    {
        public struct BlockMaterialSetterData : INetworkStruct
        {
            [Networked] public float WhiteRatio { get; set; }
            public PlayerRef PlayerRef { get; set; }
        }

        [Networked] public ref BlockMaterialSetterData Data => ref MakeRef<BlockMaterialSetterData>();

        Material[]? _materials;
        CancellationTokenSource _cts = new CancellationTokenSource();

        public void Init(BlockInfo info)
        {
            _materials = info.BlockViewObj.GetComponentsInChildren<Renderer>().Select(render => render.material).ToArray();
        }

        public override void Render()
        {
            if(_materials == null) return; 
            // Debug.Log($"PlayerRef: {Data.PlayerRef}, Data.WhiteRatio: {Data.WhiteRatio}");
            if (Runner.LocalPlayer.IsValid)
            {
                if (Runner.LocalPlayer == Data.PlayerRef)
                {
                    foreach (var material in _materials)
                    {
                        material.SetFloat("_WhiteRatio", Data.WhiteRatio);
                    }
                }

                
            }

        }

        public void OnDestroy()
        {
            _cts.Cancel();
        }

        public void ChangeWhite(PlayerRef playerRef)
        {
            _cts.Cancel();
            _cts = new CancellationTokenSource();
            var _ = ChangeWhiteAsync( _cts.Token,playerRef);
            
        }
        
        async UniTaskVoid ChangeWhiteAsync(CancellationToken token, PlayerRef playerRef)
        {
            // Debug.Log($"ChangeWhiteAsync"); 
            Data.WhiteRatio = 0.5f;
            if(playerRef != PlayerRef.None) Data.PlayerRef = playerRef;
            await UniTask.Delay(TimeSpan.FromSeconds(0.1f),cancellationToken: token);
            Data.WhiteRatio = 0;  //InvalidOperationException: Error when accessing BlockMaterialSetter.Data. Networked properties can only be accessed when Spawned() has been called.が出る可能性がある
        } 
    }
}