using System;
using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.Block.Scripts;
using Carry.CarrySystem.CarriableBlock.Scripts;
using UnityEngine;
using UnityEngine.Serialization;

#nullable enable

namespace Carry.CarrySystem.Block.Info
{
    [Serializable]
    public class BlockInfo
    {
        [SerializeField] BlockTypeEnum blockType;
        [NonSerialized] public IBlockMaterialSetter BlockMaterialSetterNet = null!;
        [NonSerialized] public GameObject BlockViewObj = null!;
        [NonSerialized] public IBlockController BlockController = null!;
        public Type BlockType => DecideBlockType();

        public void Init(GameObject blockViewObj, IBlockController blockController)
        {
            BlockViewObj = blockViewObj;
            BlockController = blockController;
            BlockMaterialSetterNet = blockController.GetMonoBehaviour.GetComponent<IBlockMaterialSetter>();  // BlockControllerNetと同じオブジェクトにアタッチしている
            BlockMaterialSetterNet.Init(this);
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
                BlockTypeEnum.ConfusionBlock => typeof(ConfusionBlock),
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
            ConfusionBlock,
            CannonBlock,
        }
    }

}