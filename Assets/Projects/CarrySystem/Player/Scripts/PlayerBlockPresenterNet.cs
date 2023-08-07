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
            [Networked] public Int16 HoldingBlockType { get; set; } // enumは共有できない(?)ので、int16で送る
        }
        Dictionary<BlockType, GameObject> blockTypeToGameObjectMap = new Dictionary<BlockType, GameObject>();

        [Networked] public ref PresentData PresentDataRef => ref MakeRef<PresentData>();

        // このぐらいなら、PrefabLoadするまでもなく直接アタッチした方がよい
        [SerializeField] GameObject BasicBlockView= null!;
        [SerializeField] GameObject UnmovableBlockView= null!;
        [SerializeField] GameObject HeavyBlockView= null!;

        public void Awake()
        {
            // これらの処理はクライアントでも必要なことに注意
            blockTypeToGameObjectMap[BlockType.None] = null; // No game object for 'None'
            blockTypeToGameObjectMap[BlockType.BasicBlock] = BasicBlockView;
            blockTypeToGameObjectMap[BlockType.HeavyBlock] = HeavyBlockView;
        }

        public void Init(ICharacter character)
        {
            character.SetHoldPresenter(this);
        }
        public override void Render()
        {
            BlockType currentBlockType = (BlockType)PresentDataRef.HoldingBlockType;

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
            var blockType  = block switch
            {
                BasicBlock _ => BlockType.BasicBlock,
                UnmovableBlock _ => BlockType.UnmovableBlock,
                HeavyBlock _ => BlockType.HeavyBlock,
                _ => throw new ArgumentOutOfRangeException(nameof(block), block, null)
            };
            PresentDataRef.HoldingBlockType = (Int16)blockType;
        }

        public void PutDownBlock()
        {
            PresentDataRef.HoldingBlockType = (Int16)BlockType.None;
        }
        
        public void ReceiveBlock()
        {
            PresentDataRef.HoldingBlockType = (Int16)BlockType.BasicBlock;
        }
        
        public void PassBlock()
        {
            PresentDataRef.HoldingBlockType = (Int16)BlockType.None;
        }
    }
}