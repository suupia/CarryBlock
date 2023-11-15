using UnityEngine;
using UnityEngine.Assertions;
using VContainer;
using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.Map.Interfaces;
using Carry.EditMapSystem.EditMap.Scripts;
using Carry.EditMapSystem.EditMapForPlayer.Scripts;

#nullable enable

namespace Carry.UISystem.UI.EditMap
{
    public class EditMapToolCanvas : MonoBehaviour
    {
        [SerializeField] Transform buttonParent = null!;
        [SerializeField] CustomButton buttonPrefab;

        IMapGetter _mapGetter = null!;
        MemorableEditMapBlockAttacher _editMapBlockAttacher = null!;

        [Inject]
        public void Construct(IMapGetter mapGetter, MemorableEditMapBlockAttacher memorableEditMapBlockAttacher)
        {
            _mapGetter = mapGetter;
            _editMapBlockAttacher = memorableEditMapBlockAttacher;
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
            customButton.AddListener(() => _editMapBlockAttacher.Redo(_mapGetter.GetMap()));
            
            customButton = Instantiate(buttonPrefab, buttonParent);
            customButton.Init();
            customButton.SetText("Undo");
            customButton.AddListener(() => _editMapBlockAttacher.Undo(_mapGetter.GetMap()));
        }

        void ClearMap()
        {
            _editMapBlockAttacher.Reset();
            var map = _mapGetter .GetMap();

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
