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
        readonly float _pickerHeight = 5.0f;
        IPlayerUnitMove _move;
    
        public Tank(PlayerInfo info) 
        {
            _info = info;
            _runner = info._runner;
            _move = new RegularMove()
            {
                transform = _info.playerObj.transform,
                rd = _info.playerRd,
                acceleration = _info.acceleration,
                maxVelocity = _info.maxVelocity,
                maxAngularVelocity = _info.maxAngularVelocity,
                torque = _info.torque
            };
            _info.playerRd.useGravity = true;
        }

        public void Move(Vector3 direction)
        {
            _move.Move(direction);
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
