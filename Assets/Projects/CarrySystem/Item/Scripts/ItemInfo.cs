#nullable enable
using System;
using Carry.CarrySystem.Block;
using Carry.CarrySystem.Block.Info;
using Carry.CarrySystem.Block.Scripts;
using Carry.CarrySystem.CarriableBlock.Scripts;
using UnityEngine;
using UnityEngine.Serialization;

namespace Projects.CarrySystem.Item.Scripts
{
    [Serializable]
    public class ItemInfo
    {
        [SerializeField] ItemInfo.ItemTypeEnum itemType;
        // [NonSerialized] public BlockMaterialSetter BlockMaterialSetter = null!;
        [NonSerialized] public GameObject ItemViewObj = null!;
        [NonSerialized] public ItemControllerNet ItemController = null!;
        public Type ItemType => DecideItemType();

        public void Init(GameObject itemViewObj, ItemControllerNet itemController)
        {
            ItemViewObj = itemViewObj;
            ItemController = itemController;
            // BlockMaterialSetter = blockController.GetComponent<BlockMaterialSetter>();  // BlockControllerNetと同じオブジェクトにアタッチしている
            // BlockMaterialSetter.Init(this);
        }

        Type DecideItemType()
        {
            Type? result = itemType switch
            {
                ItemInfo.ItemTypeEnum.None => null,
                ItemInfo.ItemTypeEnum.TreasureCoin => typeof(TreasureCoin),
                _ => throw new ArgumentOutOfRangeException()
            };
            if(result == null)
            {
                Debug.LogError($"itemType is None");
                return null!;
            }

            return result;
        }

        enum ItemTypeEnum
        {
            None,
            TreasureCoin,
        }
    }
}