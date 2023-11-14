using UnityEngine;
using UnityEngine.Assertions;
using VContainer;
using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.Map.Interfaces;
using Carry.EditMapSystem.EditMapForPlayer.Scripts;

#nullable enable

namespace Carry.UISystem.UI.EditMap
{
    public class EditMapToolCanvas : MonoBehaviour
    {
        [SerializeField] Transform buttonParent = null!;
        [SerializeField] CustomButton buttonPrefab;

        IMapUpdater _editMapUpdater = null!;

        [Inject]
        public void Construct(IMapUpdater editMapUpdater)
        {
            _editMapUpdater = editMapUpdater;
        }

        void Start()
        {
            Assert.IsNotNull(buttonParent);
            
            var editMapForPlayerInput = FindObjectOfType<EditMapForPlayerInput>();


            var customButton = Instantiate(buttonPrefab, buttonParent);
            customButton.Init();
            customButton.SetText("Erase Mode");
            customButton.AddListener(() => editMapForPlayerInput.ToggleEraseMode());

            customButton = Instantiate(buttonPrefab, buttonParent);
            customButton.Init();
            customButton.SetText("Reset Map");
            customButton.AddListener(() => ClearMap());

            customButton = Instantiate(buttonPrefab, buttonParent);
            customButton.Init();
            customButton.SetText("Redo");
            customButton.AddListener(() => ClearMap());

            customButton = Instantiate(buttonPrefab, buttonParent);
            customButton.Init();
            customButton.SetText("Undo");
            customButton.AddListener(() => ClearMap());
        }

        void ClearMap()
        {
            var map = _editMapUpdater.GetMap();

            //mapの全要素にRemoveEntityを適用する
            for (int i = 0; i < map.Length; i++)
            {
                var entities = map.GetSingleEntityList<IPlaceable>(i);

                //entitiesの数まで繰り返す 
                int count = entities.Count;
                
                //entitiesの数だけRemoveEntityを適用する
                for (int j = 0; j < count; j++)
                {
                    map.RemoveEntity(map.ToVector(i), entities[j]);
                }
            }
        }
    }
}