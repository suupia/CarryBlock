using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

namespace Main
{
    public interface IUnitAction
    {
        void Action();
        bool InAction();
        float ActionCooldown();
    }
    
    public class EjectPicker : IUnitAction
    {
        NetworkRunner _runner;
        GameObject _playerObj;
        PlayerInfoForPicker _info;
        PrefabLoaderFromResources<NetworkPickerController> _prefabLoaderFromResources;
        
        float _pickerHeight = 5.0f;
        
        public EjectPicker(NetworkRunner runner, GameObject playerObj, PlayerInfoForPicker playerInfoForPicker)
        {
            _runner = runner;
            _playerObj = playerObj;
            _info = playerInfoForPicker;
            _prefabLoaderFromResources = new PrefabLoaderFromResources<NetworkPickerController>("Prefabs/Players");
        }

        public float ActionCooldown() => 0.1f;
        
        public bool InAction() => false;

        public void Action()
        {
            Debug.Log($"Action()");
            var pickerPos = _playerObj.transform.position + new Vector3(0, _pickerHeight, 0);
            Debug.Log($"_runner = {_runner}, _info.pickerPrefab = {_prefabLoaderFromResources.Load("Picker")}, pickerPos = {pickerPos}, PlayerRef.None = {PlayerRef.None}");
            var picker = _runner.Spawn(_prefabLoaderFromResources.Load("Picker"), pickerPos, Quaternion.identity, PlayerRef.None);
            Debug.Log($"picker = {picker}");
            picker.Init(_runner,_playerObj, _info);

        }
    }

}
