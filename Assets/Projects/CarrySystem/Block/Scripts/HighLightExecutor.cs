using Carry.CarrySystem.Block.Interfaces;
using UnityEngine;
#nullable enable

namespace Projects.CarrySystem.Block.Scripts
{
    public class HighlightExecutor :IHighlightExecutor
    {
        readonly BlockMaterialSetter _blockMaterialSetter;
        public HighlightExecutor(BlockMaterialSetter blockMaterialSetter )
        {
            _blockMaterialSetter =blockMaterialSetter;
        }
        public void Highlight()
        {
            Debug.Log($"HighLight!!!");
            if (_blockMaterialSetter == null)
            {
                Debug.Log($"_blockMaterialSetter is null!!!!!!!!!!!!!!!");
            }
            _blockMaterialSetter.ChangeWhite();
        }
    }
}