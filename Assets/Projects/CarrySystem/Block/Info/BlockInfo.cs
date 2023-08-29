using System;
using Carry.CarrySystem.Block.Scripts;
using Projects.CarrySystem.Block.Scripts;
using UnityEngine;
#nullable enable

namespace Projects.CarrySystem.Block.Info
{
    [Serializable]
    public class BlockInfo
    {
        [SerializeField] BlockTypeEnum _blockType;
        [NonSerialized] public GameObject blockViewObj;
        [NonSerialized] public BlockControllerNet blockController;
        public Type BlockType => DecideBlockType();

        public void Init(GameObject blockObj, BlockControllerNet blockController)
        {
            this.blockViewObj = blockObj;
            this.blockController = blockController;
        }

        Type DecideBlockType()
        {
            Type? result = _blockType switch
            {
                BlockTypeEnum.None => null,
                BlockTypeEnum.BasicBlock => typeof(BasicBlock),
                BlockTypeEnum.UnmovableBlock => typeof(UnmovableBlock),
                BlockTypeEnum.HeavyBlock => typeof(HeavyBlock),
                BlockTypeEnum.FragileBlock => typeof(FragileBlock),
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
        }
    }

}