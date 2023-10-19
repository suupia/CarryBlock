using System.Collections.Generic;
using System.Linq;
using Carry.CarrySystem.Player.Info;
using Fusion;
using UnityEngine;
using UnityEngine.Serialization;
#nullable enable

namespace Carry.CarrySystem.Player.Scripts
{
    /// <summary>
    /// パスの範囲を可視化するオブジェクトにアタッチするクラス
    /// CarryPlayerControllerからGetComponentInChildrenで取得する
    /// </summary>
    public class PassRangeNet : NetworkBehaviour
    {
        [SerializeField] CapsuleCollider detectCollider = null!;
        PlayerInfo _info = null!;

         SearchPlayer _searchPlayer = null!;

        public void Init(PlayerInfo info, int layerMask)
        {
            _info = info;
            _searchPlayer = new SearchPlayer(detectCollider,detectCollider.radius,_info.PlayerObj.transform,layerMask);
        }


        public CarryPlayerControllerNet? DetectedTarget()
        {
            return _searchPlayer.DetectedTarget();
        }

    }
}