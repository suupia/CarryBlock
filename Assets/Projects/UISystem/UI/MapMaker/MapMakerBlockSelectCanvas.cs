using System.Collections.Generic;
using Carry.CarrySystem.Block.Scripts;
using Carry.CarrySystem.CarriableBlock.Scripts;
using Carry.UISystem.UI.Prefabs;
using Projects.CarrySystem.Gimmick.Scripts;
using Projects.CarrySystem.Item.Scripts;
using Projects.MapMakerSystem.Scripts;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

#nullable enable

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

        readonly Dictionary<string, CustomViewButton> _buttonDictionary = new();
        
        void Start()
        {
            Assert.IsNotNull(buttonParent);

            //EditMapForPlayerInputを探す
            var mapMakerInput = FindObjectOfType<MapMakerInput>();

            InstantiateBlockSelectButton(basicBlockTexture, "Basic", () =>
            {
                SelectButton("Basic");
                mapMakerInput.SetBlockType(typeof(BasicBlock));
            });
            InstantiateBlockSelectButton(unmovableBlockTexture, "Unmovable", () =>
            {
                SelectButton("Unmovable");
                mapMakerInput.SetBlockType(typeof(UnmovableBlock));
            });
            InstantiateBlockSelectButton(heavyBlockTexture, "Heavy", () =>
            {
                SelectButton("Heavy");
                mapMakerInput.SetBlockType(typeof(HeavyBlock));
            });
            InstantiateBlockSelectButton(fragileBlockTexture, "Fragile", () =>
            {
                SelectButton("Fragile");
                mapMakerInput.SetBlockType(typeof(FragileBlock));
            });
            InstantiateBlockSelectButton(spikeGimmickTexture, "Spike", () =>
            {
                SelectButton("Spike");
                mapMakerInput.SetBlockType(typeof(SpikeGimmick));
            });
            InstantiateBlockSelectButton(confusionBlockTexture, "Confusion", () =>
            {
                SelectButton("Confusion");
                mapMakerInput.SetBlockType(typeof(ConfusionBlock));
            });
            InstantiateBlockSelectButton(cannonBlockTexture, "Cannon", () =>
            {
                SelectButton("Cannon");
                mapMakerInput.SetBlockType(typeof(CannonBlock));
            });
            InstantiateBlockSelectButton(treasureCoinTexture, "Coin", () =>
            {
                SelectButton("Coin");
                mapMakerInput.SetBlockType(typeof(TreasureCoin));
            });
            
            SelectButton("Basic");
        }

        void InstantiateBlockSelectButton(Texture2D blockTex, string text, UnityAction action)
        {
            var customButton = Instantiate(buttonPrefab, buttonParent);
            customButton.SetImage(blockTex);
            customButton.SetText(text);
            customButton.ClickAction = action;
            
            _buttonDictionary.Add(text, customButton);
        }
        
        void SelectButton(string text)
        {
            foreach (var button in _buttonDictionary.Values)
            {
                button.Interactable = true;
            }

            _buttonDictionary[text].Interactable = false;
        }
    }
}