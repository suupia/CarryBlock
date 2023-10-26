using System;
using Projects.CarrySystem.Item.Scripts;
using UnityEngine;

#nullable enable
namespace Projects.CarrySystem.Gimmick.Scripts
{
    [Serializable]
    public class GimmickInfo
    {
        // [SerializeField] ItemInfo.ItemTypeEnum itemType;
        // [NonSerialized] public BlockMaterialSetter BlockMaterialSetter = null!;
        [NonSerialized] public GameObject ItemViewObj = null!;
        [NonSerialized] public GimmickControllerNet GimmickController = null!;
        public void Init(GameObject itemViewObj, GimmickControllerNet gimmickController)
        {
            ItemViewObj = itemViewObj;
            GimmickController = gimmickController;
            // BlockMaterialSetter = blockController.GetComponent<BlockMaterialSetter>();  // BlockControllerNetと同じオブジェクトにアタッチしている
            // BlockMaterialSetter.Init(this);
        }

    }
}