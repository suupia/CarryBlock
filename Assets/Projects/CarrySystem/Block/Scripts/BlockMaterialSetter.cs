using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using Fusion;
using Projects.CarrySystem.Block.Info;
using Projects.CarrySystem.Block.Scripts;
using UnityEngine;
#nullable enable

namespace Projects.CarrySystem.Block
{
    [RequireComponent(typeof(BlockControllerNet))]
    public class BlockMaterialSetter : NetworkBehaviour
    {
        public struct BlockMaterialSetterData : INetworkStruct
        {
            [Networked] public float WhiteRatio { get; set; }
        }

        [Networked] public ref BlockMaterialSetterData Data => ref MakeRef<BlockMaterialSetterData>();

        Material[]? _materials;

        public void Init(BlockInfo info)
        {
            _materials = info.blockViewObj.GetComponentsInChildren<Renderer>().Select(render => render.material).ToArray();
        }

        public override void Render()
        {
            if(_materials == null) return;
            // Debug.Log($"Data.WhiteRatio: {Data.WhiteRatio}  ");
            if (Runner.LocalPlayer)
            {
                foreach (var material in _materials)
                {
                    material?.SetFloat("_WhiteRatio", Data.WhiteRatio);
                }
            }

        }

        public void ChangeWhite()
        {
            var _ = ChangeWhiteAsync();
        }
        
        async UniTaskVoid ChangeWhiteAsync()
        {
            Debug.Log($"ChangeWhiteAsync"); 
            Data.WhiteRatio = 0.5f;
            await UniTask.Delay(500);
            Data.WhiteRatio = 0;
        } 
    }
}