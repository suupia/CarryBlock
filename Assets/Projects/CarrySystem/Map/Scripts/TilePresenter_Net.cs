using System.Collections;
using System.Collections.Generic;
using Carry.CarrySystem.Map.Scripts;
using UnityEngine;
using Fusion;

namespace Carry.CarrySystem.Map.Scripts
{
    public class TilePresenter_Net : NetworkBehaviour
    {
        // [Networked] NetworkBool ChangeActiveFlag { get; set; } // クライアントが変更を検知する必要があるので、[Networked]が必要
        // NetworkBool _preChangeActiveFlag; // それぞれのローカルが持てばよいので、[Networked]は不要

        // [Networked] Vector2Int UpdatePos { set; get; }
        // [Networked] NetworkBool IsActive { set; get; }

        [Networked] public ref PresentData presentDataRef => ref MakeRef<PresentData>();
        
        // 一旦、べた貼り付けにする
        [SerializeField] GameObject groundObject;
        [SerializeField] GameObject rockObject;
        
        EntityGridMap _map;
        public override void Render()
        {
            // UpdatePosにあるタイルの子オブジェクトのactiveSelfの状態をIsActiveに一致させる
            // どうやってもらうか？
            if (groundObject.activeSelf != presentDataRef.isGroundActive)
            {
                groundObject.SetActive(presentDataRef.isGroundActive);
            }

            if (rockObject.activeSelf != presentDataRef.isRockActive)
            {
                rockObject.SetActive(presentDataRef.isRockActive);
            }
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
