using System;
using Projects.CarrySystem.Block.Scripts;
using UnityEngine;

namespace Projects.CarrySystem.Block.Info
{
    [Serializable]
    public class BlockInfo
    {
        [NonSerialized] public GameObject blockViewObj;
        [NonSerialized] public BlockControllerNet blockController;

        public void Init(GameObject blockObj, BlockControllerNet blockController)
        {
            this.blockViewObj = blockObj;
            this.blockController = blockController;
        }

    }
}