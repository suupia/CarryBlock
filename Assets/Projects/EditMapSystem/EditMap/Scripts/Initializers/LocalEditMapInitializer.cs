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
        IMapUpdater _editMapUpdater;
        
        [Inject]
        public void Construct(
            IMapUpdater editMapUpdater)
        {
            _editMapUpdater = editMapUpdater;

        }

        void Awake()
        {
            _editMapUpdater.InitUpdateMap(MapKey.Default,-1); // -1が初期マップ
            
        }
    }
}