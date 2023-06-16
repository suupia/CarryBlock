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
        public struct PresentData : INetworkStruct
        {
            public NetworkBool IsGroundActive;
            public NetworkBool IsRockActive;
        }

        [Networked] public ref PresentData PresentDataRef => ref MakeRef<PresentData>();
        
        // 一旦、べた貼り付けにする
        [SerializeField] GameObject groundObject;
        [SerializeField] GameObject rockObject;
        
        EntityGridMap _map;
        public override void Render()
        {
            if (groundObject.activeSelf != PresentDataRef.IsGroundActive)
            {
                groundObject.SetActive(PresentDataRef.IsGroundActive);
            }

            if (rockObject.activeSelf != PresentDataRef.IsRockActive)
            {
                rockObject.SetActive(PresentDataRef.IsRockActive);
            }
        }
        
        public void SetEntityActiveData(IEntity entity, bool isActive)
        {
            if (entity is Ground)
            {
                PresentDataRef.IsGroundActive = isActive;
            }else if (entity is Rock)
            {
                PresentDataRef.IsRockActive = isActive;
            }
            else
            {
                throw new System.Exception("想定外のEntityが渡されました");
            }
        }

    }

}
