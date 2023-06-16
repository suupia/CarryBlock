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
            public NetworkBool isGroundActive;
            public NetworkBool isRockActive;
        }

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

}
