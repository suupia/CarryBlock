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
        IMapGetter _editMapGetter;
        
        [Inject]
        public void Construct(
            IMapGetter editMapGetter)
        {
            _editMapGetter = editMapGetter;

        }

        void Awake()
        {
            _editMapGetter.InitUpdateMap(MapKey.Morita,-1); // -1が初期マップ
            
        }
    }
}