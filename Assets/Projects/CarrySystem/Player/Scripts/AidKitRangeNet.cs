using System;
using System.Collections.Generic;
using System.Linq;
using Carry.CarrySystem.Player.Info;
using Fusion;
using UnityEngine;

#nullable enable

namespace Carry.CarrySystem.Player.Scripts
{
    /// <summary>
    /// AidKitの適用範囲を決めるオブジェクトにアタッチするクラス
    /// 一応可視化できるようにオブジェクトとコライダーの組み合わせで実装しておく
    /// CarryPlayerControllerからGetComponentInChildrenで取得する
    /// </summary>
    public class AidKitRangeNet : NetworkBehaviour
    {
        [SerializeField] SphereCollider detectCollider = null!;
        PlayerInfo _info = null!;
        
        SearchPlayer _searchPlayer = null!;

        public void Init(PlayerInfo info, int layerMask)
        {
            _info = info;
            _searchPlayer =
                new SearchPlayer(detectCollider, detectCollider.radius, _info.PlayerObj.transform, layerMask);
        }

        public CarryPlayerControllerNet? DetectedTarget()
        {
            return _searchPlayer.DetectedTarget();
        }
    }
}