using Carry.CarrySystem.Map.Scripts;
using TMPro;
using UnityEngine;
using VContainer;

namespace Carry.EditMapSystem.EditMap.Scripts
{
    public class EditMapCUILoad
    {
        [SerializeField] GameObject CUICanvas;
        [SerializeField] TextMeshProUGUI messageText;
        [SerializeField] TextMeshProUGUI inputText;
        EditMapManager _editMapManager;
        EntityGridMapSaver _entityGridMapSaver;

        readonly int _maxDigit = 10; // インデックスの最大の桁数
        readonly float _displayTime = 2.0f; // メッセージを表示する時間
        bool _isOpened = false;
        int _index = 0;

        MapKey _key = MapKey.Morita; // ToDo: とりあえずKokiで固定
        

        enum CUIInputState
        {
            InputIndex,
            Save,
            DecideOverride,
            OverrideSave,
            CancelOverride,
            Cancel,
            End,
        }
        [Inject]
        public void Construct(EditMapManager editMapManager, EntityGridMapSaver entityGridMapSaver)
        {
            _editMapManager = editMapManager;
            _entityGridMapSaver = entityGridMapSaver;
        }
        public void OpenLoadUI()
        {

        }
    }
}