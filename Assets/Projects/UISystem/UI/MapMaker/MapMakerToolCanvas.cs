using Carry.CarrySystem.Map.Interfaces;
using Carry.EditMapSystem.EditMapForPlayer.Scripts;
using Carry.UISystem.UI.Prefabs;
using UnityEngine;
using UnityEngine.Assertions;
using VContainer;

#nullable enable

namespace Carry.UISystem.UI.EditMap
{
    public class MapMakerToolCanvas : MonoBehaviour
    {
        [SerializeField] Transform buttonParent = null!;
        
        //以下，Addressableから取得するべき
        [SerializeField] CustomViewButton buttonPrefab = null!;
        
        [SerializeField] Texture2D resetTexture = null!;
        [SerializeField] Texture2D redoTexture = null!;
        [SerializeField] Texture2D undoTexture = null!;

        IMapGetter _mapGetter = null!;
        MemorableEditMapBlockAttacher _editMapBlockAttacher = null!;

        CustomViewButton _redoButton = null!;
        CustomViewButton _undoButton = null!;

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
            resetButton.SetImage(resetTexture);
            resetButton.ClickAction = () => _editMapBlockAttacher.Clear(_mapGetter.GetMap());

            _redoButton = Instantiate(buttonPrefab, buttonParent);
            _redoButton.SetImage(redoTexture);
            _redoButton.ClickAction = () => _editMapBlockAttacher.Redo(_mapGetter.GetMap());

            _undoButton = Instantiate(buttonPrefab, buttonParent);
            _undoButton.SetImage(undoTexture);
            _undoButton.ClickAction = () => _editMapBlockAttacher.Undo(_mapGetter.GetMap());
        }

        void Update()
        {
            _redoButton.Interactable = _editMapBlockAttacher.GetRedoStackCount() > 0;
            _undoButton.Interactable = _editMapBlockAttacher.GetUndoStackCount() > 0;
        }
    }
}