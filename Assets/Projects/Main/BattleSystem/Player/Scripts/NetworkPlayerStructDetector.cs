using Fusion;
using System;
using System.Linq;
using UnityEngine;
using Animations;
using Decoration;

namespace Main
{
    public class NetworkPlayerStructDetector : NetworkBehaviour
    {
       　readonly IDecorationPlayer _animatorSetter;
        [Networked] int MainActionCount { get; set; }
        [Networked] int AttackCount { get; set; }
        [Networked] int PreHp { get; set; }

        public NetworkPlayerStructDetector(IDecorationPlayer animatorSetter)
        {
            _animatorSetter = animatorSetter;
        }
        
        public void IncrementMainActionCount()
        {
            MainActionCount++; // 今NetworkPlayerControllerで直に＋しているところを代わりにこれを呼ぶようにする
        }
        
        public void IncrementAttackCount()
        {
            AttackCount++;
        }

        public void Update(in NetworkPlayerStruct player) // NetworkPlayerController.Render()ではこれだけを呼ぶようにする
        {
            if (player.IsAlive)
            {
                if (player.Hp < PreHp)
                {
                    Debug.Log("OnDamaged!");
                    // _animatorSetter.OnDamaged();
                }
                if (player.Hp == 0)
                {
                    Debug.Log("OnDead!");
                    _animatorSetter.OnDead();
                }
                PreHp = player.Hp;
            }
        }
    }

}