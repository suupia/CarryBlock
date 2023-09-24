using System;
using Carry.CarrySystem.Block.Scripts;
using UnityEngine;
using UnityEngine.Serialization;

#nullable enable

namespace Carry.CarrySystem.Block.Info
{
    [Serializable]
    public class BlockInfo
    {
        [FormerlySerializedAs("_blockType")] [SerializeField] BlockTypeEnum blockType;
        [NonSerialized] public BlockMaterialSetter BlockMaterialSetter = null!;
        [NonSerialized] public GameObject BlockViewObj = null!;
        [NonSerialized] public BlockControllerNet BlockController = null!;
        public Type BlockType => DecideBlockType();

        public void Init(GameObject blockViewObj, BlockControllerNet blockController)
        {
            this.BlockViewObj = blockViewObj;
            this.BlockController = blockController;
            BlockMaterialSetter = blockController.GetComponent<BlockMaterialSetter>();  // BlockControllerNetと同じオブジェクトにアタッチしている
            BlockMaterialSetter.Init(this);
        }

        Type DecideBlockType()
        {
            Type? result = blockType switch
            {
                BlockTypeEnum.None => null,
                BlockTypeEnum.BasicBlock => typeof(BasicBlock),
                BlockTypeEnum.UnmovableBlock => typeof(UnmovableBlock),
                BlockTypeEnum.HeavyBlock => typeof(HeavyBlock),
                BlockTypeEnum.FragileBlock => typeof(FragileBlock),
                BlockTypeEnum.CannonBlock => typeof(CannonBlock),
                _ => throw new ArgumentOutOfRangeException()
            };
            if(result == null)
            {
                Debug.LogError($"_blockType is None");
                return null!;
            }

            return result;
        }

        enum BlockTypeEnum
        {
            None,
            BasicBlock,
            UnmovableBlock,
            HeavyBlock,
            FragileBlock,
            CannonBlock,
        }
    }

}