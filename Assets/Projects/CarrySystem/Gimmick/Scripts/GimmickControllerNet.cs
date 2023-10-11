using System.Collections.Generic;
using Carry.CarrySystem.Gimmick.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using Fusion;
using Projects.CarrySystem.Item.Interfaces;
using Projects.CarrySystem.Item.Scripts;
using UnityEngine;

#nullable enable

namespace Projects.CarrySystem.Gimmick.Scripts
{
    public class GimmickControllerNet : NetworkBehaviour
    {
        [SerializeField] GameObject itemViewObj = null!;  // ランタイムで生成しないので、SerializeFieldで受け取れる
        [SerializeField] GimmickInfo info = null!;
        public GimmickInfo Info => info;
        IList<IGimmick>  _gimmicks;

        public void Init(IList<IGimmick> gimmicks)
        {
            _gimmicks = gimmicks;
        }

        public override void Spawned()
        {
            info.Init(itemViewObj, this);
        }


    }
}