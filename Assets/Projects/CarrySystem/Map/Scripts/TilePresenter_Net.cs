using System.Collections;
using System.Collections.Generic;
using Carry.CarrySystem.Entity.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using UnityEngine;
using Fusion;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Entity.Scripts;


namespace Carry.CarrySystem.Map.Scripts
{
    public class TilePresenter_Net : NetworkBehaviour, ITilePresenter
    {
        // [Networked] NetworkBool ChangeActiveFlag { get; set; } // クライアントが変更を検知する必要があるので、[Networked]が必要
        // NetworkBool _preChangeActiveFlag; // それぞれのローカルが持てばよいので、[Networked]は不要

        // [Networked] Vector2Int UpdatePos { set; get; }
        // [Networked] NetworkBool IsActive { set; get; }

        [Networked] public ref PresentData PresentDataRef => ref MakeRef<PresentData>();
        
        // 一旦、べた貼り付けにする
        [SerializeField] GameObject groundObject;
        [SerializeField] GameObject rockObject;
        
        EntityGridMap _map;
        public override void Render()
        {
            if (groundObject.activeSelf != PresentDataRef.isGroundActive)
            {
                groundObject.SetActive(PresentDataRef.isGroundActive);
            }

            if (rockObject.activeSelf != PresentDataRef.isRockActive)
            {
                rockObject.SetActive(PresentDataRef.isRockActive);
            }
        }
        
        /// <summary>
        /// ランタイムでTilePresenter_Netが生成される関係上、インターフェイスでのSetEntityActiveDataをドメインの処理に挟み込めない
        /// そのため、最初にこのメソッドを一度呼ぶことにする
        /// </summary>
        /// <param name="presentData"></param>
        public void SetPresentData(PresentData presentData)
        {
            PresentDataRef = presentData;
        }

        public void SetEntityActiveData(IEntity entity, bool isActive)
        {
            if (entity is Ground)
            {
                PresentDataRef.isGroundActive = isActive;
            }else if (entity is Rock)
            {
                PresentDataRef.isRockActive = isActive;
            }
            else
            {
                throw new System.Exception("想定外のEntityが渡されました");
            }
        }

    }
    public struct PresentData : INetworkStruct
    {
        public NetworkBool isGroundActive;
        public NetworkBool isRockActive;
    }
}
