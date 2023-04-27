using Fusion;
using System;
using System.Linq;
using UnityEngine;
using Animations;

namespace Main
{
    public class NetworkPlayerStructDetector
    {
        IAnimatorPlayerUnit _animatorSetter;
         int _preHp;

        public NetworkPlayerStructDetector(IAnimatorPlayerUnit animatorSetter , NetworkPlayerStruct player)
        {
            _animatorSetter = animatorSetter;
            _preHp = player.Hp;
        }

        public void Update(in NetworkPlayerStruct player)
        {
            if (player.IsAlive)
            {
                if (player.Hp < _preHp)
                {
                    Debug.Log("OnDamaged!");
                    // _animatorSetter.OnDamaged();
                }
                if (player.Hp == 0)
                {
                    Debug.Log("OnDead!");
                    _animatorSetter.OnDead();
                }
                _preHp = player.Hp;
            }
        }
    }

}