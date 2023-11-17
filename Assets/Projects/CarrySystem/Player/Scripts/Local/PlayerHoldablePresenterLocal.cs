using System;
using System.Collections.Generic;
using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.CarriableBlock.Scripts;
using Carry.CarrySystem.Player.Interfaces;
using Carry.CarrySystem.Player.Scripts;
using Fusion;
using UnityEngine;
#nullable enable

namespace Projects.CarrySystem.Player.Scripts.Local
{
    [RequireComponent(typeof(CarryPlayerControllerLocal))]
    public class PlayerHoldablePresenterLocal : MonoBehaviour, IPlayerHoldablePresenter
    {
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
        struct PresentData
        {
           internal BlockType HoldingBlockType { get; set; }
        }
        Dictionary<BlockType, GameObject> blockTypeToGameObjectMap = new Dictionary<BlockType, GameObject>();

        PresentData _presentData = new PresentData();

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
            _presentData.HoldingBlockType = BlockType.None;
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

        void Update() 
        {
            BlockType currentBlockType = _presentData.HoldingBlockType;

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
            _presentData.HoldingBlockType = DecideBlockType(holdable);
        }

        public void DisableHoldableView()
        {
            _presentData.HoldingBlockType = BlockType.None;
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