using System;
using System.Linq;
using System.Threading;
using Carry.CarrySystem.Block.Info;
using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.Block.Scripts;
using Cysharp.Threading.Tasks;
using Fusion;
using UnityEngine;

#nullable enable

namespace Projects.CarrySystem.Block.Scripts
{
    [RequireComponent(typeof(BlockControllerLocal))]
    public class BlockMaterialSetterLocal : MonoBehaviour , IBlockMaterialSetter
    {
        struct BlockMaterialSetterData
        {
            public float WhiteRatio { get; set; }
        }
         BlockMaterialSetterData _data;

        Material[]? _materials;
        CancellationTokenSource _cts = new CancellationTokenSource();

        public void Init(BlockInfo info)
        {
            _materials = info.BlockViewObj.GetComponentsInChildren<Renderer>().Select(render => render.material).ToArray();
        }

        public  void Update()
        {
            if(_materials == null) return; 
            // Debug.Log($"PlayerRef: {Data.PlayerRef}, Data.WhiteRatio: {Data.WhiteRatio}");
            foreach (var material in _materials)
            {
                material.SetFloat("_WhiteRatio", _data.WhiteRatio);
            }

        }

        void OnDestroy()
        {
            _cts.Cancel();
        }

        public void ChangeWhite(PlayerRef _)
        {
            _cts.Cancel();
            _cts = new CancellationTokenSource();
            ChangeWhiteAsync( _cts.Token).Forget();
            
        }
        
        async UniTaskVoid ChangeWhiteAsync(CancellationToken token)
        {
            // Debug.Log($"ChangeWhiteAsync"); 
            _data.WhiteRatio = 0.5f;
            await UniTask.Delay(TimeSpan.FromSeconds(0.1f),cancellationToken: token);
            _data.WhiteRatio = 0;  //InvalidOperationException: Error when accessing BlockMaterialSetter.Data. Networked properties can only be accessed when Spawned() has been called.が出る可能性がある
        } 
    }
}