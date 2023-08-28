using System.Collections.Generic;
using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.Player.Interfaces;
using UnityEngine;

namespace Projects.CarrySystem.Block.Scripts
{
    public abstract class AbstractBlock : IBlock
    {
        public Vector2Int GridPosition { get; set; }
        public abstract  int MaxPlacedBlockCount { get; }
        public abstract bool CanPickUp();
        public abstract void PickUp(ICharacter character);
        public abstract bool CanPutDown(IList<IBlock> blocks);
        public abstract void PutDown(ICharacter character);

        public void Highlight()
        {
            Debug.Log($"highlight name : {this.GetType().Name}");
            // Hightが呼ばれている間だけハイライトするようにする
        }
    }
}