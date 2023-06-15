using System.Collections;
using System.Collections.Generic;
using Carry.CarrySystem.Map.Scripts;
using UnityEngine;
using Fusion;

namespace Carry.CarrySystem.Map.Scripts
{
    public class TilePresenter : NetworkBehaviour
    {
        // [Networked] NetworkBool ChangeActiveFlag { get; set; } // クライアントが変更を検知する必要があるので、[Networked]が必要
        // NetworkBool _preChangeActiveFlag; // それぞれのローカルが持てばよいので、[Networked]は不要

        // [Networked] Vector2Int UpdatePos { set; get; }
        // [Networked] NetworkBool IsActive { set; get; }

        [Networked] ref PresentData presentDataRef => ref MakeRef<PresentData>();
        
        // 一旦、べた貼り付けにする
        [SerializeField] GameObject groundObject;
        [SerializeField] GameObject rockObject;
        
        EntityGridMap _map;
        public override void Render()
        {
            // UpdatePosにあるタイルの子オブジェクトのactiveSelfの状態をIsActiveに一致させる
            // どうやってもらうか？
        }

        public void SetPresentData(ref PresentData presentData)
        {
            presentDataRef = presentData;
        }

    }
    public struct PresentData : INetworkStruct
    {
        public NetworkBool isGroundActive;
        public NetworkBool isRockActive;
    }
}
