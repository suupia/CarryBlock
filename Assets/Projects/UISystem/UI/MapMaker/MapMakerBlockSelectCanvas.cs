using Carry.CarrySystem.Block.Scripts;
using Carry.CarrySystem.CarriableBlock.Scripts;
using Carry.UISystem.UI.Prefabs;
using Projects.CarrySystem.Gimmick.Scripts;
using Projects.CarrySystem.Item.Scripts;
using Projects.MapMakerSystem.Scripts;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

namespace Carry.UISystem.UI.EditMap
{
    public class MapMakerBlockSelectCanvas : MonoBehaviour
    {
        [SerializeField] Transform buttonParent = null!;
        
        //以下，Addressableから取得するべき
        [SerializeField] CustomViewButton buttonPrefab = null!;
        
        [SerializeField] Texture2D basicBlockTexture = null!;
        [SerializeField] Texture2D unmovableBlockTexture = null!;
        [SerializeField] Texture2D heavyBlockTexture = null!;
        [SerializeField] Texture2D fragileBlockTexture = null!;
        [SerializeField] Texture2D spikeGimmickTexture = null!;
        [SerializeField] Texture2D confusionBlockTexture = null!;
        [SerializeField] Texture2D cannonBlockTexture = null!;
        [SerializeField] Texture2D treasureCoinTexture = null!;

        void Start()
        {
            Assert.IsNotNull(buttonParent);

            //EditMapForPlayerInputを探す
            var mapMakerInput = FindObjectOfType<MapMakerInput>();

            InstantiateBlockSelectButton(basicBlockTexture, () => mapMakerInput.SetBlockType(typeof(BasicBlock)));
            InstantiateBlockSelectButton(unmovableBlockTexture,
                () => mapMakerInput.SetBlockType(typeof(UnmovableBlock)));
            InstantiateBlockSelectButton(heavyBlockTexture, () => mapMakerInput.SetBlockType(typeof(HeavyBlock)));
            InstantiateBlockSelectButton(fragileBlockTexture,
                () => mapMakerInput.SetBlockType(typeof(FragileBlock)));
            InstantiateBlockSelectButton(spikeGimmickTexture,
                () => mapMakerInput.SetBlockType(typeof(SpikeGimmick)));
            InstantiateBlockSelectButton(confusionBlockTexture,
                () => mapMakerInput.SetBlockType(typeof(ConfusionBlock)));
            InstantiateBlockSelectButton(cannonBlockTexture, () => mapMakerInput.SetBlockType(typeof(CannonBlock)));
            InstantiateBlockSelectButton(treasureCoinTexture,
                () => mapMakerInput.SetBlockType(typeof(TreasureCoin)));
        }


        void InstantiateBlockSelectButton(Texture2D blockTex, UnityAction action)
        {
            var customButton = Instantiate(buttonPrefab, buttonParent);
            customButton.SetImage(blockTex);
            customButton.ClickAction = action;
        }
    }
}