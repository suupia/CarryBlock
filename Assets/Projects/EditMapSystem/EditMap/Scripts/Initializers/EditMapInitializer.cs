

using System;
using System.Linq;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.Spawners.Scripts;
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
        IMapGetter _editMapGetter;
        
        [Inject]
        public void Construct(
            IMapGetter editMapGetter)
        {
            _editMapGetter = editMapGetter;

        }

        async void Start()
        {
            var runner = FindObjectOfType<NetworkRunner>();
            if(runner == null) Debug.LogError($"NetworkRunner is not found.");
            await UniTask.WaitUntil(() => runner.SceneManager.IsReady(runner));
            
            _editMapGetter.InitUpdateMap(MapKey.Default,-1); // -1が初期マップ

            // 準備シーンからMapKeyを受け取る
            var mapKeyContainer = FindObjectOfType<MapKeyContainer>();
            mapKeyText.text = $"MapKey : {mapKeyContainer.MapKey.ToString()}" ;
        }
    }
}