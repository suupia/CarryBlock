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
            public NetworkBool IsDoubleRockActive;
        }

        [Networked] public ref PresentData PresentDataRef => ref MakeRef<PresentData>();
        
        // 一旦、べた貼り付けにする
        [SerializeField] GameObject groundObject;
        [SerializeField] GameObject rockObject;
        [SerializeField] GameObject doubleRockObject;
        
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
            if (doubleRockObject.activeSelf != PresentDataRef.IsDoubleRockActive)
            {
                doubleRockObject.SetActive(PresentDataRef.IsDoubleRockActive);
            }
        }
        
        public void SetInitEntityActiveData(IEntity entity, bool isActive)
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
        public void SetEntityActiveData(IEntity entity,int count)
        {
            if (entity is Ground)
            {
                PresentDataRef.IsGroundActive = true; // とりあえずGroundは常にtrue
            }else if (entity is Rock)
            {
                switch (count)
                {
                    case 0:
                        PresentDataRef.IsRockActive = false;
                        PresentDataRef.IsDoubleRockActive = false;
                        break;
                    case 1:
                        PresentDataRef.IsRockActive = true;
                        PresentDataRef.IsDoubleRockActive = false;
                        break;
                    case 2:
                        PresentDataRef.IsRockActive = false;
                        PresentDataRef.IsDoubleRockActive = true;
                        break;
                    default:
                        throw new System.Exception("想定外のcountが渡されました");
                        break;
                }
            }
            else
            {
                throw new System.Exception("想定外のEntityが渡されました");
            }
        }

    }

}
