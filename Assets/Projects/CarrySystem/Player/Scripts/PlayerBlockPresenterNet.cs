using System;
using System.Collections;
using System.Collections.Generic;
using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.Block.Scripts;
using Carry.CarrySystem.Entity.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using UnityEngine;
using Fusion;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Entity.Scripts;
using Carry.CarrySystem.Player.Interfaces;
using UnityEngine.Serialization;
using VContainer;
#nullable enable


namespace Carry.CarrySystem.Player.Scripts
{
    [RequireComponent(typeof(CarryPlayerControllerNet))]
    public class PlayerBlockPresenterNet : NetworkBehaviour, IPlayerBlockPresenter
    {
        // Presenter系のクラスはホストとクライアントで状態を一致させるためにNetworkedプロパティを持つので、
        // ドメインの情報を持ってはいけない
        public struct PresentData : INetworkStruct
        {
            public BlockType HoldingBlockType;
        }
        Dictionary<BlockType, GameObject> blockTypeToGameObjectMap = new Dictionary<BlockType, GameObject>();

        [Networked] public ref PresentData PresentDataRef => ref MakeRef<PresentData>();

        // このぐらいなら、PrefabLoadするまでもなく直接アタッチした方がよい
        [SerializeField] GameObject BasicBlockView= null!;
        [SerializeField] GameObject UnmovableBlockView= null!;
        [SerializeField] GameObject HeavyBlockView= null!;
        
        public void Init(ICharacter character)
        {
            character.SetHoldPresenter(this);
            blockTypeToGameObjectMap[BlockType.None] = null; // No game object for 'None'
            blockTypeToGameObjectMap[BlockType.BasicBlock] = BasicBlockView;
            blockTypeToGameObjectMap[BlockType.HeavyBlock] = HeavyBlockView;
        }
        public override void Render()
        {
            BlockType currentBlockType = PresentDataRef.HoldingBlockType;

            foreach (var blockType in blockTypeToGameObjectMap.Keys)
            {
                GameObject blockGameObject = blockTypeToGameObjectMap[blockType];
        
                // If the game object exists (i.e., not 'None'), set its active state.
                if (blockGameObject)
                {
                    blockGameObject.SetActive(blockType == currentBlockType);
                }
            }
        }

        // ホストのみで呼ばれることに注意
        // 以下の処理はアニメーション、音、エフェクトの再生を行いたくなったら、それぞれのクラスの対応するメソッドを呼ぶようにするかも
        public void PickUpBlock(IBlock block)
        {
            PresentDataRef.HoldingBlockType = block switch
            {
                BasicBlock _ => BlockType.BasicBlock,
                UnmovableBlock _ => BlockType.UnmovableBlock,
                HeavyBlock _ => BlockType.HeavyBlock,
                _ => throw new ArgumentOutOfRangeException(nameof(block), block, null)
            };
        }

        public void PutDownBlock()
        {
            PresentDataRef.HoldingBlockType = BlockType.None;
        }
        
        public void ReceiveBlock()
        {
            PresentDataRef.HoldingBlockType = BlockType.BasicBlock;
        }
        
        public void PassBlock()
        {
            PresentDataRef.HoldingBlockType = BlockType.None;
        }
    }
}