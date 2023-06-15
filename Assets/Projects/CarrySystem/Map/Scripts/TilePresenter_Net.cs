using System.Collections;
using System.Collections.Generic;
using Carry.CarrySystem.Map.Scripts;
using UnityEngine;
using Fusion;

public class TilePresenter : NetworkBehaviour
{
    // [Networked] NetworkBool ChangeActiveFlag { get; set; } // クライアントが変更を検知する必要があるので、[Networked]が必要
    // NetworkBool _preChangeActiveFlag; // それぞれのローカルが持てばよいので、[Networked]は不要

    [Networked] Vector2Int UpdatePos { set; get; }
    [Networked] NetworkBool IsActive { set; get; }

     EntityGridMap _map;

    public override void Render()
    {
        
        // UpdatePosにあるタイルの子オブジェクトのactiveSelfの状態をIsActiveに一致させる
        // どうやってもらうか？
    }

}
