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
    
        public Tank(PlayerInfo info) 
        {
            _info = info;
            _runner = info._runner;
        }

        public void Move(Vector3 direction)
        {
            _info.playerRd.AddForce(_info.acceleration * direction, ForceMode.Acceleration);
            if (_info.playerRd.velocity.magnitude >= _info.maxVelocity)
                _info.playerRd.velocity = _info.maxVelocity * _info.playerRd.velocity.normalized;
            if (direction == Vector3.zero)
                _info.playerRd.velocity = _info.resistance * _info.playerRd.velocity; //Decelerate when there is no key input
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
