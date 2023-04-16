using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows;

namespace Main
{
    public class Tank : IPlayerUnit
    {
        readonly NetworkRunner　_runner;
        PlayerInfo _info;
        NetworkCharacterControllerPrototype _cc;
        readonly float _pickerHeight = 5.0f;
    
        public Tank(PlayerInfo info) 
        {
            _info = info;
            _runner = info.runner;
            _cc = info.networkCharacterController; 
            _cc.Controller.height = 0.0f;
            _cc.maxSpeed = 6.0f; 
        }

        public void Move(Vector3 direction)
        {
            _cc.Move(direction);
        }
    
        public float ActionCooldown() => 0.1f;

        public void Action()
        {
            Debug.Log($"Action()");
            var pickerPos = _info.playerObj.transform.position + new Vector3(0, _pickerHeight, 0);
            var picker = _runner.Spawn(_info.pickerPrefab, pickerPos,  Quaternion.identity, PlayerRef.None).GetComponent<NetworkPickerController>();
            picker.Init(_runner,_info.playerObj, _info.playerInfoForPicker);

        }
    

    }

}
