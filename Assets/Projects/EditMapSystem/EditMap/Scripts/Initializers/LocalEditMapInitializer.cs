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
        IMapSwitcher _editMapSwitcher;
        
        [Inject]
        public void Construct(
            IMapSwitcher editMapSwitcher)
        {
            _editMapSwitcher = editMapSwitcher;

        }

        void Awake()
        {
            _editMapSwitcher.InitSwitchMap();
            
        }
    }
}