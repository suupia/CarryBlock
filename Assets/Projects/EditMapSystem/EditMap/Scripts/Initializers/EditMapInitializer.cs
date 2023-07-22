

using System;
using System.Linq;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.Spawners;
using Cysharp.Threading.Tasks;
using Fusion;
using TMPro;
using UnityEngine;
using VContainer;

namespace Carry.EditMapSystem.EditMap.Scripts
{
    public class EditMapInitializer : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI mapKeyText;
        IMapUpdater _editMapUpdater;
        
        [Inject]
        public void Construct(
            IMapUpdater editMapUpdater)
        {
            _editMapUpdater = editMapUpdater;

        }

        async void Start()
        {
            var runner = FindObjectOfType<NetworkRunner>();
            if(runner == null) Debug.LogError($"NetworkRunner is not found.");
            await UniTask.WaitUntil(() => runner.SceneManager.IsReady(runner));
            
            _editMapUpdater.InitUpdateMap(MapKey.Default,-1); // -1が初期マップ

            // 準備シーンからMapKeyを受け取る
            var mapKeyContainer = FindObjectOfType<MapKeyContainer>();
            mapKeyText.text = $"MapKey : {mapKeyContainer.MapKey.ToString()}" ;
        }
    }
}