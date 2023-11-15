#nullable enable
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using Cysharp.Threading.Tasks;
using Fusion;
using TMPro;
using UnityEngine;
using VContainer;

namespace Carry.EditMapSystem.EditMap.Scripts
{
    public class LocalEditMapInitializer : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI mapKeyText;
        EditingMapTransporter _editingMapTransporter;
        
        [Inject]
        public void Construct(
            EditingMapTransporter editingMapTransporter)
        {
            _editingMapTransporter = editingMapTransporter;

        }

        void Awake()
        {
            //将来的にロードする関数に置き換える
            Debug.Log($"stage id: {_editingMapTransporter.StageId}, map index: {_editingMapTransporter.Index}");
        }
    }
}