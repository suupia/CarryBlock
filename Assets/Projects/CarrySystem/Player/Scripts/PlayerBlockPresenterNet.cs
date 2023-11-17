using System;
using System.Collections;
using System.Collections.Generic;
using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.Block.Scripts;
using Carry.CarrySystem.CarriableBlock.Scripts;
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
        
        /// <summary>
        /// switch文ではなるべく使用しないようにする。代わりにパターンマッチングを使う
        /// HoldingBlockTypeのためにinternalにしているが他のクラスでは使用しないようにする
        /// </summary>
        enum BlockType
        {
            None,
            BasicBlock,
            UnmovableBlock,
            HeavyBlock,
            FragileBlock,
            ConfusionBlock
        }
        struct PresentData : INetworkStruct
        {
            internal BlockType HoldingBlockType { get; set; }
        }
        Dictionary<BlockType, GameObject> blockTypeToGameObjectMap = new Dictionary<BlockType, GameObject>();

        [Networked] ref PresentData PresentDataRef => ref MakeRef<PresentData>();

        // このぐらいなら、PrefabLoadするまでもなく直接アタッチした方がよい
        [SerializeField] GameObject basicBlockView= null!;
        [SerializeField] GameObject unmovableBlockView= null!;
        [SerializeField] GameObject heavyBlockView= null!;
        [SerializeField] GameObject fragileBlockView = null!;
        [SerializeField] GameObject ConfusionBlockView = null!;
        public void Init(IHoldActionExecutor holdActionExecutor, IPassActionExecutor passActionExecutor)
        {
            Debug.Log($"PlayerBlockPresenterNet.Init()");
            holdActionExecutor.SetPlayerBlockPresenter(this);
            passActionExecutor.SetPlayerBlockPresenter(this);
            PresentDataRef.HoldingBlockType = BlockType.None;
        }

        public void Awake()
        {
            // これらの処理はクライアントでも必要なことに注意
            blockTypeToGameObjectMap[BlockType.None] = null!; // No game object for 'None'
            blockTypeToGameObjectMap[BlockType.BasicBlock] = basicBlockView;
            blockTypeToGameObjectMap[BlockType.UnmovableBlock] = unmovableBlockView;
            blockTypeToGameObjectMap[BlockType.HeavyBlock] = heavyBlockView;
            blockTypeToGameObjectMap[BlockType.FragileBlock] = fragileBlockView;
            blockTypeToGameObjectMap[BlockType.ConfusionBlock] = ConfusionBlockView;

            basicBlockView.GetComponent<Collider>().enabled = false;
            unmovableBlockView.GetComponent<Collider>().enabled = false;
            heavyBlockView.GetComponent<Collider>().enabled = false;
            fragileBlockView.GetComponent<Collider>().enabled = false;
            ConfusionBlockView.GetComponent<Collider>().enabled = false;
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
        public void EnableHoldableView(IHoldable holdable)
        {
            Debug.Log($"PlayerBlockPresenterNet.PickUpBlock()");
            PresentDataRef.HoldingBlockType = DecideBlockType(holdable);
        }

        public void DisableHoldableView()
        {
            PresentDataRef.HoldingBlockType = BlockType.None;
        }
        

        BlockType DecideBlockType(IHoldable holdable)
        {
            var blockType  = holdable switch
            {
                BasicBlock _ => BlockType.BasicBlock,
                UnmovableBlock _ => BlockType.UnmovableBlock,
                HeavyBlock _ => BlockType.HeavyBlock,
                FragileBlock _ => BlockType.FragileBlock,
                ConfusionBlock _ => BlockType.ConfusionBlock,
                _ => throw new ArgumentOutOfRangeException(nameof(holdable), holdable, null)
            };
            return blockType;
        }
    }
}