#nullable enable

using Carry.CarrySystem.Map.Interfaces;
using Carry.EditMapSystem.EditMapForPlayer.Scripts;
using UnityEngine;
using UnityEngine.Assertions;
using VContainer;

namespace Carry.UISystem.UI.EditMap
{
    public class MapMakerToolCanvas : MonoBehaviour
    {
        [SerializeField] Transform buttonParent = null!;
        [SerializeField] CustomButton buttonPrefab = null!;

        IMapGetter _mapGetter = null!;
        MemorableEditMapBlockAttacher _editMapBlockAttacher = null!;

        CustomButton _redoButton = null!;
        CustomButton _undoButton = null!;

        [Inject]
        public void Construct(IMapGetter mapGetter, MemorableEditMapBlockAttacher memorableEditMapBlockAttacher)
        {
            _mapGetter = mapGetter;
            _editMapBlockAttacher = memorableEditMapBlockAttacher;
        }

        void Start()
        {
            Assert.IsNotNull(buttonParent);
            Assert.IsNotNull(buttonPrefab);

            var resetButton = Instantiate(buttonPrefab, buttonParent);
            resetButton.Init();
            resetButton.SetText("Reset Map");
            resetButton.AddListener(() => _editMapBlockAttacher.Clear(_mapGetter.GetMap()));

            _redoButton = Instantiate(buttonPrefab, buttonParent);
            _redoButton.Init();
            _redoButton.SetText("Redo");
            _redoButton.AddListener(() => _editMapBlockAttacher.Redo(_mapGetter.GetMap()));

            _undoButton = Instantiate(buttonPrefab, buttonParent);
            _undoButton.Init();
            _undoButton.SetText("Undo");
            _undoButton.AddListener(() => _editMapBlockAttacher.Undo(_mapGetter.GetMap()));
        }

        void Update()
        {
            _redoButton.Interactable = _editMapBlockAttacher.GetRedoStackCount() > 0;
            _undoButton.Interactable = _editMapBlockAttacher.GetUndoStackCount() > 0;
        }
    }
}