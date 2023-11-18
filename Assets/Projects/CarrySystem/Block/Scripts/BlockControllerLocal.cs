using Carry.CarrySystem.Block.Info;
using Carry.CarrySystem.Block.Interfaces;
using UnityEngine;
#nullable enable

namespace Carry.CarrySystem.Block.Scripts
{
    public class BlockControllerLocal : MonoBehaviour, IBlockController
    {
        public MonoBehaviour GetMonoBehaviour => this;
        public BlockInfo Info => info;
        [SerializeField] GameObject blockViewObj = null!;  // ランタイムで生成しないので、SerializeFieldで受け取れる

        [SerializeField] BlockInfo info = null!;
        public void Awake()
        {
            info.Init(blockViewObj, this);
        }
    }
}