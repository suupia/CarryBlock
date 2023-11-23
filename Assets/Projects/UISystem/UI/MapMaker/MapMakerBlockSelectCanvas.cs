using Carry.CarrySystem.Block.Scripts;
using Carry.CarrySystem.CarriableBlock.Scripts;
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
        [SerializeField] CustomButton buttonPrefab;

        void Start()
        {
            Assert.IsNotNull(buttonParent);

            //EditMapForPlayerInputを探す
            var editMapForPlayerInput = FindObjectOfType<MapMakerInput>();

            InstantiateBlockSelectButton("Basic", () => editMapForPlayerInput.SetBlockType(typeof(BasicBlock)));
            InstantiateBlockSelectButton("UnmovableBlock",
                () => editMapForPlayerInput.SetBlockType(typeof(UnmovableBlock)));
            InstantiateBlockSelectButton("HeavyBlock", () => editMapForPlayerInput.SetBlockType(typeof(HeavyBlock)));
            InstantiateBlockSelectButton("FragileBlock",
                () => editMapForPlayerInput.SetBlockType(typeof(FragileBlock)));
            InstantiateBlockSelectButton("SpikeGimmick",
                () => editMapForPlayerInput.SetBlockType(typeof(SpikeGimmick)));
            InstantiateBlockSelectButton("ConfusionBlock",
                () => editMapForPlayerInput.SetBlockType(typeof(ConfusionBlock)));
            InstantiateBlockSelectButton("CannonBlock", () => editMapForPlayerInput.SetBlockType(typeof(CannonBlock)));
            InstantiateBlockSelectButton("TreasureCoin",
                () => editMapForPlayerInput.SetBlockType(typeof(TreasureCoin)));
        }


        void InstantiateBlockSelectButton(string blockName, UnityAction action)
        {
            var customButton = Instantiate(buttonPrefab, buttonParent);
            customButton.Init();
            customButton.SetText(blockName);
            customButton.AddListener(action);
        }
    }
}